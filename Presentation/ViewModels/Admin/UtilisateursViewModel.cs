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
        private readonly CentreService _centreService;

        [ObservableProperty]
        private ObservableCollection<Utilisateur> utilisateurs = new();

        [ObservableProperty]
        private Utilisateur? selectedUtilisateur;

        [ObservableProperty]
        private Utilisateur editUtilisateur = new Utilisateur();

        [ObservableProperty]
        private string motDePasse = string.Empty;

        public ObservableCollection<Centre> Centres { get; } = new();

        [ObservableProperty]
        private Centre? centreSelectionne;

        [ObservableProperty]
        private bool afficherChampCentre;

        public IEnumerable<RoleUtilisateur> RolesDisponibles => Enum.GetValues(typeof(RoleUtilisateur)).Cast<RoleUtilisateur>();

        public IAsyncRelayCommand LoadUtilisateursCommand { get; }
        public IAsyncRelayCommand AddUtilisateurCommand { get; }
        public IAsyncRelayCommand DeleteUtilisateurCommand { get; }
        public IAsyncRelayCommand UpdateUtilisateurCommand { get; }

        // Rôles qui nécessitent un centre
        private static readonly RoleUtilisateur[] RolesAyantCentre = new[]
        {
            RoleUtilisateur.Centre,
            RoleUtilisateur.Utilisateur
        };

        public UtilisateursViewModel(UtilisateurService utilisateurService, CentreService centreService)
        {
            _utilisateurService = utilisateurService;
            _centreService = centreService;

            LoadUtilisateursCommand = new AsyncRelayCommand(LoadUtilisateursAsync);
            AddUtilisateurCommand = new AsyncRelayCommand(AddUtilisateurAsync, CanAdd);
            DeleteUtilisateurCommand = new AsyncRelayCommand(DeleteUtilisateurAsync, CanDeleteOrEdit);
            UpdateUtilisateurCommand = new AsyncRelayCommand(UpdateUtilisateurAsync, CanDeleteOrEdit);

            // Abonne-toi à PropertyChanged sur EditUtilisateur
            AttachEditUtilisateurPropertyChanged(EditUtilisateur);

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

            // Charge les listes au lancement
            LoadUtilisateursCommand.Execute(null);
            _ = ChargerCentresAsync();
        }

        // Abonnement dynamique à PropertyChanged sur EditUtilisateur
        private void AttachEditUtilisateurPropertyChanged(Utilisateur utilisateur)
        {
            utilisateur.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(utilisateur.Role))
                {
                    AfficherChampCentre = RolesAyantCentre.Contains(utilisateur.Role);
                    if (!AfficherChampCentre) CentreSelectionne = null;
                }
                AddUtilisateurCommand.NotifyCanExecuteChanged();
            };
        }

        private async Task ChargerCentresAsync()
        {
            Centres.Clear();
            var centres = await _centreService.GetAllAsync();
            foreach (var centre in centres)
                Centres.Add(centre);
        }

        private void ResetEdit()
        {
            EditUtilisateur = new Utilisateur();
            MotDePasse = string.Empty;
            CentreSelectionne = null;
            AttachEditUtilisateurPropertyChanged(EditUtilisateur);
        }

        partial void OnSelectedUtilisateurChanged(Utilisateur? oldValue, Utilisateur? newValue)
        {
            if (newValue != null)
            {
                EditUtilisateur = new Utilisateur
                {
                    Id = newValue.Id,
                    Nom = newValue.Nom,
                    MotDePasseHash = newValue.MotDePasseHash,
                    Role = newValue.Role,
                    CentreId = newValue.CentreId
                };
                MotDePasse = newValue.MotDePasseHash;
                CentreSelectionne = Centres.FirstOrDefault(c => c.Id == newValue.CentreId);
                AfficherChampCentre = RolesAyantCentre.Contains(newValue.Role);
                AttachEditUtilisateurPropertyChanged(EditUtilisateur);
            }
            else
            {
                ResetEdit();
                AfficherChampCentre = false;
            }
            UpdateUtilisateurCommand.NotifyCanExecuteChanged();
        }

        private async Task LoadUtilisateursAsync()
        {
            var list = await _utilisateurService.GetAllAsync();
            Utilisateurs.Clear();
            foreach (var utilisateur in list)
                Utilisateurs.Add(utilisateur);

            SelectedUtilisateur = null;
            ResetEdit();
        }

        private async Task AddUtilisateurAsync()
        {
            if (!CanAdd()) return;
            EditUtilisateur.MotDePasseHash = MotDePasse;
            EditUtilisateur.CentreId = AfficherChampCentre && CentreSelectionne != null
                ? CentreSelectionne.Id
                : null;
            await _utilisateurService.AddAsync(EditUtilisateur);
            await LoadUtilisateursAsync();
        }

        private async Task DeleteUtilisateurAsync()
        {
            if (SelectedUtilisateur == null) return;
            await _utilisateurService.DeleteAsync(SelectedUtilisateur.Id);
            await LoadUtilisateursAsync();
        }

        private async Task UpdateUtilisateurAsync()
        {
            if (SelectedUtilisateur == null) return;

            SelectedUtilisateur.Nom = EditUtilisateur.Nom;
            SelectedUtilisateur.Role = EditUtilisateur.Role;
            SelectedUtilisateur.MotDePasseHash = MotDePasse;
            SelectedUtilisateur.CentreId = AfficherChampCentre && CentreSelectionne != null
                ? CentreSelectionne.Id
                : null;

            await _utilisateurService.UpdateAsync(SelectedUtilisateur);
            await LoadUtilisateursAsync();
        }

        private bool CanAdd()
            => !string.IsNullOrWhiteSpace(EditUtilisateur?.Nom)
            && !string.IsNullOrWhiteSpace(MotDePasse)
            && (!AfficherChampCentre || (CentreSelectionne != null));

        private bool CanDeleteOrEdit()
            => SelectedUtilisateur != null;
    }
}
