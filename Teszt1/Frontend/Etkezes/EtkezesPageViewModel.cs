using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Frontend
{
    [QueryProperty (nameof(EtkezesHozzaadasaPage), "UjEtkezes")]
    public partial class EtkezesPageViewModel : ObservableObject
    {
        //private readonly DatabaseService _databaseService;

        //// Ezeket a változókat kötjük a beviteli mezőkhöz
        //[ObservableProperty] private float caloroies;
        //[ObservableProperty] private float carb;
        //[ObservableProperty] private float fat;
        //[ObservableProperty] private double protein;
        //ICollection<Food> today;


        //public EtkezesPageViewModel(DatabaseService databaseService)
        //{
        //    this._databaseService = databaseService;
        //    today = new List<Food>();
        //    if (caloroies != null && carb != null && fat != null && protein != null)
        //    {

        //    }
        //}

        //public void EtkezesHozzadas()
        //{

        //}







    }
}
