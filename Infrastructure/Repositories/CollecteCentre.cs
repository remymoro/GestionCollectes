using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionCollectes.Infrastructure.Repositories
{
    public class CollecteCentreRepository : IRepository<CollecteCentre>
    {
        private readonly AppDbContext _db;

        public CollecteCentreRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CollecteCentre>> GetAllAsync()
        {
            return await _db.CollecteCentres
                .Include(cc => cc.Collecte)
                .Include(cc => cc.Centre)
                .ToListAsync();
        }

        public async Task<CollecteCentre?> GetByIdAsync(int id)
        {
            // Comme la clé est composite, tu ne peux pas utiliser seulement "id"
            // Soit tu n’implémentes pas cette méthode, soit tu adaptes la signature (CollecteId, CentreId)
            // On peut mettre "return null;" ou lever une exception si tu veux
            return null;
        }

        public async Task AddAsync(CollecteCentre entity)
        {
            _db.CollecteCentres.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(CollecteCentre entity)
        {
            // Pas d’update typique pour une liaison N-N (on supprime puis on recrée en général)
            // Ici, rien à faire (ou éventuellement lever une NotImplementedException)
            throw new System.NotImplementedException("Update not supported for CollecteCentre (N-N liaison)");
        }

        public async Task DeleteAsync(int id)
        {
            // Idem, pas vraiment de delete par id unique ici. À la place : il faut CollecteId et CentreId
            // Tu peux lever une exception ou laisser vide
            throw new System.NotImplementedException("Delete by single id not supported for CollecteCentre");
        }

        // Méthode personnalisée : supprimer une liaison par clé composite
        public async Task DeleteAsync(int collecteId, int centreId)
        {
            var liaison = await _db.CollecteCentres
                .FirstOrDefaultAsync(cc => cc.CollecteId == collecteId && cc.CentreId == centreId);
            if (liaison != null)
            {
                _db.CollecteCentres.Remove(liaison);
                await _db.SaveChangesAsync();
            }
        }
    }
}
