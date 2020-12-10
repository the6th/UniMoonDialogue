#if ENABLE_Bolt
using Bolt;
#endif
using UnityEngine;

namespace UniMoonDialogue
{
#if ENABLE_Bolt
    [UnitCategory("UniMoonDialogue")]
#endif

    public class ExampleCommon
    {
        public static void RotateByForce(GameObject gameObject, bool enable)
        {
            if (gameObject.TryGetComponent<ConstantForce>(out var force))
                force.torque = new Vector3(0, (enable ? 1f : 0f), 0);
        }
    }
}