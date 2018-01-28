using System;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WritersFlock
{
    public class NetworkService : WebSocketBehavior
    {

        protected override void OnMessage (MessageEventArgs e)
        {
            string data = e.Data;
            var jsonClass = ClientToServerMessage.CreateFromJSON(data);
            Debug.Log("Recieving Message of type: " + jsonClass.messageType);
            ParseMessage(jsonClass);
        }

        protected override void OnClose (CloseEventArgs e)
        {
            Debug.Log(e.Reason);
        }

        private void ParseMessage (ClientToServerMessage message)
        {
            switch (message.messageType)
            {
                case MessageType.Connect:
                    Debug.Log("Trying to connect a player.");
                    UnityMainThreadDispatcher.Instance().Enqueue(ServerManager.instance.AddNewPlayer(message.playerName, this));
                    break;
                case MessageType.Ready:
                    Debug.Log("Starting game!");
                    UnityMainThreadDispatcher.Instance().Enqueue(ServerManager.instance.StartGame(message.playerName));
                    break;
            }
        }

        public void SendMessageToClient (ServerToClientMessage message)
        {
            Debug.Log("Sending message to client...");
            string json = message.SaveJSON();
            Send(json);
        }
    }
}
