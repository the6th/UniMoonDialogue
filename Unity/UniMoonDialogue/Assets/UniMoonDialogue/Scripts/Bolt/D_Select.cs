#if ENABLE_BOLT
using Bolt;
using Ludiq;
using System.Collections.Generic;
using static UniMoonDialogue.ScenarioEngine;

namespace UniMoonDialogue
{

    public class D_Select : AbstractDialogueUnit
    {
        [DoNotSerialize] public ValueInput message { private set; get; }
        [DoNotSerialize] public ValueInput answers { private set; get; }
        [DoNotSerialize] public ValueOutput answer { private set; get; }

        private List<string> answerList;
        private int answerID;
        private int ReturnSelected(Flow flow) => answerID;

        protected override void Definition()
        {
            message = ValueInput<string>("msg", "");
            answers = ValueInput<List<string>>("ans", null);
            answer = ValueOutput<int>("ans", ReturnSelected);

            base.Definition();
        }


        protected override ControlOutput Enter(Flow flow)
        {
            answerList = new List<string>();
            var msg = flow.GetValue<string>(message);
            answerList = flow.GetValue<List<string>>(answers);

            base.Enter(flow);

            ScenarioEngine.Instance.OnUserInput += OnUserInput;
            ScenarioEngine.Instance.currentEventData.choice(msg, answerList.ToArray());

            return null;
        }

        private void OnUserInput(EventData data, ScenarioChoice choice)
        {
            //選択肢以外が届いたら、無視する
            if ((int)choice <= 0) return;

            answerID = (int)choice - 1;
            Flow.New(reference).Run(onFinished);
            ScenarioEngine.Instance.OnUserInput -= OnUserInput;
        }
    }
}
#endif