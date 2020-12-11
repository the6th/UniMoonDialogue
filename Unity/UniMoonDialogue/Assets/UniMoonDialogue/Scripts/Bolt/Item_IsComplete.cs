#if ENABLE_BOLT
using Bolt;
using Ludiq;
using System.Collections.Generic;

namespace UniMoonDialogue
{
    [TypeIcon(typeof(Expose))]
    public class Item_IsComplete : AbstractInventryUnit
    {
        [DoNotSerialize] public ValueInput items { private set; get; }
        [DoNotSerialize] public ValueOutput isComplete { private set; get; }

        private bool result;
        private bool ReturnResult(Flow flow) => result;
        protected override void Definition()
        {
            items = ValueInput<List<Item>>("Items");
            isComplete = ValueOutput<bool>("isComplete", ReturnResult);
            base.Definition();
        }

        protected override ControlOutput Enter(Flow flow)
        {
            var _items = flow.GetValue<List<Item>>(items);

            base.Enter(flow);

            result = InventryEngine.Instance.IsCompliteItems(_items);

            return CO_finished;

        }
    }
}
#endif