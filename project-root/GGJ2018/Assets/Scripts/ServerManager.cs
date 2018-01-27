using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;

public class ServerManager : MonoBehaviour {

    [SerializeField] Text ipDisplay;

    private void Start ()
    {
        var wssv = new WebSocketServer(9999);
        wssv.AddWebSocketService<TextDisplay>("/");
        wssv.Start();
        ipDisplay.text = Network.player.ipAddress;
    }

}


public class TextDisplay : WebSocketBehavior {
    protected override void OnMessage(MessageEventArgs e) {
        Debug.Log(e.Data);
        string data = e.Data;
        var jsonClass = MessageJSON.CreateFromJSON(data);

        Debug.Log("name: " + jsonClass.name);
        Debug.Log("q type: " + jsonClass.questionType);
        foreach (var item in jsonClass.answers)
        {
            Debug.Log("answer: " + item);
        }

        jsonClass.name += " is a person";

        data = jsonClass.SaveJSON();
        Send(data);
    }
}

[System.Serializable]
public class MessageJSON {
    public string name;
    public string questionType;
    public string[] answers;

    public static MessageJSON CreateFromJSON(string json)
    {
        return JsonUtility.FromJson<MessageJSON>(json);
    }

    public string SaveJSON()
    {
        return JsonUtility.ToJson(this);
    }
}

