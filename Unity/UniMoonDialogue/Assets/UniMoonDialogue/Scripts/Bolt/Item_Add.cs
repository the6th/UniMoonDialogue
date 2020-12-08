using Bolt;
using Ludiq;
using System.Collections.Generic;
using UniMoonDialogue.Inventry;
using UnityEngine;

namespace UniMoonDialogue
{
    [TypeIcon(typeof(ScriptableObject))]

    public class Item_Add : AbstractInventryUnit
    {
        [DoNotSerialize] public ValueInput item { private set; get; }
        [DoNotSerialize] public ValueOutput result { private set; get; }
        [DoNotSerialize] public ValueOutput status { private set; get; }

        protected override void Definition()
        {
            item = ValueInput<Inv_Item>("item");
            result = ValueOutput<bool>("success");
            status = ValueOutput<InventryEngine.ItemStoreResult>("result");


            base.Definition();
        }

        protected override ControlOutput Enter(Flow flow)
        {
            var _item = flow.GetValue<Inv_Item>(item);
            base.Enter(flow);

            var _result = InventryEngine.Instance.GetItem(_item.name, out var result);

        

            return CO_finished;
        }


    }
}
