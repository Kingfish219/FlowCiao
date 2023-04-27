using SampleWebApp.Activities;
using SmartFlow.Builders;
using SmartFlow.Interfaces;
using SmartFlow.Models.Flow;

namespace SampleWebApp.Flows
{
    public class SampleStateMachine : ISmartFlow
    {
        public string FlowKey { get; set; } = "Sample";

        public ISmartFlowBuilder Construct<T>(ISmartFlowBuilder builder) where T : ISmartFlow, new()
        {
            builder.Initial()
                    .From(new State(1, "First"))
                    .Allow(new State(2, "Second"), new ProcessAction(1))
                    .Allow(new State(3, "Third"), new ProcessAction(2))
                    .OnEntry<HelloWorld>()
                .NewStep().From(new State(2, "Second"))
                    .Allow(new State(4, "Fourth"), new ProcessAction(3))
                    .OnExit<GoodbyeWorld>();
                ;

            return builder;
        }
    }
}
