using SmartFlow.Core;
using SmartFlow.Core.Models;
using Action = SmartFlow.Core.Models.Action;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var workflowBuilder = new WorkflowBuilder();
workflowBuilder.UseSqlServer("")
               .NewStep().From(new State())
                         .Allow(new State(), new Action())
               .Build();

workflowBuilder.Build();

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
