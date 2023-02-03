using SampleWebApp.Flows;
using SmartFlow.Core;
using SmartFlow.Core.Builders;
using SmartFlow.Core.Operators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSmartFlow(settings =>
{
    settings.UseSqlServer("Password=Abc1234;TrustServerCertificate=True;Persist Security Info=True;User ID=sa;Initial Catalog=SmartFlow;Data Source=.\\mssql2019");
});

var app = builder.Build();

//var workflow = workflowBuilder
//               .Build<SampleStateMachine>(builder =>
//               {
//                   builder.NewStep()
//                            .From(new State(1, "First"))
//                            .Allow(new State(2, "Second"), new ProcessAction(1))
//                            .Allow(new State(3, "Third"), new ProcessAction(2))
//                            .OnEntry<HelloWorld>()
//                          .NewStep().From(new State(2, "Second"))
//                            .Allow(new State(4, "Fourth"), new ProcessAction(3))
//                            .Allow(new State(5, "Fifth"), new ProcessAction(4))
//                            .OnEntry<HelloWorld>()
//                            .OnExit<GoodbyeWorld>();
//               });

//var smartFlowOperator = (IStateMachineOperator)app.Services.GetService(typeof(IStateMachineOperator))!;
//smartFlowOperator.RegisterFlow<SampleStateMachine>();
//var executionResult = smartFlowOperator.Execute(stateMachine);

var stateMachineBuilder = app.Services.GetService<ISmartFlowBuilder>();
var workflow = stateMachineBuilder?.Build<SampleStateMachine>();
var defaultWorkflowOperator = app.Services.GetService<ISmartFlowOperator>();
defaultWorkflowOperator?.ExecuteAsync(workflow);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
