using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Domain.Entities
{
    public class SousFamille
    {
        public int Id { get; set; }
        public string Nom { get; set; } = "";
        public int FamilleId { get; set; }
        public Famille? Famille { get; set; }

        // CORRECTION : Ajoute la bonne navigation
        public ICollection<ProduitCatalogue> Produits { get; set; } = new List<ProduitCatalogue>();
    }


}
