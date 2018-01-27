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
        Quit
    }

    public class Message
    {
        public string messageTitle;
        public MessageType messageType;
    }

    [System.Serializable]
    public class ClientToServerMessage : Message
    {
        public string playerName;
        public string questionType;
        public string answers;
        public string message;

        public static ClientToServerMessage CreateFromJSON (string json)
        {
            return JsonUtility.FromJson<ClientToServerMessage>(json);
        }

        public string SaveJSON ()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [System.Serializable]
    public class ServerToClientMessage : Message
    {
        public List<string> message;

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
