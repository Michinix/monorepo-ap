using System;

namespace mission5.Models
{
    public class Assistant
    {
        public int Id { get; set; }
        public int UtilisateurId { get; set; }
        public string Adresse { get; set; } = string.Empty;
        public string CodePostal { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string NumeroAgrement { get; set; } = string.Empty;
        public DateTime DateObtentionAgrement { get; set; }
        public DateTime? DateFinAgrement { get; set; }
        public int CapaciteAccueil { get; set; }
        public int Experience { get; set; }
        public string? Disponibilites { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Utilisateur? Utilisateur { get; set; }

        public string NomComplet => Utilisateur != null
            ? $"{Utilisateur.Prenom} {Utilisateur.Nom}"
            : "N/A";
    }
}
