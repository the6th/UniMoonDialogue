using Bolt;
using Ludiq;

namespace UniMoonDialogue
{

    [UnitCategory("UniMoonDialogue")]

    public class TestUnit : Unit
    {
        [DoNotSerialize]
        public ControlInput input { get; private set; }
        [DoNotSerialize]
        public ControlOutput output { get; private set; }
        [DoNotSerialize]
        public ValueInput valueIn { get; private set; }
        [DoNotSerialize]
        public ValueOutput valueOut { get; private set; }

        protected override void Definition()
        {
            input = ControlInput("in", Enter);
            output = ControlOutput("output");

            valueIn = ValueInput<float>("valueIn");
            valueOut = ValueOutput<float>("valueOut", ReturnFloat);

            Requirement(valueIn, valueOut);
            Succession(input, output);
        }

        public ControlOutput Enter(Flow flow)
        {
            return output;
        }

        public float ReturnFloat(Flow flow)
        {
            return flow.GetValue<float>(valueIn);
        }

    }
}