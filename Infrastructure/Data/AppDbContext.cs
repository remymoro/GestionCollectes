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

        public DbSet<Famille> Familles { get; set; }
        public DbSet<SousFamille> SousFamilles { get; set; }
        public DbSet<ProduitCatalogue> ProduitsCatalogue { get; set; }

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


            // ================= Famille / SousFamille / ProduitCatalogue =================

            // Unicité du nom de famille
            modelBuilder.Entity<Famille>()
                .HasIndex(f => f.Nom)
                .IsUnique();

            // Sous-famille liée à une famille
            modelBuilder.Entity<SousFamille>()
                .HasOne(sf => sf.Famille)
                .WithMany(f => f.SousFamilles)
                .HasForeignKey(sf => sf.FamilleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unicité du nom de sous-famille dans une même famille
            modelBuilder.Entity<SousFamille>()
                .HasIndex(sf => new { sf.Nom, sf.FamilleId })
                .IsUnique();

            // ProduitCatalogue lié à famille et sous-famille
            modelBuilder.Entity<ProduitCatalogue>()
                .HasOne(p => p.Famille)
                .WithMany()
                .HasForeignKey(p => p.FamilleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProduitCatalogue>()
                .HasOne(p => p.SousFamille)
                .WithMany(sf => sf.Produits)
                .HasForeignKey(p => p.SousFamilleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unicité du code-barres
            modelBuilder.Entity<ProduitCatalogue>()
                .HasIndex(p => p.CodeBarre)
                .IsUnique();

        }
    }
}
