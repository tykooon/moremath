using Serilog;

namespace MoreMath.Api.Extensions;

public static class SerilogExtensions
{
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        var logService = new LoggerFactory().AddSerilog();
        builder.Services.AddSingleton(logService);

        return builder;
    }
}
