using System.Collections.Generic;
using UnityEngine;

namespace UniMoonDialogue
{
    public class Scenario
    {
        public Dictionary<int, Dialogue> dialogs { private set; get; } = null;
        public Scenario(Dictionary<int, Dialogue> dialogs)
        {
            this.dialogs = dialogs;
        }
        public string ToJson()
        {
            return null;
        }
    }

    public class Dialogue
    {
        public string message { private set; get; }
        public bool selectable => (choices?.Count > 0);
        public List<Choice> choices { private set; get; } = null;
        public int nextID { private set; get; } = 0;
        public Dialogue(string message, int nextID = 0, List<Choice> choices = null)
        {
            this.message = message;
            this.choices = choices;
            this.nextID = nextID;
        }
    }

    public class Choice
    {
        public string text { private set; get; }
        public int nextID { private set; get; } = 0;
        public Choice(string text, int nextID)
        {
            Debug.Assert(nextID != 0);
            this.text = text;
            this.nextID = nextID;
        }
    }
}