using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionCollectes.Infrastructure.Repositories
{
    // Implements the new specific interface instead of IRepository<CollecteCentre>
    public class CollecteCentreRepository : Repository<CollecteCentre>, ICollecteCentreRepository 
    {
        public CollecteCentreRepository(AppDbContext context) : base(context) { }

        public override async Task<IEnumerable<CollecteCentre>> GetAllAsync()
        {
            return await _context.CollecteCentres // _db changed to _context
                .Include(cc => cc.Collecte)
                .Include(cc => cc.Centre)
                .ToListAsync();
        }

        // GetByIdAsync(long id) is inherited but may not be suitable for a composite key entity.
        // Consider implementing a specific method if finding by a single ID is needed and makes sense.
        // For now, the base implementation will be inherited. If it needs to be disabled:
        // public override Task<CollecteCentre?> GetByIdAsync(int id) => throw new System.NotSupportedException("GetByIdAsync with a single ID is not supported for CollecteCentre."); 
        // The GetByIdAsync from base class is still inherited but not part of ICollecteCentreRepository.
        // AddAsync is inherited from Repository<CollecteCentre> and matches ICollecteCentreRepository.AddAsync.

        // UpdateAsync is removed as it's not in ICollecteCentreRepository and has specific logic for join tables.
        // public override async Task UpdateAsync(CollecteCentre entity) ... (Removed)

        // DeleteAsync(int id) is removed as it's not in ICollecteCentreRepository.
        // public override async Task DeleteAsync(int id) ... (Removed)

        // Méthode personnalisée pour supprimer une liaison par clé composite, part of ICollecteCentreRepository
        public async Task DeleteAsync(int collecteId, int centreId) // This matches the interface
        {
            var liaison = await _context.CollecteCentres
                .FirstOrDefaultAsync(cc => cc.CollecteId == collecteId && cc.CentreId == centreId);
            if (liaison != null)
            {
                _context.CollecteCentres.Remove(liaison);
                await _context.SaveChangesAsync();
            }
        }
    }
}
