using SampleWebApp.Activities;
using SmartFlow.Core.Builders;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;

namespace SampleWebApp.Flows
{
    public class SampleStateMachine : ISmartFlow
    {
        public Guid Id { get; set; }
        public string FlowKey { get; set; } = "Test";
        public List<Transition> Transitions { get; set; }

        public ISmartFlow Construct<T>(ISmartFlowBuilder builder) where T : ISmartFlow, new()
        {
            builder.Initial()
                .From(new State(1, "First"))
                .Allow(new State(2, "Second"), new ProcessAction(1))
                .Allow(new State(3, "Third"), new ProcessAction(2))
                .OnEntry<HelloWorld>()
                //.NewStep().From(new State(2, "Second"))
                //  .Allow(new State(4, "Fourth"), new ProcessAction(3))
                //  .Allow(new State(5, "Fifth"), new ProcessAction(4))
                //  .OnEntry<HelloWorld>()
                //  .OnExit<GoodbyeWorld>();
                ;

            return this;
        }
    }
}
