using SampleWebApp.Flows;
using SmartFlow;
using SmartFlow.Builders;
using SmartFlow.Operators;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration; // allows both to access and to set up the config

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSmartFlow(settings =>
{
    settings.Persist()
        .UseSqlServer(configuration.GetConnectionString("SmartFlow"));
});

var app = builder.Build();


// Add SmartFlow
var stateMachineBuilder = app.Services.GetService<ISmartFlowBuilder>();
var workflow = stateMachineBuilder?.Build<SampleStateMachine>();
var defaultWorkflowOperator = app.Services.GetService<ISmartFlowOperator>();

// Fire your Smart Flow
defaultWorkflowOperator?.Fire("Sample", 1);


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
