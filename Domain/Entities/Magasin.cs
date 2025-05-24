using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Domain.Entities
{
    public class Magasin
    {
        public int Id { get; set; }

        public string Nom { get; set; } = string.Empty;

        public string Adresse { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        public bool Actif { get; set; } = true;

        public int CentreId { get; set; }
        public Centre? Centre { get; set; }
    }
}
