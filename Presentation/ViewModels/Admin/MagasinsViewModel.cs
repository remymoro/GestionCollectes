using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Presentation.Messages;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class MagasinsViewModel : ObservableObject
    {
        private readonly MagasinService _magasinService;
        private readonly CentreService _centreService;

        [ObservableProperty]
        private ObservableCollection<Centre> centres = new();

        [ObservableProperty]
        private Centre? centreSelectionne;

        [ObservableProperty]
        private string newNom = string.Empty;

        [ObservableProperty]
        private string newAdresse = string.Empty;

        [ObservableProperty]
        private string? newImagePath;

        public IRelayCommand AjouterMagasinCommand { get; }
        public IRelayCommand ParcourirImageCommand { get; }

        public MagasinsViewModel(MagasinService magasinService, CentreService centreService)
        {
            WeakReferenceMessenger.Default.Register<CentresChangedMessage>(this, (r, m) =>
            {
                System.Diagnostics.Debug.WriteLine("Message CentresChanged reçu !");
                _ = LoadCentresAsync();
            });


            _magasinService = magasinService;
            _centreService = centreService;

            AjouterMagasinCommand = new RelayCommand(AjouterMagasin, PeutAjouterMagasin);
            ParcourirImageCommand = new RelayCommand(ParcourirImage);

            // Met à jour l’état du bouton quand on modifie des champs
            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName is nameof(NewNom) or nameof(NewAdresse) or nameof(CentreSelectionne))
                    AjouterMagasinCommand.NotifyCanExecuteChanged();
            };

            // Chargement initial des centres
            _ = LoadCentresAsync();
        }

        private bool PeutAjouterMagasin()
        {
            return !string.IsNullOrWhiteSpace(NewNom)
                && !string.IsNullOrWhiteSpace(NewAdresse)
                && CentreSelectionne != null;
        }

        private async void AjouterMagasin()
        {
            if (CentreSelectionne == null) return;

            string? savedImagePath = null;
            if (!string.IsNullOrWhiteSpace(NewImagePath) && File.Exists(NewImagePath))
            {
                // Copie l'image sélectionnée dans /Resources/images/uploaded
                var dossierImages = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Presentation", "Resources", "Images", "Uploaded"
                );
                if (!Directory.Exists(dossierImages))
                    Directory.CreateDirectory(dossierImages);

                var fileName = Path.GetFileName(NewImagePath);
                var cible = Path.Combine(dossierImages, fileName);

                File.Copy(NewImagePath, cible, overwrite: true);
                // Chemin relatif pour stockage BDD (pour affichage plus tard avec un converter)
                savedImagePath = $"Resources/Images/Uploaded/{fileName}";
            }

            var magasin = new Magasin
            {
                Nom = NewNom.Trim(),
                Adresse = NewAdresse.Trim(),
                ImagePath = savedImagePath,
                CentreId = CentreSelectionne.Id
            };
            await _magasinService.AddAsync(magasin);

            // Reset le formulaire
            NewNom = string.Empty;
            NewAdresse = string.Empty;
            NewImagePath = null;
        }

        private void ParcourirImage()
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Images (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp",
                Title = "Sélectionnez une image"
            };
            if (ofd.ShowDialog() == true)
            {
                NewImagePath = ofd.FileName;
            }
        }

        public async Task RefreshCentresAsync()
        {
            await LoadCentresAsync();
        }

        private async Task LoadCentresAsync()
        {
            var allCentres = await _centreService.GetAllAsync();
            Centres = new ObservableCollection<Centre>(allCentres);
            CentreSelectionne = Centres.FirstOrDefault();
        }
    }
}
