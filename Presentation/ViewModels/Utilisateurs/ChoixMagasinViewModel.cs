using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionCollectes.Presentation.ViewModels.Utilisateurs
{
    public partial class ChoixMagasinViewModel : ObservableObject
    {
        private readonly MagasinService _magasinService;
        [ObservableProperty]
        private Collecte collecte;
        private readonly int _centreId;
        private readonly DashboardUtilisateurViewModel _dashboard;

        [ObservableProperty]
        private ObservableCollection<Magasin> magasins = new();

       

        public ChoixMagasinViewModel(MagasinService magasinService, Collecte collecte, int centreId, DashboardUtilisateurViewModel dashboard)
        {
            this.collecte = collecte;

            _magasinService = magasinService;
            _centreId = centreId;
            _dashboard = dashboard;
            _ = LoadMagasinsAsync();
        }

        private async Task LoadMagasinsAsync()
        {
            var result = await _magasinService.GetMagasinsActifsParCentreAsync(_centreId);
            Magasins = new ObservableCollection<Magasin>(result);
        }

        [RelayCommand]
        private void AccederSaisieProduit(Magasin magasin)
        {
         
        }
    }
}
