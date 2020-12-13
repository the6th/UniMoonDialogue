using System.Collections.Generic;
using UnityEngine;

namespace UniMoonDialogue
{
    /// <summary>
    /// ScenarioCanvasで購読対象のGameObjectを指定するためのコンポーネント
    /// 購読対象としたいGameObjectに Observableコンポーネットを追加して、
    /// observablesに追加する
    /// </summary>
    public class ScenarioObserver : MonoBehaviour
    {
        public string Groupname => m_GroupName;
        [SerializeField] private string m_GroupName = "ObserverName";

        public ScenarioCanvas scenarioCanvas { private set; get; }

        public List<ScenarioObservable> observables = new List<ScenarioObservable>();

        private void Awake()
        {
            scenarioCanvas = GetComponent<ScenarioCanvas>();
        }
    }
}