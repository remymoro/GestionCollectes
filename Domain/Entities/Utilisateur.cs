using CommunityToolkit.Mvvm.ComponentModel;
using GestionCollectes.Domain.Enums;

namespace GestionCollectes.Domain.Entities
{
    public partial class Utilisateur : ObservableObject
    {
        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string nom = string.Empty;

        [ObservableProperty]
        private string motDePasseHash = string.Empty;

        [ObservableProperty]
        private RoleUtilisateur role = RoleUtilisateur.Centre; // valeur par défaut !


        [ObservableProperty]
        private int? centreId;
    }
}
