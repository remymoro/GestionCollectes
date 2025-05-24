// ========================
// USINGS
// ========================
using System;                               // Types de base (DateTime, etc.)
using System.Collections.Generic;           // Listes génériques (List<T>, etc.)
using System.Linq;                          // Extensions LINQ (Select, Where, etc.)
using System.Text;                          // Manipulations avancées de texte (rarement utilisé ici)
using System.Threading.Tasks;               // Pour async/await (programmation asynchrone)

using CommunityToolkit.Mvvm.ComponentModel; // ObservableObject (MVVM Toolkit : notifications de changements)
using CommunityToolkit.Mvvm.Input;          // RelayCommand, IRelayCommand (commandes MVVM)
using System.Collections.ObjectModel;       // ObservableCollection<T> (listes observables pour la Vue)
using GestionCollectes.ApplicationLayer.Services; // Service métier pour gérer les collectes
using GestionCollectes.Domain.Entities;    

// Entités du domaine (Collecte, etc.)

// ========================
// NAMESPACE
// ========================
namespace GestionCollectes.Presentation.ViewModels.Utilisateurs
{
    // ========================
    // VIEWMODEL PRINCIPAL
    // ========================
    /// <summary>
    /// ViewModel pour la gestion et l’affichage des collectes côté utilisateur.
    /// Hérite de ObservableObject pour le binding automatique avec la Vue (MVVM Toolkit).
    /// </summary>
    public class CollecteUtilisateurViewModel : ObservableObject
    {
        // ========================
        // CHAMP ET PROPRIÉTÉ OBSERVABLE
        // ========================
        /// <summary>
        /// Liste des collectes à afficher dans la Vue.
        /// ObservableCollection permet la mise à jour automatique de la Vue lorsqu’on modifie la liste.
        /// </summary>
        private ObservableCollection<CollecteDisplay> _collectes = new();

        public ObservableCollection<CollecteDisplay> Collectes
        {
            get => _collectes;
            set => SetProperty(ref _collectes, value); // SetProperty déclenche PropertyChanged si la valeur change
        }

        // ========================
        // SERVICE INJECTÉ
        // ========================
        /// <summary>
        /// Service métier pour accéder aux données de collecte (récupération, filtrage, etc.).
        /// "readonly" : ne peut être assigné que dans le constructeur, gage d’immuabilité.
        /// </summary>
        private readonly CollecteService _collecteService;

        // ========================
        // CONSTRUCTEUR
        // ========================
        /// <summary>
        /// Constructeur du ViewModel, on injecte le service métier en paramètre (DI recommandée).
        /// </summary>
        /// <param name="collecteService">Service métier pour accéder aux collectes</param>
        public CollecteUtilisateurViewModel(CollecteService collecteService)
        {
            _collecteService = collecteService;
            // À l'instanciation, lance la récupération des collectes (logique métier).
            LoadCollectes();
        }

        // ========================
        // CLASSE D'AFFICHAGE INTERNE
        // ========================
        /// <summary>
        /// Classe interne représentant une collecte "pour l'affichage" (séparation de l’entité métier et du modèle de vue).
        /// </summary>
        public class CollecteDisplay
        {
            public string Nom { get; set; }
            public DateTime DateDebut { get; set; }
            public DateTime DateFin { get; set; }
            public string Statut { get; set; }
            public bool EstAccessible { get; set; }
        }

        // ========================
        // MÉTHODE DE CHARGEMENT ASYNCHRONE (À AJOUTER POUR COMPLÉTER)
        // ========================
        
        private async void LoadCollectes()
        {
            // Ex: Récupérer la liste des collectes via le service
            var liste = await _collecteService.GetAllAsync();
            Collectes = new ObservableCollection<CollecteDisplay>(
                liste.Select(c => new CollecteDisplay
                {
                    Nom = c.Nom,
                    DateDebut = c.DateDebut,
                    DateFin = c.DateFin,
                    Statut = c.Statut.ToString(),
                    EstAccessible = c.Statut == Domain.Enums.StatutCollecte.EnCours
                            && DateTime.Now >= c.DateDebut
                            && DateTime.Now <= c.DateFin




                })
            
                
                
                
                );



        }
        
    }
}
