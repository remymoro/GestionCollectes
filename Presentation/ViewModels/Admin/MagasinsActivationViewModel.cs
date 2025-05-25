using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class MagasinsActivationViewModel : ObservableObject
    {
        private readonly CentreService _centreService;
        private readonly MagasinService _magasinService;

        [ObservableProperty]
        private ObservableCollection<Centre> centres = new();

        [ObservableProperty]
        private Centre? centreSelectionne;

        [ObservableProperty]
        private ObservableCollection<Magasin> magasins = new();

        public IRelayCommand EnregistrerCommand { get; }

        public MagasinsActivationViewModel(CentreService centreService, MagasinService magasinService)
        {
            _centreService = centreService;
            _magasinService = magasinService;

            EnregistrerCommand = new RelayCommand(async () => await EnregistrerAsync());

            // Charger la liste des centres au démarrage
            _ = ChargerCentresAsync();

            // Surveille les changements de centre
            PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(CentreSelectionne))
                    await ChargerMagasinsAsync();
            };
        }

        private async Task ChargerCentresAsync()
        {
            var allCentres = await _centreService.GetAllAsync();
            Centres = new ObservableCollection<Centre>(allCentres);
            CentreSelectionne = Centres.FirstOrDefault();
        }

        private async Task ChargerMagasinsAsync()
        {
            Magasins.Clear();
            if (CentreSelectionne == null) return;

            var allMagasins = await _magasinService.GetAllAsync();
            var filtered = allMagasins.Where(m => m.CentreId == CentreSelectionne.Id).ToList();
            Magasins = new ObservableCollection<Magasin>(filtered);
        }

        private async Task EnregistrerAsync()
        {
            foreach (var magasin in Magasins)
                await _magasinService.UpdateAsync(magasin);
        }
    }
}
