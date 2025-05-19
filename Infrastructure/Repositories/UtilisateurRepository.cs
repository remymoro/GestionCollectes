// /Infrastructure/Repositories/UtilisateurRepository.cs
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCollectes.Infrastructure.Repositories
{
    public class UtilisateurRepository : IRepository<Utilisateur>
    {
        private readonly AppDbContext _context;

        public UtilisateurRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Utilisateur>> GetAllAsync()
        {
            return await _context.Utilisateurs.ToListAsync();
        }

        public async Task<Utilisateur?> GetByIdAsync(int id)
        {
            return await _context.Utilisateurs.FindAsync(id);
        }

        public async Task AddAsync(Utilisateur entity)
        {
            _context.Utilisateurs.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Utilisateur entity)
        {
            _context.Utilisateurs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Utilisateurs.FindAsync(id);
            if (entity != null)
            {
                _context.Utilisateurs.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
