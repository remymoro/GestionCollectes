using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCollectes.ApplicationLayer.Services
{
    public class MagasinService
    {
        private readonly IRepository<Magasin> _repo;

        public MagasinService(IRepository<Magasin> repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Magasin>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Magasin?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task AddAsync(Magasin magasin) => _repo.AddAsync(magasin);
        public Task UpdateAsync(Magasin magasin) => _repo.UpdateAsync(magasin);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
