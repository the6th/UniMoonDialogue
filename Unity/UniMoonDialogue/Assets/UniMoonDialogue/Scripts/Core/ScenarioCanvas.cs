﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UniMoonDialogue.ScenarioEngine;

namespace UniMoonDialogue
{
    public class ScenarioCanvas : MonoBehaviour
    {
        [Header("UIComponents")]
        [SerializeField] private RectTransform dialoguePanel = null;
        [SerializeField] private ScenarioChoiceButton choiceButtonSeed = null;
        [SerializeField] private Text messageText = null;
        [SerializeField] private ScenarioChoiceButton NextButton = null;
        [SerializeField] private string NextButtonText = "次へ";
        [SerializeField] private AudioClip voiceClip = null;
        private AudioSource audioSource = null;
        private ScenarioEngine engine = null;
        private float defaultDialoguePanelScaleX = 0f;
        private float durationToOpen = 0.5f;
        private bool isCoroutineRunnning = false;
        [Header("Observe (Optional)")]
        [SerializeField]
        private ScenarioObserver observer = null;

        private List<ScenarioChoiceButton> activeChoiceButtonList = new List<ScenarioChoiceButton>();
        private void Awake()
        {
            messageText.text = "";

            if (voiceClip != null && !gameObject.TryGetComponent<AudioSource>(out audioSource))
                audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.5f;
            audioSource.pitch = 2f;

            NextButton.SetupChoiceButton(NextButtonText, 0, audioSource);

            defaultDialoguePanelScaleX = dialoguePanel.localScale.x;

            if (observer == null) gameObject.TryGetComponent<ScenarioObserver>(out observer);

            if (observer != null && observer.observables.Count < 1)
                Debug.LogError("Observer Component にobservablesが設定されていません。Observerを削除するか、購読対象のobservablesを追加してください。");

        }

        private void Start()
        {
            dialoguePanel.gameObject.SetActive(false);
            choiceButtonSeed.gameObject.SetActive(false);

            engine = ScenarioEngine.Instance;

            engine.OnMessageStart += (_) =>
            {
                if (!isOverveTarget(_)) return;
                StartCoroutine("ShowDialoguePanel", true);
            };

            engine.OnMessageEnd += (_) =>
            {
                if (!isOverveTarget(_)) return;

                StartCoroutine("ShowDialoguePanel", false);
            };

            engine.OnMessageUpdate += (EventData data, string arg1, float progress) =>
            {
                if (!isOverveTarget(data)) return;

                OnMessageUpdate(data, arg1, progress);
            };

            engine.OnChoiceStart += (EventData data, string[] messages) =>
            {
                if (!isOverveTarget(data)) return;

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

        bool isOverveTarget(EventData data)
        {
            if (observer == null) return true;

            var b = (observer.observables.FirstOrDefault(_ => _.gameObject == data.gameObject) != null);
            return b;
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
                messageText.text = "";
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
                ScenarioEngine.Instance.isPaused = false;

                Debug.Log("Coroutine Cancelled");
                yield break;
            }
            isCoroutineRunnning = true;

            ScenarioEngine.Instance.isPaused = true;
            while (diff < durationToOpen)
            {

                //Canceled
                if (!isCoroutineRunnning) yield break;

                diff = Time.time - startTime;
                dialoguePanel.localScale = Vector3.Lerp(startScale, endScale, diff / durationToOpen);
                yield return null;
            }
            ScenarioEngine.Instance.isPaused = false;

            dialoguePanel.localScale = endScale;
            isCoroutineRunnning = false;

            if (!b) dialoguePanel.gameObject.SetActive(false);
        }

        private void OnMessageUpdate(EventData data, string arg1, float progress)
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
    }
}