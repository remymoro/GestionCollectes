using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCollectes.Infrastructure.Repositories
{
    public class CollecteRepository : Repository<Collecte>, IRepository<Collecte>
    {
        public CollecteRepository(AppDbContext context) : base(context) { }

        // GetAllAsync, GetByIdAsync, AddAsync, UpdateAsync, DeleteAsync are inherited from Repository<Collecte>
        // Add any Collecte-specific methods here if needed in the future
    }
}
