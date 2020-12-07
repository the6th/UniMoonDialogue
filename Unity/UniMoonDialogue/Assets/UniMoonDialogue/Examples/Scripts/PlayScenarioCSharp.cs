using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UniMoonDialogue.ScenarioEngine;

namespace UniMoonDialogue
{
    public class PlayScenarioCSharp : MonoBehaviour
    {
        private int index = 0;

        Scenario scenario = new Scenario(
            dialogs: new Dictionary<int, Dialogue>
            {
                {1,  new Dialogue("こんにちは。私はC#で書かれています。")},
                {2,  new Dialogue("次のメッセージ")},
                {3,  new Dialogue("質問です！",
                    choices: new List<Choice>
                    {
                        new Choice("はい",4),
                        new Choice("いいえ",5),
                    }
                )},
                {4,  new Dialogue("「はい」を選びましたね",6)},
                {5,  new Dialogue("「いいえ」を選びましたね",6)},
                {6,  new Dialogue("おしまい")}
            }
        );

        public void StartScenario()
        {
            if (ScenarioEngine.Instance.isRunning) return;

            var data = new EventData(gameObject);
            if (ScenarioEngine.Instance.StartScenario(data))
            {
                ScenarioEngine.Instance.OnMessageStart += OnMessageStart;
                ScenarioEngine.Instance.OnMessageEnd += OnMessageEnd;
                ScenarioEngine.Instance.OnUserInput += OnUserInput;

                index = scenario.dialogs.Keys.Min();
                ShowDialogue(data, index);
            }
        }

        private void OnUserInput(EventData data, ScenarioChoice choice)
        {
            if (data.gameObject != gameObject) return;

            //ユーザが質問に答えて、次のIDが指定されている場合
            if ((int)choice > 0 && scenario.dialogs[index].choices[(int)choice - 1].nextID > 0)
                index = scenario.dialogs[index].choices[(int)choice - 1].nextID;
            //そのまま次に進む
            else if (scenario.dialogs[index].nextID < 1)
                index++;
            //次のIDが指定されている場合
            else
                index = scenario.dialogs[index].nextID;

            if (scenario.dialogs.Keys.Max() >= index)
                ShowDialogue(data, index);
            else
                ScenarioEngine.Instance.StopScenario();
        }
        private void ShowDialogue(EventData data, int key)
        {
            if (!scenario.dialogs[key].selectable)
                data.msg(scenario.dialogs[key].message);
            else
            {
                var choiceTexts = scenario.dialogs[key].choices.Select(item => item.text).ToArray();
                //data.msg(scenario.dialogs[key].message);
                data.choice(
                    title: scenario.dialogs[key].message,
                    choices: choiceTexts
                );
            }
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
