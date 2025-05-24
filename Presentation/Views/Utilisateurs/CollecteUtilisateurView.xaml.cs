using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Infrastructure.Data;
using GestionCollectes.Infrastructure.Repositories;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestionCollectes.Presentation.Views.Utilisateurs
{
    /// <summary>
    /// Logique d'interaction pour CollecteUtilisateurView.xaml
    /// </summary>
    public partial class CollecteUtilisateurView : UserControl
    {
        public CollecteUtilisateurView()
        {
            InitializeComponent();



        }

        private void GuideButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "GUIDE UTILISATEUR\n\n" +
                "1. Choisissez la collecte souhaitée dans la liste.\n" +
                "2. Cliquez sur « Accéder » pour démarrer.\n" +
                "3. Si le bouton est gris, la collecte n'est pas encore accessible.\n" +
                "4. Pour toute question, demandez à votre responsable.",
                "Guide utilisateur",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

    }
}
