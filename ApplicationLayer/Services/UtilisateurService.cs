using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionCollectes.ApplicationLayer.Services
{
    public class UtilisateurService
    {
        private readonly IRepository<Utilisateur> _repository;

        public UtilisateurService(IRepository<Utilisateur> repository)
        {
            _repository = repository;
        }

        // 1. Récupérer tous les utilisateurs
        public Task<IEnumerable<Utilisateur>> GetAllAsync() => _repository.GetAllAsync();

        // 2. Récupérer un utilisateur par NOM
        public async Task<Utilisateur?> GetByNomAsync(string nom)
        {
            var users = await _repository.GetAllAsync();
            return users.FirstOrDefault(u => u.Nom.Equals(nom, System.StringComparison.OrdinalIgnoreCase));
        }

        // 3. Authentification par NOM + mot de passe (hash à vérifier)
        public async Task<Utilisateur?> AuthenticateAsync(string nom, string password)
        {
            var utilisateur = await GetByNomAsync(nom);
            if (utilisateur == null) return null;
            // Remplace par une vraie vérif de hash pour la prod !
            if (utilisateur.MotDePasseHash == password)
                return utilisateur;
            return null;
        }

        // 4. Ajout d'utilisateur
        public Task AddAsync(Utilisateur utilisateur) => _repository.AddAsync(utilisateur);

        // 5. Suppression
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);

        // 6. Modification
        public Task UpdateAsync(Utilisateur utilisateur) => _repository.UpdateAsync(utilisateur);
    }
}
