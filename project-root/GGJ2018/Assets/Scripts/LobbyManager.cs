using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WritersFlock
{
    public class LobbyManager : MonoBehaviour
    {

        public Text serverAddressText;
        public List<GameObject> playerPanels;

        public void Start ()
        {
            serverAddressText.text = Network.player.ipAddress;
        }

        public void AddNewPlayerToScreen (Player player)
        {
            playerPanels[player.playerIndex].GetComponentInChildren<Text>().text = player.name;
        }
    }
}
