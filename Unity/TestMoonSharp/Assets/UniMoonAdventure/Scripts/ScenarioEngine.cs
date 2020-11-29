using MoonSharp.Interpreter;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UniMoonAdventure
{
    public class ScenarioEngine : SingletonMonoBehaviour<ScenarioEngine>
    {
        [MoonSharp.Interpreter.MoonSharpUserData]
        public class LuaEventData
        {
            /// <summary>
            /// 画面にセリフを表示
            /// </summary>
            /// <param name="mes"></param>
            public void msg(string mes)
            {
                ScenarioEngine.Instance.UpdateScenario(ScenarioType.TapToNext, new string[] { mes });
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
                ScenarioEngine.Instance.UpdateScenario(ScenarioType.Select, message);
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
            SELECT_5,
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

        [SerializeField]
        private LuaTextAsset luaScript;

        [SerializeField]
        private bool AutoStart = false;
        [SerializeField]
        private bool StepScenario = true;
        [SerializeField]
        private float stepSpeed = 0.1f;

        private ScenarioChoice scenarioChoice = ScenarioChoice.None;
        private DynValue coroutine;
        private string[] currentMessages;
        private string currentText;

        #region Public Method
        /// <summary>
        /// メッセージ表示中か？
        /// </summary>
        public bool isRunning => coroutine?.Coroutine.State != CoroutineState.Dead;
        //Step表示実行中か？
        private bool isStepping = false;
        //Step表示を中止するか？（早送り表示)
        public bool skipStep { private set; get; } = false;

        public UnityAction<ScenarioType, string, float> OnMessageUpdate;
        public UnityAction OnMessageStart;
        public UnityAction OnMessageEnd;
        public UnityAction<string[]> OnChoiceStart;



        private void UpdateScenario(ScenarioType type, string[] messageArray)
        {
            scenarioType = type;
            if(type == ScenarioType.Select)
            {
                OnChoiceStart?.Invoke(messageArray);
            }
            else if (!StepScenario)
            {
                currentText = messageArray[0];
                OnMessageUpdate?.Invoke(scenarioType, currentText, 1f);
            }
            else
            {
                currentMessages = messageArray;
                StartCoroutine("StepMessage");
            }
        }

        IEnumerator StepMessage()
        {
            int messageCount = 0; //現在表示中の文字数
            currentText = "";
            isStepping = true;
            float progress = 0f;
            while (currentMessages[0].Length > messageCount)//文字をすべて表示していない場合ループ
            {
                currentText += currentMessages[0][messageCount];//一文字追加
                messageCount++;//現在の文字数
                progress = (float)messageCount / currentMessages[0].Length;

                if (scenarioType == ScenarioType.Select)
                {
                    if (progress == 1f)
                    {
                        currentText += "\r\n";
                        for (int i = 1; i < currentMessages.Length; i++)
                        {
                            currentText += $"{i}:{currentMessages[i]} ";
                        }
                    }
                }

                OnMessageUpdate?.Invoke(scenarioType, currentText, progress);

                if (skipStep)
                    yield return new WaitForSeconds(0);//任意の時間待つ
                else
                    yield return new WaitForSeconds(stepSpeed);//任意の時間待つ
            }
            isStepping = false;
            skipStep = false;
        }

        public void StartScenario(LuaTextAsset lua,bool forceRefresh = false)
        {
            if (coroutine != null ) return;

            UserData.RegisterAssembly(typeof(LuaEventData).Assembly);
            Script script = new Script();
            script.Globals["scene"] = new LuaEventData();
          
            //var scenario = LoadScenario();
            var scenario = LoadScenario(lua);

            DynValue function = script.DoString(scenario);
            coroutine = script.CreateCoroutine(function);
            OnMessageStart?.Invoke();

            coroutine.Coroutine.Resume();

        }

        #endregion


        private void Start()
        {
            if (AutoStart) StartScenario(luaScript);
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
                if (StepScenario && isStepping)
                {
                    skipStep = true;
                }
                else
                {
                    scenarioChoice = choice;
                    coroutine.Coroutine.Resume((int)scenarioChoice);
                }
            }
            else if (scenarioType != ScenarioType.None)
            {
                scenarioType = ScenarioType.None;
                coroutine = null;
                OnMessageEnd?.Invoke();
            }

        }

        private string LoadScenario(LuaTextAsset luaTextAsset)
        {
            return luaScript.text;
        }

        private string LoadScenario()
        {
            string code =
            @"
            return function()
 
            event.serif( 'やあ。ウチは[word:15000]から来た[name:10103]やで' )
            coroutine.yield()
 
            event.serif( '自分はどっからきたん？' )
            coroutine.yield()
 
            event.select( 'どこから？', 'ここが地元', '別の島', '[word:15000]', 'わからない' )
            local selected = coroutine.yield()
            if selected == 0 then
 
                event.serif( 'へえ。じゃあウチよりこの辺には詳しそうやね' )
                coroutine.yield()
 
            elseif selected == 1 then
 
                event.serif( 'この島へは何しにきたんやろなあ' )
                coroutine.yield()
 
            elseif selected == 2 then
 
                event.serif( '同郷やね。でもそんな風には見えへんなあ？' )
                coroutine.yield()
 
            elseif selected == 3 then
 
                event.serif( 'どういうこっちゃ' )
                coroutine.yield()
 
            end
            event.serif( 'おしまい' )
 
            end
        ";
            return code;

        }

    }
}