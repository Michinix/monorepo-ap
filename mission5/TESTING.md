# Guide de Test - Mission 5

## Prérequis

### Backend

L'API backend doit être lancée et accessible sur `http://localhost:3000`

```powershell
cd backend
npm install
npm run start:dev
```

### Base de données

Assurez-vous que PostgreSQL est en cours d'exécution et que les migrations sont appliquées :

```powershell
cd backend
npx prisma migrate dev
npx prisma db seed # Optionnel : données de test
```

## Lancement de l'Application

### Option 1 : Via .NET CLI

```powershell
cd mission5
dotnet restore
dotnet run
```

### Option 2 : Via Visual Studio

1. Ouvrir `mission5.slnx`
2. Appuyer sur `F5` ou cliquer sur "Démarrer"

## Scénarios de Test

### 1. Authentification

#### Test 1.1 : Connexion réussie

**Objectif** : Vérifier qu'un utilisateur peut se connecter

**Étapes** :

1. Lancer l'application
2. Entrer un email valide existant dans la base de données
3. Entrer le mot de passe correspondant
4. Cliquer sur "Se connecter"

**Résultat attendu** :

- ✅ La fenêtre principale s'ouvre
- ✅ La sidebar affiche "Les Fripouilles"
- ✅ La liste des ateliers s'affiche

#### Test 1.2 : Connexion échouée

**Objectif** : Vérifier la gestion des erreurs d'authentification

**Étapes** :

1. Entrer un email invalide
2. Entrer un mot de passe quelconque
3. Cliquer sur "Se connecter"

**Résultat attendu** :

- ✅ Message d'erreur en rouge s'affiche
- ✅ La fenêtre de connexion reste ouverte
- ✅ Les champs ne sont pas vidés

#### Test 1.3 : Validation des champs

**Objectif** : Vérifier la validation côté client

**Étapes** :

1. Laisser les champs vides
2. Cliquer sur "Se connecter"

**Résultat attendu** :

- ✅ Message "Email ou mot de passe invalide." s'affiche
- ✅ Aucune requête API n'est envoyée

### 2. Gestion des Ateliers

#### Test 2.1 : Affichage de la liste

**Objectif** : Vérifier l'affichage de tous les ateliers

**Étapes** :

1. Se connecter
2. Observer la liste des ateliers

**Résultat attendu** :

- ✅ Tous les ateliers sont affichés sous forme de cartes
- ✅ Chaque carte affiche : date, nom, horaire, lieu, places
- ✅ Le type de public est affiché dans un badge coloré

#### Test 2.2 : Création d'un atelier

**Objectif** : Créer un nouvel atelier

**Étapes** :

1. Cliquer sur "Nouvel Atelier"
2. Remplir le formulaire :
   - Nom : "Atelier Test Musique"
   - Description : "Découverte des instruments"
   - Date : Choisir une date future
   - Heure début : 09:00
   - Heure fin : 11:00
   - Lieu : "Salle d'activité"
   - Nombre de places : 15
   - Type de public : "Parents"
   - Date limite d'inscription : Date avant l'atelier
3. Cliquer sur "Enregistrer"

**Résultat attendu** :

- ✅ Message de succès en vert s'affiche
- ✅ Redirection vers la liste des ateliers
- ✅ Le nouvel atelier apparaît dans la liste
- ✅ Toutes les informations sont correctes

#### Test 2.3 : Modification d'un atelier

**Objectif** : Modifier un atelier existant

**Étapes** :

1. Cliquer sur l'icône ✏️ d'un atelier
2. Modifier le nom : "Atelier Test Musique (Modifié)"
3. Modifier le nombre de places : 20
4. Cliquer sur "Enregistrer"

**Résultat attendu** :

- ✅ Message de succès s'affiche
- ✅ Retour à la liste
- ✅ Les modifications sont visibles

#### Test 2.4 : Suppression d'un atelier

**Objectif** : Supprimer un atelier

**Étapes** :

