using System.Net;
using FlowCiao.Studio.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace FlowCiao.Studio.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    logger.LogError($"Something went wrong: {contextFeature.Error}");
                    await context.Response.WriteAsJsonAsync(
                        new ApiResponse(ApiResponse.ApiResponseStatus.Error,
                            message: contextFeature.Error.Message)
                    );
                }
            });
        });
    }
}