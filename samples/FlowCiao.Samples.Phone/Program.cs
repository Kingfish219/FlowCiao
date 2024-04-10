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

// Call UseFlowCiao if you want to Persist FlowCiao
app.UseFlowCiao();

using (var scope = app.Services.CreateScope()) {
    // Build your custom Flow
    var flowBuilder = scope.ServiceProvider.GetRequiredService<IFlowBuilder>();
    var flow = flowBuilder.Build<PhoneStateMachine>();
    
    // Initialize it using Ciao and Fire!!!
    var flowOperator = scope.ServiceProvider.GetService<IFlowOperator>();
    var instance = flowOperator.Ciao(flow).GetAwaiter().GetResult();
    var result = flowOperator.FireAsync(instance, (int)PhoneStateMachine.Triggers.Call).GetAwaiter().GetResult();
    Console.WriteLine(result.Message);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
