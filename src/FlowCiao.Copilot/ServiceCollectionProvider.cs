using Asp.Versioning;
using FlowCiao.Models;

namespace FlowCiao.Copilot
{
    public static class ServiceCollectionProvider
    {
        // public static IServiceCollection AddFlowCiao(this IServiceCollection services, string[] args)
        // {
        //     var webApplicationBuilder = ConfigWebAppBuilder(args);
        //     _ = RunWebApp(webApplicationBuilder);
        //     
        //     return services;
        // }
        
        public static FlowSettings UseCopilot(this FlowSettings settings, IConfiguration configurationManager)
        {
            var webApplicationBuilder = ConfigWebAppBuilder(configurationManager);
            _ = RunWebApp(webApplicationBuilder);
            
            return settings;
        }

        private static WebApplicationBuilder ConfigWebAppBuilder(IConfiguration configurationManager)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddConfiguration(configurationManager);
            builder.Services.AddControllers();
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

            return builder;
        }

        private static async Task RunWebApp(WebApplicationBuilder builder)
        {
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();
            await app.RunAsync();
        }
    }
}