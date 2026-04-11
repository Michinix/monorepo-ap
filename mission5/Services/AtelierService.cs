using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using mission5.Models;

namespace mission5.Services
{
    public class AtelierService
    {
        private static AtelierService? _instance;
        public static AtelierService Instance => _instance ??= new AtelierService();

        private AtelierService() { }

        public async Task<List<Atelier>> GetAteliersAsync()
        {
            try
            {
                var result = await ApiClient.Instance.GetAsync<List<Atelier>>("/api/ateliers");
                return result ?? new List<Atelier>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur GetAteliersAsync: {ex.Message}");
                return new List<Atelier>();
            }
        }

        public async Task<Atelier?> GetAtelierByIdAsync(int id)
        {
            try
            {
                return await ApiClient.Instance.GetAsync<Atelier>($"/api/ateliers/{id}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur GetAtelierByIdAsync ({id}): {ex.Message}");
                return null;
            }
        }

        public async Task<Atelier?> CreateAtelierAsync(AtelierCreateDto atelier)
        {
            try
            {
                var response = await ApiClient.Instance.PostAsync<CreateAtelierResponse>("/api/ateliers", atelier);
                return response?.Data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur CreateAtelierAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<Atelier?> UpdateAtelierAsync(int id, AtelierUpdateDto atelier)
        {
            try
            {
                var response = await ApiClient.Instance.PutAsync<UpdateAtelierResponse>($"/api/ateliers/{id}", atelier);
                return response?.Data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur UpdateAtelierAsync ({id}): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteAtelierAsync(int id)
        {
            try
            {
                await ApiClient.Instance.DeleteAsync($"/api/ateliers/{id}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur DeleteAtelierAsync ({id}): {ex.Message}");
                return false;
            }
        }

        public async Task<List<InscriptionAtelier>> GetInscriptionsAsync(int atelierId)
        {
            try
            {
                var result = await ApiClient.Instance.GetAsync<List<InscriptionAtelier>>($"/api/ateliers/{atelierId}/inscriptions");
                return result ?? new List<InscriptionAtelier>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur GetInscriptionsAsync ({atelierId}): {ex.Message}");
                return new List<InscriptionAtelier>();
            }
        }
    }

    public class AtelierCreateDto
    {
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
    }

    public class AtelierUpdateDto : AtelierCreateDto
    {
    }

    public class CreateAtelierResponse
    {
        public string? Message { get; set; }
        public Atelier? Data { get; set; }
    }

    public class UpdateAtelierResponse
    {
        public string? Message { get; set; }
        public Atelier? Data { get; set; }
    }
}
