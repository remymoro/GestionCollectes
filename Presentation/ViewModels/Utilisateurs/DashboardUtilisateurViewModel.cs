using CommunityToolkit.Mvvm.ComponentModel;
using GestionCollectes.ApplicationLayer.Services;

namespace GestionCollectes.Presentation.ViewModels.Utilisateurs
{
    public partial class DashboardUtilisateurViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableObject? vueCourante;

        // CONSTRUCTEUR PUBLIC !
        public DashboardUtilisateurViewModel(CollecteService collecteService)
        {
            VueCourante = new CollecteUtilisateurViewModel(collecteService); // Vue de départ
        }
    }
}
