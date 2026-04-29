using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace mission5.Services
{
    internal class AuthService
    {
        private static AuthService? _instance;
        public static AuthService Instance => _instance ??= new AuthService();

        private AuthService() { }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                Debug.WriteLine($"LoginAsync - Tentative de connexion avec {email}");
                var result = await ApiClient.Instance.PostAsync<LoginResponse>("/api/auth/login", new { email, password });

                Debug.WriteLine($"LoginAsync - Réponse reçue, Token: {result?.Token}");

                if (result?.Token == null)
                {
                    Debug.WriteLine("LoginAsync - Token null");
                    return false;
                }

                ApiClient.Instance.SetToken(result.Token);
                Debug.WriteLine("LoginAsync - Token défini avec succès");
                return true;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"LoginAsync - HttpRequestException: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoginAsync - Exception: {ex.Message}");
                Debug.WriteLine($"LoginAsync - StackTrace: {ex.StackTrace}");
                return false;
            }
        }

        public void Logout()
        {
            ApiClient.Instance.ClearToken();
        }
    }

    public class LoginResponse
    {
        public string? Token { get; set; }
    }
}
