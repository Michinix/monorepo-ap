using ReactiveUI;
using System.Reactive;
using mission5.Services;

namespace mission5.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentPage;

        public MainWindowViewModel()
        {
            ShowAteliersCommand = ReactiveCommand.Create(ShowAteliers);
            ShowPaiementAdminCommand = ReactiveCommand.Create(ShowPaiementAdmin);
            LogoutCommand = ReactiveCommand.Create(Logout);

            // Par défaut, afficher la liste des ateliers
            _currentPage = new AteliersListViewModel(this);
        }

        public ReactiveCommand<Unit, Unit> ShowAteliersCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowPaiementAdminCommand { get; }
        public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

        public ViewModelBase CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public void ShowAteliers()
        {
            CurrentPage = new AteliersListViewModel(this);
        }

        public void ShowAtelierForm(int? atelierId = null)
        {
            CurrentPage = new AtelierFormViewModel(this, atelierId);
        }

        public void ShowInscriptions(int atelierId)
        {
            CurrentPage = new InscriptionsViewModel(this, atelierId);
        }

        public void ShowPaiementAdmin()
        {
            CurrentPage = new PaiementAdminViewModel(this);
        }

        private void Logout()
        {
            AuthService.Instance.Logout();
            // La gestion de la déconnexion sera gérée dans App.axaml.cs
        }
    }
}
