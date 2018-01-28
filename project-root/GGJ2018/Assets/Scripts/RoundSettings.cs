using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WritersFlock
{
    [CreateAssetMenu()]
    public class RoundSettings : ScriptableObject
    {
        public int roundNumber;
        public bool importantPlayerStarts;
        public string importantPlayerPrompt;
        public int numberOfWritingTurns;
        public bool votingTurnsIsStoryCount;
        public List<string> data;
    }
}
