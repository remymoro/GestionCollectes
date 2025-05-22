using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.ApplicationLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class DashboardAdminViewModel : ObservableObject
    {
        public ICommand NavigateCommand { get; }

        public CollecteViewModel CollectesVM { get; }
        public CentresViewModel CentresVM { get; }


        public UtilisateursViewModel UtilisateursVM { get; }



        [ObservableProperty]
        private object currentView;

        public DashboardAdminViewModel()
        {
            // Récupère les services via la DI
            var collecteService = App.ServiceProvider.GetRequiredService<CollecteService>();
            var centreService = App.ServiceProvider.GetRequiredService<CentreService>();
            var utilisateurService = App.ServiceProvider.GetRequiredService<UtilisateurService>();

            CollectesVM = new CollecteViewModel(collecteService);
            CentresVM = new CentresViewModel(centreService);
            UtilisateursVM = new UtilisateursViewModel(utilisateurService);

            CurrentView = CollectesVM; // Par défaut

            NavigateCommand = new RelayCommand<string>(Navigate);
        }

        private void Navigate(string? page)
        {
            switch (page)
            {
                case "Collectes":
                    CurrentView = CollectesVM;
                    break;
                case "Centres":
                    CurrentView = CentresVM;
                    break;
                case "Utilisateurs":
                    CurrentView = UtilisateursVM;
                    break;
                default:
                    CurrentView = CollectesVM;
                    break;
            }
        }

    }

}
