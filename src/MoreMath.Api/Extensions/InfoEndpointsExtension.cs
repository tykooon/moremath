namespace MoreMath.Api.Extensions;

public static class InfoEndpointsExtension
{
    public static IEndpointConventionBuilder AddInfoEndpoint(this WebApplication app) => 
        app.MapGet("/info", () => Results.Ok(
            new
            {
                ApiName = "More Math backend for frontend API",
                Version = "0.0.1",
                //Error = "0123"[5]
            }))
        .WithSummary("provide general information about API")
        .WithOpenApi();
}
