using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCollectes.Domain.Enums;

namespace GestionCollectes.Domain.Entities
{
    
        

        public class Collecte
        {
            public int Id { get; set; }
            public string Nom { get; set; } = string.Empty;
            public DateTime DateDebut { get; set; }
            public DateTime DateFin { get; set; }
            public StatutCollecte Statut { get; set; }

        // 🟢 AJOUTER ICI :
            public string Description { get; set; } = string.Empty;
            public string Objectif { get; set; } = string.Empty;


        public ICollection<CollecteCentre> CollecteCentres { get; set; } = new List<CollecteCentre>();
    }

    // Liens futurs (ex: Produits, Magasin, etc.)
    // public List<ProduitCollecte> Produits { get; set; }
}

