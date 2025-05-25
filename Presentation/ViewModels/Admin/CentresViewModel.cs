using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Presentation.Stores;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class CentresViewModel : ObservableObject
    {
        private readonly CentreService _service;
        private readonly CentreStore _store;

        // 🔗 Plus besoin d'une ObservableCollection locale !
        public ObservableCollection<Centre> Centres => _store.Centres;

        [ObservableProperty] private string nom = string.Empty;
        [ObservableProperty] private string adresse = string.Empty;
        [ObservableProperty] private string ville = string.Empty;
        [ObservableProperty] private string codePostal = string.Empty;
        [ObservableProperty] private string responsable = string.Empty;
        [ObservableProperty] private string telephone = string.Empty;
        [ObservableProperty] private Centre? selectedCentre;

        public IRelayCommand ReinitialiserCommand { get; }
        public IAsyncRelayCommand AddCentreCommand { get; }
        public IAsyncRelayCommand<Centre> DeleteCentreCommand { get; }
        public IAsyncRelayCommand EditCentreCommand { get; }

        public CentresViewModel(CentreService service, CentreStore store)
        {
            _service = service;
            _store = store;

            AddCentreCommand = new AsyncRelayCommand(AddCentreAsync);
            DeleteCentreCommand = new AsyncRelayCommand<Centre>(DeleteCentreAsync);
            EditCentreCommand = new AsyncRelayCommand(EditCentreAsync);
            ReinitialiserCommand = new RelayCommand(ReinitialiserFormulaire);

            // ⚡️ À l'initialisation : charger les centres dans le store (si pas déjà fait)
            _ = LoadCentresInStoreAsync();
        }

        // Charge et met à jour la liste dans le store, pas juste localement
        private async Task LoadCentresInStoreAsync()
        {
            var centres = await _service.GetAllAsync();
            _store.SetCentres(centres); // → Notifie automatiquement tous les ViewModels branchés
        }

        private async Task AddCentreAsync()
        {
            if (string.IsNullOrWhiteSpace(Nom) ||
                string.IsNullOrWhiteSpace(Adresse) ||
                string.IsNullOrWhiteSpace(Ville) ||
                string.IsNullOrWhiteSpace(CodePostal) ||
                string.IsNullOrWhiteSpace(Responsable) ||
                string.IsNullOrWhiteSpace(Telephone))
            {
                System.Windows.MessageBox.Show("Veuillez remplir tous les champs avant d'ajouter le centre.", "Champs manquants", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var centre = new Centre
            {
                Nom = Nom,
                Adresse = Adresse,
                Ville = Ville,
                CodePostal = CodePostal,
                Responsable = Responsable,
                Telephone = Telephone
            };
            await _service.AddAsync(centre);

            // 🔄 Recharge à jour (et donc notification partout)
            await LoadCentresInStoreAsync();


            // Reset
            Nom = Adresse = Ville = CodePostal = Responsable = Telephone = string.Empty;
        }

        private async Task DeleteCentreAsync(Centre centre)
        {
            if (centre == null) return;
            var result = System.Windows.MessageBox.Show(
                $"Voulez-vous vraiment supprimer le centre « {centre.Nom} » ?",
                "Suppression",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning
            );
            if (result != System.Windows.MessageBoxResult.Yes) return;

            await _service.DeleteAsync(centre.Id);
            await LoadCentresInStoreAsync();
        }

        private async Task EditCentreAsync()
        {
            if (SelectedCentre == null)
                return;

            SelectedCentre.Nom = Nom;
            SelectedCentre.Adresse = Adresse;
            SelectedCentre.Ville = Ville;
            SelectedCentre.CodePostal = CodePostal;
            SelectedCentre.Responsable = Responsable;
            SelectedCentre.Telephone = Telephone;

            await _service.UpdateAsync(SelectedCentre);
            await LoadCentresInStoreAsync();

            // Reset
            SelectedCentre = null;
            Nom = Adresse = Ville = CodePostal = Responsable = Telephone = string.Empty;
        }

        partial void OnSelectedCentreChanged(Centre? value)
        {
            if (value != null)
            {
                Nom = value.Nom;
                Adresse = value.Adresse;
                Ville = value.Ville;
                CodePostal = value.CodePostal;
                Responsable = value.Responsable;
                Telephone = value.Telephone;
            }
            else
            {
                Nom = Adresse = Ville = CodePostal = Responsable = Telephone = string.Empty;
            }
        }

        private void ReinitialiserFormulaire()
        {
            Nom = string.Empty;
            Adresse = string.Empty;
            Ville = string.Empty;
            CodePostal = string.Empty;
            Responsable = string.Empty;
            Telephone = string.Empty;
            SelectedCentre = null;
        }
    }
}
