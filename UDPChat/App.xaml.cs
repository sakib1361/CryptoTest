using System.Windows;
using UDPChat.Pages;

namespace UDPChat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var home = new HomePage();
            home.Show();
        }
    }
}
