using CommunityToolkit.Mvvm.ComponentModel;
using GestionCollectes.Presentation.ViewModels.Admin;



namespace GestionCollectes.Presentation.Navigations
{
    public partial class NavigationService : ObservableObject, INavigationService
    {
        [ObservableProperty]
        private ObservableObject? currentView;

        private readonly Func<CollecteViewModel> _collectesFactory;
        private readonly Func<CentresViewModel> _centresFactory;
        private readonly Func<UtilisateursViewModel> _utilisateursFactory;
        private readonly Func<MagasinsViewModel> _magasinsFactory;
        private readonly Func<MagasinsActivationViewModel> _magasinsActivationFactory;

        public NavigationService(
            Func<CollecteViewModel> collectesFactory,
            Func<CentresViewModel> centresFactory,
            Func<UtilisateursViewModel> utilisateursFactory,
            Func<MagasinsViewModel> magasinsFactory,
            Func<MagasinsActivationViewModel> magasinsActivationFactory
        )
        {
            _collectesFactory = collectesFactory;
            _centresFactory = centresFactory;
            _utilisateursFactory = utilisateursFactory;
            _magasinsFactory = magasinsFactory;
            _magasinsActivationFactory = magasinsActivationFactory;

            NavigateToCollectes(); // Vue par défaut
        }

        public void NavigateToCollectes() => CurrentView = _collectesFactory();
        public void NavigateToCentres() => CurrentView = _centresFactory();
        public void NavigateToUtilisateurs() => CurrentView = _utilisateursFactory();
        public void NavigateToMagasins() => CurrentView = _magasinsFactory();
        public void NavigateToMagasinsActivation() => CurrentView = _magasinsActivationFactory();
    }
}
