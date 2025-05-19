using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCollectes.Infrastructure.Repositories
{
    public class CollecteRepository : IRepository<Collecte>
    {
        private readonly AppDbContext _context;
        public CollecteRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Collecte>> GetAllAsync() => await _context.Collectes.ToListAsync();
        public async Task<Collecte?> GetByIdAsync(int id) => await _context.Collectes.FindAsync(id);
        public async Task AddAsync(Collecte collecte)
        {
            _context.Collectes.Add(collecte);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Collecte collecte)
        {
            _context.Collectes.Update(collecte);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Collectes.FindAsync(id);
            if (entity != null)
            {
                _context.Collectes.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
