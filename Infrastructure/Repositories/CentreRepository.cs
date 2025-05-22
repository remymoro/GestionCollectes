using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCollectes.Infrastructure.Repositories
{
    public class CentreRepository : IRepository<Centre>
    {
        private readonly AppDbContext _context;

        public CentreRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Centre>> GetAllAsync()
        {
            return await _context.Centres.ToListAsync();
        }

        public async Task<Centre?> GetByIdAsync(int id)
        {
            return await _context.Centres.FindAsync(id);
        }

        public async Task AddAsync(Centre entity)
        {
            _context.Centres.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Centre entity)
        {
            _context.Centres.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Centres.FindAsync(id);
            if (entity != null)
            {
                _context.Centres.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
