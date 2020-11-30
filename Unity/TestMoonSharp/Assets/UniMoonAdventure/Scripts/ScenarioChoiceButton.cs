using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniMoonAdventure
{
    public class ScenarioChoiceButton : MonoBehaviour
    {
        [SerializeField]
        private Text ButtonText;
        [SerializeField]
        Button button;
        [SerializeField]
        ScenarioEngine.ScenarioChoice choice;

        void Awake()
        {
            if (ButtonText == null) ButtonText = GetComponentInChildren<Text>();
            if (button == null) button = GetComponent<Button>();
        }

        public void SetupChoiceButton(string text, int index)
        {
            ButtonText.text = text;
            this.gameObject.SetActive(true);
            choice = (ScenarioEngine.ScenarioChoice)Enum.ToObject(typeof(ScenarioEngine.ScenarioChoice), index);
            //Debug.Log($"SetupChoiceButton:{text}/{index}/{choice.ToString()}");
            button.onClick.AddListener(() =>
            {
                ScenarioEngine.Instance.ScenarioSelect(choice);
            });
        }
    }
}