1. Cliquer sur l'icône 🗑️ d'un atelier
2. Observer

**Résultat attendu** :

- ✅ L'atelier disparaît de la liste
- ✅ Le compteur de places se met à jour

#### Test 2.5 : Validation du formulaire

**Objectif** : Vérifier les validations

**Test 2.5.1 : Nom vide**

- Laisser le nom vide
- Cliquer sur "Enregistrer"
- ✅ Message d'erreur : "Le nom est obligatoire."

**Test 2.5.2 : Horaires invalides**

- Heure début : 11:00
- Heure fin : 09:00
- Cliquer sur "Enregistrer"
- ✅ Message d'erreur : "L'heure de fin doit être après l'heure de début."

**Test 2.5.3 : Nombre de places invalide**

- Nombre de places : 0 ou négatif
- Cliquer sur "Enregistrer"
- ✅ Message d'erreur : "Le nombre de places doit être supérieur à 0."

#### Test 2.6 : Actualisation de la liste

**Objectif** : Vérifier le bouton d'actualisation

**Étapes** :

1. Cliquer sur "🔄 Actualiser"

**Résultat attendu** :

- ✅ Message de chargement s'affiche brièvement
- ✅ La liste se recharge
- ✅ Les données sont à jour

### 3. Gestion des Inscriptions

#### Test 3.1 : Affichage des inscriptions

**Objectif** : Consulter les inscriptions d'un atelier

**Étapes** :

1. Cliquer sur "👥 Inscriptions" d'un atelier
2. Observer la page

**Résultat attendu** :

