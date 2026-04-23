using Microsoft.Extensions.Logging;
using Teszt1.Bakckend.Calsses;
using Teszt1.Frontend;
using Teszt1.Bakckend.Database;

namespace Teszt1
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

            // 1. A mi MySQL szervizünk (Singleton: csak egy példány készül belőle)
            builder.Services.AddSingleton<DatabaseService>();

            // 2. Oldalak és a hozzájuk tartozó ViewModel-ek
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();

            // Ezt a kettőt adtuk most hozzá:
            builder.Services.AddTransient<EdzesPage>();
            builder.Services.AddTransient<EdzesPageViewModel>();

            // Ezt a kettőt kell hozzáírni az eddigiek alá:
            builder.Services.AddTransient<EtkezesHozzaadasaPage>();
            builder.Services.AddTransient<EtkezesHozzaadasaPageViewModel>();

            builder.Services.AddTransient<EuAdatokPage>();
            builder.Services.AddTransient<EuAdatokPageViewModel>();

            builder.Services.AddTransient<BaratokPage>();
            builder.Services.AddTransient<BaratokPageViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        
    }
}
