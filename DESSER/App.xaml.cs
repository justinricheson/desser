using System.Windows;
using System.Windows.Navigation;

namespace DESSER
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var view = new MainView();
            var viewmodel = new MainViewModel();
            viewmodel.PopupMessage = (m) => { MessageBox.Show(m); };
            view.DataContext = viewmodel;
            this.MainWindow = view;
            this.MainWindow.Show();
        }
    }
}
