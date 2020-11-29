using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using static UniMoonAdventure.ScenarioEngine;

namespace UniMoonAdventure
{
    [RequireComponent(typeof(AudioSource))]
    public class ScenarioToCanvasText : MonoBehaviour
    {
        [SerializeField]
        Text messageText;

        [SerializeField]
        RectTransform NextUI;

        [SerializeField]
        AudioClip voiceClip;
        AudioSource audioSource;

        private ScenarioEngine engine = null;


        private void Awake()
        {
            engine = ScenarioEngine.Instance;
            engine.OnMessageUpdate += OnMessageUpdate;
            messageText.text = "";

            audioSource = GetComponent<AudioSource>();
        }

        private void OnMessageUpdate(ScenarioEngine.ScenarioType arg0, string arg1, float progress)
        {
            if (progress != 1f)
                NextUI?.gameObject.SetActive(false);
            else
                NextUI?.gameObject.SetActive(true);

            messageText.text = arg1;

            if (voiceClip == null) return;
            if (progress == 1 || audioVaild(arg1) && !engine.skipStep)
                audioSource.PlayOneShot(voiceClip);
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
            CheckKey();
        }

        private void CheckKey()
        {
            if (engine.scenarioType == ScenarioType.None) return;

            if (Input.GetKeyDown(KeyCode.Alpha1)) engine.ScenarioSelect(ScenarioChoice.SELECT_1);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) engine.ScenarioSelect(ScenarioChoice.SELECT_2);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) engine.ScenarioSelect(ScenarioChoice.SELECT_3);
            else if (Input.GetKeyDown(KeyCode.Alpha4)) engine.ScenarioSelect(ScenarioChoice.SELECT_4);
            else if (Input.GetKeyDown(KeyCode.Alpha5)) engine.ScenarioSelect(ScenarioChoice.SELECT_5);
            else if (Input.GetMouseButtonDown(0)) engine.ScenarioSelect(ScenarioChoice.SKIP);
        }

    }
}