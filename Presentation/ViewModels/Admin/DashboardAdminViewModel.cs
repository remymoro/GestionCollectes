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

        public MagasinsActifsViewModel MagasinsActifsVM { get; }

        public MagasinsViewModel MagasinsVM { get; }

        [ObservableProperty]
        private object currentView;

        public DashboardAdminViewModel()
        {
            // Récupère une nouvelle instance à chaque fois pour éviter les partages de contexte
            var collecteService = App.ServiceProvider.GetRequiredService<CollecteService>();
            var utilisateurService = App.ServiceProvider.GetRequiredService<UtilisateurService>();

            // Important : demande deux instances différentes !
            var centreService1 = App.ServiceProvider.GetRequiredService<CentreService>();
            var centreService2 = App.ServiceProvider.GetRequiredService<CentreService>();
            var magasinService = App.ServiceProvider.GetRequiredService<MagasinService>();
            MagasinsActifsVM = new MagasinsActifsViewModel(magasinService, centreService2);

            CollectesVM = new CollecteViewModel(collecteService);
            CentresVM = new CentresViewModel(centreService1);
            UtilisateursVM = new UtilisateursViewModel(utilisateurService);
            MagasinsVM = new MagasinsViewModel(magasinService, centreService2); // Utilise l'autre instance

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
                case "Magasins":
                    CurrentView = MagasinsVM;
                    break;
                case "MagasinsActifs": // Ajout du bouton magasins actifs
                    CurrentView = MagasinsActifsVM;
                    break;
                default:
                    CurrentView = CollectesVM;
                    break;
            }
        }

    }
}
