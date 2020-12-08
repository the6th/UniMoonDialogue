using UnityEngine;

namespace UniMoonDialogue
{
    public class LuaScenarioLoader : MonoBehaviour
    {
        [SerializeField] private LuaTextAsset luaScript = null;
        [SerializeField] private bool AutoStart = false;
        [SerializeField] private string DisplayName ="John Doe";
        // Start is called before the first frame update
        void Start()
        {
            if (AutoStart)
                ScenarioEngine.Instance.StartScenario(luaScript, gameObject, DisplayName);
        }
    }
}
