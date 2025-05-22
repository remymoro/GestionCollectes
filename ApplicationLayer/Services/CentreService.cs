using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.ApplicationLayer.Services
{
    public class CentreService
    {
        private readonly IRepository<Centre> _centreRepository;

        public CentreService(IRepository<Centre> centreRepository)
        {
            _centreRepository = centreRepository;
        }

        public async Task<IEnumerable<Centre>> GetAllAsync() => await _centreRepository.GetAllAsync();
        public async Task<Centre?> GetByIdAsync(int id) => await _centreRepository.GetByIdAsync(id);
        public async Task AddAsync(Centre centre) => await _centreRepository.AddAsync(centre);
        public async Task UpdateAsync(Centre centre) => await _centreRepository.UpdateAsync(centre);
        public async Task DeleteAsync(int id) => await _centreRepository.DeleteAsync(id);
    }
}
