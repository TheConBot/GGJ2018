using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WritersFlock
{
    public class ServerManager : MonoBehaviour
    {

        [Header("Server Config")]
        public Text ipDisplay;
        public int port = 9999;

        private List<string> players;
        private WebSocketServer socketConnection;

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
            socketConnection = new WebSocketServer(port);
            socketConnection.AddWebSocketService<MessageService>("/");
            socketConnection.Start();
            Debug.Log("Server running...");
            //TODO: Change this shit
            ipDisplay.text = Network.player.ipAddress;
        }

        public bool AddNewPlayer(string name)
        {
            if(players.Contains(name)) { return false; }
            players.Add(name);
            return true;
        }
        
    }


    public class MessageService : WebSocketBehavior
    {

        protected override void OnMessage (MessageEventArgs e)
        {
            Debug.Log("Recieving Message...");
            string data = e.Data;
            var jsonClass = ClientToServerMessage.CreateFromJSON(data);
            ParseMessage(jsonClass);
        }

        private void ParseMessage (ClientToServerMessage message)
        {
            var server = ServerManager.instance;
            switch (message.messageType)
            {
                case MessageType.Connect:
                    if (server.AddNewPlayer(message.playerName))
                    {
                        var successMessage = new ServerToClientMessage();
                        successMessage.messageType = MessageType.Connect;
                        successMessage.messageTitle = "Connected";
                        SendMessageToClient(successMessage);
                    }
                    else
                    {
                        var failMessage = new ServerToClientMessage();
                        failMessage.messageType = MessageType.Connect;
                        failMessage.messageTitle = "Error";
                        failMessage.message.Add("That name is taken!");
                        SendMessageToClient(failMessage);
                    }
                    break;
            }
        }

        private void SendMessageToClient (ServerToClientMessage message)
        {
            string json = message.SaveJSON();
            Send(json);
        }
    }
}


