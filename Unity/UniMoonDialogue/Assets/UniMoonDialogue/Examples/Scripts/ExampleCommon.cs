using Bolt;
using UnityEngine;

namespace UniMoonDialogue
{
    [UnitCategory("UniMoonDialogue")]

    public class ExampleCommon
    {
        public static void RotateByForce(GameObject gameObject, bool enable)
        {
            if (gameObject.TryGetComponent<ConstantForce>(out var force))
                force.torque = new Vector3(0, (enable ? 1f : 0f), 0);
        }
    }
}