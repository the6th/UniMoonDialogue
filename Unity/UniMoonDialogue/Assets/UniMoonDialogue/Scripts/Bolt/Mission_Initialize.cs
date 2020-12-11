#if ENABLE_BOLT
using Bolt;
using Ludiq;
using System.Collections.Generic;

namespace UniMoonDialogue
{
    [TypeIcon(typeof(Bolt.Add<int>))]
    public class Mission_Initialize : AbstractInventryUnit
    {
        [DoNotSerialize] public ValueInput missions { private set; get; }
        protected override void Definition()
        {
            missions = ValueInput<List<Mission>>("Items");
            base.Definition();
        }

        protected override ControlOutput Enter(Flow flow)
        {
            var _missions = flow.GetValue<List<Mission>>(missions);
            base.Enter(flow);
            InventryEngine.Instance.AllMissions = _missions;
            return CO_finished;
        }
    }
}
#endif