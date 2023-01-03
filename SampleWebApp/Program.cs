using SampleWebApp.Activities;
using SmartFlow.Core;
using SmartFlow.Core.Models;
using Action = SmartFlow.Core.Models.Action;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var workflowBuilder = new WorkflowBuilder("sample");
var workflow = workflowBuilder.UseSqlServer("")
               .NewStep().From(new State(1, "First"))
                         .Allow(new State(2, "Second"), new Action(1))
                         .Allow(new State(3, "Third"), new Action(2))
                         .OnEntry<HelloWorld>()
               .NewStep().From(new State(2, "Second"))
                         .Allow(new State(4, "Fourth"), new Action(3))
                         .Allow(new State(5, "Fifth"), new Action(4))
                         .OnEntry<HelloWorld>()
                         .OnExit<GoodbyeWorld>()
               .Build();

var defaultWorkflowOperator = new DefaultWorkflowOperator();
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
