using Asp.Versioning;
using FlowCiao.Models;

namespace FlowCiao.Copilot.Extensions;

public static class CopilotExtensions
{
    public static FlowSettings UseCopilot(this FlowSettings settings, IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-flowciao-api-version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return settings;
    }

    public static void UseCopilot(this IApplicationBuilder app)
    {
        app.UseRouting();
    }
}