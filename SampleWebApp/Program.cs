using SampleWebApp.Flows;
using SmartFlow;
using SmartFlow.Builders;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SmartFlow to services
builder.Services.AddSmartFlow(settings =>
{
    //settings.Persist()
    //    .UseSqlServer(configuration.GetConnectionString("SmartFlow"));
});

var app = builder.Build();

// Build your custom flow and Fire!!!
var stateMachineBuilder = app.Services.GetService<ISmartFlowBuilder>();
var workflow = stateMachineBuilder?.Build<PhoneStateMachine>();
//var defaultWorkflowOperator = app.Services.GetService<ISmartFlowOperator>();
//var result = defaultWorkflowOperator?.Fire("phone", 1);
//result = defaultWorkflowOperator?.Fire("phone", 1);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapControllers();

app.Run();
