using System.Text.Json.Serialization;
using Asp.Versioning;
using FlowCiao;
using FlowCiao.Studio.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddFlowCiao(settings =>
    {
        settings
            .Persist(persistenceSettings =>
            {
                persistenceSettings.UseSqlServer(builder.Configuration.GetConnectionString("FlowCiao"));
            });
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DevelopmentCorsPolicy",
            builder => builder
                .WithOrigins("http://localhost:3000", "http://127.0.0.1:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
        );
    });


    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors("DevelopmentCorsPolicy");
    }


    app.UseFlowCiao();
    var logger = app.Services.GetRequiredService<ILogger<Exception>>();
    app.ConfigureExceptionHandler(logger);

    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.MapControllers();
    app.Run();
}
