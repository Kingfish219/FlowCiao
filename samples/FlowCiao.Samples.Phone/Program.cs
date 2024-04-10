using FlowCiao;
using FlowCiao.Interfaces;
using FlowCiao.Operators;
using FlowCiao.Samples.Phone.FlowCiao;

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

// Call UseFlowCiao if you want to use Data Persistency
app.UseFlowCiao();

using (var scope = app.Services.CreateScope()) {
    // Build your custom Flow
    var flowBuilder = scope.ServiceProvider.GetRequiredService<IFlowBuilder>();
    var flow = flowBuilder.Build<PhoneStateMachine>();
    
    // And fire it using Ciao!!!
    var flowOperator = scope.ServiceProvider.GetService<IFlowOperator>();
    var instance = flowOperator.Ciao(flow).GetAwaiter().GetResult();
    //var result = flowOperator.FireAsync(instance, 1, new Dictionary<object, object>()).GetAwaiter().GetResult();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
