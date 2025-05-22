using GestionCollectes.Domain.Entities;
using GestionCollectes.ApplicationLayer.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GestionCollectes.Domain.Enums;
using System;
using System.Linq;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class UtilisateursViewModel : ObservableObject
    {
        private readonly UtilisateurService _utilisateurService;

        [ObservableProperty]
        private ObservableCollection<Utilisateur> utilisateurs = new();

        [ObservableProperty]
        private Utilisateur? selectedUtilisateur;

        [ObservableProperty]
        private Utilisateur editUtilisateur = new Utilisateur();

        [ObservableProperty]
        private string motDePasse = string.Empty; // pour la saisie/édition

        public IEnumerable<RoleUtilisateur> RolesDisponibles => Enum.GetValues(typeof(RoleUtilisateur)).Cast<RoleUtilisateur>();

        public IAsyncRelayCommand LoadUtilisateursCommand { get; }
        public IAsyncRelayCommand AddUtilisateurCommand { get; }
        public IAsyncRelayCommand DeleteUtilisateurCommand { get; }
        public IAsyncRelayCommand UpdateUtilisateurCommand { get; }

        public UtilisateursViewModel(UtilisateurService utilisateurService)
        {
            _utilisateurService = utilisateurService;

            LoadUtilisateursCommand = new AsyncRelayCommand(LoadUtilisateursAsync);
            AddUtilisateurCommand = new AsyncRelayCommand(AddUtilisateurAsync, CanAdd);
            DeleteUtilisateurCommand = new AsyncRelayCommand(DeleteUtilisateurAsync, CanDeleteOrEdit);
            UpdateUtilisateurCommand = new AsyncRelayCommand(UpdateUtilisateurAsync, CanDeleteOrEdit);

            // Surveille les changements de saisie dans le formulaire d'ajout/modif
            EditUtilisateur.PropertyChanged += (s, e) => AddUtilisateurCommand.NotifyCanExecuteChanged();
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MotDePasse))
                    AddUtilisateurCommand.NotifyCanExecuteChanged();

                if (e.PropertyName == nameof(SelectedUtilisateur))
                {
                    DeleteUtilisateurCommand.NotifyCanExecuteChanged();
                    UpdateUtilisateurCommand.NotifyCanExecuteChanged();
                }
            };

            // Charge la liste au lancement
            LoadUtilisateursCommand.Execute(null);
        }

        // Utilitaire : reset le formulaire d’édition/après ajout
        private void ResetEdit()
        {
            EditUtilisateur = new Utilisateur();
            MotDePasse = string.Empty;
            EditUtilisateur.PropertyChanged += (s, e) => AddUtilisateurCommand.NotifyCanExecuteChanged();
        }

        // Quand tu sélectionnes un utilisateur dans la liste
        partial void OnSelectedUtilisateurChanged(Utilisateur? oldValue, Utilisateur? newValue)
        {
            if (newValue != null)
            {
                // Clone pour édition (sinon tu modifies direct la liste)
                EditUtilisateur = new Utilisateur
                {
                    Id = newValue.Id,
                    Nom = newValue.Nom,
                    MotDePasseHash = newValue.MotDePasseHash,
                    Role = newValue.Role
                };
                MotDePasse = newValue.MotDePasseHash;
            }
            else
            {
                ResetEdit();
            }
            UpdateUtilisateurCommand.NotifyCanExecuteChanged();
        }

        // CHARGEMENT
        private async Task LoadUtilisateursAsync()
        {
            var list = await _utilisateurService.GetAllAsync();
            Utilisateurs.Clear();
            foreach (var utilisateur in list)
                Utilisateurs.Add(utilisateur);

            SelectedUtilisateur = null;
            ResetEdit();
        }

        // AJOUT
        private async Task AddUtilisateurAsync()
        {
            if (!CanAdd()) return;
            EditUtilisateur.MotDePasseHash = MotDePasse;
            await _utilisateurService.AddAsync(EditUtilisateur);
            await LoadUtilisateursAsync();
        }

        // SUPPRESSION
        private async Task DeleteUtilisateurAsync()
        {
            if (SelectedUtilisateur == null) return;
            await _utilisateurService.DeleteAsync(SelectedUtilisateur.Id);
            await LoadUtilisateursAsync();
        }

        // MODIFICATION
        private async Task UpdateUtilisateurAsync()
        {
            if (SelectedUtilisateur == null) return;

            // Recopie les valeurs éditées dans l'objet sélectionné avant update
            SelectedUtilisateur.Nom = EditUtilisateur.Nom;
            SelectedUtilisateur.Role = EditUtilisateur.Role;
            SelectedUtilisateur.MotDePasseHash = MotDePasse;

            await _utilisateurService.UpdateAsync(SelectedUtilisateur);
            await LoadUtilisateursAsync();
        }

        // CONDITIONS POUR COMMANDES
        private bool CanAdd()
            => !string.IsNullOrWhiteSpace(EditUtilisateur?.Nom)
            && !string.IsNullOrWhiteSpace(MotDePasse);

        private bool CanDeleteOrEdit()
            => SelectedUtilisateur != null;
    }
}
