using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace WritersFlock
{
    public struct Player
    {
        public string name;
        public int playerIndex;
        public Image playerAvatar;

        public int PlayerNumber
        {
            get
            {
                return playerIndex + 1;
            }
        }
    }

    public class ServerManager : MonoBehaviour
    {

        [Header("Server Config")]
        public List<Player> players = new List<Player>();

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
            socketConnection.AddWebSocketService<MessageService>("/");
            socketConnection.Start();
            Debug.Log("Server running...");
        }

        public IEnumerator AddNewPlayer (string name, MessageService network)
        {
            if (NameAlreadyTaken(name)) {
                var failMessage = new ServerToClientMessage(MessageType.Connect, "Error: That name is taken.", null);
                network.SendMessageToClient(failMessage);
                yield break;
            }
            var player = new Player();
            player.name = name;
            players.Add(player);
            player.playerIndex = players.IndexOf(player);
            Debug.Log("Player " + player.PlayerNumber + " '" + player.name + "' has joined the game!");
            var lobbyManager = FindObjectOfType<LobbyManager>();
            lobbyManager.AddNewPlayerToScreen(player);
            var successMessage = new ServerToClientMessage(MessageType.Connect, "Connected", null);
            network.SendMessageToClient(successMessage);
            yield return null;
        }

        private bool NameAlreadyTaken (string name)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].name == name) { return true; }
            }
            return false;
        }
    }
}


