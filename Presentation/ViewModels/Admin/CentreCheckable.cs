using CommunityToolkit.Mvvm.ComponentModel; // For ObservableObject
using GestionCollectes.Domain.Entities;

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class CentreCheckable : ObservableObject
    {
        [ObservableProperty]
        private bool _isSelected;

        public Centre Centre { get; }

        public CentreCheckable(Centre centre)
        {
            Centre = centre;
        }

        public int Id => Centre.Id;
        public string Nom => Centre.Nom;
        // Add other properties from Centre you might want to display directly
    }
}
