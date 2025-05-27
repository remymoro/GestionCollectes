using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionCollectes.Domain.Entities;
using GestionCollectes.Domain.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;



namespace GestionCollectes.Presentation.ViewModels.Admin
{

public partial class AdminProduitViewModel : ObservableObject
{
    private readonly IRepository<Famille> _familleRepo;
    private readonly IRepository<SousFamille> _sousFamilleRepo;
    private readonly IRepository<ProduitCatalogue> _produitRepo;

    public ObservableCollection<Famille> Familles { get; } = new();
    public ObservableCollection<SousFamille> SousFamilles { get; } = new();
    public ObservableCollection<ProduitCatalogue> Produits { get; } = new();

    [ObservableProperty] private Famille? selectedFamille;
    [ObservableProperty] private SousFamille? selectedSousFamille;
    [ObservableProperty] private string? nomProduit;
    [ObservableProperty] private string? codeBarre;

    public AdminProduitViewModel(
        IRepository<Famille> familleRepo,
        IRepository<SousFamille> sousFamilleRepo,
        IRepository<ProduitCatalogue> produitRepo)
    {
        _familleRepo = familleRepo;
        _sousFamilleRepo = sousFamilleRepo;
        _produitRepo = produitRepo;

        _ = LoadFamillesAsync();
        _ = LoadProduitsAsync();
    }



        public async Task ChargerProduitsParFamilleAsync(int familleId)
        {
            Produits.Clear();
            var produits = await _produitRepo.GetByFamilleIdAsync(familleId);
            foreach (var p in produits)
                Produits.Add(p);
        }

        private async Task LoadFamillesAsync()
    {
        Familles.Clear();
        foreach (var f in await _familleRepo.GetAllAsync())
            Familles.Add(f);
    }

    private async Task LoadSousFamillesAsync()
    {
        SousFamilles.Clear();
        if (SelectedFamille is not null)
        {
            var allSousFamilles = await _sousFamilleRepo.GetAllAsync();
            foreach (var sf in allSousFamilles.Where(sf => sf.FamilleId == SelectedFamille.Id))
                SousFamilles.Add(sf);
        }
    }

    private async Task LoadProduitsAsync()
    {
        Produits.Clear();
        foreach (var p in await _produitRepo.GetAllAsync())
            Produits.Add(p);
    }

    partial void OnSelectedFamilleChanged(Famille? value)
    {
        _ = LoadSousFamillesAsync();
    }

    [RelayCommand]
    private async Task AjouterProduitAsync()
    {
        if (SelectedFamille is null || SelectedSousFamille is null || string.IsNullOrWhiteSpace(NomProduit) || string.IsNullOrWhiteSpace(CodeBarre))
            return;

        var produit = new ProduitCatalogue
        {
            Nom = NomProduit,
            CodeBarre = CodeBarre,
            FamilleId = SelectedFamille.Id,
            SousFamilleId = SelectedSousFamille.Id
        };
        await _produitRepo.AddAsync(produit);
        await LoadProduitsAsync();

        // Reset form
        NomProduit = "";
        CodeBarre = "";
        SelectedFamille = null;
        SelectedSousFamille = null;
    }
}

}