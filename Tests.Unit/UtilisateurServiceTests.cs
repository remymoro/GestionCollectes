using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using GestionCollectes.ApplicationLayer.Services;

namespace Tests.Unit
{
    public class UtilisateurServiceTests
    {
        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsUser()
        {
            // Arrange : on prépare un utilisateur avec Nom et MotDePasseHash
            var user = new Utilisateur { Nom = "Jean", MotDePasseHash = "hashedpassword123" };

            // Mock du repo pour que GetAllAsync retourne la liste contenant user
            var repoMock = new Mock<IRepository<Utilisateur>>();
            repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Utilisateur> { user });

            var service = new UtilisateurService(repoMock.Object);

            // Act : appel de AuthenticateAsync avec nom et mot de passe correct
            var result = await service.AuthenticateAsync("Jean", "hashedpassword123");

            // Assert : on récupère bien l'utilisateur
            Assert.NotNull(result);
            Assert.Equal("Jean", result.Nom);
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidPassword_ReturnsNull()
        {
            var user = new Utilisateur { Nom = "Jean", MotDePasseHash = "hashedpassword123" };

            var repoMock = new Mock<IRepository<Utilisateur>>();
            repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Utilisateur> { user });

            var service = new UtilisateurService(repoMock.Object);

            // Mot de passe erroné
            var result = await service.AuthenticateAsync("Jean", "wrongpassword");

            Assert.Null(result);
        }

        [Fact]
        public async Task AuthenticateAsync_UserNotFound_ReturnsNull()
        {
            var repoMock = new Mock<IRepository<Utilisateur>>();
            repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Utilisateur>()); // liste vide

            var service = new UtilisateurService(repoMock.Object);

            var result = await service.AuthenticateAsync("Inconnu", "password");

            Assert.Null(result);
        }
    }
}
