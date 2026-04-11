using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using mission5.Services;
using mission5.Views;
using ReactiveUI;
using System.Windows.Input;
using System.Threading.Tasks;

namespace mission5.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public ICommand CancelCommand { get; }
        public ICommand LoginCommand { get; }

        public LoginWindowViewModel()
        {
            CancelCommand = ReactiveCommand.Create(OnCancel);
            LoginCommand = ReactiveCommand.CreateFromTask(OnLoginAsync);
        }

        private void OnCancel()
        {
            (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Shutdown();
        }

        private async Task OnLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Email ou mot de passe invalide.";
                return;
            }

            bool success = await AuthService.Instance.LoginAsync(Email, Password);

            if (success)
            {
                ErrorMessage = string.Empty;

                // Ouvrir la fenêtre principale
                var mainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel()
                };

                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var loginWindow = desktop.MainWindow;
                    desktop.MainWindow = mainWindow;
                    mainWindow.Show();
                    loginWindow?.Close();
                }
            }
            else
            {
                ErrorMessage = "Une erreur s'est produite lors de la connexion. Veuillez réessayer.";
            }
        }

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }
    }
}
