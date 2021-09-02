using NeuralNet_UI.ViewModels;
using NeuralNet_UI.Views;
using Autofac;
using System.Windows;

namespace NeuralNet_UI
{
    public partial class App : Application
    {
        // async void event
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IContainer Container = new DIManager().Container;

            using (var scope = Container.BeginLifetimeScope())
            {
                MainWindow = scope.Resolve<MainWindow>();
                await (MainWindow.DataContext as IMainWindowVM)
                    .SetDefaultValues(scope.Resolve<ISessionContext>());
                MainWindow.Show();
                scope.Resolve<SampleImportWindow>().Owner = MainWindow;
            }
        }
    }
}
