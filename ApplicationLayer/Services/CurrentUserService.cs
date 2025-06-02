using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;

namespace GestionCollectes.ApplicationLayer.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private Utilisateur? _currentUser;

        public Utilisateur? CurrentUser => _currentUser;

        public bool IsLoggedIn => _currentUser != null;

        public void SetCurrentUser(Utilisateur user)
        {
            _currentUser = user;
            // Potentiellement, déclencher un événement ici si d'autres parties de l'application
            // doivent réagir dynamiquement au changement d'utilisateur.
        }

        public void ClearCurrentUser()
        {
            _currentUser = null;
            // Potentiellement, déclencher un événement ici.
        }
    }
}
