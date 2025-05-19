using GestionCollectes.Presentation.ViewModels;
using GestionCollectes.ApplicationLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace GestionCollectes.Presentation.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _vm;

        public LoginWindow()
        {
            InitializeComponent();
            var utilisateurService = App.ServiceProvider.GetRequiredService<UtilisateurService>();
            _vm = new LoginViewModel(utilisateurService);
            _vm.ConnexionRéussie += OnConnexionRéussie;
            DataContext = _vm;
        }

        private void OnConnexionRéussie(GestionCollectes.Domain.Entities.Utilisateur utilisateur)
        {

            App.UtilisateurCourant = utilisateur;
            // Redirection Dashboard selon le rôle
            Window dashboard;
            switch (utilisateur.Role)
            {
                case Domain.Enums.RoleUtilisateur.SiegeAdmin:
                    dashboard = new Admin.DashboardAdminWindow();
                    break;
                case Domain.Enums.RoleUtilisateur.Centre:
                    dashboard = new Centre.DashboardCentreWindow();
                    break;
                default:
                    dashboard = new Utilisateur.DashboardUtilisateurWindow();
                    break;
            }
            dashboard.Show();
            this.Close();
        }



        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm && sender is PasswordBox pb)
            {
                vm.MotDePasse = pb.Password;
            }
        }

    }
}
