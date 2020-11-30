﻿using System.Collections.Generic;
using UnityEngine;
using static UniMoonDialogue.ScenarioEngine;

namespace UniMoonDialogue
{
    public class PlayScenarioCSharp : MonoBehaviour
    {
        List<string> messages = new List<string>()
        {
            "Hi,This message is generated by csharp script,PlayScenarioCSharp.cs",
            "I am 2nd message ",
            "last message ,thanks.",
        };

        [SerializeField]
        private int cnt = 0;
        public void StartScenario()
        {
            ScenarioEngine.Instance.OnMessageStart += OnMessageStart;
            ScenarioEngine.Instance.OnMessageEnd += OnMessageEnd;
            ScenarioEngine.Instance.OnUserInput += OnUserInput;

            var data = new EventData(gameObject);
            if (ScenarioEngine.Instance.StartScenario(data))
            {
                cnt = 0;
                data.msg(messages[cnt]);
            }
        }

        private void OnUserInput(EventData data, ScenarioChoice choice)
        {
            if (data.gameObject != gameObject) return;

            cnt++;
            if (messages.Count > cnt)
                data.msg(messages[cnt]);
            else
                ScenarioEngine.Instance.StopScenario();
        }

        private void OnMessageStart(ScenarioEngine.EventData data)
        {
            if (data.gameObject != gameObject) return;
            //Debug.Log("OnMessageStart" + data.displayName);
            ExampleCommon.RotateByForce(gameObject, true);

        }

        private void OnMessageEnd(ScenarioEngine.EventData data)
        {
            if (data.gameObject != gameObject) return;

            //Debug.Log("OnMessageEnd" + data.displayName);
            ScenarioEngine.Instance.OnMessageStart -= OnMessageStart;
            ScenarioEngine.Instance.OnMessageEnd -= OnMessageEnd;
            ScenarioEngine.Instance.OnUserInput -= OnUserInput;
            ExampleCommon.RotateByForce(gameObject, false);
        }
    }
}
