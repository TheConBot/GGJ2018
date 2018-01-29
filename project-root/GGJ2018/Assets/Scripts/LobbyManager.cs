using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WritersFlock
{
    public class LobbyManager : MonoBehaviour
    {

        public List<AudioClip> music;

        public AudioSource narSource;
        public AudioClip introClip;
        public AudioClip resultsClip;

        public Text serverAddressText;
        public List<GameObject> playerPanels;

        public GameObject joinPanel;
        public GameObject resultsPanel;
        public Text bestStoryTitleText;
        public Text bestStoryBodyText;
        public Text featuredAuthorText;
        public Image featuresAuthorImage;
        public Text featuredSentance;

        private AudioSource audioSource;
        public AudioClip mainMenuMusic;
        public AudioClip resultsMusic;


        [SerializeField] List<Sprite> characters;
        List<Sprite> shuffledCharacters;


        public void CheckLobbyScreenState ()
        {
            if (ServerManager.instance.isPlaying)
            {
                joinPanel.SetActive(false);
                resultsPanel.SetActive(true);
                DisplayResults(ServerManager.instance.selectedStory, ServerManager.instance.importantPlayer);
                audioSource.clip = resultsMusic;
                audioSource.Play();
                narSource.clip = resultsClip;
                narSource.Play();
            }
            else
            {
                joinPanel.SetActive(true);
                resultsPanel.SetActive(false);
                audioSource.clip = mainMenuMusic;
                audioSource.Play();
                narSource.clip = introClip;
                narSource.Play();
            }
        }

        public void Start ()
        {
            audioSource = GetComponent<AudioSource>();
            serverAddressText.text = "Server Address: " + Network.player.ipAddress;
            shuffledCharacters = new List<Sprite>(characters);
            shuffledCharacters.Shuffle();
            CheckLobbyScreenState();
        }

        public void DisplayResults(Story chosenStory, Player chosenPlayer)
        {
            bestStoryTitleText.text = chosenStory.title;
            string body = "";
            var sentances = chosenStory.sentances;
            for (int j = 0; j < sentances.Count; j++)
            {
                body += sentances[j] + " ";
            }
            bestStoryBodyText.text = body;
            featuredAuthorText.text = "Featured Author:\n" + chosenPlayer.name;
            featuresAuthorImage.sprite = chosenPlayer.playerAvatar;
            StartCoroutine(ChangeFeaturedSentance(chosenPlayer.playerSentances));
        }

        private IEnumerator ChangeFeaturedSentance(List<string> sentances)
        {
            sentances.Shuffle();
            while (ServerManager.instance.isPlaying)
            {
                for (int i = 0; i < sentances.Count; i++)
                {
                    featuredSentance.text = sentances[i];
                    yield return new WaitForSeconds(3);
                }
            }
            yield return null;
        }

        public IEnumerator AddNewPlayerToScreen (Player player)
        {
            int index = player.playerIndex;
            playerPanels[index].GetComponentInChildren<Text>().text = player.name;
            playerPanels[index].GetComponentInChildren<Image>().sprite = shuffledCharacters[index];
            player.playerAvatar = shuffledCharacters[index];
            yield return null;
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
