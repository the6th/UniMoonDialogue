using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UniMoonDialogue.ScenarioEngine;

namespace UniMoonDialogue
{
    public class ScenarioCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform dialoguePanel = null;
        [SerializeField] private ScenarioChoiceButton choiceButtonSeed = null;
        [SerializeField] private Text messageText = null;
        [SerializeField] private ScenarioChoiceButton NextButton = null;

        [SerializeField] private AudioClip voiceClip = null;

        private AudioSource audioSource = null;
        private ScenarioEngine engine = null;
        private float defaultDialoguePanelScaleX = 0f;
        private float durationToOpen = 0.5f;
        private bool isCoroutineRunnning = false;

        private List<ScenarioChoiceButton> activeChoiceButtonList = new List<ScenarioChoiceButton>();
        private void Awake()
        {
            messageText.text = "";

            if (voiceClip != null && !gameObject.TryGetComponent<AudioSource>(out audioSource))
                audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.5f;
            audioSource.pitch = 2f;

            NextButton.SetupChoiceButton("次へ", 0, audioSource);

            defaultDialoguePanelScaleX = dialoguePanel.localScale.x;
            dialoguePanel.gameObject.SetActive(false);
            choiceButtonSeed.gameObject.SetActive(false);

            engine = ScenarioEngine.Instance;

            engine.OnMessageStart += (_) => StartCoroutine("ShowDialoguePanel", true);
            engine.OnMessageEnd += (_) => StartCoroutine("ShowDialoguePanel", false);

            engine.OnMessageUpdate += OnMessageUpdate;

            engine.OnChoiceStart += (EventData data, string[] messages) =>
            {
                NextButton.gameObject.SetActive(false);
                activeChoiceButtonList.Clear();
                messageText.text = $"<i>[質問]:</i>\r\n{messages[0]}";
                for (int i = 1; i < messages.Length; i++)
                {
                    var choice = GameObject.Instantiate(choiceButtonSeed, choiceButtonSeed.transform.parent) as ScenarioChoiceButton;
                    choice.SetupChoiceButton(messages[i], i, audioSource);
                    activeChoiceButtonList.Add(choice);
                }
            };
        }

        IEnumerator ShowDialoguePanel(bool b)
        {
            var startTime = Time.time;
            var scale = dialoguePanel.localScale;
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.zero;
            var diff = Time.time - startTime;

            //openDialogue
            if (b)
            {
                startScale = new Vector3(0, scale.y, scale.z);
                endScale = new Vector3(defaultDialoguePanelScaleX, scale.y, scale.z);
                dialoguePanel.localScale = startScale;
                dialoguePanel.gameObject.SetActive(true);
            }
            else
            {
                endScale = new Vector3(0, scale.y, scale.z);
                startScale = new Vector3(defaultDialoguePanelScaleX, scale.y, scale.z);
            }

            if (isCoroutineRunnning)
            {
                isCoroutineRunnning = false;
                dialoguePanel.localScale = endScale;

                Debug.Log("Coroutine Cancelled");
                yield break;
            }
            isCoroutineRunnning = true;

            while (diff < durationToOpen)
            {
                //Canceled
                if (!isCoroutineRunnning) yield break;

                diff = Time.time - startTime;
                dialoguePanel.localScale = Vector3.Lerp(startScale, endScale, diff / durationToOpen);
                yield return null;
            }

            dialoguePanel.localScale = endScale;
            isCoroutineRunnning = false;

            if (!b) dialoguePanel.gameObject.SetActive(false);
        }

        private void OnMessageUpdate(EventData data, ScenarioEngine.ScenarioType arg0, string arg1, float progress)
        {
            if (activeChoiceButtonList.Count > 0)
            {
                foreach (var c in activeChoiceButtonList)
                {
                    Destroy(c.gameObject);
                }
                activeChoiceButtonList.Clear();
                NextButton.gameObject.SetActive(true);
            }

            messageText.text = $"<i>{data.displayName}:</i> {arg1}";

            if (voiceClip == null) return;
            if (progress == 1f || audioVaild(arg1) && !engine.skipStep)
            {
                audioSource.PlayOneShot(voiceClip);
            }
        }

        private bool audioVaild(string text)
        {
            var last = text.Substring(text.Length - 1);
            return StringChecker.isNormalString(last);
        }

        //private void Update()
        //{
        //    if (!ScenarioEngine.Instance.isRunning) return;
        //    CheckKey();
        //}

        //private void CheckKey()
        //{
        //    switch (engine.scenarioType)
        //    {
        //        case ScenarioType.TapToNext:
        //            if (Input.GetMouseButtonDown(0)) engine.ScenarioSelect(ScenarioChoice.SKIP);
        //            break;
        //    }
        //}

    }
}