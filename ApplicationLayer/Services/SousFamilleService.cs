using Microsoft.EntityFrameworkCore;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Infrastructure.Data;

public class SousFamilleService
{
    private readonly IRepository<SousFamille> _repository;
    private readonly AppDbContext _context; // pour les requêtes avancées

    public SousFamilleService(IRepository<SousFamille> repository, AppDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public Task<IEnumerable<SousFamille>> GetAllAsync() => _repository.GetAllAsync();
    public Task<SousFamille?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
    public Task AddAsync(SousFamille sf) => _repository.AddAsync(sf);
    public Task UpdateAsync(SousFamille sf) => _repository.UpdateAsync(sf);
    public Task DeleteAsync(int id) => _repository.DeleteAsync(id);

    // ===== Méthode avancée =====
    public async Task<List<SousFamille>> GetByFamilleIdAsync(int familleId)
        => await _context.SousFamilles
                         .Where(sf => sf.FamilleId == familleId)
                         .ToListAsync();
}
