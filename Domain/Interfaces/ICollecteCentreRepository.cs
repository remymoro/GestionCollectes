using GestionCollectes.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionCollectes.Domain.Interfaces
{
    public interface ICollecteCentreRepository
    {
        Task<IEnumerable<CollecteCentre>> GetAllAsync();
        Task AddAsync(CollecteCentre entity);
        Task DeleteAsync(int collecteId, int centreId);
        // Note: GetById(id) n'est pas inclus car CollecteCentre a une clé composite.
        // UpdateAsync pourrait être ajouté si nécessaire, mais sa logique est souvent spécifique pour les tables de jointure.
    }
}
