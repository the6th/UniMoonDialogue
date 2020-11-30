using MoonSharp.Interpreter;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UniMoonDialogue
{
    public class ScenarioEngine : SingletonMonoBehaviour<ScenarioEngine>
    {
        [MoonSharp.Interpreter.MoonSharpUserData]
        public class EventData
        {
            public GameObject gameObject;
            public string displayName = "";
            public EventData(GameObject gameObject, string displayName = "")
            {
                this.gameObject = gameObject;

                if (displayName == "")
                    this.displayName = gameObject.name;
                else
                    this.displayName = displayName;
            }

            /// <summary>
            /// 画面にセリフを表示
            /// </summary>
            /// <param name="mes"></param>
            public void msg(string mes)
            {
                ScenarioEngine.Instance.UpdateScenario(
                    type: ScenarioType.TapToNext,
                    messages: new string[] { mes },
                    data: this
                );
            }

            /// <summary>
            /// 選択肢を表示
            /// </summary>
            /// <param name="title"></param>
            /// <param name="choices"></param>
            public void choice(string title, params string[] choices)
            {
                var message = new string[choices.Length + 1];
                message[0] = title;
                choices.CopyTo(message, 1);
                ScenarioEngine.Instance.UpdateScenario(
                    type: ScenarioType.Select,
                    messages: message,
                    data: this
                );
            }
        }

        public enum ScenarioType { TapToNext, Select, None };
        public enum ScenarioChoice
        {
            SKIP = 0,
            SELECT_1,
            SELECT_2,
            SELECT_3,
            SELECT_4,
            None
        };

        public ScenarioType scenarioType
        {
            private set
            {
                m_scenarioType = value;
            }
            get { return m_scenarioType; }
        }
        [SerializeField]
        private ScenarioType m_scenarioType = ScenarioType.None;

        [SerializeField] private LuaTextAsset luaScript = null;
        [SerializeField] private bool AutoStart = false;
        [SerializeField] private bool StepScenario = true;
        [SerializeField] private float stepSpeed = 0.1f;

        private ScenarioChoice scenarioChoice = ScenarioChoice.None;
        private DynValue coroutine;
        private string[] currentMessages;
        private string currentText;
        private EventData currentEventData;

        #region Public Method
        /// <summary>
        /// メッセージ表示中か？
        /// </summary>
        public bool isRunning
        {
            get
            {
                //Debug.Log($"{isMonoRunning}");
                //Debug.Log($"{coroutine?.Coroutine.State != CoroutineState.Dead}");
                if (isMonoRunning == true) return true;
                if (coroutine == null) return false;
                return (coroutine.Coroutine.State != CoroutineState.Dead);
            }
        }
        //Step表示実行中か？
        private bool isStepRunning = false;
        [SerializeField]
        private bool isMonoRunning = false;
        //Step表示を中止するか？（早送り表示)
        public bool skipStep { private set; get; } = false;


        public UnityAction<EventData, ScenarioType, string, float> OnMessageUpdate;
        public UnityAction<EventData> OnMessageStart;
        public UnityAction<EventData> OnMessageEnd;
        public UnityAction<EventData, string[]> OnChoiceStart;
        public UnityAction<EventData, ScenarioChoice> OnUserInput;

        private void UpdateScenario(ScenarioType type, string[] messages, EventData data)
        {
            scenarioType = type;
            if (type == ScenarioType.Select)
            {
                OnChoiceStart?.Invoke(data, messages);
            }
            else if (!StepScenario)
            {
                currentText = messages[0];
                OnMessageUpdate?.Invoke(data, scenarioType, currentText, 1f);
            }
            else
            {
                currentMessages = messages;
                StartCoroutine("StepMessage", data);
            }
        }

        IEnumerator StepMessage(EventData data)
        {
            int messageCount = 0; //現在表示中の文字数
            currentText = "";
            isStepRunning = true;
            float progress = 0f;

            //一文字ずつ表示する場合はタグがあると、おかしくなるため一時的にタグ除去を行う
            var stripped = StringChecker.StripHTMLTags(currentMessages[0]);
            while (stripped.Length > messageCount)//文字をすべて表示していない場合ループ
            {
                currentText += stripped[messageCount];//一文字追加
                messageCount++;//現在の文字数
                progress = (float)messageCount / stripped.Length;

                if (progress == 1f)
                {
                    //最後まで読んだらタグ除去を解除する
                    currentText = currentMessages[0];
                    //選択式の場合は選択文も文字列に追加する
                    if (scenarioType == ScenarioType.Select)
                    {
                        currentText += "\r\n";
                        for (int i = 1; i < currentMessages.Length; i++)
                        {
                            currentText += $"{i}:{currentMessages[i]} ";
                        }
                    }
                }

                OnMessageUpdate?.Invoke(data, scenarioType, currentText, progress);

                if (skipStep)
                {
                    messageCount = stripped.Length - 1;
                    skipStep = false;
                    yield return null;
                }
                else
                    yield return new WaitForSeconds(stepSpeed);//任意の時間待つ
            }
            isStepRunning = false;
        }

        public void StartScenario(LuaTextAsset lua, GameObject owner, string displayOwnername = "")
        {
            //if (isRunning) return;
            if (coroutine != null || isMonoRunning) return;

            UserData.RegisterAssembly(typeof(EventData).Assembly);
            Script script = new Script();
            currentEventData = new EventData(owner, displayOwnername);
            script.Globals["scene"] = currentEventData;

            //var scenario = LoadScenario();
            var scenario = LoadScenario(lua);
            //Debug.Log(scenario);
            DynValue function = script.DoString(scenario);
            coroutine = script.CreateCoroutine(function);
            OnMessageStart?.Invoke(currentEventData);

            coroutine?.Coroutine.Resume();
        }

        public bool StartScenario(EventData data)
        {
            if (isRunning) return false;

            isMonoRunning = true;
            currentEventData = data;
            OnMessageStart?.Invoke(currentEventData);
            return true;
        }
        public void StopScenario()
        {
            scenarioType = ScenarioType.None;
            isMonoRunning = false;
            OnMessageEnd?.Invoke(currentEventData);
        }

        #endregion


        private void Start()
        {
            if (AutoStart) StartScenario(luaScript, gameObject);
        }

        /// <summary>
        /// ユーザーイベント、クリックや選択が発生
        /// </summary>
        /// <param name="choice"></param>
        public void ScenarioSelect(ScenarioChoice choice)
        {
            //Debug.Log("ScenarioSelect" + choice.ToString());
            if (isRunning && scenarioType != ScenarioType.None)
            {
                if (StepScenario && isStepRunning)
                {
                    skipStep = true;
                }
                else
                {
                    scenarioChoice = choice;
                    coroutine?.Coroutine.Resume((int)scenarioChoice);
                    OnUserInput?.Invoke(currentEventData, scenarioChoice);
                }
            }
            else if (scenarioType != ScenarioType.None)
            {
                scenarioType = ScenarioType.None;
                coroutine = null;
                OnMessageEnd?.Invoke(currentEventData);
            }
        }

        private string LoadScenario(LuaTextAsset luaTextAsset)
        {
            return luaTextAsset.text;
        }

    }
}