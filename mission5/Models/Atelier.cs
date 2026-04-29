using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace mission5.Models
{
    public enum TypePublicAtelier
    {
        ENFANT,
        PARENT_UNIQUEMENT,
        ASSISTANT_UNIQUEMENT,
        MIXTE
    }

    public class Atelier
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int DebutMinutes { get; set; }
        public int FinMinutes { get; set; }
        public DateTime DateLimiteInscription { get; set; }
        public int NombrePlaces { get; set; }
        public string Lieu { get; set; } = string.Empty;
        public TypePublicAtelier TypePublic { get; set; }
        public int? AgeMinMois { get; set; }
        public int? AgeMaxMois { get; set; }
        public int? AnimateurId { get; set; }
        public decimal Prix { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<InscriptionAtelier>? Inscriptions { get; set; }

        // Propriétés calculées pour l'affichage
        public string HeureDebut => $"{DebutMinutes / 60:D2}:{DebutMinutes % 60:D2}";
        public string HeureFin => $"{FinMinutes / 60:D2}:{FinMinutes % 60:D2}";
        public string Horaire => $"{HeureDebut} - {HeureFin}";
        public int PlacesRestantes => NombrePlaces - (Inscriptions?.Count ?? 0);
        public string PlacesInfo => $"{Inscriptions?.Count ?? 0}/{NombrePlaces}";
        public string TypePublicDisplay => TypePublic switch
        {
            TypePublicAtelier.ENFANT => "Enfants",
            TypePublicAtelier.PARENT_UNIQUEMENT => "Parents uniquement",
            TypePublicAtelier.ASSISTANT_UNIQUEMENT => "Assistantes uniquement",
            TypePublicAtelier.MIXTE => "Mixte",
            _ => TypePublic.ToString()
        };

        public string PrixDisplay => Prix == null || Prix == 0 ? "Gratuit" : $"{Prix:F2}€";
    }
}
