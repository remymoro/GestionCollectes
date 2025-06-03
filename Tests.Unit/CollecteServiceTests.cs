using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.Domain.Enums; // Added for StatutCollecte
using System; // For ArgumentException

namespace Tests.Unit
{
    public class CollecteServiceTests
    {
        private readonly Mock<IRepository<Collecte>> _mockCollecteRepo;
        private readonly Mock<ICollecteCentreRepository> _mockCollecteCentreRepo;
        private readonly Mock<IRepository<Centre>> _mockCentreRepo;
        private readonly CollecteService _collecteService;

        public CollecteServiceTests()
        {
            _mockCollecteRepo = new Mock<IRepository<Collecte>>();
            _mockCollecteCentreRepo = new Mock<ICollecteCentreRepository>();
            _mockCentreRepo = new Mock<IRepository<Centre>>();
            _collecteService = new CollecteService(
                _mockCollecteRepo.Object,
                _mockCollecteCentreRepo.Object,
                _mockCentreRepo.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCollectes()
        {
            // Arrange
            var data = new List<Collecte>
            {
                new Collecte { Id = 1, Nom = "Collecte1" },
                new Collecte { Id = 2, Nom = "Collecte2" }
            };
            _mockCollecteRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(data);

            // Act
            var result = await _collecteService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectCollecte()
        {
            // Arrange
            var collecte = new Collecte { Id = 1, Nom = "Collecte1" };
            _mockCollecteRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(collecte);

            // Act
            var result = await _collecteService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Collecte1", result.Nom);
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryAddAsync()
        {
            // Arrange
            var collecte = new Collecte { Id = 1, Nom = "NewCollecte" };
            _mockCollecteRepo.Setup(repo => repo.AddAsync(collecte)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _collecteService.AddAsync(collecte);

            // Assert
            _mockCollecteRepo.Verify(repo => repo.AddAsync(collecte), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepositoryUpdateAsync()
        {
            // Arrange
            var collecte = new Collecte { Id = 1, Nom = "UpdatedCollecte" };
            _mockCollecteRepo.Setup(repo => repo.UpdateAsync(collecte)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _collecteService.UpdateAsync(collecte);

            // Assert
            _mockCollecteRepo.Verify(repo => repo.UpdateAsync(collecte), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepositoryDeleteAsync()
        {
            // Arrange
            _mockCollecteRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _collecteService.DeleteAsync(1);

            // Assert
            _mockCollecteRepo.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreerCollecteAsync_ValidInput_CreatesCollecteAndLinksCentres()
        {
            // Arrange
            var nom = "Nouvelle Collecte";
            var dateDebut = DateTime.Today;
            var dateFin = DateTime.Today.AddDays(1);
            var statut = StatutCollecte.Planifiee;
            var description = "Description test";
            var objectif = "Objectif test";
            var centreIds = new List<int> { 1, 2 };

            var createdCollecte = new Collecte { Id = 100 }; // Simulate ID generation

            _mockCentreRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Centre { Id = 1 });
            _mockCentreRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(new Centre { Id = 2 });

            _mockCollecteRepo.Setup(r => r.AddAsync(It.IsAny<Collecte>()))
                .Callback<Collecte>(c => c.Id = createdCollecte.Id) // Simulate ID assignment by DB
                .Returns(Task.CompletedTask);

            _mockCollecteCentreRepo.Setup(r => r.AddAsync(It.IsAny<CollecteCentre>())).Returns(Task.CompletedTask);

            // Act
            await _collecteService.CreerCollecteAsync(nom, dateDebut, dateFin, statut, description, objectif, centreIds);

            // Assert
            _mockCollecteRepo.Verify(r => r.AddAsync(It.Is<Collecte>(c =>
                c.Nom == nom &&
                c.DateDebut == dateDebut &&
                c.DateFin == dateFin &&
                c.Statut == statut &&
                c.Description == description &&
                c.Objectif == objectif
            )), Times.Once);

            _mockCollecteCentreRepo.Verify(r => r.AddAsync(It.Is<CollecteCentre>(cc => cc.CollecteId == createdCollecte.Id && cc.CentreId == 1)), Times.Once);
            _mockCollecteCentreRepo.Verify(r => r.AddAsync(It.Is<CollecteCentre>(cc => cc.CollecteId == createdCollecte.Id && cc.CentreId == 2)), Times.Once);
            _mockCentreRepo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(centreIds.Count)); // Verify centre validation
        }

        [Fact]
        public async Task CreerCollecteAsync_EmptyNom_ThrowsArgumentException()
        {
            // Arrange
            var centreIds = new List<int> { 1 };
             _mockCentreRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Centre { Id = 1 });


            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _collecteService.CreerCollecteAsync("", DateTime.Today, DateTime.Today.AddDays(1), StatutCollecte.Planifiee, "Desc", "Obj", centreIds)
            );
            Assert.Equal("nom", exception.ParamName); // Check ParamName for more specific assertion
        }

        [Fact]
        public async Task CreerCollecteAsync_InvalidCentreId_ThrowsArgumentException()
        {
            // Arrange
            var centreIds = new List<int> { 1, 99 }; // 99 is invalid
            _mockCentreRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Centre { Id = 1 });
            _mockCentreRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Centre?)null); // Centre 99 does not exist

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _collecteService.CreerCollecteAsync("NomValide", DateTime.Today, DateTime.Today.AddDays(1), StatutCollecte.Planifiee, "Desc", "Obj", centreIds)
            );
            Assert.Equal("centreIds", exception.ParamName);
            Assert.Contains("Le centre avec l'ID 99 n'existe pas.", exception.Message);
        }
    }
}
