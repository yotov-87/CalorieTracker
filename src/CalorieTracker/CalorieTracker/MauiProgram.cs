using CalorieTracker.Services;
using CalorieTracker.Shared.Services;
using Microsoft.Extensions.Logging;

namespace CalorieTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add device-specific services used by the CalorieTracker.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();
            builder.Services.AddSingleton<ITokenStorageService, MauiTokenStorageService>();
            builder.Services.AddTransient<BearerTokenHandler>();

            builder.Services.AddScoped(sp =>
            {
                var handler = sp.GetRequiredService<BearerTokenHandler>();
                handler.InnerHandler = new HttpClientHandler();

                return new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://localhost:7030/")
                };
            });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}