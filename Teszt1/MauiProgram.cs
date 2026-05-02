using Microsoft.Extensions.Logging;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;
using Teszt1.Bakckend.Services;
using Teszt1.Frontend;
using Teszt1.Bakckend.Database;
using Teszt1.Frontend.Edzes;
using Teszt1.Frontend.Statisztika;

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
            // --- 1. ADATBÁZIS REGISZTRÁLÁSA ---
            // Ez teszi lehetővé, hogy a DataProviderek megkapják a db-t a konstruktorban
            builder.Services.AddDbContext<FitnessDbContext>();

            // --- 2. DATA PROVIDEREK REGISZTRÁLÁSA ---
            builder.Services.AddScoped<IFoodDataProvider, FoodDataProvider>();
            builder.Services.AddScoped<IBadgeDataProvider, BadgeDataProvider>();
            builder.Services.AddScoped<IExerciseDataProvider, ExerciseDataProvider>();
            builder.Services.AddScoped<IStepsDataProvider, StepsDataProvider>();
            builder.Services.AddScoped<IWeightDataProvider, WeightDataProvider>();
            builder.Services.AddScoped<IWorkoutDataProvider, WorkoutDataProvider>();
            builder.Services.AddScoped<IWorkoutplanDataProvider, WorkoutplanDataProvider>();
            builder.Services.AddScoped<IWorkoutEntryDataProvider, WorkoutEntryDataProvider>();
            builder.Services.AddScoped<IMealDataProvider, MealDataProvider>();
            builder.Services.AddScoped<IMealEntryDataProvider, MealEntryDataProvider>();
            builder.Services.AddScoped<IUserDataProvider, UserDataProvider>();

            // --- 3. SZOLGÁLTATÁSOK (SERVICES) ---
            builder.Services.AddScoped<MealService>();
            builder.Services.AddScoped<WorkoutService>();


            // 2. Oldalak és a hozzájuk tartozó ViewModel-ek
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();

            // Ezt a kettőt adtuk most hozzá:
            builder.Services.AddTransient<EdzesPage>();
            builder.Services.AddTransient<EdzesPageViewModel>();

            builder.Services.AddTransient<UjEdzesPage>();
            builder.Services.AddTransient<UjEdzesPageViewModel>();

            builder.Services.AddTransient<EdzesSzerkesztesPage>();
            builder.Services.AddTransient<EdzesSzerkesztesPageViewModel>();

            // Ezt a kettőt kell hozzáírni az eddigiek alá:
            builder.Services.AddTransient<EtkezesHozzaadasaPage>();
            builder.Services.AddTransient<EtkezesHozzaadasaPageViewModel>();

            builder.Services.AddTransient<EuAdatokPage>();
            builder.Services.AddTransient<EuAdatokPageViewModel>();

            builder.Services.AddTransient<BaratokPage>();
            builder.Services.AddTransient<BaratokPageViewModel>();

            builder.Services.AddTransient<StatisztikaPage>();
            builder.Services.AddTransient<StatisztikaPageViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        
    }
}
