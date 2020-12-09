#if ENABLE_Bolt
using Bolt;
using Ludiq;
using System.Collections.Generic;
using UniMoonDialogue;
using UnityEngine;
using static UniMoonDialogue.ScenarioEngine;

namespace UniMoonDialogue
{
    public class D_Messages : AbstractDialogueUnit
    {
        [DoNotSerialize] public ValueInput message { private set; get; }

        private List<string> messageList;
        private int index = 0;

        protected override void Definition()
        {
            message = ValueInput<List<string>>("msgs", null);
            base.Definition();
        }

        protected override ControlOutput Enter(Flow flow)
        {
            index = 0;
            messageList = new List<string>();
            messageList = flow.GetValue<List<string>>(message);

            base.Enter(flow);

            ScenarioEngine.Instance.OnUserInput += OnUserInput;
            ScenarioEngine.Instance.currentEventData.msg(messageList[index]);

            return null;
        }

        private void OnUserInput(EventData data, ScenarioChoice choice)
        {
            index++;
            if (messageList.Count > index)
                data.msg(messageList[index]);
            else
            {
                Flow.New(reference).Run(onFinished);
                ScenarioEngine.Instance.OnUserInput -= OnUserInput;
            }
        }
    }
}
#endif
