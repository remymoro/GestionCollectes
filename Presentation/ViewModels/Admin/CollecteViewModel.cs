using CommunityToolkit.Mvvm.ComponentModel;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;


namespace GestionCollectes.Presentation.ViewModels.Admin
{

    public partial class CollecteViewModel : ObservableObject
    {
        private readonly CollecteService _service;

        public IAsyncRelayCommand AddCollecteCommand { get; }

        public ObservableCollection<Collecte> Collectes { get; set; } = new();

        // Propriétés liées à la saisie d'une nouvelle collecte
        [ObservableProperty] private string nom;
        [ObservableProperty] private DateTime dateDebut = DateTime.Today;
        [ObservableProperty] private DateTime dateFin = DateTime.Today.AddDays(1);
        [ObservableProperty] private string lieu;

        // Commande pour ajouter une collecte

        public CollecteViewModel(CollecteService service)
        {
            _service = service;
            AddCollecteCommand = new AsyncRelayCommand(AddCollecteAsync);

            Task.Run(LoadCollectesAsync);
        }

        public async Task LoadCollectesAsync()
        {
            var result = await _service.GetAllAsync();
            App.Current.Dispatcher.Invoke(() =>
            {
                Collectes.Clear();
                foreach (var collecte in result)
                    Collectes.Add(collecte);
            });
        }

        private async Task AddCollecteAsync()
        {
            var newCollecte = new Collecte
            {
                Nom = Nom,
                DateDebut = DateDebut,
                DateFin = DateFin,
                // Statut, etc.
            };

            await _service.AddAsync(newCollecte);
            await LoadCollectesAsync();

            // Remise à zéro des champs
            Nom = string.Empty;
            DateDebut = DateTime.Today;
            DateFin = DateTime.Today.AddDays(1);
            Lieu = string.Empty;
        }

    }

}
