using UnityEngine;
using UnityEngine.UI;
using static UniMoonAdventure.ScenarioEngine;

namespace UniMoonAdventure
{
    public class ScenarioToCanvasText : MonoBehaviour
    {
        [SerializeField]
        Text messageText;

        [SerializeField]
        RectTransform NextUI;

        private ScenarioEngine engine = null;

        private void Awake()
        {
            engine = ScenarioEngine.Instance;
            engine.OnMessageUpdate += OnMessageUpdate;
            messageText.text = "";
        }

        private void OnMessageUpdate(ScenarioEngine.ScenarioType arg0, string arg1, float progress)
        {
            if (progress != 1f)
                NextUI?.gameObject.SetActive(false);
            else
                NextUI?.gameObject.SetActive(true);

            messageText.text = arg1;
        }

        private void Update()
        {
            CheckKey();
        }

        private void CheckKey()
        {
            if (engine.scenarioType == ScenarioType.None) return;

            if (Input.GetKeyDown(KeyCode.Alpha1)) engine.ScenarioSelect(ScenarioChoice.SELECT_1);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) engine.ScenarioSelect(ScenarioChoice.SELECT_2);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) engine.ScenarioSelect(ScenarioChoice.SELECT_3);
            else if (Input.GetKeyDown(KeyCode.Alpha4)) engine.ScenarioSelect(ScenarioChoice.SELECT_4);
            else if (Input.GetKeyDown(KeyCode.Alpha5)) engine.ScenarioSelect(ScenarioChoice.SELECT_5);
            else if (Input.GetMouseButtonDown(0)) engine.ScenarioSelect(ScenarioChoice.SKIP);
        }

    }
}