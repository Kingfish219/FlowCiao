using FlowCiao;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.Services.AddFlowCiao(settings =>
    {
        settings
          .Persist()
            .UseSqlServer(builder.Configuration.GetConnectionString("FlowCiao"));
    });
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
