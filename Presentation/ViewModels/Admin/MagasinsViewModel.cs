using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Presentation.Stores;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class MagasinsViewModel : ObservableObject
    {
        private readonly MagasinService _magasinService;
        private readonly CentreStore _centreStore;

        [ObservableProperty] private ObservableCollection<Centre> centres = new();
        [ObservableProperty] private Centre? centreSelectionne;
        [ObservableProperty] private string newNom = string.Empty;
        [ObservableProperty] private string newAdresse = string.Empty;
        [ObservableProperty] private string? newImagePath;

        public IRelayCommand AjouterMagasinCommand { get; }
        public IRelayCommand ParcourirImageCommand { get; }

        public MagasinsViewModel(MagasinService magasinService, CentreStore centreStore)
        {
            _magasinService = magasinService;
            _centreStore = centreStore;

            // Bind la liste des centres au Store (et écoute les changements !)
            Centres = _centreStore.Centres;
            if (Centres.Count > 0)
                CentreSelectionne = Centres.First();

            // Rafraîchit la liste des centres si modifiée ailleurs
            _centreStore.CentresChanged += (_, __) =>
            {
                Centres = _centreStore.Centres;
                // Ajuste la sélection
                if (Centres.Count == 0)
                    CentreSelectionne = null;
                else if (CentreSelectionne == null || !Centres.Any(c => c.Id == CentreSelectionne.Id))
                    CentreSelectionne = Centres.First();
            };

            // Commandes
            AjouterMagasinCommand = new RelayCommand(AjouterMagasin, PeutAjouterMagasin);
            ParcourirImageCommand = new RelayCommand(ParcourirImage);

            // Met à jour l’état du bouton quand on modifie des champs
            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName is nameof(NewNom) || e.PropertyName is nameof(NewAdresse) || e.PropertyName is nameof(CentreSelectionne))
                    AjouterMagasinCommand.NotifyCanExecuteChanged();
            };
        }

        private bool PeutAjouterMagasin()
        {
            return !string.IsNullOrWhiteSpace(NewNom)
                && !string.IsNullOrWhiteSpace(NewAdresse)
                && CentreSelectionne != null;
        }



        private async void AjouterMagasin()
        {
            if (CentreSelectionne == null)
                return;

            string? savedImagePath = null;

            // 1. Si une image a été sélectionnée ET existe sur le disque
            if (!string.IsNullOrWhiteSpace(NewImagePath) && File.Exists(NewImagePath))
            {
                // 2. Dossier de destination dans ton appli
                var dossierImages = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Presentation", "Resources", "Images", "Uploaded"
                );
                if (!Directory.Exists(dossierImages))
                    Directory.CreateDirectory(dossierImages);

                // 3. Garde le nom du fichier (pour éviter écrasement, tu peux ajouter un GUID ou un timestamp si besoin)
                var fileName = Path.GetFileName(NewImagePath);
                var cible = Path.Combine(dossierImages, fileName);

                try
                {
                    // 4. Copie le fichier dans le dossier cible, écrase s'il existe déjà
                    File.Copy(NewImagePath, cible, overwrite: true);
                    // 5. Chemin RELATIF pour la BDD (commence bien par Presentation/…)
                    savedImagePath = $"Presentation/Resources/Images/Uploaded/{fileName}";
                }
                catch (Exception ex)
                {
                    // Message à l'utilisateur ou log pour débogage
                    System.Diagnostics.Debug.WriteLine("Erreur lors de la copie de l'image : " + ex.Message);
                    // savedImagePath restera null => image par défaut
                }
            }

            // 6. Création du magasin
            var nouveauMagasin = new Magasin
            {
                Nom = NewNom,
                Adresse = NewAdresse,
                ImagePath = savedImagePath, // Peut être null, ton converter gère l’image par défaut
                CentreId = CentreSelectionne.Id,
                // ...autres propriétés selon ton modèle
            };

            // 7. Ajoute à la BDD via ton service ou repository
            await _magasinService.AddAsync(nouveauMagasin);

            // 8. Reset le formulaire, recharge la liste, etc.
            NewNom = string.Empty;
            NewAdresse = string.Empty;
            NewImagePath = string.Empty;
            // ...ton code habituel de rafraîchissement
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
    }
}
