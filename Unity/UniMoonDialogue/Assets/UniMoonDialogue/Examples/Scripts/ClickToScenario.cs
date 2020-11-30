using UnityEngine;

namespace UniMoonDialogue.Example
{
    public class ClickToScenario : MonoBehaviour
    {
        [SerializeField] private LuaTextAsset luaScript = null;

        public void StartScenario()
        {
            //Debug.Log("StartScenario");
            ScenarioEngine.Instance.StartScenario(luaScript);
        }
    }
}