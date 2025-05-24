using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Domain.Entities
{
    public class Centre
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string CodePostal { get; set; } = string.Empty;
        public string Responsable { get; set; } = string.Empty; // Nom du responsable du centre
        public string Telephone { get; set; } = string.Empty;   // Optionnel

        public ICollection<Magasin> Magasins { get; set; } = new List<Magasin>();


    }
}
