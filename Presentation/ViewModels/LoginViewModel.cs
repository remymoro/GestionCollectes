using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Enums;

namespace GestionCollectes.Presentation.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly UtilisateurService _utilisateurService;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action<Utilisateur>? ConnexionRéussie;

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

        public LoginViewModel(UtilisateurService utilisateurService)
        {
            _utilisateurService = utilisateurService;
            ConnexionCommand = new RelayCommand(async _ => await ConnexionAsync());
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
                ConnexionRéussie?.Invoke(user);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
