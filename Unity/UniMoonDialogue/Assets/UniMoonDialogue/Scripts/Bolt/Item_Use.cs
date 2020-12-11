#if ENABLE_BOLT
using Bolt;
using Ludiq;

namespace UniMoonDialogue
{
    [TypeIcon(typeof(Bolt.Subtract<int>))]
    public class Item_Use : AbstractInventryUnit
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
            var _item = flow.GetValue<Item>(item);
            var _ammount = flow.GetValue<int>(ammount);
            base.Enter(flow);

            var _result = InventryEngine.Instance.PushMyItem(_item, out resultStatus, _ammount);

            if (_result)
                return CO_finished;
            else
                return OnFailed;
        }

    }

    //[Descriptor(typeof(Item_Take))]
    //public class Item_TakeDescriptor : UnitDescriptor<Item_Take>
    //{
    //    public Item_TakeDescriptor(Item_Take target) : base(target) { }
    //    private string iconname = "BoltIcons/Inv_Item";

    //    private Texture2D _icon;
    //    public Texture2D icon
    //    {
    //        get
    //        {
    //            if (_icon == null) _icon = Resources.Load(iconname) as Texture2D;
    //            return _icon;
    //        }
    //    }

    //    protected override EditorTexture DefinedIcon()
    //    {
    //        return EditorTexture.Single(icon);
    //    }

    //    protected override EditorTexture DefaultIcon()
    //    {
    //        return EditorTexture.Single(_icon);
    //    }
    //}
}
#endif
