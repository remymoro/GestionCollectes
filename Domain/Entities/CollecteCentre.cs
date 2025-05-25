using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Domain.Entities
{
    public class CollecteCentre
    {
        public int CollecteId { get; set; }
        public Collecte Collecte { get; set; }
        public int CentreId { get; set; }
        public Centre Centre { get; set; }
    }
}
