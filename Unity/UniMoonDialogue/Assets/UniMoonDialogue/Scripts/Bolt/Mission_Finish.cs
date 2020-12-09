#if ENABLE_Bolt
using Bolt;
using Ludiq;
using UniMoonDialogue.Inventry;

namespace UniMoonDialogue
{
    [TypeIcon(typeof(Bolt.Subtract<int>))]

    public class Mission_Finish : AbstractInventryUnit
    {
        [DoNotSerialize] public ControlOutput CO_failed { get; private set; }

        [DoNotSerialize] public ValueInput item { private set; get; }
        [DoNotSerialize] public ValueOutput status { private set; get; }

        private ControlOutput OnFailed => CO_failed;
        private InventryEngine.ItemStoreResult resultStatus;
        private InventryEngine.ItemStoreResult ReturnResultState(Flow flow) => resultStatus;

        protected override void Definition()
        {
            item = ValueInput<Mission>("Mission", null);
            status = ValueOutput<InventryEngine.ItemStoreResult>("Result", ReturnResultState);
            base.Definition();
            CO_failed = ControlOutput("failed");
        }

        protected override ControlOutput Enter(Flow flow)
        {
            var _item = flow.GetValue<Mission>(item);
            base.Enter(flow);

            var _result = InventryEngine.Instance.AddMission(_item, out resultStatus);

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
