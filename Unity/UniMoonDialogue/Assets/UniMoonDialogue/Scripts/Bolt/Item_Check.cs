#if ENABLE_BOLT
using Bolt;
using Ludiq;

namespace UniMoonDialogue
{

    [TypeIcon(typeof(Expose))]

    public class Item_Check : AbstractInventryUnit
    {
        [DoNotSerialize] public ControlOutput CO_NoItems { get; private set; }
        [DoNotSerialize] public ValueInput item { private set; get; }
        [DoNotSerialize] public ValueOutput ammount { private set; get; }

        private int numOfItems;
        private int ReturnAmmount(Flow flow) => numOfItems;

        protected override void Definition()
        {
            item = ValueInput<Item>("Item", null);
            ammount = ValueOutput<int>("ammount", ReturnAmmount);

            base.Definition();
            CO_NoItems = ControlOutput("noitem");
        }

        protected override ControlOutput Enter(Flow flow)
        {
            var _item = flow.GetValue<Item>(item);
            base.Enter(flow);

            numOfItems = InventryEngine.Instance.GetAmmountMyItem(_item);

            if (numOfItems < 1)
                return CO_NoItems;
            else
                return CO_finished;
        }
    }
}
#endif
