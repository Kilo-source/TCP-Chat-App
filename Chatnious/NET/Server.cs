using System.Net.Sockets;
using System.Net;
using ChatClient.NET.IO;
using ChatServer.NET.IO;
using System;
using System.Threading.Tasks;

namespace ChatClient.NET
{
    internal class Server
    {
        TcpClient _client;
        byte[] _data = { 127, 0, 0, 1 };
        IPAddress _server;

        public PacketReader _pr;


        public event Action ConnectedEvent;
        public event Action PorukaPrimljena;
        public event Action Diskonektovan;
        public Server()
        {
            _client = new TcpClient();
            _server = new IPAddress(_data);
        }

        public void _ConnectToTheServer(string imebudale)
        {
            if(!_client.Connected)
            {
                _client.Connect(_server,666);
                _pr = new PacketReader(_client.GetStream());
                if (!string.IsNullOrEmpty(imebudale))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOPCode(0);
                    connectPacket.WriteString(imebudale);
                    _client.Client.Send(connectPacket.GetPacketByByte());
                }
                ReadPacket();
            }
        }

        private void ReadPacket()
        {
            Task.Run(() =>
            { 
                    while(true)
                {
                    var OPCode = _pr.ReadByte();
                    switch (OPCode)
                    {
                        case 1:
                            ConnectedEvent?.Invoke();
                            break;
                        case 5:
                            PorukaPrimljena?.Invoke();
                            break;
                        case 10:
                            Diskonektovan?.Invoke();
                            break;
                        default:
                            Console.WriteLine("DA DA DA!");
                            break;
                    }
                }
            });
        }
        public void SendMessageToServer(string msg)
        {
            var msgPaket = new PacketBuilder();
            msgPaket.WriteOPCode(5);
            msgPaket.WriteString(msg);
            _client.Client.Send(msgPaket.GetPacketByByte());
        }
    }
}
