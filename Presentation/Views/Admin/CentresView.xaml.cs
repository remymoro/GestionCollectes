using GestionCollectes.Domain.Entities;
using GestionCollectes.Presentation.ViewModels.Admin;
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

namespace GestionCollectes.Presentation.Views.Admin
{
    /// <summary>
    /// Logique d'interaction pour CentresView.xaml
    /// </summary>
    public partial class CentresView : UserControl
    {
        public CentresView()
        {
            InitializeComponent();
            this.Loaded += CentresView_Loaded; // On abonne à l’événement Loaded
        }

        private async void CentresView_Loaded(object sender, RoutedEventArgs e)
        {
            // On récupère le ViewModel du DataContext
            if (DataContext is CentresViewModel vm)
                await vm.LoadCentresAsync();
        }


    }
}
