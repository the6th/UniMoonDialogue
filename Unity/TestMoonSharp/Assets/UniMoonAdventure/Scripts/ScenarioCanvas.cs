using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using static UniMoonAdventure.ScenarioEngine;

namespace UniMoonAdventure
{
    public class ScenarioCanvas : MonoBehaviour
    {
        [SerializeField]
        private RectTransform dialoguePanel;
        [SerializeField]
        private ScenarioChoiceButton choiceButtonSeed;


        [SerializeField]
        private Text messageText;

        [SerializeField]
        private ScenarioChoiceButton NextButton;

        [SerializeField]
        private AudioClip voiceClip;
        private AudioSource audioSource = null;
        private ScenarioEngine engine = null;
        private float defaultDialoguePanelScaleX = 0f;
        private float durationToOpen = 0.5f;
        private bool isCoroutineRunnning = false;

        private List<ScenarioChoiceButton> activeChoiceButtonList = new List<ScenarioChoiceButton>();
        private void Awake()
        {
            engine = ScenarioEngine.Instance;

            engine.OnMessageUpdate += OnMessageUpdate;
            engine.OnMessageStart += () =>
            {
                StartCoroutine("ShowDialoguePanel", true);
            };
            engine.OnMessageEnd += () =>
            {
                StartCoroutine("ShowDialoguePanel", false);

            };
            engine.OnChoiceStart += (string[] messages) =>
            {
                NextButton.gameObject.SetActive(false);
                activeChoiceButtonList.Clear();
                for (int i = 1; i < messages.Length; i++)
                {
                    var choice = GameObject.Instantiate(choiceButtonSeed, choiceButtonSeed.transform.parent) as ScenarioChoiceButton;
                    choice.SetupChoiceButton(messages[i], i);
                    activeChoiceButtonList.Add(choice);
                }
            };

            NextButton.GetComponent<Button>().onClick.AddListener(() => {
                engine.ScenarioSelect(ScenarioChoice.SKIP);
            });


            messageText.text = "";

            if (voiceClip != null && !gameObject.TryGetComponent<AudioSource>(out audioSource))
                audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.02f;
            audioSource.pitch = 2f;

            defaultDialoguePanelScaleX = dialoguePanel.localScale.x;
            dialoguePanel.gameObject.SetActive(false);
            choiceButtonSeed.gameObject.SetActive(false);
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
                if (!isCoroutineRunnning)
                {
                    //dialoguePanel.localScale = startScale;
                    yield break;
                }

                diff = Time.time - startTime;

                dialoguePanel.localScale = Vector3.Lerp(startScale, endScale, diff / durationToOpen);
                yield return null;
            }
            dialoguePanel.localScale = endScale;
            if (!b)
            {
                dialoguePanel.gameObject.SetActive(false);
            }
            isCoroutineRunnning = false;
        }

        private void OnMessageUpdate(ScenarioEngine.ScenarioType arg0, string arg1, float progress)
        {
            if(activeChoiceButtonList.Count > 0)
            {
                foreach(var c in activeChoiceButtonList)
                {
                    Destroy(c.gameObject);
                }
                activeChoiceButtonList.Clear();
                NextButton.gameObject.SetActive(true);
            }

            if (progress != 1f) NextButton?.gameObject.SetActive(false);
            else NextButton?.gameObject.SetActive(true);

            messageText.text = arg1;

            if (voiceClip == null) return;
            if (progress == 1 || audioVaild(arg1) && !engine.skipStep)
            {
                if (audioSource.isPlaying) audioSource.Stop();
                audioSource.PlayOneShot(voiceClip);
                //Debug.Log("Sound");

            }
        }

        private bool audioVaild(string text)
        {
            var last = text.Substring(text.Length - 1);

            //英数記号
            if (Regex.IsMatch(last, @"^[0-9a-zA-Z]+$"))
                return true;
            //漢字
            if (IsKanji(last[0]))
                return true;
            //カタカナ
            if (Regex.IsMatch(last, @"^[\p{IsKatakana}\u31F0-\u31FF\u3099-\u309C\uFF65-\uFF9F]+$"))
                return true;
            //ひらがな
            if (Regex.IsMatch(last, @"^\p{IsHiragana}+$"))
                return true;

            //Debug.Log($"invaild: '{last}'");
            return false;
        }

        public static bool IsKanji(char c)
        {
            //CJK統合漢字、CJK互換漢字、CJK統合漢字拡張Aの範囲にあるか調べる
            return ('\u4E00' <= c && c <= '\u9FCF')
                || ('\uF900' <= c && c <= '\uFAFF')
                || ('\u3400' <= c && c <= '\u4DBF');
        }

        private void Update()
        {
            if (!ScenarioEngine.Instance.isRunning) return;
            CheckKey();
        }

        private void CheckKey()
        {
            switch (engine.scenarioType)
            {
                case ScenarioType.TapToNext:
                    if (Input.GetMouseButtonDown(0)) engine.ScenarioSelect(ScenarioChoice.SKIP);
                    break;
            }
        }

    }
}