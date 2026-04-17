using Microsoft.Extensions.Logging;
using TrackerApp.MAUI;
using TrackerApp.MAUI.ViewModels;
using TrackerApp.MAUI.Views;

namespace TrackerApp.Maui
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ViewModels
            builder.Services.AddTransient<LoginViewModel>();

            // Views
            builder.Services.AddTransient<LoginPage>();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
