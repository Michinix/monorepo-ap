using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System;
using mission5.Models;
using mission5.Services;

namespace mission5.ViewModels
{
    public class InscriptionsViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly int _atelierId;

        private Atelier? _atelier;
        private ObservableCollection<InscriptionAtelier> _inscriptions;
        private bool _isLoading;

        public InscriptionsViewModel(MainWindowViewModel mainWindowViewModel, int atelierId)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _atelierId = atelierId;
            _inscriptions = new ObservableCollection<InscriptionAtelier>();

            BackCommand = ReactiveCommand.Create(Back);
            RefreshCommand = ReactiveCommand.CreateFromTask(LoadDataAsync);
            ExportCommand = ReactiveCommand.CreateFromTask(ExportAsync);
            PrintCommand = ReactiveCommand.CreateFromTask(PrintAsync);

            _ = LoadDataAsync();
        }

        public ReactiveCommand<Unit, Unit> BackCommand { get; }
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }
        public ReactiveCommand<Unit, Unit> ExportCommand { get; }
        public ReactiveCommand<Unit, Unit> PrintCommand { get; }

        public Atelier? Atelier
        {
            get => _atelier;
            set => this.RaiseAndSetIfChanged(ref _atelier, value);
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

        public int NombreInscrits => Inscriptions.Count;
        public int PlacesRestantes => (Atelier?.NombrePlaces ?? 0) - NombreInscrits;

        private async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                // Charger l'atelier (avec les inscriptions incluses)
                Atelier = await AtelierService.Instance.GetAtelierByIdAsync(_atelierId);

                // Utiliser les inscriptions déjà incluses dans l'atelier
                Inscriptions.Clear();
                if (Atelier?.Inscriptions != null)
                {
                    foreach (var inscription in Atelier.Inscriptions)
                    {
                        Inscriptions.Add(inscription);
                    }
                }

                this.RaisePropertyChanged(nameof(NombreInscrits));
                this.RaisePropertyChanged(nameof(PlacesRestantes));
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void Back()
        {
            _mainWindowViewModel.ShowAteliers();
        }

        private async Task ExportAsync()
        {
            if (Atelier == null) return;

            try
            {
                var fileName = $"Inscriptions_{Atelier.Nom.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.csv";
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                var success = await ExportService.Instance.ExportInscriptionsToCSV(
                    Atelier,
                    Inscriptions.ToList(),
                    filePath
                );

                if (success)
                {
                    // Optionnel: Ouvrir le fichier
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task PrintAsync()
        {
            if (Atelier == null) return;

            try
            {
                var html = ExportService.Instance.GenerateInscriptionsHTML(Atelier, Inscriptions.ToList());
                var fileName = $"Inscriptions_{Atelier.Nom.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.html";
                var filePath = Path.Combine(Path.GetTempPath(), fileName);

                await File.WriteAllTextAsync(filePath, html);

                // Ouvrir le fichier HTML dans le navigateur par défaut pour impression
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
            }
        }
    }
}
