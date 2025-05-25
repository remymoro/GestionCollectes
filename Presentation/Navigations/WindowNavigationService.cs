using System;
using System.Linq;
using System.Windows;
using GestionCollectes.Domain.Enums;
using GestionCollectes.Presentation.Views;
using GestionCollectes.Presentation.Views.Admin;
using GestionCollectes.Presentation.Views.Centre;
using GestionCollectes.Presentation.Views.Utilisateurs;

namespace GestionCollectes.Presentation.Navigation
{
    public class WindowNavigationService : IWindowNavigationService
    {
        private readonly Func<LoginWindow> _loginWindowFactory;
        private readonly Func<DashboardAdminWindow> _dashboardAdminFactory;
        private readonly Func<DashboardCentreWindow> _dashboardCentreFactory;
        private readonly Func<DashboardUtilisateurWindow> _dashboardUtilisateurFactory;

        public WindowNavigationService(
            Func<LoginWindow> loginWindowFactory,
            Func<DashboardAdminWindow> dashboardAdminFactory,
            Func<DashboardCentreWindow> dashboardCentreFactory,
            Func<DashboardUtilisateurWindow> dashboardUtilisateurFactory)
        {
            _loginWindowFactory = loginWindowFactory;
            _dashboardAdminFactory = dashboardAdminFactory;
            _dashboardCentreFactory = dashboardCentreFactory;
            _dashboardUtilisateurFactory = dashboardUtilisateurFactory;
        }

        public void ShowLogin()
        {
            var login = _loginWindowFactory();
            login.Show();
            CloseOtherWindows(typeof(LoginWindow));
        }

        public void ShowDashboardForRole(RoleUtilisateur role)
        {
            Window dashboard = role switch
            {
                RoleUtilisateur.SiegeAdmin => _dashboardAdminFactory(),
                RoleUtilisateur.Centre => _dashboardCentreFactory(),
                _ => _dashboardUtilisateurFactory(),
            };

            dashboard.Show();
            CloseOtherWindows(dashboard.GetType());
        }

        public void Logout()
        {
            ShowLogin();
        }

        private void CloseOtherWindows(Type typeToKeep)
        {
            foreach (var win in Application.Current.Windows.OfType<Window>().ToList())
            {
                if (win.GetType() != typeToKeep)
                    win.Close();
            }
        }
    }
}
