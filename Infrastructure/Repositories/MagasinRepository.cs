using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCollectes.Infrastructure.Repositories
{
    public class MagasinRepository : Repository<Magasin>, IRepository<Magasin>
    {
        public MagasinRepository(AppDbContext context) : base(context) { }

        public override async Task<IEnumerable<Magasin>> GetAllAsync()
        {
            // Inclut le Centre lié au magasin pour l’affichage
            return await _context.Magasins
                .Include(m => m.Centre)
                .ToListAsync();
        }

        public override async Task<Magasin?> GetByIdAsync(int id) // Changed long back to int for Id
        {
            return await _context.Magasins
                .Include(m => m.Centre)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // AddAsync, UpdateAsync, DeleteAsync are inherited from Repository<Magasin>
        // Add any Magasin-specific methods here if needed in the future
    }
}
