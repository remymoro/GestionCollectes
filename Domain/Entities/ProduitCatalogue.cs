using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Domain.Entities
{
        public class ProduitCatalogue
        {
            public int Id { get; set; }
            public string CodeBarre { get; set; } = "";
            public string Nom { get; set; } = "";
            public int FamilleId { get; set; }
            public Famille? Famille { get; set; }
            public int SousFamilleId { get; set; }
            public SousFamille? SousFamille { get; set; }
            // PAS de ICollection ici
        }

}
