using UnityEngine;

namespace UniMoonDialogue.Example
{
    public class PlayScenarioLua : MonoBehaviour
    {
        [SerializeField] private LuaTextAsset luaScript = null;

        public void StartScenario()
        {
            ScenarioEngine.Instance.OnMessageStart += OnMessageStart;
            ScenarioEngine.Instance.OnMessageEnd += OnMessageEnd;
            ScenarioEngine.Instance.StartScenario(luaScript, gameObject);
        }

        private void OnMessageStart(ScenarioEngine.EventData data)
        {
            if (data.gameObject != gameObject) return;

            ExampleCommon.RotateByForce(gameObject, true);
        }

        private void OnMessageEnd(ScenarioEngine.EventData data)
        {
            if (data.gameObject != gameObject) return;

            ScenarioEngine.Instance.OnMessageStart -= OnMessageStart;
            ScenarioEngine.Instance.OnMessageEnd -= OnMessageEnd;

            ExampleCommon.RotateByForce(gameObject, false);
        }
    }
}