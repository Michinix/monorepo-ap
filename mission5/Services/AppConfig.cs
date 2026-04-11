using System;
using DotNetEnv;

namespace mission5.Services
{
    public static class AppConfig
    {
        public static string ApiBaseUrl { get; private set; }

        static AppConfig()
        {
            // Charger le fichier .env
            Env.Load();

            // Lire la variable d'environnement ou utiliser une valeur par défaut
            ApiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:3000";
        }
    }
}
