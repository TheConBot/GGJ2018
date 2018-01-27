using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WritersFlock
{
    public class Story
    {
        public string title;
        public string startingPrefix;
        public List<string> sentances;
        public Dictionary<string, int> sentanceVotes;
    }
}
