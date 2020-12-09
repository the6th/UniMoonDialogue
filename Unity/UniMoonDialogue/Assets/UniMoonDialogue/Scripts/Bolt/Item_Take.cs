using Bolt;
using Ludiq;
using UniMoonDialogue.Inventry;
using UnityEngine;

namespace UniMoonDialogue
{
    public class Item_Take : AbstractInventryUnit
    {
        [DoNotSerialize] public ControlOutput CO_failed { get; private set; }

        [DoNotSerialize] public ValueInput item { private set; get; }
        [DoNotSerialize] public ValueInput ammount { private set; get; }
        [DoNotSerialize] public ValueOutput status { private set; get; }

        private ControlOutput OnFailed => CO_failed;
        private InventryEngine.ItemStoreResult resultStatus;
        private InventryEngine.ItemStoreResult ReturnResultState(Flow flow) => resultStatus;

        protected override void Definition()
        {
            item = ValueInput<Inv_Item>("Item", null);
            ammount = ValueInput<int>("ammount", 1);
            status = ValueOutput<InventryEngine.ItemStoreResult>("Result", ReturnResultState);
            base.Definition();
            CO_failed = ControlOutput("failed");
        }

        protected override ControlOutput Enter(Flow flow)
        {
            var _item = flow.GetValue<Inv_Item>(item);
            var _ammount = flow.GetValue<int>(ammount);
            base.Enter(flow);

            var _result = InventryEngine.Instance.AddItem(_item.name, out resultStatus, _ammount);

            if (_result)
                return CO_finished;
            else
                return OnFailed;
        }

    }

    [Descriptor(typeof(Item_Take))]
    public class Item_TakeDescriptor : UnitDescriptor<Item_Take>
    {
        public Item_TakeDescriptor(Item_Take unit) : base(unit) { }

        protected override EditorTexture DefinedIcon()
        {
            var texture = Resources.Load("BoltIcons/Inv_Item") as Texture2D;

            return EditorTexture.Single(texture);
        }
    }
}
