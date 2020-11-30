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
            if (data.gameObject != this) return;
            Debug.Log("OnMessageStart" + data.displayName);
        }

        private void OnMessageEnd(ScenarioEngine.EventData data)
        {
            if (data.gameObject != this) return;
            Debug.Log("OnMessageEnd" + data.displayName);
            ScenarioEngine.Instance.OnMessageStart -= OnMessageStart;
            ScenarioEngine.Instance.OnMessageEnd -= OnMessageEnd;
        }
    }
}