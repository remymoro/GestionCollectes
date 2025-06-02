using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCollectes.Infrastructure.Repositories
{
    public class CentreRepository : Repository<Centre>, IRepository<Centre>
    {
        public CentreRepository(AppDbContext context) : base(context) { }

        // GetAllAsync, GetByIdAsync, AddAsync, UpdateAsync, DeleteAsync are inherited from Repository<Centre>
        // Add any Centre-specific methods here if needed in the future
    }
}
