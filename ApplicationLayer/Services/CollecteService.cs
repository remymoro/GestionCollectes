using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCollectes.ApplicationLayer.Services
{

    public class CollecteService
    {

        private readonly IRepository<Collecte> _repository;

        public CollecteService(IRepository<Collecte> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Collecte>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Collecte?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task AddAsync(Collecte collecte) => _repository.AddAsync(collecte);
        public Task UpdateAsync(Collecte collecte) => _repository.UpdateAsync(collecte);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}