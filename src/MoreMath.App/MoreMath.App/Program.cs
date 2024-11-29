using MoreMath.App.Components;
using MoreMath.App.Extensions;
using Syncfusion.Blazor;
using MoreMath.App.Services.Localization;
using System.Globalization;
using MoreMath.App.Services.Cache;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddSecrets();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.AddAppServices();

builder.Services.AddHttpServices();
builder.Services.AddHttpClients(builder.Configuration);

builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<MoreMath.App.Components.Pages.MathHebrewTableS.CustomHebWordAdaptor>();
builder.Services.AddScoped<MoreMath.App.Components.Pages.Search.CustomArticlesAdaptor>();

builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<AuthorCacheService>();
builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru-RU");

var app = builder.Build();

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(app.Configuration["Syncfusion:License:RegisterKey"]);


app.UseForwardedHeaders();
app.ApplyMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();     //app.UseMigrationsEndPoint();
}
// else { app.UseExceptionHandler("/Error", createScopeForErrors: true); }


app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MoreMath.App.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();