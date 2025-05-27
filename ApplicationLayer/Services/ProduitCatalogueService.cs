using Microsoft.EntityFrameworkCore;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;

public class ProduitCatalogueService
{
    private readonly IRepository<ProduitCatalogue> _repository;
    private readonly AppDbContext _context;

    public ProduitCatalogueService(IRepository<ProduitCatalogue> repository, AppDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public Task<IEnumerable<ProduitCatalogue>> GetAllAsync() => _repository.GetAllAsync();
    public Task<ProduitCatalogue?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
    public Task AddAsync(ProduitCatalogue produit) => _repository.AddAsync(produit);
    public Task UpdateAsync(ProduitCatalogue produit) => _repository.UpdateAsync(produit);
    public Task DeleteAsync(int id) => _repository.DeleteAsync(id);

    // ===== Méthode avancée =====
    public async Task<List<ProduitCatalogue>> GetByFamilleIdAsync(int familleId)
        => await _context.ProduitsCatalogue
                         .Where(p => p.FamilleId == familleId)
                         .Include(p => p.Famille)
                         .Include(p => p.SousFamille)
                         .ToListAsync();

    public async Task<List<ProduitCatalogue>> GetAllWithDetailsAsync()
    => await _context.ProduitsCatalogue
        .Include(p => p.Famille)
        .Include(p => p.SousFamille)
        .ToListAsync();


    public async Task<List<ProduitCatalogue>> GetBySousFamilleIdAsync(int sousFamilleId)
        => await _context.ProduitsCatalogue
                         .Where(p => p.SousFamilleId == sousFamilleId)
                         .Include(p => p.Famille)
                         .Include(p => p.SousFamille)
                         .ToListAsync();
}
