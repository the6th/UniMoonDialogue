using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniMoonDialogue
{
    public class ScenarioChoiceButton : MonoBehaviour
    {
        [SerializeField] private Text ButtonText;
        [SerializeField] private Button button;
        [SerializeField] private ScenarioEngine.ScenarioChoice choice;
        [SerializeField] private AudioClip ClickClip = null;

        void Awake()
        {
            if (ButtonText == null) ButtonText = GetComponentInChildren<Text>();
            if (button == null) button = GetComponent<Button>();
        }

        public void SetupChoiceButton(string text, int index, AudioSource audioSource = null)
        {
            ButtonText.text = text;
            this.gameObject.SetActive(true);
            choice = (ScenarioEngine.ScenarioChoice)Enum.ToObject(typeof(ScenarioEngine.ScenarioChoice), index);
            //Debug.Log($"SetupChoiceButton:{text}/{index}/{choice.ToString()}");
            gameObject.name = choice.ToString();

            button.onClick.AddListener(() =>
            {
                if (audioSource != null && ClickClip != null)
                {
                    var pitch = audioSource.pitch;
                    audioSource.pitch = 1f;
                    audioSource.PlayOneShot(ClickClip);
                    audioSource.pitch = pitch;
                }

                Invoke("DelaySelect", 0.3f);
            });
        }
        private void DelaySelect()
        {
            ScenarioEngine.Instance.ScenarioSelect(choice);
        }
    }
}