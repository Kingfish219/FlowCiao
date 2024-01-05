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
    //settings
    //  .Persist()
    //    .UseSqlServer(builder.Configuration.GetConnectionString("FlowCiao"));
});

var app = builder.Build();

// Build your custom flow and Fire!!!
var stateMachineBuilder = app.Services.GetService<IFlowBuilder>();
stateMachineBuilder?.Build<PhoneStateMachine>();
//var defaultFlowOperator = app.Services.GetService<IFlowOperator>();
//var result = defaultFlowOperator?.Fire("phone", 1);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapControllers();

app.Run();
