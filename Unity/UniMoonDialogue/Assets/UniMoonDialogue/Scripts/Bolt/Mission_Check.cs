﻿#if ENABLE_BOLT
using Bolt;
using Ludiq;

namespace UniMoonDialogue
{
    [TypeIcon(typeof(Expose))]
    public class Mission_Check : AbstractInventryUnit
    {
        [DoNotSerialize] public ControlOutput CO_NoItems { get; private set; }
        [DoNotSerialize] public ValueInput item { private set; get; }

        private bool hasMission;
        private ControlOutput OnFailed => CO_NoItems;

        protected override void Definition()
        {
            item = ValueInput<Mission>("Mission", null);

            base.Definition();
            CO_NoItems = ControlOutput("NoMission");
        }

        protected override ControlOutput Enter(Flow flow)
        {
            var _item = flow.GetValue<Mission>(item);
            base.Enter(flow);

            hasMission = InventryEngine.Instance.IsMyMission(_item);

            if (hasMission)
                return CO_finished;
            else
                return OnFailed;
        }
    }
}
#endif
