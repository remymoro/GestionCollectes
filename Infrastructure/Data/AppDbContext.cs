using GestionCollectes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.DirectoryServices.ActiveDirectory;


namespace GestionCollectes.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Collecte> Collectes { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Centre> Centres { get; set; }
        public DbSet<Magasin> Magasins { get; set; }
        public DbSet<CollecteCentre> CollecteCentres { get; set; } // AJOUT table de liaison

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relations Centre - Magasin
            modelBuilder.Entity<Centre>()
                .HasMany(c => c.Magasins)
                .WithOne(m => m.Centre)
                .HasForeignKey(m => m.CentreId)
                .IsRequired();

            // Unicité adresse par centre
            modelBuilder.Entity<Magasin>()
                .HasIndex(m => new { m.CentreId, m.Adresse })
                .IsUnique();

            // ========= Configuration N-N Collecte <-> Centre =========
            modelBuilder.Entity<CollecteCentre>()
                .HasKey(cc => new { cc.CollecteId, cc.CentreId }); // Clé composite

            modelBuilder.Entity<CollecteCentre>()
                .HasOne(cc => cc.Collecte)
                .WithMany(c => c.CollecteCentres)
                .HasForeignKey(cc => cc.CollecteId);

            modelBuilder.Entity<CollecteCentre>()
                .HasOne(cc => cc.Centre)
                .WithMany(c => c.CollecteCentres)
                .HasForeignKey(cc => cc.CentreId);
            // ========================================================
        }
    }
}
