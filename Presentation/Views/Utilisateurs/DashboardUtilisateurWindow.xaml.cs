using GestionCollectes.Presentation.ViewModels.Utilisateurs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestionCollectes.Presentation.Views.Utilisateurs
{
    /// <summary>
    /// Logique d'interaction pour DashboardUtilisateurWindow.xaml
    /// </summary>
    public partial class DashboardUtilisateurWindow : Window
    {
        public DashboardUtilisateurWindow()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<GestionCollectes.Presentation.ViewModels.Utilisateurs.DashboardUtilisateurViewModel>();
        }

    }


     
}
