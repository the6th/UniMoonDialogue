using UnityEngine;

namespace UniMoonAdventure.Example
{
    public class ClickToScenario : MonoBehaviour
    {
        [SerializeField]
        LuaTextAsset luaScript;

        public void StartScenario()
        {
            //Debug.Log("StartScenario");
            ScenarioEngine.Instance.StartScenario(luaScript);
        }
    }
}