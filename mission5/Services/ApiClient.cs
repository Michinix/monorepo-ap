using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace mission5.Services
{
    internal class ApiClient
    {
        private static ApiClient? _instance;
        public static ApiClient Instance => _instance ??= new ApiClient();

        private readonly HttpClient _httpClient;

        private ApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(AppConfig.ApiBaseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip
                };
                options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

                return JsonSerializer.Deserialize<T>(json, options);
            }
            catch (JsonException ex)
            {
                throw;
            }
        }

        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            serializeOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

            var body = JsonSerializer.Serialize(data, serializeOptions);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var deserializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip
            };
            deserializeOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

            return JsonSerializer.Deserialize<T>(json, deserializeOptions);
        }

        public async Task<T?> PutAsync<T>(string endpoint, object data)
        {
            return await PatchAsync<T>(endpoint, data);
        }

        public async Task<T?> PatchAsync<T>(string endpoint, object data)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            serializeOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

            var body = JsonSerializer.Serialize(data, serializeOptions);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var deserializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip
            };
            deserializeOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

            return JsonSerializer.Deserialize<T>(json, deserializeOptions);
        }

        public async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
        }
    }
}
