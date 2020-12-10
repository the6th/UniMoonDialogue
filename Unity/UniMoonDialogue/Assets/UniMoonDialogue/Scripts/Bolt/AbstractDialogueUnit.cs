#if ENABLE_BOLT
using Bolt;
using Ludiq;
using UnityEngine;

namespace UniMoonDialogue
{
    [UnitCategory("UniMoonDialogue")]

    [TypeIcon(typeof(TextAsset))]
    public class AbstractDialogueUnit : Unit
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
            Debug.Assert(ScenarioEngine.Instance != null, "[ScenarioCanvas.prefab]をシーンに追加してください");

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