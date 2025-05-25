using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GestionCollectes.ApplicationLayer.Services
{
    public class CollecteCentreService
    {
        private readonly IRepository<CollecteCentre> _repo;

        public CollecteCentreService(IRepository<CollecteCentre> repo)
        {
            _repo = repo;
        }

        // Ajouter une liaison
        public async Task AddAsync(CollecteCentre liaison)
        {
            await _repo.AddAsync(liaison);
        }

        // Ajouter plusieurs liaisons d’un coup
        public async Task AddRangeAsync(IEnumerable<CollecteCentre> liaisons)
        {
            foreach (var liaison in liaisons)
                await _repo.AddAsync(liaison);
        }

        // Récupérer toutes les liaisons
        public async Task<List<CollecteCentre>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.ToList();
        }

        // Récupérer toutes les collectes pour un centre donné
        public async Task<List<Collecte>> GetCollectesForCentreAsync(int centreId)
        {
            var all = await _repo.GetAllAsync();
            return all.Where(cc => cc.CentreId == centreId)
                      .Select(cc => cc.Collecte)
                      .ToList();
        }

        // Récupérer tous les centres pour une collecte donnée
        public async Task<List<Centre>> GetCentresForCollecteAsync(int collecteId)
        {
            var all = await _repo.GetAllAsync();
            return all.Where(cc => cc.CollecteId == collecteId)
                      .Select(cc => cc.Centre)
                      .ToList();
        }

        // Supprimer une liaison Collecte <-> Centre (clé composite)
        public async Task DeleteAsync(int collecteId, int centreId)
        {
            // Méthode personnalisée du repo si besoin
            if (_repo is Infrastructure.Repositories.CollecteCentreRepository repoSpecifique)
                await repoSpecifique.DeleteAsync(collecteId, centreId);
            // Sinon : ignore ou fais rien (repo générique ne sait pas faire sur clé composite)
        }
    }
}
