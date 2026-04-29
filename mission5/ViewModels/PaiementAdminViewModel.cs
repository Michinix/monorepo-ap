using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using mission5.Models;
using mission5.Services;

namespace mission5.ViewModels
{
    public class PaiementAdminViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        private ObservableCollection<Atelier> _ateliers;
        private Atelier? _selectedAtelier;
        private ObservableCollection<InscriptionAtelier> _inscriptions;
        private bool _isLoading;
        private string _filterText = string.Empty;

        public PaiementAdminViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _ateliers = new ObservableCollection<Atelier>();
            _inscriptions = new ObservableCollection<InscriptionAtelier>();

            BackCommand = ReactiveCommand.Create(Back);
            RefreshCommand = ReactiveCommand.CreateFromTask(LoadAteliersAsync);
            SelectAtelierCommand = ReactiveCommand.CreateFromTask<Atelier>(SelectAtelier);
            TogglePaiementCommand = ReactiveCommand.CreateFromTask<InscriptionAtelier>(TogglePaiement);

            _ = LoadAteliersAsync();
        }

        public ReactiveCommand<Unit, Unit> BackCommand { get; }
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }
        public ReactiveCommand<Atelier, Unit> SelectAtelierCommand { get; }
        public ReactiveCommand<InscriptionAtelier, Unit> TogglePaiementCommand { get; }

        public ObservableCollection<Atelier> Ateliers
        {
            get => _ateliers;
            set => this.RaiseAndSetIfChanged(ref _ateliers, value);
        }

        public Atelier? SelectedAtelier
        {
            get => _selectedAtelier;
            set => this.RaiseAndSetIfChanged(ref _selectedAtelier, value);
        }

        public ObservableCollection<InscriptionAtelier> Inscriptions
        {
            get => _inscriptions;
            set => this.RaiseAndSetIfChanged(ref _inscriptions, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                this.RaiseAndSetIfChanged(ref _filterText, value);
                this.RaisePropertyChanged(nameof(FilteredInscriptions));
            }
        }

        public ObservableCollection<InscriptionAtelier> FilteredInscriptions
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_filterText))
                    return _inscriptions;

                var filtered = _inscriptions
                    .Where(i => i.NomComplet.Contains(_filterText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return new ObservableCollection<InscriptionAtelier>(filtered);
            }
        }

        public int NombreNonPayes => Inscriptions.Count(i => i.StatutPaiement == StatutPaiement.A_REGLER);
        public int NombrePayes => Inscriptions.Count(i => i.StatutPaiement == StatutPaiement.REGLE);

        private async Task LoadAteliersAsync()
        {
            IsLoading = true;
            try
            {
                var ateliers = await AtelierService.Instance.GetAteliersAsync();
                Ateliers.Clear();
                foreach (var atelier in ateliers.OrderByDescending(a => a.Date))
                {
                    Ateliers.Add(atelier);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SelectAtelier(Atelier atelier)
        {
            SelectedAtelier = atelier;
            IsLoading = true;
            try
            {
                var fullAtelier = await AtelierService.Instance.GetAtelierByIdAsync(atelier.Id);
                if (fullAtelier?.Inscriptions != null)
                {
                    Inscriptions.Clear();
                    foreach (var inscription in fullAtelier.Inscriptions)
                    {
                        Inscriptions.Add(inscription);
                    }
                }

                this.RaisePropertyChanged(nameof(NombreNonPayes));
                this.RaisePropertyChanged(nameof(NombrePayes));
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task TogglePaiement(InscriptionAtelier inscription)
        {
            try
            {
                var newStatut = inscription.StatutPaiement == StatutPaiement.REGLE
                    ? StatutPaiement.A_REGLER
                    : StatutPaiement.REGLE;

                var success = await AtelierService.Instance.UpdatePaiementAsync(inscription.Id, newStatut);
                if (success && SelectedAtelier != null)
                {
                    // Recharger les inscriptions
                    await SelectAtelier(SelectedAtelier);
                }
            }
            catch (Exception ex)
            {
                // Log error
            }
        }

        private void Back()
        {
            _mainWindowViewModel.ShowAteliers();
        }
    }
}
