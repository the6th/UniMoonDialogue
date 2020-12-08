using Bolt;
using Ludiq;
using UniMoonDialogue;
using UnityEngine;
using static UniMoonDialogue.ScenarioEngine;

namespace UniMoonDialogue
{
    public class D_Message : AbstractDialogueUnit
    {
        [DoNotSerialize] public ValueInput message { private set; get; }

        protected override void Definition()
        {
            message = ValueInput<string>("msg", "");
            base.Definition();
        }

        protected override ControlOutput Enter(Flow flow)
        {
            var msg = flow.GetValue<string>(message);
            base.Enter(flow);

            ScenarioEngine.Instance.OnUserInput += OnUserInput;
            ScenarioEngine.Instance.currentEventData.msg(msg);

            return null;
        }

        private void OnUserInput(EventData data, ScenarioChoice choice)
        {
            Flow.New(reference).Run(onFinished);
            ScenarioEngine.Instance.OnUserInput -= OnUserInput;
        }
    }
}
