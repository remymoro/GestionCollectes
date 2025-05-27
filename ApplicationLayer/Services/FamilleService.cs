using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;

public class FamilleService
{
    private readonly IRepository<Famille> _repository;

    public FamilleService(IRepository<Famille> repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Famille>> GetAllAsync() => _repository.GetAllAsync();
    public Task<Famille?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
    public Task AddAsync(Famille famille) => _repository.AddAsync(famille);
    public Task UpdateAsync(Famille famille) => _repository.UpdateAsync(famille);
    public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
}
