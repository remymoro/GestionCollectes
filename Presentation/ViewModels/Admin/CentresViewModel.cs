using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class CentresViewModel : ObservableObject
    {
        private readonly CentreService _service;

        public ObservableCollection<Centre> Centres { get; } = new();

        [ObservableProperty] private string nom = string.Empty;
        [ObservableProperty] private string adresse = string.Empty;
        [ObservableProperty] private string ville = string.Empty;
        [ObservableProperty] private string codePostal = string.Empty;
        [ObservableProperty] private string responsable = string.Empty;
        [ObservableProperty] private string telephone = string.Empty;
        [ObservableProperty] private Centre? selectedCentre;



        // N'oublie pas d'appeler NotifyCanExecuteChanged à chaque modif d’un champ :
        partial void OnNomChanged(string value) => AddCentreCommand.NotifyCanExecuteChanged();
        partial void OnAdresseChanged(string value) => AddCentreCommand.NotifyCanExecuteChanged();
        partial void OnVilleChanged(string value) => AddCentreCommand.NotifyCanExecuteChanged();
        partial void OnCodePostalChanged(string value) => AddCentreCommand.NotifyCanExecuteChanged();
        partial void OnResponsableChanged(string value) => AddCentreCommand.NotifyCanExecuteChanged();
        partial void OnTelephoneChanged(string value) => AddCentreCommand.NotifyCanExecuteChanged();

        public IAsyncRelayCommand AddCentreCommand { get; }
        public IAsyncRelayCommand<Centre> DeleteCentreCommand { get; }
        public IAsyncRelayCommand EditCentreCommand { get; }

        public CentresViewModel(CentreService service)
        {
            _service = service;

            AddCentreCommand = new AsyncRelayCommand(AddCentreAsync);
            DeleteCentreCommand = new AsyncRelayCommand<Centre>(DeleteCentreAsync);
            EditCentreCommand = new AsyncRelayCommand(EditCentreAsync);

            Task.Run(LoadCentresAsync);
        }

        public async Task LoadCentresAsync()
        {
            var centres = await _service.GetAllAsync();
            App.Current.Dispatcher.Invoke(() =>
            {
                Centres.Clear();
                foreach (var centre in centres)
                    Centres.Add(centre);
            });
        }

        private async Task AddCentreAsync()
        {
            // Vérification simple des champs obligatoires
            if (string.IsNullOrWhiteSpace(Nom) ||
                string.IsNullOrWhiteSpace(Adresse) ||
                string.IsNullOrWhiteSpace(Ville) ||
                string.IsNullOrWhiteSpace(CodePostal) ||
                string.IsNullOrWhiteSpace(Responsable) ||
                string.IsNullOrWhiteSpace(Telephone))
            {
                // Option : Afficher un message à l'utilisateur
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
            await LoadCentresAsync();

            // Reset des champs après ajout
            Nom = Adresse = Ville = CodePostal = Responsable = Telephone = string.Empty;
        }

        private async Task DeleteCentreAsync(Centre centre)
        {
            if (centre == null) return;

            // Message de confirmation (optionnel)
            var result = System.Windows.MessageBox.Show(
                $"Voulez-vous vraiment supprimer le centre « {centre.Nom} » ?",
                "Suppression",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning
            );
            if (result != System.Windows.MessageBoxResult.Yes) return;

            await _service.DeleteAsync(centre.Id);
            await LoadCentresAsync();
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
            await LoadCentresAsync();

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
    }
}
