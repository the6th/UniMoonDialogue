using System.Collections;
using UnityEngine;
using UnityEngine.Events;
#if ENABLE_MOONSHARP
using MoonSharp.Interpreter;
#endif

namespace UniMoonDialogue
{
    public class ScenarioEngine : SingletonMonoBehaviour<ScenarioEngine>
    {
#if ENABLE_MOONSHARP
        [MoonSharp.Interpreter.MoonSharpUserData]
#endif
        public class EventData
        {
            /// <summary>
            /// GameObject of scenario owner
            /// </summary>
            public GameObject gameObject { private set; get; }

            /// <summary>
            /// owner name.
            /// </summary>
            public string displayName { private set; get; }

            /// <summary>
            /// current scenarioType
            /// </summary>
            public ScenarioType scenarioType
            {
                private set
                {
                    m_scenarioType = value;
                }
                get { return m_scenarioType; }
            }

            private ScenarioType m_scenarioType = ScenarioType.None;

            /// <summary>
            /// create new scenario event
            /// </summary>
            /// <param name="gameObject">GameObject of scenario owner</param>
            /// <param name="displayName">owner name（無指定だとGameObjectの名前となる)</param>
            public EventData(GameObject gameObject, string displayName = "")
            {
                this.gameObject = gameObject;

                if (displayName == "")
                    this.displayName = gameObject.name;
                else
                    this.displayName = displayName;

                m_scenarioType = ScenarioType.None;
            }

            /// <summary>
            /// 画面にセリフを表示
            /// </summary>
            /// <param name="mes"></param>
            public void msg(string mes)
            {
                scenarioType = ScenarioType.TapToNext;
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
                scenarioType = ScenarioType.Select;
                var message = new string[choices.Length + 1];
                message[0] = title;
                choices.CopyTo(message, 1);
                ScenarioEngine.Instance.UpdateScenario(
                    type: ScenarioType.Select,
                    messages: message,
                    data: this
                );
            }

            /// <summary>
            /// シナリオを停止状態にする
            /// </summary>
            public void Stop()
            {
                scenarioType = ScenarioType.None;
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

        [SerializeField] private bool StepScenario = true;
        [SerializeField] private float stepSpeed = 0.1f;

        public EventData currentEventData { private set; get; }

        private ScenarioChoice scenarioChoice = ScenarioChoice.None;
#if ENABLE_MOONSHARP
        private DynValue coroutine;
#endif
        private string[] currentMessages;
        private string currentText;

        #region Public Method
        /// <summary>
        /// メッセージ表示中か？
        /// </summary>
        public bool isRunning
        {
            get
            {
#if ENABLE_MOONSHARP
                if (isMonoRunning == true) return true;
                if (coroutine == null) return false;
                return (coroutine.Coroutine.State != CoroutineState.Dead);
#else
                return (isMonoRunning == true);
#endif
            }
        }

        /// <summary>
        /// Step 表示実行中か？
        /// </summary>
        private bool isStepRunning = false;
        private bool isMonoRunning = false;
        
        /// <summary>
        /// Step 表示（早送り表示)を中止するか？
        /// </summary>
        public bool skipStep { private set; get; } = false;
        [HideInInspector]
        public bool isPaused = false;


        public UnityAction<EventData, string, float> OnMessageUpdate;
        public UnityAction<EventData> OnMessageStart;
        public UnityAction<EventData> OnMessageEnd;
        public UnityAction<EventData, string[]> OnChoiceStart;
        public UnityAction<EventData, ScenarioChoice> OnUserInput;

        private void UpdateScenario(ScenarioType type, string[] messages, EventData data)
        {
            if (type == ScenarioType.Select)
            {
                OnChoiceStart?.Invoke(data, messages);
            }
            else if (!StepScenario)
            {
                OnMessageUpdate?.Invoke(data, messages[0], 1f);
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
                while (isPaused) yield return null;

                currentText += stripped[messageCount];//一文字追加
                messageCount++;//現在の文字数
                progress = (float)messageCount / stripped.Length;

                if (progress == 1f)
                {
                    //最後まで読んだらタグ除去を解除する
                    currentText = currentMessages[0];
                    //選択式の場合は選択文も文字列に追加する
                    if (data.scenarioType == ScenarioType.Select)
                    {
                        currentText += "\r\n";
                        for (int i = 1; i < currentMessages.Length; i++)
                        {
                            currentText += $"{i}:{currentMessages[i]} ";
                        }
                    }
                }

                OnMessageUpdate?.Invoke(data, currentText, progress);

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
#if !ENABLE_MOONSHARP
            Debug.Assert(false, "Plugin [MoonSharp] is disabled. set symbol `ENABLE_MoonSharp` to scripting define symbols on Player Settings.. > Other settings > Scripting Define Symbols");
#else
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
#endif
        }

        public bool StartScenario(EventData data)
        {
            if (isRunning) return false;

            isMonoRunning = true;
            currentEventData = data;
            OnMessageStart?.Invoke(currentEventData);
            return true;
        }
        public void StopScenario(EventData data)
        {
            //currentEventData.scenarioType = ScenarioType.None;
            data.Stop();
            isMonoRunning = false;
            OnMessageEnd?.Invoke(currentEventData);
        }

        #endregion


        /// <summary>
        /// ユーザーイベント、クリックや選択が発生
        /// </summary>
        /// <param name="choice"></param>
        public void ScenarioSelect(ScenarioChoice choice)
        {
            //Debug.Log("ScenarioSelect" + choice.ToString());
            if (isRunning && currentEventData.scenarioType != ScenarioType.None)
            {
                if (StepScenario && isStepRunning)
                {
                    skipStep = true;
                }
                else
                {
                    scenarioChoice = choice;
#if ENABLE_MOONSHARP
                    coroutine?.Coroutine.Resume((int)scenarioChoice);
#endif
                    OnUserInput?.Invoke(currentEventData, scenarioChoice);
                }
            }
            else if (currentEventData.scenarioType != ScenarioType.None)
            {
                currentEventData.Stop();
#if ENABLE_MOONSHARP
                coroutine = null;
#endif
                OnMessageEnd?.Invoke(currentEventData);
            }
        }

        private string LoadScenario(LuaTextAsset luaTextAsset)
        {
            return luaTextAsset.text;
        }
    }
}