using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WritersFlock
{
    public class LobbyManager : MonoBehaviour
    {

        public List<AudioClip> music;

        public Text serverAddressText;
        public List<GameObject> playerPanels;

        [SerializeField] List<Sprite> characters;
        List<Sprite> shuffledCharacters;

        public void Start ()
        {
            serverAddressText.text = "Server Address: " + Network.player.ipAddress;
            shuffledCharacters = new List<Sprite>(characters);
            shuffledCharacters.Shuffle();
        }

        public void AddNewPlayerToScreen (Player player)
        {
            int index = player.playerIndex;
            playerPanels[index].GetComponentInChildren<Text>().text = player.name;
            playerPanels[index].GetComponentInChildren<Image>().sprite = shuffledCharacters[index];
            player.playerAvatar = shuffledCharacters[index];
        }



    }
    static class Extensions {
        private static System.Random rng = new System.Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}
