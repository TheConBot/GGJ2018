using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace WritersFlock
{
    public class MainManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public List<AudioClip> music;
        public GameObject writingPanel;
        public GameObject singleVotingPanel;
        public GameObject groupVotingPanel;
        public Text singleVotingTitle;
        public Text singleVotingBody;
        public List<Text> groupVotingTitles;
        public List<Text> groupVotingBody;
        public List<RoundSettings> rounds;
        [HideInInspector]
        public int roundIndex = 0;
        [HideInInspector]
        public int currentTurn = 0;

        public void Start ()
        {
            ChangeToWritingPanel();
            ServerManager.instance.StartWritingRound(this);
        }

        public void ChangeToGroupVotingPanel (List<Title> titles)
        {
            currentTurn = 0;
            groupVotingPanel.SetActive(true);
            writingPanel.SetActive(false);
            singleVotingPanel.SetActive(false);
            for (int i = 0; i < groupVotingTitles.Count; i++)
            {
                if(i >= titles.Count)
                {
                    groupVotingTitles[i].transform.parent.gameObject.SetActive(false);
                    continue;
                }
                groupVotingTitles[i].text = titles[i].titleText;
            }
        }

        public void ChangeToGroupVotingPanel (List<Story> stories)
        {
            currentTurn = 0;
            groupVotingPanel.SetActive(true);
            writingPanel.SetActive(false);
            singleVotingPanel.SetActive(false);
            for (int i = 0; i < groupVotingTitles.Count; i++)
            {
                if (i >= stories.Count)
                {
                    groupVotingTitles[i].transform.parent.gameObject.SetActive(false);
                    continue;
                }

                string body = "";
                var sentances = stories[i].sentances;
                for (int j = 0; j < sentances.Count; j++)
                {
                    body += sentances[j] + " ";
                }

                groupVotingTitles[i].text = stories[i].title;
                groupVotingBody[i].text = body;
            }
        }

        public void ChangeToGroupVotingPanel (List<string> titles, List<string> bodys)
        {
            currentTurn = 0;
            groupVotingPanel.SetActive(true);
            writingPanel.SetActive(false);
            singleVotingPanel.SetActive(false);
            for (int i = 0; i < groupVotingTitles.Count; i++)
            {
                if (i >= titles.Count)
                {
                    groupVotingTitles[i].transform.parent.gameObject.SetActive(false);
                    continue;
                }
                groupVotingTitles[i].text = titles[i];
                groupVotingBody[i].text = bodys[i];
            }
        }

        public void ChangeToSingleVotingPanel ()
        {
            currentTurn = 0;
            singleVotingPanel.SetActive(true);
            writingPanel.SetActive(false);
            groupVotingPanel.SetActive(false);
        }

        public void AdvanceSingleVotingPanel(string title, string body)
        {
            singleVotingTitle.text = title;
            singleVotingBody.text = body;
        }

        public void ChangeToWritingPanel ()
        {
            currentTurn = 0;
            writingPanel.SetActive(true);
            groupVotingPanel.SetActive(false);
            singleVotingPanel.SetActive(false);
        }

        public RoundSettings CurrentRound ()
        {
            return rounds[roundIndex];
        }
    }
}
