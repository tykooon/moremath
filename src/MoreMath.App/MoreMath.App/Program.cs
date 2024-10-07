using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MoreMath.App.Components;
using MoreMath.App.Components.Account;
using MoreMath.App.Data;
using MoreMath.App.Extensions;
using MoreMath.App.Services.Mail;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddSecrets();

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddHttpClient("MoreMath.Backend", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Backend:Url"] ?? "http://localhost:5050");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "MoreMath.App");
});
builder.Services.AddHttpClient("MoreMath.Content", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Content:BaseUrl"] ?? "https://stmoremathdev001.blob.core.windows.net/mm-content");
    //client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "MoreMath.App");
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddDbContext<WebAppDbContext>(opt =>
            opt.UseMySql(
                builder.Configuration.GetConnectionString("IdentityDb:Development:MariaDb"),
                new MariaDbServerVersion(new Version(10, 6, 18))));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<WebAppDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, AppInfoMailSender>();
// TODO: Check if singleton is the right lifetime for this service.
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityAppEmailSender>();

var app = builder.Build();

app.ApplyMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    //app.UseMigrationsEndPoint();
}
else
{
    //app.UseExceptionHandler("/Error", createScopeForErrors: true);
}


app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MoreMath.App.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
