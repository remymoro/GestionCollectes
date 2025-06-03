using GestionCollectes.Presentation.ViewModels.Admin;
using System.Windows.Controls;
using System.Windows; // Required for RoutedEventArgs

namespace GestionCollectes.Presentation.Views.Admin
{
    public partial class CreerCollecteView : UserControl
    {
        public CreerCollecteView()
        {
            InitializeComponent();
            // DataContext can be set here or in XAML or by DI framework.
            // For simplicity, if not using a DI framework to inject ViewModel into View:
            // this.DataContext = new CreerCollecteViewModel(parameters...);
            // However, with DI, the ViewModel is usually resolved by the DI container.

            // Automatically load centres when the view is loaded
            this.Loaded += CreerCollecteView_Loaded;
        }

        private async void CreerCollecteView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreerCollecteViewModel viewModel)
            {
                if (viewModel.LoadCentresCommand.CanExecute(null))
                {
                    await viewModel.LoadCentresCommand.ExecuteAsync(null);
                }
            }
        }
    }
}
