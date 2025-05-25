using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.Presentation.Navigations;
using System.Windows.Input;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class DashboardAdminViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public DashboardAdminViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new RelayCommand<string>(Navigate);

            // Initialiser CurrentView avec la vue par défaut du service
            CurrentView = _navigationService.CurrentView;

            // Écouter le changement de vue dans le service de navigation
            _navigationService.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(INavigationService.CurrentView))
                    CurrentView = _navigationService.CurrentView;
            };
        }

        [ObservableProperty]
        private object currentView;

        public ICommand NavigateCommand { get; }

        private void Navigate(string? page)
        {
            switch (page)
            {
                case "Collectes":
                    _navigationService.NavigateToCollectes();
                    break;
                case "Centres":
                    _navigationService.NavigateToCentres();
                    break;
                case "Utilisateurs":
                    _navigationService.NavigateToUtilisateurs();
                    break;
                case "Magasins":
                    _navigationService.NavigateToMagasins();
                    break;
                case "MagasinsActivation":
                    _navigationService.NavigateToMagasinsActivation();
                    break;
                default:
                    _navigationService.NavigateToCollectes();
                    break;
            }
        }
    }
}
