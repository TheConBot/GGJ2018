using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine.SceneManagement;


namespace WritersFlock
{
    public class ServerManager : MonoBehaviour
    {

        [Header("Server Config")]
        public List<Player> players = new List<Player>();
        public bool isPlaying = false;

        //Singleton
        public static ServerManager instance { get; private set; }

        private void Awake ()
        {
            if (instance != null && instance != this)
            {
                // Destroy if another Gamemanager already exists
                Destroy(gameObject);
            }
            else
            {

                // Here we save our singleton S
                instance = this;
                // Furthermore we make sure that we don't destroy between scenes
                DontDestroyOnLoad(gameObject);
            }
        }

        //Server Init
        private void Start ()
        {
            var socketConnection = new WebSocketServer(1024);
            socketConnection.AddWebSocketService<NetworkService>("/");
            socketConnection.Start();
            Debug.Log("Server running...");
        }

        public IEnumerator AddNewPlayer (string name, NetworkService network)
        {
            //Each client gets a their own instance of Message Service, which means the SendDatatoClient method is reliant on the object instance (aka keep your players and their messageservices in order)
            if (GetPlayerByName(name) != null)
            {
                if (isPlaying)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].name == name)
                        {
                            players[i] = new Player(players[i].name, i, players[i].playerAvatar, network);
                            //May want to put data for loading back into the game in the args here
                            var reconnect = new ServerToClientMessage(MessageType.Connect, "Reconnected", null);
                            network.SendMessageToClient(reconnect);
                        }
                    }
                }
                else
                {
                    var failMessage = new ServerToClientMessage(MessageType.Connect, "Error: That name is taken.", null);
                    network.SendMessageToClient(failMessage);
                }
                yield break;
            }

            var player = new Player(name, players.Count, null, network);
            players.Add(player);
            Debug.Log("Player " + player.PlayerNumber + " '" + player.name + "' has joined the game!");

            var lobbyManager = FindObjectOfType<LobbyManager>();
            lobbyManager.AddNewPlayerToScreen(player);

            var successMessage = new ServerToClientMessage(MessageType.Connect, "Connected", null);
            if(players.Count == 1)
            {
                player.isHost = true;
                successMessage.message = new List<string> { "host" };
            }
            player.networkService.SendMessageToClient(successMessage);
            yield return null;
        }

        public IEnumerator StartGame (string playerName)
        {
            var player = GetPlayerByName(playerName);
            player.networkService.SendMessageToClient(new ServerToClientMessage(MessageType.Ready, "Starting Game!", null));
            SceneManager.LoadScene(1);
            yield return null;
        }

        private Player GetPlayerByName (string name)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].name == name) { return players[i]; }
            }
            return null;
        }
    }

    [Serializable]
    public class Player
    {
        public Player (string name, int index, Sprite avatar, NetworkService network)
        {
            this.name = name;
            this.playerIndex = index;
            this.playerAvatar = avatar;
            this.networkService = network;
            this.isHost = false;
        }

        public string name;
        public int playerIndex;
        public Sprite playerAvatar;
        public NetworkService networkService;
        public bool isHost;
        public int PlayerNumber
        {
            get
            {
                return playerIndex + 1;
            }
        }
    }
}


