using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPChat.Model
{
    public class ChatObject
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string EncryptedMessage { get; set; }
    }
}
