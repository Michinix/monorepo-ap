using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using mission5.Models;
using mission5.Services;

namespace mission5.ViewModels
{
    public class AteliersListViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private ObservableCollection<Atelier> _ateliers;
        private bool _isLoading;
        private string _searchText = string.Empty;

        public AteliersListViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _ateliers = new ObservableCollection<Atelier>();

            CreateAtelierCommand = ReactiveCommand.Create(CreateAtelier);
            EditAtelierCommand = ReactiveCommand.Create<Atelier>(EditAtelier);
            DeleteAtelierCommand = ReactiveCommand.CreateFromTask<Atelier>(DeleteAtelier);
            ViewInscriptionsCommand = ReactiveCommand.Create<Atelier>(ViewInscriptions);
            RefreshCommand = ReactiveCommand.CreateFromTask(LoadAteliersAsync);

            // Charger les ateliers au démarrage
            _ = LoadAteliersAsync();
        }

        public ReactiveCommand<Unit, Unit> CreateAtelierCommand { get; }
        public ReactiveCommand<Atelier, Unit> EditAtelierCommand { get; }
        public ReactiveCommand<Atelier, Unit> DeleteAtelierCommand { get; }
        public ReactiveCommand<Atelier, Unit> ViewInscriptionsCommand { get; }
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

        public ObservableCollection<Atelier> Ateliers
        {
            get => _ateliers;
            set => this.RaiseAndSetIfChanged(ref _ateliers, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        private async Task LoadAteliersAsync()
        {
            IsLoading = true;
            try
            {
                var ateliers = await AtelierService.Instance.GetAteliersAsync();
                Ateliers.Clear();
                foreach (var atelier in ateliers)
                {
                    Ateliers.Add(atelier);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CreateAtelier()
        {
            _mainWindowViewModel.ShowAtelierForm();
        }

        private void EditAtelier(Atelier atelier)
        {
            _mainWindowViewModel.ShowAtelierForm(atelier.Id);
        }

        private async Task DeleteAtelier(Atelier atelier)
        {
            var success = await AtelierService.Instance.DeleteAtelierAsync(atelier.Id);
            if (success)
            {
                await LoadAteliersAsync();
            }
        }

        private void ViewInscriptions(Atelier atelier)
        {
            _mainWindowViewModel.ShowInscriptions(atelier.Id);
        }
    }
}
