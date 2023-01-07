using SampleWebApp.Activities;
using SmartFlow.Core.Builders;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;

namespace SampleWebApp.Flows
{
    public class SampleStateMachine : IStateMachine
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public List<Transition> Transitions { get; set; }

        public IStateMachine Construct<T>(IStateMachineBuilder builder) where T : IStateMachine, new()
        {
            builder.NewStep()
                      .From(new State(1, "First"))
                      .Allow(new State(2, "Second"), new ProcessAction(1))
                      .Allow(new State(3, "Third"), new ProcessAction(2))
                      .OnEntry<HelloWorld>()
                    .NewStep().From(new State(2, "Second"))
                      .Allow(new State(4, "Fourth"), new ProcessAction(3))
                      .Allow(new State(5, "Fifth"), new ProcessAction(4))
                      .OnEntry<HelloWorld>()
                      .OnExit<GoodbyeWorld>();

            return this;
        }
    }
}
