using FlowCiao;
using FlowCiao.Builders;
using SampleWebApp.FLowCiao;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add FlowCiao to services
builder.Services.AddFlowCiao(settings =>
{
    //settings.Persist()
    //    .UseSqlServer(configuration.GetConnectionString("SmartFlow"));
});

var app = builder.Build();

// Build your custom flow and Fire!!!
var stateMachineBuilder = app.Services.GetService<IFlowBuilder>();
var workflow = stateMachineBuilder?.Build<PhoneStateMachine>();
//var defaultWorkflowOperator = app.Services.GetService<IFlowOperator>();
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
