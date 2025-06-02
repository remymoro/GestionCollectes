// /Infrastructure/Repositories/UtilisateurRepository.cs
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCollectes.Infrastructure.Repositories
{
    public class UtilisateurRepository : Repository<Utilisateur>, IRepository<Utilisateur>
    {
        public UtilisateurRepository(AppDbContext context) : base(context) { }

        // GetAllAsync, GetByIdAsync, AddAsync, UpdateAsync, DeleteAsync are inherited from Repository<Utilisateur>
        // Add any Utilisateur-specific methods here if needed in the future
    }
}
