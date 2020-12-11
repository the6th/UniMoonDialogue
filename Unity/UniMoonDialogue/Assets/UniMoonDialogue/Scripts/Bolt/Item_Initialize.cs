#if ENABLE_BOLT
using Bolt;
using Ludiq;
using System.Collections.Generic;

namespace UniMoonDialogue
{
    [TypeIcon(typeof(Bolt.Add<int>))]
    public class Item_Initialize : AbstractInventryUnit
    {
        [DoNotSerialize] public ValueInput items { private set; get; }
        protected override void Definition()
        {
            items = ValueInput<List<Item>>("Items");
            base.Definition();
        }

        protected override ControlOutput Enter(Flow flow)
        {
            var _items = flow.GetValue<List<Item>>(items);

            base.Enter(flow);

            InventryEngine.Instance.AllItems = _items;

            return CO_finished;

        }
    }
}
#endif