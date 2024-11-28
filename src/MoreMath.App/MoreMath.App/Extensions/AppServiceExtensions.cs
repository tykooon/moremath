using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MoreMath.App.Components.Account;
using MoreMath.App.Data;
using MoreMath.App.Services.Mail;
using MoreMath.Infrastructure;
using MoreMath.Application;

namespace MoreMath.App.Extensions;

public static class AppServiceExtensions
{
    public static WebApplicationBuilder AddAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastucture(builder.Configuration);
        builder.Services.AddApplicationServices(builder.Configuration);
        return builder;
    }


    public static IServiceCollection AddHttpClients(this IServiceCollection services, ConfigurationManager configuration)
    {
        // TODO After Tranferring to MediatR in BlazorApp no need in REST Backend
        services.AddHttpClient("MoreMath.Backend", client =>
        {
            client.BaseAddress = new Uri(configuration["Backend:Url"] ?? "http://localhost:5050");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "MoreMath.App");
        });

        services.AddHttpClient("MoreMath.Content", client =>
        {
            client.BaseAddress = new Uri(configuration["Content:BaseUrl"] ?? "https://stmoremathdev001.blob.core.windows.net/mm-content");
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "MoreMath.App");
        });

        return services;
    }

    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies();

        services.AddDbContext<WebAppDbContext>(opt =>
            opt.UseMySql(
                configuration.GetConnectionString("IdentityDb:Development:MariaDb"),
                new MariaDbServerVersion(new Version(10, 6, 18))));

        services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<WebAppDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

        return services;
    }

    public static IServiceCollection AddHttpServices(this IServiceCollection services)
    {
        // Add services to the container.
        services.AddRouting(options => options.LowercaseUrls = true);
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        
        services.AddTransient<IEmailSender, AppInfoMailSender>();

        // TODO: Check if singleton is the right lifetime for this service.
        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityAppEmailSender>();

        return services;

    }
}
