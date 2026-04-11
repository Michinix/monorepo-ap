#!/bin/sh
set -e

echo "🚀 Démarrage du conteneur..."

# Attente de la base de données (Utilise les variables d'environnement du .env)
echo "⏳ Attente de la base de données sur $DB_HOST:5432..."
until nc -z "$DB_HOST" 5432; do
  echo "❌ DB non prête, nouvelle tentative dans 2s..."
  sleep 2
done
echo "✅ Base de données prête !"

# Application des migrations
echo "📦 Application des migrations Prisma..."
npx prisma migrate deploy --config prisma.config.ts

if [ -f "prisma/seed.ts" ]; then
  echo "🌱 Exécution du seed..."
  npx tsx prisma/seed.ts || echo "⚠️ Seed échoué"
fi

echo "🔥 Lancement de l'application..."
exec node dist/src/main.js
