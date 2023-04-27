namespace SmartFlow.Builders
{
    //public class StateMachineBuilder : IStateMachineBuilder
    //{
    //    public List<IStateMachineStepBuilder> StateMachineStepBuilders { get; set; }
    //    private readonly SmartFlowService _processService;

    //    public StateMachineBuilder(SmartFlowService processService)
    //    {
    //        StateMachineStepBuilders = new List<IStateMachineStepBuilder>();
    //        _processService = processService;
    //    }

    //    public IStateMachineStepBuilder NewStep()
    //    {
    //        var builder = new StateMachineStepBuilder(this);
    //        StateMachineStepBuilders.Add(builder);

    //        return builder;
    //    }

    //    public IStateMachineStepBuilder NewStep(IStateMachineStepBuilder builder)
    //    {
    //        StateMachineStepBuilders.Add(builder);

    //        return builder;
    //    }

    //    public IStateMachine Build<T>() where T : IStateMachine, new()
    //    {
    //        try
    //        {
    //            var stateMachine = (IStateMachine)Activator.CreateInstance(typeof(T));
    //            if (stateMachine is null)
    //            {
    //                throw new Exception();
    //            }

    //            stateMachine.Construct<T>(this);

    //            var process = (IStateMachine)_processService.GetProcess<T>(key: stateMachine.Key).GetAwaiter().GetResult();
    //            if (process != null)
    //            {
    //                return process;
    //            }

    //            process = new T();
    //            foreach (var builder in StateMachineStepBuilders)
    //            {
    //                builder.InitialState.Activities = new List<Activity>
    //                {
    //                    new Activity
    //                    {
    //                        ProcessActivityExecutor = builder.OnEntryActivty
    //                    }
    //                };

    //                foreach (var allowedTransition in builder.AllowedTransitions)
    //                {
    //                    process.Transitions.Add(new Transition
    //                    {
    //                        From = builder.InitialState,
    //                        To = allowedTransition.Item1,
    //                        Activities = new List<Activity>
    //                        {
    //                            new Activity
    //                            {
    //                                ProcessActivityExecutor = builder.OnExitActivity
    //                            }
    //                        },
    //                        Actions = allowedTransition.Item2
    //                    });
    //                }
    //            }

    //            var result = _processService.Create<T>(process).GetAwaiter().GetResult();
    //            if (result == default)
    //            {
    //                throw new SmartFlowPersistencyException("Check your database connection!");
    //            }

    //            return process;
    //        }
    //        catch (Exception)
    //        {
    //            Rollback();

    //            throw;
    //        }
    //    }

    //    public IStateMachine Build<T>(Action<IStateMachineBuilder> constructor) where T : IStateMachine, new()
    //    {
    //        try
    //        {
    //            var stateMachine = (IStateMachine)Activator.CreateInstance(typeof(T));
    //            constructor.Invoke(this);
                 
    //            var process = (IStateMachine)_processService.GetProcess<T>(key: stateMachine.Key).GetAwaiter().GetResult();
    //            if (process != null)
    //            {
    //                return process;
    //            }

    //            foreach (var builder in StateMachineStepBuilders)
    //            {
    //                builder.InitialState.Activities = new List<Activity>
    //                {
    //                    new Activity
    //                    {
    //                        ProcessActivityExecutor = builder.OnEntryActivty
    //                    }
    //                };

    //                foreach (var allowedTransition in builder.AllowedTransitions)
    //                {
    //                    process.Transitions.Add(new Transition
    //                    {
    //                        From = builder.InitialState,
    //                        To = allowedTransition.Item1,
    //                        Activities = new List<Activity>
    //                        {
    //                            new Activity
    //                            {
    //                                ProcessActivityExecutor = builder.OnExitActivity
    //                            }
    //                        },
    //                        Actions = allowedTransition.Item2
    //                    });
    //                }
    //            }

    //            var result = _processService.Create<T>(process).GetAwaiter().GetResult();
    //            if (result == default)
    //            {
    //                throw new SmartFlowPersistencyException("Check your database connection!");
    //            }

    //            return process;
    //        }
    //        catch (Exception)
    //        {
    //            Rollback();

    //            throw;
    //        }
    //    }

    //    public void Rollback()
    //    {

    //    }

    //    public IStateMachine Build<T>(Action<ISmartFlowBuilder> action) where T : IStateMachine, new()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
