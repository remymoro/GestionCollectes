using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Domain.Entities
{
    
        public enum StatutCollecte
        {
            Planifiee,
            EnCours,
            Terminee,
            Annulee
        }

        public class Collecte
        {
            public int Id { get; set; }
            public string Nom { get; set; } = string.Empty;
            public DateTime DateDebut { get; set; }
            public DateTime DateFin { get; set; }
            public StatutCollecte Statut { get; set; }
            public string Lieu { get; set; } = string.Empty;

            // Liens futurs (ex: Produits, Magasin, etc.)
            // public List<ProduitCollecte> Produits { get; set; }
        }
    }

