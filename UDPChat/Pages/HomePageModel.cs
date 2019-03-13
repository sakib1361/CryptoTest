using System.Collections.ObjectModel;
using UDPChat.Engine;
using UDPChat.Model;
using UDPChat.ViewModels;

namespace UDPChat.Pages
{
    public class HomePageModel : MainViewModel
    {
        private ChatEngine ChatEngine;
        public string Address { get; set; }
        public string Password { get; set; }
        public bool AddressEdit { get; set; }
        public string Message { get; set; }
        public ObservableCollection<ChatObject> ChatObjects { get; set; }
        public HomePageModel(ChatEngine chatEngine)
        {
            ChatEngine = chatEngine;
            ChatObjects = new ObservableCollection<ChatObject>();
            ChatEngine.MesssageReceived += ChatEngine_MesssageReceived;
        }

        private void ChatEngine_MesssageReceived(object sender, Model.ChatObject e)
        {
            
        }
    }
}