- ✅ Informations de l'atelier affichées en haut
- ✅ Statistiques (nombre d'inscrits / places totales)
- ✅ Liste des inscrits avec numéro et nom complet
- ✅ Statut de présence affiché

#### Test 3.2 : Export CSV

**Objectif** : Exporter la liste en CSV

**Étapes** :

1. Sur la page des inscriptions
2. Cliquer sur "📥 Exporter CSV"
3. Vérifier sur le Bureau

**Résultat attendu** :

- ✅ Fichier CSV créé sur le Bureau
- ✅ Nom du fichier : `Inscriptions_[Nom]_[Date].csv`
- ✅ Le fichier s'ouvre automatiquement
- ✅ Contenu correct avec en-tête et données

#### Test 3.3 : Impression HTML

**Objectif** : Générer et imprimer la liste

**Étapes** :

1. Sur la page des inscriptions
2. Cliquer sur "🖨️ Imprimer"
3. Observer

**Résultat attendu** :

- ✅ Fichier HTML généré dans le dossier temporaire
- ✅ Le fichier s'ouvre dans le navigateur
- ✅ Mise en forme professionnelle
- ✅ Tableau avec :
  - N°
  - Nom complet
  - Colonne Présence
  - Colonne Signature (vide pour remplir à la main)

#### Test 3.4 : Retour à la liste

**Objectif** : Navigation retour

**Étapes** :

1. Sur la page des inscriptions
2. Cliquer sur "← Retour"

**Résultat attendu** :

- ✅ Retour à la liste des ateliers
- ✅ Aucune perte de données

### 4. Navigation

#### Test 4.1 : Navigation via sidebar

**Objectif** : Tester la navigation principale

**Étapes** :

1. Cliquer sur "📅 Ateliers" dans la sidebar

**Résultat attendu** :

- ✅ Retour à la liste des ateliers
- ✅ Changement de vue fluide

#### Test 4.2 : Déconnexion

**Objectif** : Se déconnecter de l'application

**Étapes** :

1. Cliquer sur "🚪 Déconnexion" dans la sidebar
2. Observer

**Résultat attendu** :

- ✅ Token supprimé
- ✅ Retour à la fenêtre de connexion
- ✅ Impossible d'accéder aux données sans se reconnecter

### 5. Tests d'Interface

#### Test 5.1 : Messages vides

**Objectif** : Vérifier l'affichage quand il n'y a pas de données

**Test si aucun atelier** :

- ✅ Message "Aucun atelier disponible"
- ✅ Icône 📅 affichée
- ✅ Texte d'aide affiché

**Test si aucune inscription** :

- ✅ Message "Aucune inscription pour le moment"
- ✅ Icône 📋 affichée

#### Test 5.2 : États de chargement

**Objectif** : Vérifier les indicateurs de chargement

**Étapes** :

1. Actualiser une liste
2. Observer brièvement

**Résultat attendu** :

- ✅ Icône ⏳ affichée
- ✅ Message "Chargement..."
- ✅ Interface non bloquée

#### Test 5.3 : Responsivité

**Objectif** : Tester la taille de la fenêtre

**Étapes** :

1. Redimensionner la fenêtre
2. Observer l'adaptation

**Résultat attendu** :

- ✅ Contenu s'adapte
- ✅ Pas de débordement
- ✅ Lisibilité maintenue

### 6. Tests de Performances

#### Test 6.1 : Liste avec beaucoup d'ateliers

**Objectif** : Vérifier les performances avec volume de données

**Prérequis** : Avoir au moins 20-30 ateliers dans la base

**Étapes** :

1. Charger la liste des ateliers
2. Observer

**Résultat attendu** :

- ✅ Chargement en moins de 2 secondes
- ✅ Pas de ralentissement
- ✅ Scrolling fluide

#### Test 6.2 : Création rapide

**Objectif** : Vérifier la réactivité du formulaire

**Étapes** :

1. Remplir le formulaire rapidement
2. Cliquer plusieurs fois sur "Enregistrer"

**Résultat attendu** :

- ✅ Un seul atelier créé (pas de doublons)
- ✅ Bouton désactivé pendant le traitement
- ✅ Feedback immédiat

### 7. Tests d'Erreurs

#### Test 7.1 : Backend inaccessible

**Objectif** : Comportement quand l'API est hors ligne

**Étapes** :

1. Arrêter le backend
2. Tenter de se connecter

**Résultat attendu** :

- ✅ Message d'erreur explicite
- ✅ Pas de crash de l'application

#### Test 7.2 : Timeout réseau

**Objectif** : Comportement en cas de lenteur réseau

**Résultat attendu** :

- ✅ Message d'erreur après timeout
- ✅ Possibilité de réessayer

## Checklist de Validation Complète

### Fonctionnalités

- [ ] Connexion/Déconnexion
- [ ] Liste des ateliers
- [ ] Créer un atelier
- [ ] Modifier un atelier
- [ ] Supprimer un atelier
- [ ] Voir les inscriptions
- [ ] Exporter en CSV
- [ ] Imprimer en HTML
- [ ] Navigation complète

### Interface

- [ ] Design cohérent
- [ ] Couleurs harmonieuses
- [ ] Icônes appropriées
- [ ] Messages de feedback
- [ ] États de chargement
- [ ] Messages d'erreur

### Données

- [ ] Sauvegarde correcte
- [ ] Chargement correct
- [ ] Mise à jour correcte
- [ ] Suppression correcte

### Performance

- [ ] Chargement rapide
- [ ] Pas de ralentissement
- [ ] Pas de fuite mémoire

## Bugs Connus

Aucun bug connu à ce jour.

## Rapporter un Bug

Si vous trouvez un bug :

1. Noter les étapes pour le reproduire
2. Capturer une capture d'écran si possible
3. Vérifier les logs de la console

## Notes de Test

- Toujours tester avec des données réelles
- Vérifier les cas limites (0, négatif, très grand)
- Tester avec différentes résolutions d'écran
- Vérifier la cohérence des données entre refresh

## Environnement de Test Recommandé

- **OS** : Windows 10/11
- **RAM** : 4 GB minimum
- **Backend** : Node.js 18+
- **Database** : PostgreSQL 14+
- **.NET** : 10.0 SDK

## Support

Pour toute question sur les tests, consulter :

- README.md - Documentation générale
- TECHNICAL.md - Documentation technique
- CHANGELOG.md - Historique des changements
