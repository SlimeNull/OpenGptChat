using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using OpenGptChat_MAUI.Services;

namespace OpenGptChat_MAUI
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
                

#if DEBUG
            builder.Logging.AddDebug();
#endif



            builder.Services
             .AddSingleton<ConfigurationService>()
             .AddTransient<ChatStorageService>();



            builder.Services.AddSingleton<IMauiInitializeService>(new InitService());
            return builder.Build();
        }
    }
}