using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.ApplicationLayer.Services;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Enums; // For StatutCollecte
using GestionCollectes.Domain.Interfaces; // For IRepository<Centre>
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows; // For MessageBox, consider a dedicated dialog service in a real app

namespace GestionCollectes.Presentation.ViewModels.Admin
{
    public partial class CreerCollecteViewModel : ObservableObject
    {
        private readonly CollecteService _collecteService;
        private readonly IRepository<Centre> _centreRepository; // Or CentreService

        [ObservableProperty]
        private string _nom = string.Empty;

        [ObservableProperty]
        private DateTime _dateDebut = DateTime.Today;

        [ObservableProperty]
        private DateTime _dateFin = DateTime.Today.AddDays(7);

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private string _objectif = string.Empty;

        [ObservableProperty]
        private StatutCollecte _selectedStatut = StatutCollecte.Planifiee; // Default status

        public ObservableCollection<CentreCheckable> AvailableCentres { get; } = new();

        // Expose StatutCollecte values for binding in the View (e.g., ComboBox)
        public IEnumerable<StatutCollecte> StatutCollecteValues =>
            Enum.GetValues(typeof(StatutCollecte)).Cast<StatutCollecte>();

        public CreerCollecteViewModel(CollecteService collecteService, IRepository<Centre> centreRepository)
        {
            _collecteService = collecteService;
            _centreRepository = centreRepository;
            // LoadCentresAsync(); // Call load method
        }

        [RelayCommand]
        private async Task LoadCentresAsync()
        {
            try
            {
                AvailableCentres.Clear();
                var centres = await _centreRepository.GetAllAsync();
                foreach (var centre in centres)
                {
                    AvailableCentres.Add(new CentreCheckable(centre));
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it, show a message)
                MessageBox.Show($"Erreur lors du chargement des centres: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task CreerCollecteAsync()
        {
            var selectedCentreIds = AvailableCentres.Where(c => c.IsSelected).Select(c => c.Id).ToList();

            if (!selectedCentreIds.Any())
            {
                MessageBox.Show("Veuillez sélectionner au moins un centre participant.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DateFin < DateDebut)
            {
                 MessageBox.Show("La date de fin ne peut pas être antérieure à la date de début.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Nom))
            {
                 MessageBox.Show("Le nom de la collecte est requis.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            try
            {
                await _collecteService.CreerCollecteAsync(Nom, DateDebut, DateFin, SelectedStatut, Description, Objectif, selectedCentreIds);
                MessageBox.Show("Collecte créée avec succès!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                // Optionally: Reset form or navigate away
                ResetForm();
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show($"Erreur de validation: {argEx.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors de la création de la collecte: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetForm()
        {
            Nom = string.Empty;
            DateDebut = DateTime.Today;
            DateFin = DateTime.Today.AddDays(7);
            Description = string.Empty;
            Objectif = string.Empty;
            SelectedStatut = StatutCollecte.Planifiee;
            foreach(var centreCheckable in AvailableCentres)
            {
                centreCheckable.IsSelected = false;
            }
        }
    }
}
