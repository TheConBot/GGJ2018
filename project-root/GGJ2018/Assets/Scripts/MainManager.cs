using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WritersFlock
{
    public class MainManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public List<AudioClip> music;
        public GameObject writingPanel;
        public GameObject votingPanel;
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

        public void ChangeToVotingPanel ()
        {
            currentTurn = 0;
            writingPanel.SetActive(false);
            votingPanel.SetActive(true);
        }

        public void ChangeToWritingPanel ()
        {
            currentTurn = 0;
            writingPanel.SetActive(true);
            votingPanel.SetActive(false);
        }

        public RoundSettings CurrentRound ()
        {
            return rounds[roundIndex];
        }
    }
}
