using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
namespace GestionCollectes;



public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Accès direct via le membre static
        var service = App.ServiceProvider.GetRequiredService<CollecteService>();
        this.DataContext = new CollecteViewModel(service);
    }
}
