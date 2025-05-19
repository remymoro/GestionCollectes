using GestionCollectes.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Domain.Entities
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string MotDePasseHash { get; set; } = string.Empty;
        public RoleUtilisateur Role { get; set; }
        public int? CentreId { get; set; }           // Optionnel, si tu veux rattacher un centre plus tard
        // public Centre? Centre { get; set; }       // Navigation si besoin (à ajouter si Centre existe déjà)
    }
}
