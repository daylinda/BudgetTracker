using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TrackerApp.MAUI;
using TrackerApp.MAUI.Config;
using TrackerApp.MAUI.Services;
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


            // load appsettings.json
            var assembly = Assembly.GetExecutingAssembly();

            // check exact resource name
            var resources = assembly.GetManifestResourceNames();
            foreach (var r in resources)
                System.Diagnostics.Debug.WriteLine(r);

            var resourceName = resources.FirstOrDefault(r => r.EndsWith("appsettings.json"));

            if (resourceName is null)
                throw new FileNotFoundException("appsettings.json not found as embedded resource");

            var stream = assembly.GetManifestResourceStream(resourceName);
            
            var config = new ConfigurationBuilder().AddJsonStream(stream!).Build();
            builder.Configuration.AddConfiguration(config);

            // register settings
            builder.Services.Configure<ApiSettings>(
                builder.Configuration.GetSection("ApiSettings"));


            // ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<BudgetListViewModel>();
            builder.Services.AddTransient<AddBudgetViewModel>();
            builder.Services.AddTransient<BudgetDetailViewModel>();
            builder.Services.AddTransient<AddTransactionViewModel>();

            // Views
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<BudgetListPage>();
            builder.Services.AddTransient<AddBudgetPage>();
            builder.Services.AddTransient<BudgetDetailPage>();
            builder.Services.AddTransient<AddTransactionPage>();



            // Services
            builder.Services.AddHttpClient<ApiBudgetService>();

            



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
