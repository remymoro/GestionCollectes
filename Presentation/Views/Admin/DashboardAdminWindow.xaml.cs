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
using GestionCollectes.Presentation.ViewModels.Admin;
using Microsoft.Extensions.DependencyInjection;



namespace GestionCollectes.Presentation.Views.Admin
{
    /// <summary>
    /// Logique d'interaction pour DashboardAdminWindow.xaml
    /// </summary>
    public partial class DashboardAdminWindow : Window
    {
        public DashboardAdminWindow()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<DashboardAdminViewModel>();
        }
    }
}
