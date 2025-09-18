using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Tracker.Config;
using Tracker.IServices;
using Tracker.Services;
using Tracker.View;
using Tracker.ViewModel;

namespace Tracker
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

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Tracker.Config.settings.json";

            var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new FileNotFoundException("The embedded resource 'settings.json' was not found.", resourceName);
            }

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

            builder.Configuration.AddConfiguration(config);


            builder.Services.AddHttpClient<INotificationService,NotificationService>("ApiClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Settings:ApiBaseUrl"]);
            });


            builder.Services.AddTransient<INotificationService, NotificationService>();
            builder.Services.AddHttpClient<NotificationService>();

            builder.Services.AddSingleton<NotificationViewModel>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<NotificationView>();

            builder.Services.AddSingleton<MainPage>();
           


           






#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
