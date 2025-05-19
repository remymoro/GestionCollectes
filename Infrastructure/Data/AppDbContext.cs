using GestionCollectes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Collecte> Collectes { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
