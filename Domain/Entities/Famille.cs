using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Domain.Entities
{
    public class Famille
    {
        public int Id { get; set; }
        public string Nom { get; set; } = "";

        public ICollection<SousFamille> SousFamilles { get; set; } = new List<SousFamille>();
    }

}
