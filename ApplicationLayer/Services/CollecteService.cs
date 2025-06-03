using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Domain.Enums; // Added for StatutCollecte

// Add other usings if your validation or other logic requires it

namespace GestionCollectes.ApplicationLayer.Services
{

    public class CollecteService
    {

        private readonly IRepository<Collecte> _collecteRepository;
        private readonly ICollecteCentreRepository _collecteCentreRepository;
        private readonly IRepository<Centre> _centreRepository;

        public CollecteService(IRepository<Collecte> collecteRepository,
                           ICollecteCentreRepository collecteCentreRepository,
                           IRepository<Centre> centreRepository)
        {
            _collecteRepository = collecteRepository;
            _collecteCentreRepository = collecteCentreRepository;
            _centreRepository = centreRepository;
        }

        public Task<IEnumerable<Collecte>> GetAllAsync() => _collecteRepository.GetAllAsync();
        public Task<Collecte?> GetByIdAsync(int id) => _collecteRepository.GetByIdAsync(id);
        public Task AddAsync(Collecte collecte) => _collecteRepository.AddAsync(collecte);
        public Task UpdateAsync(Collecte collecte) => _collecteRepository.UpdateAsync(collecte);
        public Task DeleteAsync(int id) => _collecteRepository.DeleteAsync(id);

        public async Task CreerCollecteAsync(string nom, DateTime dateDebut, DateTime dateFin,
                                         StatutCollecte statut, string description, string objectif,
                                         IEnumerable<int> centreIds)
        {
            // 1. Validation
            if (string.IsNullOrWhiteSpace(nom))
                throw new ArgumentException("Le nom de la collecte ne peut pas être vide.", nameof(nom));
            if (dateFin < dateDebut)
                throw new ArgumentException("La date de fin ne peut pas être antérieure à la date de début.", nameof(dateFin));
            if (centreIds == null || !centreIds.Any())
                throw new ArgumentException("Au moins un centre doit être sélectionné pour la collecte.", nameof(centreIds));

            // Optional: Validate if all centreIds actually exist
            foreach (var centreId in centreIds)
            {
                var centre = await _centreRepository.GetByIdAsync(centreId);
                if (centre == null)
                    throw new ArgumentException($"Le centre avec l'ID {centreId} n'existe pas.", nameof(centreIds));
            }

            // 2. Create Collecte entity
            var nouvelleCollecte = new Collecte
            {
                Nom = nom,
                DateDebut = dateDebut,
                DateFin = dateFin,
                Statut = statut,
                Description = description,
                Objectif = objectif
                // CollecteCentres will be populated by EF Core ifinverse navigation is set up,
                // or we can manually add CollecteCentre entities to its collection if needed,
                // but creating CollecteCentre separately is often cleaner.
            };

            // 3. Save Collecte
            await _collecteRepository.AddAsync(nouvelleCollecte);
            // Assuming AddAsync updates nouvelleCollecte with the generated Id

            // 4. Create and save CollecteCentre link entities
            foreach (var centreId in centreIds)
            {
                var collecteCentre = new CollecteCentre
                {
                    CollecteId = nouvelleCollecte.Id,
                    CentreId = centreId
                };
                await _collecteCentreRepository.AddAsync(collecteCentre);
            }
        }
    }
}