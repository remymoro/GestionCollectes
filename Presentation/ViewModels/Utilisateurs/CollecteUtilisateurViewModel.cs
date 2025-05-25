using System;
using System.Collections.ObjectModel;
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

        [ObservableProperty]
        private ObservableCollection<CollecteDisplay> collectes = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string? erreurChargement;

        public Utilisateur? UtilisateurCourant => App.UtilisateurCourant;

        public string MessageBienvenue =>
            UtilisateurCourant == null
                ? "Bienvenue !"
                : $"Bonjour, {UtilisateurCourant.Nom} ({UtilisateurCourant.Role})"
                    + (UtilisateurCourant.CentreId != null
                        ? $" - Centre {UtilisateurCourant.Nom}"
                        : (UtilisateurCourant.CentreId.HasValue
                            ? $" - Centre {UtilisateurCourant.CentreId.Value}"
                            : ""));

        [RelayCommand(CanExecute = nameof(CanOuvrirCollecte))]
        private void OuvrirCollecte(CollecteDisplay collecte)
        {
            if (collecte is null || !collecte.EstAccessible) return;
            // TODO : navigation/messagerie selon ton infra
            // Messenger.Send(new NaviguerVersMagasinMessage(collecte.Id));
        }

        private bool CanOuvrirCollecte(CollecteDisplay collecte)
            => collecte is not null && collecte.EstAccessible;

        [RelayCommand]
        public async Task RafraichirAsync() => await LoadCollectesAsync();

        public CollecteUtilisateurViewModel(CollecteService collecteService)
        {
            _collecteService = collecteService;
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
        }

        private async Task LoadCollectesAsync()
        {
            try
            {
                IsLoading = true;
                ErreurChargement = null;

                var aujourdHui = DateTime.Now.Date;
                var liste = await _collecteService.GetAllAsync();

                // Filtre par centre
               
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
                            && aujourdHui <= c.DateFin.Date
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
