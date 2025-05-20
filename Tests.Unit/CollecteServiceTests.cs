using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;

namespace Tests.Unit
{
    public class CollecteServiceTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllCollectes()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Collecte>>();
            var data = new List<Collecte>
            {
                new Collecte { Id = 1, Nom = "Collecte1" },
                new Collecte { Id = 2, Nom = "Collecte2" }
            };

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(data);

            var service = new CollecteService(mockRepo.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectCollecte()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Collecte>>();
            var collecte = new Collecte { Id = 1, Nom = "Collecte1" };

            mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(collecte);

            var service = new CollecteService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Collecte1", result.Nom);
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryAddAsync()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Collecte>>();
            var collecte = new Collecte { Id = 1, Nom = "NewCollecte" };

            mockRepo.Setup(repo => repo.AddAsync(collecte)).Returns(Task.CompletedTask).Verifiable();

            var service = new CollecteService(mockRepo.Object);

            // Act
            await service.AddAsync(collecte);

            // Assert
            mockRepo.Verify(repo => repo.AddAsync(collecte), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepositoryUpdateAsync()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Collecte>>();
            var collecte = new Collecte { Id = 1, Nom = "UpdatedCollecte" };

            mockRepo.Setup(repo => repo.UpdateAsync(collecte)).Returns(Task.CompletedTask).Verifiable();

            var service = new CollecteService(mockRepo.Object);

            // Act
            await service.UpdateAsync(collecte);

            // Assert
            mockRepo.Verify(repo => repo.UpdateAsync(collecte), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepositoryDeleteAsync()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Collecte>>();

            mockRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask).Verifiable();

            var service = new CollecteService(mockRepo.Object);

            // Act
            await service.DeleteAsync(1);

            // Assert
            mockRepo.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
