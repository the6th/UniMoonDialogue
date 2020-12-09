#if ENABLE_Bolt
using Bolt;
using Ludiq;
using UniMoonDialogue.Inventry;
using UnityEngine;

namespace UniMoonDialogue
{
    [TypeIcon(typeof(Bolt.Add<int>))]

    public class Mission_Start : AbstractInventryUnit
    {
        [DoNotSerialize] public ControlOutput CO_failed { get; private set; }

        [DoNotSerialize] public ValueInput item { private set; get; }
        [DoNotSerialize] public ValueOutput status { private set; get; }

        private ControlOutput OnFailed => CO_failed;
        private InventryEngine.ItemStoreResult resultStatus;
        private InventryEngine.ItemStoreResult ReturnResultState(Flow flow) => resultStatus;

        protected override void Definition()
        {
            item = ValueInput<Mission>("Mission",null);
            
            status = ValueOutput<InventryEngine.ItemStoreResult>("Result", ReturnResultState);
            base.Definition();
            CO_failed = ControlOutput("failed");
        }

        protected override ControlOutput Enter(Flow flow)
        {
            var _item = flow.GetValue<Mission>(item);

            base.Enter(flow);
            if (_item == null) return OnFailed;

            var _result = InventryEngine.Instance.AddMission(_item, out resultStatus);

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

