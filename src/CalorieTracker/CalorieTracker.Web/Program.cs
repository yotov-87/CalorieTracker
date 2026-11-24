using CalorieTracker.Shared.DTOs;
using CalorieTracker.Shared.Services;
using CalorieTracker.Web.Components;
using CalorieTracker.Web.Data;
using CalorieTracker.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<CalorieTrackerAppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient("CalorieTrackerApi", (sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["ApiBaseUrl"] ?? "https://localhost:7030/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("CalorieTrackerApi"));

// Add device-specific services used by the CalorieTracker.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

var authGroup = app.MapGroup("/api/auth");

authGroup.MapPost("/register", async (
    RegisterRequestDto request,
    IAuthService authService,
    CancellationToken cancellationToken) =>
{
    var result = await authService.RegisterAsync(request, cancellationToken);
    return result.Success ? Results.Ok(result) : Results.BadRequest(result);
});

authGroup.MapPost("/login", async (
    LoginRequestDto request,
    IAuthService authService,
    CancellationToken cancellationToken) =>
{
    var result = await authService.LoginAsync(request, cancellationToken);
    return result.Success ? Results.Ok(result) : Results.Unauthorized();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(CalorieTracker.Shared._Imports).Assembly,
        typeof(CalorieTracker.Web.Client._Imports).Assembly);

app.Run();