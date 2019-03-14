using CryptoAlgorithm.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using UDPChat.Engine;
using UDPChat.Model;
using UDPChat.ViewModels;

namespace UDPChat.Pages
{
    public class HomePageModel : MainViewModel
    {
        private ChatEngine ChatEngine;
        private IEncryption EncryptionAlgo;

        public ObservableCollection<ChatObject> RoomChats { get; set; }
        public ObservableCollection<string> LogDatas { get; set; }
        public string Password { get; set; } = "qpeqmzforxrqtsms";
        public string Message { get; set; }
        public string Address { get; set; } = "224.0.0.1";
        public string Username { get; set; } = "User1";

        public HomePageModel(ChatEngine chatEngine, IEncryption encryption)
        {
            ChatEngine = chatEngine;
            EncryptionAlgo = encryption;
            ChatEngine.MesssageReceived += ChatEngine_MesssageReceived;
            RoomChats = new ObservableCollection<ChatObject>();
            LogDatas = new ObservableCollection<string>();
        }

        private void ChatEngine_MesssageReceived(object sender, ChatObject e)
        {
            try
            {
                var m = EncryptionAlgo.Decrypt(e.Message, Password);
                if (string.IsNullOrWhiteSpace(m))
                    LogDatas.Add(string.Format("{0}: {1}", e.From, e.Message));
                else
                {
                    e.Message = m;
                    RoomChats.Add(e);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                LogDatas.Add(string.Format("{0}: {1}", e.From, e.Message));
            }
        }

        public override void OnAppear()
        {
            base.OnAppear();
        }

        public ICommand ApplyCommand => new RelayCommand(ApplyAction);
        public ICommand SendCommand => new RelayCommand(SendAction);

        private async void SendAction()
        {
            if (string.IsNullOrWhiteSpace(Message)) return;
            try
            {
                if (ChatEngine.Active == false || string.IsNullOrWhiteSpace(Username))
                {
                    ApplyAction();
                    await Task.Delay(500);
                    var ch = new ChatObject(Username, Username + " has joined");
                    await ChatEngine.Send(ch);
                    ch = new ChatObject(Username, EncryptionAlgo.Encrypt(Message, Password));
                    await ChatEngine.Send(ch);
                }
                else
                {
                    var ch = new ChatObject(Username, EncryptionAlgo.Encrypt(Message, Password));
                    await ChatEngine.Send(ch);
                }
            }catch(Exception ex)
            {
                LogDatas.Add(ex.Message);
            }
            Message = string.Empty;
        }

        private void ApplyAction()
        {
            ChatEngine.Stop();
            ChatEngine.Start(Address);
        }
    }
}
