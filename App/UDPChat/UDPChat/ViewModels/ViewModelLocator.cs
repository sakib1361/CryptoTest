using CryptoAlgorithm.Model;
using GalaSoft.MvvmLight.Ioc;
using UDPChat.Engine;
using UDPChat.Pages;

namespace UDPChat.ViewModels
{
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<IEncryption, AESEncryption>();
            SimpleIoc.Default.Register<ChatEngine>();
            SimpleIoc.Default.Register<HomePageModel>();
        }
        public HomePageModel HomePageModel => SimpleIoc.Default.GetInstance<HomePageModel>();
    }
}
