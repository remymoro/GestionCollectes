using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class CollecteViewModel : ObservableObject
    {
        private readonly CollecteService _collecteService;
        private readonly CentreService _centreService;
        private readonly CollecteCentreService _collecteCentreService;

        public ObservableCollection<Collecte> Collectes { get; } = new();

        [ObservableProperty]
        private ObservableCollection<CentreCheckable> centres = new();  // ✅


        [ObservableProperty] private string nom = string.Empty;
        [ObservableProperty] private DateTime dateDebut = DateTime.Today;
        [ObservableProperty] private DateTime dateFin = DateTime.Today.AddDays(1);
        [ObservableProperty] private bool toutCocher;

        public IAsyncRelayCommand AddCollecteCommand { get; }

        public CollecteViewModel(
            CollecteService collecteService,
            CentreService centreService,
            CollecteCentreService collecteCentreService)
        {
            _collecteService = collecteService;
            _centreService = centreService;
            _collecteCentreService = collecteCentreService;

            AddCollecteCommand = new AsyncRelayCommand(AddCollecteAsync);

            Task.Run(LoadCollectesAsync);
            Task.Run(LoadCentresAsync);
        }

        public async Task LoadCollectesAsync()
        {
            var result = await _collecteService.GetAllAsync();
            App.Current.Dispatcher.Invoke(() =>
            {
                Collectes.Clear();
                foreach (var collecte in result)
                    Collectes.Add(collecte);
            });
        }

        private async Task LoadCentresAsync()
        {
            var centres = await _centreService.GetAllAsync();
            App.Current.Dispatcher.Invoke(() =>
            {
                Centres.Clear();
                foreach (var centre in centres)
                    Centres.Add(new CentreCheckable { Id = centre.Id, Nom = centre.Nom });
            });
        }

        partial void OnToutCocherChanged(bool value)
        {
            foreach (var centre in Centres)
                centre.IsChecked = value;
        }

        private async Task AddCollecteAsync()
        {
            // 1. Ajoute la collecte
            var newCollecte = new Collecte
            {
                Nom = Nom,
                DateDebut = DateDebut,
                DateFin = DateFin,
                Statut = GestionCollectes.Domain.Enums.StatutCollecte.Planifiee
            };
            await _collecteService.AddAsync(newCollecte);

            // 2. Lier aux centres sélectionnés
            var centresSelectionnes = Centres.Where(c => c.IsChecked).Select(c => c.Id).ToList();
            var liaisons = centresSelectionnes
                .Select(cId => new CollecteCentre { CollecteId = newCollecte.Id, CentreId = cId })
                .ToList();
            await _collecteCentreService.AddRangeAsync(liaisons);

            // 3. Rafraîchit la liste et reset formulaire
            await LoadCollectesAsync();
            Nom = string.Empty;
            DateDebut = DateTime.Today;
            DateFin = DateTime.Today.AddDays(1);
            ToutCocher = false;
            foreach (var centre in Centres)
                centre.IsChecked = false;
        }
    }
}
