using SampleWebApp.Activities;
using SmartFlow.Core;
using SmartFlow.Core.Builders;
using SmartFlow.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSmartFlow(settings =>
{
    settings.UseSqlServer("");
});

var workflowBuilder = new ProcessBuilder("sample");
var workflow = workflowBuilder
               .NewStep().From(new State(1, "First"))
                         .Allow(new State(2, "Second"), new ProcessAction(1))
                         .Allow(new State(3, "Third"), new ProcessAction(2))
                         .OnEntry<HelloWorld>()
               .NewStep().From(new State(2, "Second"))
                         .Allow(new State(4, "Fourth"), new ProcessAction(3))
                         .Allow(new State(5, "Fifth"), new ProcessAction(4))
                         .OnEntry<HelloWorld>()
                         .OnExit<GoodbyeWorld>()
               .Build();

var defaultWorkflowOperator = new DefaultProcessOperator();
defaultWorkflowOperator.Start(workflow);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
