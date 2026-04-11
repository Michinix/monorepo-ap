using System;
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
                var result = await ApiClient.Instance.PostAsync<LoginResponse>("/api/auth/login", new { email, password });

                if (result?.Token == null)
                    return false;

                ApiClient.Instance.SetToken(result.Token);
                return true;
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (Exception)
            {
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
