using System;
using System.Text.Json.Serialization;

namespace mission5.Models
{
    public enum Sexe
    {
        HOMME,
        FEMME,
        AUTRE
    }

    public class Enfant
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public DateTime DateNaissance { get; set; }
        public Sexe Sexe { get; set; }
        public string? Allergies { get; set; }
        public string? RemarquesMedicales { get; set; }
        public string MedecinTraitant { get; set; } = string.Empty;
        public string MedecinTraitantTel { get; set; } = string.Empty;
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string NomComplet => $"{Prenom} {Nom}";
        public int AgeMois
        {
            get
            {
                var today = DateTime.Today;
                var mois = (today.Year - DateNaissance.Year) * 12 + today.Month - DateNaissance.Month;
                if (today.Day < DateNaissance.Day)
                    mois--;
                return mois;
            }
        }
    }
}
