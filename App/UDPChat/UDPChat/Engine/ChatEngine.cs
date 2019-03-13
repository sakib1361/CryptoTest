using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UDPChat.Model;

namespace UDPChat.Engine
{
    public class ChatEngine
    {
        private const int Port = 1819;
        public UdpClient UdpClient;
        private bool Active;
        public event EventHandler<ChatObject> MesssageReceived;

        public void Start(string address)
        {
            var m_GrpAddr = IPAddress.Parse(address);
            UdpClient = new UdpClient();
            UdpClient.Client.Bind(new IPEndPoint(IPAddress.Any,Port));
            UdpClient.JoinMulticastGroup(m_GrpAddr);
            Active = true;
            StartReceive();
        }

        public void Stop()
        {
            try
            {
                Active = false;
                UdpClient?.Close();
                UdpClient?.Dispose();
            }
            catch { }
        }

        public async Task Send(ChatObject chObj)
        {
            try
            {
                var rawMsg = JsonConvert.SerializeObject(chObj);
                var sendBytes = Encoding.UTF8.GetBytes(rawMsg);
                await UdpClient.SendAsync(sendBytes, sendBytes.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void StartReceive()
        {
            while (Active)
            {
                try
                {
                    var byteRes = await UdpClient.ReceiveAsync();
                    var rawMsg = Encoding.UTF8.GetString(byteRes.Buffer);
                    var chObj = JsonConvert.DeserializeObject<ChatObject>(rawMsg);
                    MesssageReceived?.Invoke(this, chObj);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
