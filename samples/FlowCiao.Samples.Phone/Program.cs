using FlowCiao;
using FlowCiao.Builders;
using FlowCiao.Operators;
using FlowCiao.Samples.Phone.FLowCiao;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add FlowCiao to services
builder.Services.AddFlowCiao(settings =>
{
    settings
        .Persist(persistenceSettings =>
        {
            persistenceSettings.UseSqlServer(builder.Configuration.GetConnectionString("FlowCiao"));
        });
});

var app = builder.Build();

// Build your custom flow and Fire!!!
var flowBuilder = app.Services.GetService<IFlowBuilder>();
var flow = flowBuilder?.Build<PhoneStateMachine>();
var flowOperator = app.Services.GetService<IFlowOperator>();
var instance = flowOperator.Instantiate(flow).GetAwaiter().GetResult();
var result = flowOperator.FireAsync(instance, 1, new Dictionary<object, object>()).GetAwaiter().GetResult();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
