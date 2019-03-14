using PropertyChanged;

namespace UDPChat.Model
{
    [AddINotifyPropertyChangedInterface]
    public class ChatObject
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
        public ChatObject() { }

        public ChatObject(string from, string message)
        {
            this.From = from;
            this.Message = message;
        }
    }
}
