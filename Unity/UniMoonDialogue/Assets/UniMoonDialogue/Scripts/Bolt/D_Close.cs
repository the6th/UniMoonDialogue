#if ENABLE_BOLT
using Bolt;
using UniMoonDialogue;

namespace UniMoonDialogue
{
    public class D_Close : AbstractDialogueUnit
    {
        protected override void Definition()
        {
            base.Definition();
        }

        protected override ControlOutput Enter(Flow flow)
        {
            base.Enter(flow);
            ScenarioEngine.Instance.StopScenario(ScenarioEngine.Instance.currentEventData);
            return CO_finished;
        }
    }
}
#endif

