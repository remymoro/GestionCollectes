using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCollectes.Infrastructure.Repositories
{
    public class MagasinRepository : IRepository<Magasin>
    {
        private readonly AppDbContext _context;

        public MagasinRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Magasin>> GetAllAsync()
        {
            // Inclut le Centre lié au magasin pour l’affichage
            return await _context.Magasins
                .Include(m => m.Centre)
                .ToListAsync();
        }

        public async Task<Magasin?> GetByIdAsync(int id)
        {
            return await _context.Magasins
                .Include(m => m.Centre)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Magasin magasin)
        {
            _context.Magasins.Add(magasin);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Magasin magasin)
        {
            _context.Magasins.Update(magasin);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Magasins.FindAsync(id);
            if (entity != null)
            {
                _context.Magasins.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
