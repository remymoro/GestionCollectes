using CommunityToolkit.Mvvm.ComponentModel;
using GestionCollectes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Presentation.Navigations
{
    public interface INavigationService : INotifyPropertyChanged
    {
        ObservableObject? CurrentView { get; }

        void NavigateToCollectes();
        void NavigateToCentres();
        void NavigateToUtilisateurs();
        void NavigateToMagasins();

        void NavigateToMagasinsActivation();



        void NavigateToProduitsCatalogue();




    }
}
