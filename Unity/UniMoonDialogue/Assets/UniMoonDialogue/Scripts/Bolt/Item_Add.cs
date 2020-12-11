#if ENABLE_BOLT
using Bolt;
using Ludiq;
using UnityEngine;

namespace UniMoonDialogue
{
    [TypeIcon(typeof(Bolt.Add<int>))]

    public class Item_Add : AbstractInventryUnit
    {
        [DoNotSerialize] public ControlOutput CO_failed { get; private set; }

        [DoNotSerialize] public ValueInput item { private set; get; }
        [DoNotSerialize] public ValueInput ammount { private set; get; }
        [DoNotSerialize] public ValueOutput status { private set; get; }

        private ControlOutput OnFailed => CO_failed;
        private InventryEngine.InventyStatus resultStatus;
        private InventryEngine.InventyStatus ReturnResultState(Flow flow) => resultStatus;

        protected override void Definition()
        {
            item = ValueInput<Item>("Item", null);
            ammount = ValueInput<int>("ammount", 1);
            status = ValueOutput<InventryEngine.InventyStatus>("Result", ReturnResultState);
            base.Definition();
            CO_failed = ControlOutput("failed");
        }

        protected override ControlOutput Enter(Flow flow)
        {
            var _ammount = flow.GetValue<int>(ammount);
            var _item = flow.GetValue<Item>(item);

            base.Enter(flow);
            if (_item == null) return OnFailed;

            var _result = InventryEngine.Instance.AddItem(_item, out resultStatus, _ammount);

            Debug.Log($"アイテム追加{_item.name} :{_result}");
            if (_result)
                return CO_finished;
            else
                return OnFailed;
        }

    }

    //[Descriptor(typeof(Item_Add))]
    //public class Item_AddDescriptor : UnitDescriptor<Item_Add>
    //{
    //    public Item_AddDescriptor(Item_Add unit) : base(unit) { }

    //    protected override EditorTexture DefinedIcon()
    //    {
    //        var texture = Resources.Load("BoltIcons/Inv_Item") as Texture2D;

    //        return EditorTexture.Single(texture);
    //    }
    //}


}
#endif

