using SampleWebApp.Activities;
using SampleWebApp.Flows;
using SmartFlow.Core;
using SmartFlow.Core.Builders;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSmartFlow(settings =>
{
    settings.UseSqlServer("Server=.;Database=SmartFlow;Trusted_Connection=True;");
});

var app = builder.Build();

var smartFlowOperator = (IStateMachineOperator)app.Services.GetService(typeof(IStateMachineOperator))!;
smartFlowOperator.RegisterFlow<SampleStateMachine>();
var executionResult = smartFlowOperator.Execute(stateMachine);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
