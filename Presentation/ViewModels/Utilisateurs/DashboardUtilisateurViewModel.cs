using CommunityToolkit.Mvvm.ComponentModel;
using GestionCollectes.ApplicationLayer.Services;

namespace GestionCollectes.Presentation.ViewModels.Utilisateurs
{
    public partial class DashboardUtilisateurViewModel : ObservableObject
    {
        public MagasinService MagasinService { get; }

        [ObservableProperty]
        private ObservableObject? vueCourante;

        public DashboardUtilisateurViewModel(CollecteService collecteService, MagasinService magasinService)
        {
            MagasinService = magasinService;
            VueCourante = new CollecteUtilisateurViewModel(collecteService, magasinService, this);
        }
    }
}
