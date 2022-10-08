using System;
using System.Collections;
using System.IO;
using System.Text;

namespace ChatClient.NET.IO
{
    internal class PacketBuilder
    {
        MemoryStream _strem;
        public PacketBuilder()
        {
            _strem = new MemoryStream();
        }
        public void WriteOPCode(byte OPCode)
        {
            _strem.WriteByte(OPCode);
        }

        public void WriteString(string msg)
        {
            var msgLen = msg.Length;
            _strem.Write(BitConverter.GetBytes(msgLen));
            _strem.Write(Encoding.ASCII.GetBytes(msg));

        }
        public byte[] GetPacketByByte()
        {
            return _strem.ToArray();
        }
    }
}
