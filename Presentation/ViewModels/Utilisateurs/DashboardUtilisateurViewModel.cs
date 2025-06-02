using CommunityToolkit.Mvvm.ComponentModel;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Interfaces; // Added for ICurrentUserService
// using Microsoft.Extensions.DependencyInjection; // No longer needed
// using System; // No longer needed for IServiceProvider

namespace GestionCollectes.Presentation.ViewModels.Utilisateurs
{
    public partial class DashboardUtilisateurViewModel : ObservableObject
    {
        private readonly CollecteService _collecteService;
        private readonly MagasinService _magasinService;
        private readonly ICurrentUserService _currentUserService;
        
        public MagasinService MagasinService => _magasinService;


        [ObservableProperty]
        private ObservableObject? vueCourante;

        public DashboardUtilisateurViewModel(
            CollecteService collecteService, 
            MagasinService magasinService,
            ICurrentUserService currentUserService) // IServiceProvider removed
            // IServiceProvider serviceProvider) 
        {
            _collecteService = collecteService;
            _magasinService = magasinService;
            _currentUserService = currentUserService;

            VueCourante = new CollecteUtilisateurViewModel(_collecteService, _magasinService, this, _currentUserService);
        }
    }
}
