using System.Windows;
using GestionCollectes.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace GestionCollectes.Presentation.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            // Injection du ViewModel via le container DI
            DataContext = App.ServiceProvider.GetRequiredService<LoginViewModel>();
        }

        // Gestion du PasswordBox (WPF ne supporte pas le binding direct du Password)
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm && sender is System.Windows.Controls.PasswordBox pb)
            {
                vm.MotDePasse = pb.Password;
            }
        }
    }
}
