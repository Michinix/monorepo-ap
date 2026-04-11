using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using mission5.Models;
using mission5.Services;

namespace mission5.ViewModels
{
    public class AtelierFormViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly int? _atelierId;

        private string _nom = string.Empty;
        private string _description = string.Empty;
        private DateTime _date = DateTime.Today.AddDays(7);
        private TimeSpan _heureDebut = new TimeSpan(9, 0, 0);
        private TimeSpan _heureFin = new TimeSpan(11, 0, 0);
        private DateTime _dateLimiteInscription = DateTime.Today.AddDays(5);
        private int _nombrePlaces = 15;
        private string _lieu = string.Empty;
        private TypePublicAtelier _typePublic = TypePublicAtelier.ENFANT;
        private int? _ageMinMois;
        private int? _ageMaxMois;
        private string _errorMessage = string.Empty;
        private string _successMessage = string.Empty;

        public AtelierFormViewModel(MainWindowViewModel mainWindowViewModel, int? atelierId = null)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _atelierId = atelierId;

            SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
            CancelCommand = ReactiveCommand.Create(Cancel);

            TypesPublic = new List<TypePublicItem>
            {
                new TypePublicItem { Value = TypePublicAtelier.ENFANT, Display = "Enfants" },
                new TypePublicItem { Value = TypePublicAtelier.PARENT_UNIQUEMENT, Display = "Parents uniquement" },
                new TypePublicItem { Value = TypePublicAtelier.ASSISTANT_UNIQUEMENT, Display = "Assistantes uniquement" },
                new TypePublicItem { Value = TypePublicAtelier.MIXTE, Display = "Mixte" }
            };

            if (atelierId.HasValue)
            {
                LoadAtelierAsync(atelierId.Value);
            }
        }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
        public List<TypePublicItem> TypesPublic { get; }

        public string Title => _atelierId.HasValue ? "Modifier l'atelier" : "Nouvel atelier";

        public string Nom
        {
            get => _nom;
            set => this.RaiseAndSetIfChanged(ref _nom, value);
        }

        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        public DateTime Date
        {
            get => _date;
            set => this.RaiseAndSetIfChanged(ref _date, value);
        }

        public TimeSpan HeureDebut
        {
            get => _heureDebut;
            set => this.RaiseAndSetIfChanged(ref _heureDebut, value);
        }

        public TimeSpan HeureFin
        {
            get => _heureFin;
            set => this.RaiseAndSetIfChanged(ref _heureFin, value);
        }

        public DateTime DateLimiteInscription
        {
            get => _dateLimiteInscription;
            set => this.RaiseAndSetIfChanged(ref _dateLimiteInscription, value);
        }

        public int NombrePlaces
        {
            get => _nombrePlaces;
            set => this.RaiseAndSetIfChanged(ref _nombrePlaces, value);
        }

        public string Lieu
        {
            get => _lieu;
            set => this.RaiseAndSetIfChanged(ref _lieu, value);
        }

        public TypePublicAtelier TypePublic
        {
            get => _typePublic;
            set => this.RaiseAndSetIfChanged(ref _typePublic, value);
        }

        public int? AgeMinMois
        {
            get => _ageMinMois;
            set => this.RaiseAndSetIfChanged(ref _ageMinMois, value);
        }

        public int? AgeMaxMois
        {
            get => _ageMaxMois;
            set => this.RaiseAndSetIfChanged(ref _ageMaxMois, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public string SuccessMessage
        {
            get => _successMessage;
            set => this.RaiseAndSetIfChanged(ref _successMessage, value);
        }

        private async void LoadAtelierAsync(int id)
        {
            var atelier = await AtelierService.Instance.GetAtelierByIdAsync(id);
            if (atelier != null)
            {
                Nom = atelier.Nom;
                Description = atelier.Description ?? string.Empty;
                Date = atelier.Date;
                HeureDebut = TimeSpan.FromMinutes(atelier.DebutMinutes);
                HeureFin = TimeSpan.FromMinutes(atelier.FinMinutes);
                DateLimiteInscription = atelier.DateLimiteInscription;
                NombrePlaces = atelier.NombrePlaces;
                Lieu = atelier.Lieu;
                TypePublic = atelier.TypePublic;
                AgeMinMois = atelier.AgeMinMois;
                AgeMaxMois = atelier.AgeMaxMois;
            }
        }

        private async Task SaveAsync()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            // Validation
            if (string.IsNullOrWhiteSpace(Nom))
            {
                ErrorMessage = "Le nom est obligatoire.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Lieu))
            {
                ErrorMessage = "Le lieu est obligatoire.";
                return;
            }

            if (NombrePlaces <= 0)
            {
                ErrorMessage = "Le nombre de places doit être supérieur à 0.";
                return;
            }

            if (HeureFin <= HeureDebut)
            {
                ErrorMessage = "L'heure de fin doit être après l'heure de début.";
                return;
            }

            var dto = new AtelierCreateDto
            {
                Nom = Nom,
                Description = string.IsNullOrWhiteSpace(Description) ? null : Description,
                Date = Date,
                DebutMinutes = (int)HeureDebut.TotalMinutes,
                FinMinutes = (int)HeureFin.TotalMinutes,
                DateLimiteInscription = DateLimiteInscription,
                NombrePlaces = NombrePlaces,
                Lieu = Lieu,
                TypePublic = TypePublic,
                AgeMinMois = AgeMinMois,
                AgeMaxMois = AgeMaxMois
            };

            try
            {
                if (_atelierId.HasValue)
                {
                    var updateDto = new AtelierUpdateDto
                    {
                        Nom = dto.Nom,
                        Description = dto.Description,
                        Date = dto.Date,
                        DebutMinutes = dto.DebutMinutes,
                        FinMinutes = dto.FinMinutes,
                        DateLimiteInscription = dto.DateLimiteInscription,
                        NombrePlaces = dto.NombrePlaces,
                        Lieu = dto.Lieu,
                        TypePublic = dto.TypePublic,
                        AgeMinMois = dto.AgeMinMois,
                        AgeMaxMois = dto.AgeMaxMois,
                        AnimateurId = dto.AnimateurId
                    };
                    var result = await AtelierService.Instance.UpdateAtelierAsync(_atelierId.Value, updateDto);
                    if (result != null)
                    {
                        SuccessMessage = "Atelier modifié avec succès !";
                        await Task.Delay(1000);
                        _mainWindowViewModel.ShowAteliers();
                    }
                    else
                    {
                        ErrorMessage = "Erreur lors de la modification de l'atelier.";
                    }
                }
                else
                {
                    var result = await AtelierService.Instance.CreateAtelierAsync(dto);
                    if (result != null)
                    {
                        SuccessMessage = "Atelier créé avec succès !";
                        await Task.Delay(1000);
                        _mainWindowViewModel.ShowAteliers();
                    }
                    else
                    {
                        ErrorMessage = "Erreur lors de la création de l'atelier.";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur: {ex.Message}";
            }
        }

        private void Cancel()
        {
            _mainWindowViewModel.ShowAteliers();
        }
    }

    public class TypePublicItem
    {
        public TypePublicAtelier Value { get; set; }
        public string Display { get; set; } = string.Empty;
    }
}
