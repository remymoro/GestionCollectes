﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Enums;
using GestionCollectes.Presentation.Navigation;

namespace GestionCollectes.Presentation.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly UtilisateurService _utilisateurService;
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly ICurrentUserService _currentUserService; // Added ICurrentUserService

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _nom = "";
        public string Nom
        {
            get => _nom;
            set { _nom = value; OnPropertyChanged(); }
        }

        private string _motDePasse = "";
        public string MotDePasse
        {
            get => _motDePasse;
            set { _motDePasse = value; OnPropertyChanged(); }
        }

        private string? _erreur;
        public string? Erreur
        {
            get => _erreur;
            set { _erreur = value; OnPropertyChanged(); }
        }

        public ICommand ConnexionCommand { get; }

        public LoginViewModel(
            UtilisateurService utilisateurService, 
            IWindowNavigationService windowNavigationService,
            ICurrentUserService currentUserService) // Injected ICurrentUserService
        {
            _utilisateurService = utilisateurService;
            _windowNavigationService = windowNavigationService;
            _currentUserService = currentUserService; // Initialized ICurrentUserService
            ConnexionCommand = new OldRelayCommand(async _ => await ConnexionAsync());
        }

        private async Task ConnexionAsync()
        {
            var user = await _utilisateurService.AuthenticateAsync(Nom, MotDePasse);
            if (user == null)
            {
                Erreur = "Nom ou mot de passe incorrect";
            }
            else
            {
                Erreur = null;
                _currentUserService.SetCurrentUser(user); // Use ICurrentUserService
                _windowNavigationService.ShowDashboardForRole(user.Role);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
