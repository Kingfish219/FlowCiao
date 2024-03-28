# FlowCiao

<div align="left">
    <br />
    <img src="src\FlowCiao\Resources\FlowCiao.png" alt="Logo" width="80">
    <br />
    <br />
</div>

FlowCiao is a A lightweight .NET state machine workflow. It is a user-friendly and extensible .NET state machine library that simplifies the creation and management of state machines in your applications. With FlowCiao, you can effortlessly define and control the flow of your application's logic, making it an ideal choice for managing complex workflows and state-driven processes. Start building robust and efficient state machines with ease.

## Simplicity

Define your state machine using an Easy-to-use and Fluent Api

```csharp
phone.For(idle)
    .Allow(ringing, (int)Actions.Ring)
    .Allow(busy, (int)Actions.Call)
    .OnEntry<HelloWorld>()
    .OnExit<GoodbyeWorld>();
```

## Getting Started

To get started with FlowCiao, follow these steps:

1. Install FlowCiao via NuGet Package Manager.
2. Create your custom state machine by implementing the `IFlow` interface.
3. Define your states, transitions, and actions using the fluent API provided by FlowCiao.
4. Start managing states effortlessly in your application.

## Example

Here's a simple example of creating a state machine for implementing a simple **Phone** with FlowCiao:

```csharp
public class PhoneStateMachine : IFlow
    {
        public string FlowKey { get; set; } = "phone";

        public enum Actions
        {
            Ring = 1,
            Call = 2,
            Pickup = 3,
            Hangup = 4,
            PowerOff = 5
        }

        public IFlowBuilder Construct<T>(IFlowBuilder builder) where T :  IFlow, new()
        {
            var idle = new State(1, "idle");
            var ringing = new State(2, "ringing");
            var busy = new State(3, "busy");
            var offline = new State(4, "offline");

            builder
                .Initial(stepBuilder =>
                {
                    stepBuilder
                        .For(idle)
                        .Allow(ringing, (int)Actions.Ring)
                        .Allow(busy, (int)Actions.Call)
                        .Allow(offline, (int)Actions.PowerOff)
                        .OnEntry<HelloWorld>()
                        .OnExit<GoodbyeWorld>();
                })
                .NewStep(stepBuilder =>
                {
                    stepBuilder
                        .For(ringing)
                        .Allow(offline, (int)Actions.PowerOff)
                        .Allow(idle, (int)Actions.Hangup)
                        .Allow(busy, (int)Actions.Pickup)
                        .OnExit<GoodbyeWorld>();
                });

            return builder;
        }
    }
```

Add FlowCiao to your project:

```csharp
builder.Services.AddFlowCiao();
```

Build your state machine and Fire it with the right Actions:

```csharp
var stateMachineBuilder = app.Services.GetService<IFlowBuilder>();
stateMachineBuilder?.Build<PhoneStateMachine>();
var defaultFlowOperator = app.Services.GetService<IFlowOperator>();
var result = defaultFlowOperator?.Fire("phone", 1);
```

And that's it!

## And also Persistency if required

If there is a need to persist your state machines for long-running processes, you can connect FlowCiao to Sql Server (and other providers very soon!).
Otherwise, as long as your application is running, We track the status and data of all of your running state machines. So no wories:)

```csharp
builder.Services.AddFlowCiao(settings =>
{
    settings
        .Persist()
            .UseSqlServer(configuration.GetConnectionString("FlowCiao"));
});
```

## Features

- **Simple & Fast Development**: Speed up your workflow development with a user-friendly fluent API.
- **Action-Packed Transitions**: Trigger actions with ease to move between states effortlessly.
- **Dynamic On Entry and On Exit Actions**: Enhance states with actions upon entry and exit of each state for more responsive and intelligent state management.
- **Persistence Power**: Achieve persistency with support for Sql Server(for now).

## License
This project is licensed under the MIT License - see [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments
Inspired by:
- [ExceptionNotFound](https://exceptionnotfound.net/)
- [Stateless](https://github.com/dotnet-state-machine/stateless/)

