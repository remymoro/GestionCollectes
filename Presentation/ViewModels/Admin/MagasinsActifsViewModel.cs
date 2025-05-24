using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class MagasinsActifsViewModel : ObservableObject
    {
        private readonly MagasinService _magasinService;
        private readonly CentreService _centreService;

        [ObservableProperty] private ObservableCollection<Centre> centres = new();
        [ObservableProperty] private Centre? centreSelectionne;
        [ObservableProperty] private ObservableCollection<Magasin> magasins = new();

        public IRelayCommand<Magasin> ToggleActifCommand { get; }

        public MagasinsActifsViewModel(MagasinService magasinService, CentreService centreService)
        {
            _magasinService = magasinService;
            _centreService = centreService;

            ToggleActifCommand = new RelayCommand<Magasin>(ToggleActif);

            _ = LoadCentresAsync();
        }

        private async Task LoadCentresAsync()
        {
            var allCentres = await _centreService.GetAllAsync();
            Centres = new ObservableCollection<Centre>(allCentres);
            CentreSelectionne = Centres.FirstOrDefault();
        }

        partial void OnCentreSelectionneChanged(Centre? value)
        {
            _ = LoadMagasinsAsync();
        }

        private async Task LoadMagasinsAsync()
        {
            if (CentreSelectionne == null) { Magasins.Clear(); return; }
            var allMagasins = await _magasinService.GetAllAsync();
            var filtered = allMagasins.Where(m => m.CentreId == CentreSelectionne.Id);
            Magasins = new ObservableCollection<Magasin>(filtered);
        }

        private async void ToggleActif(Magasin magasin)
        {
            magasin.Actif = !magasin.Actif;
            await _magasinService.UpdateAsync(magasin);
            await LoadMagasinsAsync();
        }
    }
}
