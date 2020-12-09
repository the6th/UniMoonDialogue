#if ENABLE_Bolt
using Bolt;
using Ludiq;
using System.Collections;
using UniMoonDialogue.Inventry;
using UnityEngine;

namespace UniMoonDialogue
{
    [UnitCategory("UniMoonDialogue")]
    public class AbstractInventryUnit : Unit
    {

        [DoNotSerialize] public ControlInput CI_input { get; private set; }
        [DoNotSerialize] public ControlOutput CO_finished { get; private set; }


        protected ControlOutput onFinished => CO_finished;
        protected GraphReference reference;
        protected Transform m_transform;

        protected override void Definition()
        {
            CI_input = ControlInput("", Enter);
            CO_finished = ControlOutput("");
            Succession(CI_input, CO_finished);
        }

        protected virtual ControlOutput Enter(Flow flow)
        {
            Debug.Assert(InventryEngine.Instance != null, "[InventryEngine]をシーンに追加してください");

            reference = flow.stack.ToReference();

            return null;
        }

        protected virtual void Finished()
        {
            Flow.New(reference).Run(onFinished);
        }
    }
}
#endif
