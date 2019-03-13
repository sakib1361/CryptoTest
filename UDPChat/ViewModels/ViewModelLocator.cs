using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDPChat.Engine;
using UDPChat.Pages;

namespace UDPChat.ViewModels
{
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<ChatEngine>();
            SimpleIoc.Default.Register<HomePageModel>();
        }
        public HomePageModel HomePageModel => SimpleIoc.Default.GetInstance<HomePageModel>();
    }
}
