using FlowCiao;
using FlowCiao.Interfaces;
using FlowCiao.Operators;
using FlowCiao.Samples.Phone.Flow;
using FlowCiao.Samples.Phone.Flow.Models;

var builder = WebApplication.CreateBuilder(args);

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
    var flow = flowBuilder.Build<PhoneFlow>();
    
    // Call CiaoAndTriggerAsync to Initialize it using Ciao and run it using Trigger
    var flowOperator = scope.ServiceProvider.GetService<IFlowOperator>();
    var result = flowOperator.CiaoAndTriggerAsync(flow.Key, Triggers.Call).GetAwaiter().GetResult();
    Console.WriteLine(result.Message);
}

app.Run();
