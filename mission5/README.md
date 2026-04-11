# Mission 5 - Gestion des Ateliers d'Éveil

Application lourde C# développée avec Avalonia UI pour la gestion des ateliers d'éveil du RAM "Les Fripouilles".

## 📋 Fonctionnalités

### 🔐 Authentification

- Connexion sécurisée avec email/mot de passe
- Intégration avec l'API backend (NestJS)
- Gestion des tokens JWT

### 📅 Gestion des Ateliers

- **Liste des ateliers** : Visualisation de tous les ateliers programmés
  - Affichage des informations détaillées (date, horaire, lieu, places)
  - Filtres et recherche
  - Actions rapides (modifier, supprimer, voir inscriptions)

- **Création/Modification d'atelier** :
  - Nom et description
  - Date et horaires (début/fin)
  - Lieu et capacité d'accueil
  - Type de public (Parents, Assistantes maternelles, Mixte)
  - Restrictions d'âge optionnelles (min/max en mois)
  - Date limite d'inscription

### 👥 Gestion des Inscriptions

- Visualisation des inscriptions par atelier
- Affichage des informations de l'atelier
- Statistiques (places occupées/disponibles)
- Export des listes :
  - **Format CSV** : Exportation sur le bureau
  - **Format HTML** : Génération pour impression avec mise en forme

## 🎨 Interface Utilisateur

L'application utilise un design moderne et épuré inspiré de Tailwind CSS :

- Sidebar de navigation
- Palette de couleurs cohérente
- Composants avec coins arrondis
- Ombres subtiles
- Animations et transitions fluides

### Thème

- **Couleurs principales** :
  - Bleu : `#4299e1` (boutons, accents)
  - Gris : `#2d3748` (texte principal)
  - Gris clair : `#f7fafc` (arrière-plans)

## 🏗️ Architecture

### Structure du projet

```
mission5/
├── Models/                    # Modèles de données
│   ├── Atelier.cs
│   ├── InscriptionAtelier.cs
│   └── Enfant.cs
├── Services/                  # Services API et utilitaires
│   ├── ApiClient.cs          # Client HTTP générique
│   ├── AuthService.cs        # Authentification
│   ├── AtelierService.cs     # API des ateliers
│   └── ExportService.cs      # Export CSV/HTML
├── ViewModels/               # ViewModels MVVM
│   ├── ViewModelBase.cs
│   ├── LoginWindowViewModel.cs
│   ├── MainWindowViewModel.cs
│   ├── AteliersListViewModel.cs
│   ├── AtelierFormViewModel.cs
│   └── InscriptionsViewModel.cs
└── Views/                    # Vues Avalonia
    ├── LoginWindow.axaml
    ├── MainWindow.axaml
    ├── AteliersListView.axaml
    ├── AtelierFormView.axaml
    └── InscriptionsView.axaml
```

### Technologies

- **.NET 10.0**
- **Avalonia UI 11.3.12** : Framework UI cross-platform
- **ReactiveUI 11.3.8** : MVVM framework with reactive extensions
- **HttpClient** : Communication avec l'API REST

## 🚀 Installation et Lancement

### Prérequis

- .NET 10.0 SDK
- API Backend en cours d'exécution sur `http://localhost:3000`

### Configuration

L'URL de l'API est définie dans `Services/ApiClient.cs` :

```csharp
private const string BaseUrl = "http://localhost:3000/api";
```

### Lancement

```powershell
cd mission5
dotnet restore
dotnet run
```

ou depuis Visual Studio :

- Ouvrir `mission5.slnx`
- Appuyer sur F5

## 🔌 API Endpoints Utilisés

### Authentification

- `POST /api/auth/login` : Connexion

### Ateliers

- `GET /api/ateliers` : Liste des ateliers
- `GET /api/ateliers/:id` : Détails d'un atelier
- `POST /api/ateliers` : Création d'un atelier
- `PUT /api/ateliers/:id` : Modification d'un atelier
- `DELETE /api/ateliers/:id` : Suppression d'un atelier

### Inscriptions

- `GET /api/ateliers/:id/inscriptions` : Liste des inscriptions

## 📝 Utilisation

### 1. Connexion

- Lancer l'application
- Entrer vos identifiants (email/mot de passe)
- Cliquer sur "Se connecter"

### 2. Créer un atelier

- Cliquer sur "Nouvel Atelier"
- Remplir le formulaire
- Cliquer sur "Enregistrer"

### 3. Gérer les inscriptions

- Cliquer sur "👥 Inscriptions" d'un atelier
- Consulter la liste des inscrits
- Exporter en CSV ou imprimer la liste

### 4. Export et Impression

- **Export CSV** : Fichier enregistré sur le Bureau
- **Impression** : Génère un HTML qui s'ouvre dans le navigateur

## 🎯 Mission Pédagogique

Cette application répond aux exigences de la **Mission 5** du cahier des charges :

- ✅ Application lourde en C#
- ✅ Programmation orientée objet (POO)
- ✅ Architecture MVVM
- ✅ Gestion des ateliers (création, modification, planification)
- ✅ Visualisation des inscriptions
- ✅ Édition des listes d'inscrits (export/impression)
- ✅ Interface moderne et intuitive

## 👥 Crédits

Développé pour le BTS SIO 2025-2026
RAM "Les Fripouilles" - Atelier de Professionnalisation
