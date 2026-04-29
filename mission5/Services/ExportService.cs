using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mission5.Models;

namespace mission5.Services
{
    public class ExportService
    {
        private static ExportService? _instance;
        public static ExportService Instance => _instance ??= new ExportService();

        private ExportService() { }

        /// <summary>
        /// Exporte la liste des inscriptions en CSV
        /// </summary>
        public async Task<bool> ExportInscriptionsToCSV(Atelier atelier, List<InscriptionAtelier> inscriptions, string filePath)
        {
            try
            {
                var csv = new StringBuilder();

                // En-tête
                csv.AppendLine($"Liste des inscrits - {atelier.Nom}");
                csv.AppendLine($"Date: {atelier.Date:dd/MM/yyyy}");
                csv.AppendLine($"Horaire: {atelier.Horaire}");
                csv.AppendLine($"Lieu: {atelier.Lieu}");
                csv.AppendLine($"Prix: {atelier.PrixDisplay}");
                csv.AppendLine($"Nombre d'inscrits: {inscriptions.Count}/{atelier.NombrePlaces}");
                csv.AppendLine();

                // Colonnes
                csv.AppendLine("N°,Nom complet,Statut paiement,Présent");

                // Données
                int numero = 1;
                foreach (var inscription in inscriptions)
                {
                    csv.AppendLine($"{numero},{inscription.NomComplet},{inscription.StatutPaiementDisplay},{(inscription.Present ? "Oui" : "Non")}");
                    numero++;
                }

                await File.WriteAllTextAsync(filePath, csv.ToString(), Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Génère un document HTML pour l'impression
        /// </summary>
        public string GenerateInscriptionsHTML(Atelier atelier, List<InscriptionAtelier> inscriptions)
        {
            var html = new StringBuilder();

            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("    <meta charset='utf-8'>");
            html.AppendLine("    <title>Liste des inscrits</title>");
            html.AppendLine("    <style>");
            html.AppendLine("        body { font-family: Arial, sans-serif; margin: 40px; }");
            html.AppendLine("        h1 { color: #2d3748; border-bottom: 3px solid #4299e1; padding-bottom: 10px; }");
            html.AppendLine("        .info { background: #f7fafc; padding: 15px; border-radius: 8px; margin: 20px 0; }");
            html.AppendLine("        .info p { margin: 5px 0; color: #4a5568; }");
            html.AppendLine("        table { width: 100%; border-collapse: collapse; margin-top: 20px; }");
            html.AppendLine("        th { background: #4299e1; color: white; padding: 12px; text-align: left; }");
            html.AppendLine("        td { padding: 10px; border-bottom: 1px solid #e2e8f0; }");
            html.AppendLine("        tr:nth-child(even) { background: #f7fafc; }");
            html.AppendLine("        .statut-paye { background-color: #c6f6d5; color: #22543d; font-weight: bold; }");
            html.AppendLine("        .statut-a-payer { background-color: #fed7d7; color: #742a2a; font-weight: bold; }");
            html.AppendLine("        .footer { margin-top: 30px; text-align: center; color: #718096; font-size: 12px; }");
            html.AppendLine("        @media print { .no-print { display: none; } }");
            html.AppendLine("    </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            html.AppendLine($"    <h1>Liste des inscrits - {atelier.Nom}</h1>");

            html.AppendLine("    <div class='info'>");
            html.AppendLine($"        <p><strong>Date :</strong> {atelier.Date:dddd dd MMMM yyyy}</p>");
            html.AppendLine($"        <p><strong>Horaire :</strong> {atelier.Horaire}</p>");
            html.AppendLine($"        <p><strong>Lieu :</strong> {atelier.Lieu}</p>");
            html.AppendLine($"        <p><strong>Type de public :</strong> {atelier.TypePublicDisplay}</p>");
            html.AppendLine($"        <p><strong>Prix :</strong> {atelier.PrixDisplay}</p>");
            html.AppendLine($"        <p><strong>Nombre d'inscrits :</strong> {inscriptions.Count} / {atelier.NombrePlaces}</p>");
            html.AppendLine("    </div>");

            html.AppendLine("    <table>");
            html.AppendLine("        <thead>");
            html.AppendLine("            <tr>");
            html.AppendLine("                <th style='width: 50px;'>N°</th>");
            html.AppendLine("                <th>Nom complet</th>");
            html.AppendLine("                <th style='width: 120px;'>Statut paiement</th>");
            html.AppendLine("                <th style='width: 100px;'>Présence</th>");
            html.AppendLine("                <th style='width: 150px;'>Signature</th>");
            html.AppendLine("            </tr>");
            html.AppendLine("        </thead>");
            html.AppendLine("        <tbody>");

            int numero = 1;
            foreach (var inscription in inscriptions)
            {
                var statutClass = inscription.StatutPaiement == Models.StatutPaiement.REGLE ? "statut-paye" : "statut-a-payer";
                html.AppendLine("            <tr>");
                html.AppendLine($"                <td>{numero}</td>");
                html.AppendLine($"                <td>{inscription.NomComplet}</td>");
                html.AppendLine($"                <td class='{statutClass}'>{inscription.StatutPaiementDisplay}</td>");
                html.AppendLine($"                <td>{(inscription.Present ? "✓" : "☐")}</td>");
                html.AppendLine("                <td></td>");
                html.AppendLine("            </tr>");
                numero++;
            }

            html.AppendLine("        </tbody>");
            html.AppendLine("    </table>");

            html.AppendLine($"    <div class='footer'>Document généré le {DateTime.Now:dd/MM/yyyy à HH:mm} - RAM Les Fripouilles</div>");

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }
    }
}
