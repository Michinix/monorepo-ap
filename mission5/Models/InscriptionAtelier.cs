using System.Text.Json.Serialization;

namespace mission5.Models
{
    public enum StatutPaiement
    {
        REGLE,
        A_REGLER
    }

    public class InscriptionAtelier
    {
        public int Id { get; set; }
        public int AtelierId { get; set; }
        public int? ParentId { get; set; }
        public int? EnfantId { get; set; }
        public int? AssistantId { get; set; }
        public bool Present { get; set; }
        public StatutPaiement StatutPaiement { get; set; } = StatutPaiement.A_REGLER;

        // Relations
        [JsonIgnore]
        public Atelier? Atelier { get; set; }

        public Enfant? Enfant { get; set; }
        public Parent? Parent { get; set; }
        public Assistant? Assistant { get; set; }

        public string NomComplet
        {
            get
            {
                if (Enfant != null)
                    return $"{Enfant.Prenom} {Enfant.Nom}";
                if (Parent != null)
                    return Parent.NomComplet;
                if (Assistant != null)
                    return Assistant.NomComplet;
                return "N/A";
            }
        }

        public string StatutPaiementDisplay => StatutPaiement switch
        {
            StatutPaiement.REGLE => "Payé",
            StatutPaiement.A_REGLER => "À payer",
            _ => StatutPaiement.ToString()
        };

        public bool IsPaye => StatutPaiement == StatutPaiement.REGLE;
        public bool IsARegler => StatutPaiement == StatutPaiement.A_REGLER;
    }
}
