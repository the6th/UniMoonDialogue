#if ENABLE_Bolt
using Bolt;
using Ludiq;
using UniMoonDialogue;
using UnityEngine;
using static UniMoonDialogue.ScenarioEngine;

namespace UniMoonDialogue
{
    public class D_LoadLua : AbstractDialogueUnit
    {
        [DoNotSerialize] public ControlOutput CO_failed { get; private set; }
        [DoNotSerialize] public ControlOutput CO_ScriptEnd { get; private set; }
        [DoNotSerialize] public ControlOutput CO_UserInput { get; private set; }
        [DoNotSerialize] public ValueInput luaAsset { get; private set; }
        [NullMeansSelf] public ValueInput transform { private set; get; }

        private ControlOutput OnFailed => CO_failed;
        private ControlOutput OnScriptEnd => CO_ScriptEnd;
        private ControlOutput OnInput => CO_UserInput;


        protected override void Definition()
        {
            transform = ValueInput<Transform>("owner", null);
            luaAsset = ValueInput<string>("Script", "LuaSampleText");

            base.Definition();
            CO_ScriptEnd = ControlOutput("ScriptEnd");
            CO_UserInput = ControlOutput("UserClicked");
            CO_failed = ControlOutput("failed");
        }

        protected override ControlOutput Enter(Flow flow)
        {
            base.Enter(flow);
            if (ScenarioEngine.Instance.isRunning)
            {
                Debug.Log("Scenario already running by " + ScenarioEngine.Instance.currentEventData.gameObject.name);
                return OnFailed;
            }
            else
            {
                reference = flow.stack.ToReference();
                m_transform = flow.GetValue<Transform>(transform);
                var m_lua = flow.GetValue<string>(luaAsset);

                var lua = Resources.Load(m_lua) as TextAsset;
                if (lua == null)
                {
                    Debug.Log($"{m_lua}が見つからないか、LuaScriptとして実行できません。");
                    return OnFailed;
                }

                ScenarioEngine.Instance.StartScenario((LuaTextAsset)lua, m_transform.gameObject);

                ScenarioEngine.Instance.OnMessageEnd += OnMessageEnd;
                ScenarioEngine.Instance.OnUserInput += OnUserInput;
                return CO_finished;
            }
        }

        private void OnUserInput(EventData data, ScenarioChoice choice)
        {
            Flow.New(reference).Run(OnInput);
        }

        private void OnMessageEnd(EventData data)
        {
            ScenarioEngine.Instance.OnMessageEnd -= OnMessageEnd;
            ScenarioEngine.Instance.OnUserInput -= OnUserInput;
            Flow.New(reference).Run(OnScriptEnd);
        }
    }
}
#endif
