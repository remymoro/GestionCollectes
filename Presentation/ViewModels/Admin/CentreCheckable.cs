using CommunityToolkit.Mvvm.ComponentModel;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class CentreCheckable : ObservableObject
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        [ObservableProperty]
        private bool isChecked;
    }
}
