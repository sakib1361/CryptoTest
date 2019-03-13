using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UDPChat.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public bool IsBusy { get; set; }
        public event EventHandler CloseHandler;
        public ICommand LoadedCommand { get => new RelayCommand(OnAppear); }
        public ICommand UnLoadedCommand { get => new RelayCommand(OnDisAppear); }
        public ICommand CommonCommand { get => new RelayCommand(NotImplemented); }
        public ICommand CloseCommand => new RelayCommand(CloseAction);


        public virtual void OnDisAppear()
        {

        }

        public virtual void OnAppear()
        {

        }

        public virtual void CloseAction()
        {
            CloseHandler?.Invoke(this, null);
        }

        private void NotImplemented()
        {

        }
    }
}