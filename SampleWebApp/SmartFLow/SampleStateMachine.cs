using SampleWebApp.SmartFLow.Activities;
using SmartFlow.Builders;
using SmartFlow.Models.Flow;

namespace SampleWebApp.Flows
{
    public class SampleStateMachine : ISmartFlow
    {
        public string FlowKey { get; set; } = "Sample";

        public ISmartFlowBuilder Construct<T>(ISmartFlowBuilder builder) where T : ISmartFlow, new()
        {
            var first = new State(1, "First");

            builder.Initial()
                    .From(first)
                    .Allow(new State(2, "Second"), 1)
                    .Allow(new State(3, "Third"), 2)
                    .OnEntry<HelloWorld>()
                    .OnExit<GoodbyeWorld>()
                .NewStep().From(new State(2, "Second"))
                    .Allow(new State(4, "Fourth"), 3)
                    .Allow(first, 1)
                    .OnExit<GoodbyeWorld>();
                
            return builder;
        }
    }
}
