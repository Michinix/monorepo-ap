using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using mission5.ViewModels;
using mission5.Views;

namespace mission5
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new LoginWindow
                {
                    DataContext = new LoginWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}