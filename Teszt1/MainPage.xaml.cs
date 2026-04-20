using Teszt1.Bakckend.Calsses;
using Teszt1.Frontend;
using Teszt1.Bakckend.Database;

namespace Teszt1
{
    public partial class MainPage : ContentPage
    {
        DatabaseService _dbService = new DatabaseService();
        private readonly MainPageViewModel _viewModel;

        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        // Ez a függvény minden alkalommal lefut, amikor az oldal láthatóvá válik
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Szólunk a ViewModel-nek, hogy töltse le a MySQL-ből a friss adatokat
            _viewModel.LoadData();
        }




        //private async void OnEtkezesHozzaadasaClicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // 1. Kapcsolódás a XAMPP MySQL-hez
        //        using (var db = new FitnessDbContext())
        //        {
        //            var workoutDataProvider = new WorkoutDataProvider(db);

        //            // 2. Teszt adat létrehozása
        //            var tesztUser = new WorkOut
        //            {
        //                Date = DateTime.Now,
        //                User_Id = 1,
        //                User = new User(),
        //                Entries = new List<WorkoutEntry>()

        //            };

        //            // 3. Mentés megkísérlése
        //            workoutDataProvider.AddWorkout(tesztUser);

        //            MainThread.BeginInvokeOnMainThread(async () => {
        //                await DisplayAlert("Siker", "Adat mentve!", "OK");
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Ez kiírja a belső hibaüzenetet is, ami elárulja, mi a baj a MySQL-nek
        //        var innerError = ex.InnerException != null ? ex.InnerException.Message : "Nincs belső hiba";
        //        await DisplayAlert("Hiba", $"Részletek: {innerError}", "OK");
        //    }
        //}

        //// Minden alkalommal lefut, amikor az oldal láthatóvá válik
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    FrissitsdAFeluletet();
        //}

        //private void FrissitsdAFeluletet()
        //{

        //}



    }
}