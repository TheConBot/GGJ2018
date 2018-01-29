using System;
using System.Collections.Generic;
using UnityEngine;

namespace WritersFlock
{
    public enum MessageType
    {
        Connect,
        Entry,
        Vote,
        Restart,
        Quit,
        Ready,
        Wait
    }

    public class Message
    {
        public string messageTitle;
        public MessageType messageType;
    }

    [Serializable]
    public class ClientToServerMessage : Message
    {
        public string playerName;
        public string message;

        public ClientToServerMessage () { }

        public ClientToServerMessage (MessageType messageType, string messageTitle, string message, string playerName)
        {
            this.messageType = messageType;
            this.messageTitle = messageTitle;
            this.message = message;
            this.playerName = playerName;
        }

        public static ClientToServerMessage CreateFromJSON (string json)
        {
            return JsonUtility.FromJson<ClientToServerMessage>(json);
        }

        public string SaveJSON ()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [Serializable]
    public class ServerToClientMessage : Message
    {
        public List<string> message;
        public int numberOfWritingRounds;

        public ServerToClientMessage () { }

        public ServerToClientMessage (MessageType messageType, string messageTitle, List<string> message)
        {
            this.messageType = messageType;
            this.messageTitle = messageTitle;
            this.message = message;
        }

        public ServerToClientMessage (MessageType messageType, string messageTitle, List<string> message, int rounds)
        {
            this.messageType = messageType;
            this.messageTitle = messageTitle;
            this.message = message;
            this.numberOfWritingRounds = rounds;
        }

        public static ClientToServerMessage CreateFromJSON (string json)
        {
            return JsonUtility.FromJson<ClientToServerMessage>(json);
        }

        public string SaveJSON ()
        {
            return JsonUtility.ToJson(this);
        }
    }

}
