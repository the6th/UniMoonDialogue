#if ENABLE_BOLT
using Bolt;
using Ludiq;
using UniMoonDialogue;
using UnityEngine;
using static UniMoonDialogue.ScenarioEngine;

namespace UniMoonDialogue
{
    public class D_Open : AbstractDialogueUnit
    {
        [DoNotSerialize] public ControlOutput CO_failed { get; private set; }
        [NullMeansSelf] public ValueInput transform { private set; get; }
        private ControlOutput OnFailed => CO_failed;

        protected override void Definition()
        {
            transform = ValueInput<Transform>("owner", null);

            base.Definition();
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

                if (m_transform == null) m_transform = reference.gameObject.transform;

                var data = new EventData(gameObject: m_transform.gameObject);
                ScenarioEngine.Instance.StartScenario(data);
                return CO_finished;
            }
        }
    }
}
#endif
