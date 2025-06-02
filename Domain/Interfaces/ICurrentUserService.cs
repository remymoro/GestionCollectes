using GestionCollectes.Domain.Entities;

namespace GestionCollectes.Domain.Interfaces
{
    public interface ICurrentUserService
    {
        Utilisateur? CurrentUser { get; }
        bool IsLoggedIn { get; }
        void SetCurrentUser(Utilisateur user);
        void ClearCurrentUser();
    }
}
