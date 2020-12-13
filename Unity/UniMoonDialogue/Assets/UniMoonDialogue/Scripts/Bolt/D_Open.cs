#if ENABLE_BOLT
using Bolt;
using Ludiq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UniMoonDialogue.ScenarioEngine;

namespace UniMoonDialogue
{
    public class D_Open : AbstractDialogueUnit
    {
        [DoNotSerialize] public ControlOutput CO_failed { get; private set; }
        [NullMeansSelf] public ValueInput transform { private set; get; }
        private ControlOutput OnFailed => CO_failed;

        private List<EventData> eventData = new List<EventData>();
        private bool init = false;
        protected override void Definition()
        {
            transform = ValueInput<Transform>("owner", null);

            base.Definition();
            CO_failed = ControlOutput("failed");
        }

        protected override ControlOutput Enter(Flow flow)
        {
            base.Enter(flow);

            reference = flow.stack.ToReference();
            m_transform = flow.GetValue<Transform>(transform);

            if (m_transform == null) m_transform = reference.gameObject.transform;

            var data = new EventData(gameObject: m_transform.gameObject);
            if (!init)
            {
                ScenarioEngine.Instance.OnMessageStart += OnMessageStart;
                init = true;
            }
            eventData.Add(data);

            if (!ScenarioEngine.Instance.StartScenario(data))
            {
                eventData.Remove(data);
                return OnFailed;
            }
            return null;
        }

        private void OnMessageStart(EventData _event)
        {
            var _data = eventData.FirstOrDefault(_ => _.guid == _event.guid);

            if (_data != null)
            {
                eventData.Remove(_data);
                Flow.New(reference).Run(CO_finished);
            }
        }
    }
}
#endif
