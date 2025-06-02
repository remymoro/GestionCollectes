using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Enums;

namespace GestionCollectes.Presentation.ViewModels.Utilisateurs
{
    public partial class CollecteUtilisateurViewModel : ObservableObject
    {
        private readonly CollecteService _collecteService;
        private readonly MagasinService _magasinService;
        private readonly DashboardUtilisateurViewModel _dashboard;
        private readonly ICurrentUserService _currentUserService;
        // private readonly IServiceProvider _serviceProvider; // Removed IServiceProvider

        [ObservableProperty]
        private ObservableCollection<CollecteDisplay> collectes = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string? erreurChargement;

        // Replaced App.UtilisateurCourant with _currentUserService.CurrentUser
        public Utilisateur? UtilisateurCourant => _currentUserService.CurrentUser; 

        public string MessageBienvenue =>
            _currentUserService.CurrentUser == null // Used _currentUserService
                ? "Bienvenue !"
                : $"Bonjour, {_currentUserService.CurrentUser.Nom} ({_currentUserService.CurrentUser.Role})"
                    + (_currentUserService.CurrentUser.CentreId != null
                        ? $" - Centre {_currentUserService.CurrentUser.Nom}" // Assuming Nom refers to Centre Name if CentreId is present, this logic might need re-evaluation based on entity structure
                        : (_currentUserService.CurrentUser.CentreId.HasValue
                            ? $" - Centre {_currentUserService.CurrentUser.CentreId.Value}"
                            : ""));

        [RelayCommand(CanExecute = nameof(CanOuvrirCollecte))]
        private void OuvrirCollecte(CollecteDisplay collecte)
        {
            if (collecte is null || !collecte.EstAccessible || collecte.Entity is null) return;
            int centreId = UtilisateurCourant?.CentreId ?? 0;
            // Navigation locale via le parent
            // Pass _magasinService (already injected into this VM) to ChoixMagasinViewModel
            _dashboard.VueCourante = new ChoixMagasinViewModel(
                _magasinService, // Use the injected MagasinService
                collecte.Entity,
                centreId,
                _dashboard
            );
            Debug.WriteLine($"[DEBUG] Collecte date début: {collecte.DateDebut}"); // <-- pour vérifier
        }

        private bool CanOuvrirCollecte(CollecteDisplay collecte)
            => collecte is not null && collecte.EstAccessible;

        [RelayCommand]
        public async Task RafraichirAsync() => await LoadCollectesAsync();

        public CollecteUtilisateurViewModel(
            CollecteService collecteService,
            MagasinService magasinService,
            DashboardUtilisateurViewModel dashboard,
            ICurrentUserService currentUserService) // Removed IServiceProvider
            // IServiceProvider serviceProvider) 
        {
            _collecteService = collecteService;
            _magasinService = magasinService;
            _dashboard = dashboard;
            _currentUserService = currentUserService;
            // _serviceProvider = serviceProvider; // Removed IServiceProvider
            _ = LoadCollectesAsync();
        }

        public class CollecteDisplay
        {
            public int Id { get; set; }
            public string Nom { get; set; }
            public DateTime DateDebut { get; set; }
            public DateTime DateFin { get; set; }
            public string Statut { get; set; }
            public bool EstAccessible { get; set; }
            public Collecte? Entity { get; set; }
        }

        private async Task LoadCollectesAsync()
        {
            try
            {
                IsLoading = true;
                ErreurChargement = null;

                var aujourdHui = DateTime.Now.Date;
                var liste = await _collecteService.GetAllAsync();

                Collectes = new ObservableCollection<CollecteDisplay>(
                    liste.Select(c => new CollecteDisplay
                    {
                        Id = c.Id,
                        Nom = c.Nom,
                        DateDebut = c.DateDebut,
                        DateFin = c.DateFin,
                        Statut = c.Statut.ToString(),
                        EstAccessible = c.Statut == StatutCollecte.EnCours
                            && aujourdHui >= c.DateDebut.Date
                            && aujourdHui <= c.DateFin.Date,
                        Entity = c
                    })
                );
            }
            catch (Exception)
            {
                ErreurChargement = "Erreur lors du chargement des collectes.";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
