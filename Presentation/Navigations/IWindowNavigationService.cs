using GestionCollectes.Domain.Enums;

namespace GestionCollectes.Presentation.Navigation
{
    public interface IWindowNavigationService
    {
        /// <summary>
        /// Ouvre la fenêtre Login (et ferme les autres dashboards si besoin).
        /// </summary>
        void ShowLogin();

        /// <summary>
        /// Ouvre la fenêtre Dashboard adaptée au rôle (Admin, Centre, Utilisateur).
        /// Ferme la LoginWindow si nécessaire.
        /// </summary>
        void ShowDashboardForRole(RoleUtilisateur role);

        /// <summary>
        /// (Optionnel) Déconnexion globale : ferme tous les dashboards, retourne au login.
        /// </summary>
        void Logout();
    }
}
