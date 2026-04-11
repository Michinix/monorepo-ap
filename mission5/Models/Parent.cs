using System;

namespace mission5.Models
{
    public class Parent
    {
        public int Id { get; set; }
        public int UtilisateurId { get; set; }
        public string Adresse { get; set; } = string.Empty;
        public string CodePostal { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string? SituationFamiliale { get; set; }
        public string? Profession { get; set; }
        public string? Employeur { get; set; }
        public bool BeneficiaireCAF { get; set; }
        public string? NumeroAllocataire { get; set; }
        public string? ContactUrgenceNom { get; set; }
        public string? ContactUrgenceTel { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Utilisateur? Utilisateur { get; set; }

        public string NomComplet => Utilisateur != null
            ? $"{Utilisateur.Prenom} {Utilisateur.Nom}"
            : "N/A";
    }
}
