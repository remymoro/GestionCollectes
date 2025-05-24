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
        public DbSet<Centre> Centres { get; set; }

        public DbSet<Magasin> Magasins { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Centre>()
                .HasMany(c => c.Magasins)
                .WithOne(m => m.Centre)
                .HasForeignKey(m => m.CentreId)
                .IsRequired();

            // (Optionnel, pour l’unicité d’adresse par centre)
            modelBuilder.Entity<Magasin>()
                .HasIndex(m => new { m.CentreId, m.Adresse })
                .IsUnique();
        }
    }
}
