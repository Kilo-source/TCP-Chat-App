using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.NET.IO
{
    internal class PacketReader :BinaryReader
    {
        private NetworkStream _ns;
        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }

        public string ReadMsg()
        {
            byte[] msgbuffer;
            var len = ReadInt32();
            msgbuffer = new byte[len];
            _ns.Read(msgbuffer, 0, len);
            var msg = Encoding.ASCII.GetString(msgbuffer);
            return msg;
        }
    }
}
