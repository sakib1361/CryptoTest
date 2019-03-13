using System.Windows;
using Xamarin.Forms;

namespace UDPChat.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Forms.Init();
            LoadApplication(new UDPChat.App());
        }
    }
}
