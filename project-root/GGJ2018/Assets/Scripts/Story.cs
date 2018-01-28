using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WritersFlock
{
    [Serializable]
    public class Story
    {
        public Story ()
        {
            title = "Blank Story";
            sentances = new List<string>();
        }

        public string title;
        public int points;
        public List<string> sentances;
    }

    [Serializable]
    public class Title
    {
        public Title (string title)
        {
            titleText = title;
        }

        public string titleText;
        public int points;
    }
}
