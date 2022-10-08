using ChatServer.NET.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    internal class Budala
    {
        public string ImeBudale { get; set; }
        public Guid UID { get; set; }
        public TcpClient BudalinSoket { get; set; }
        PacketReader _pr;
        public Budala(TcpClient bSocket)
        {
            BudalinSoket = bSocket;
            UID = Guid.NewGuid();
            _pr = new PacketReader(BudalinSoket.GetStream());
            var OPCode = _pr.ReadByte();
            ImeBudale = _pr.ReadMsg();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{DateTime.Now.ToString("t")}]{ImeBudale} Konektovan!");
            Console.ResetColor();
            Task.Run(() => Procesi());
        }
        void Procesi()
        {
            while(true)
            {
                try
                {
                    var opcode = _pr.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = _pr.ReadMsg();
                            Program.PrikaziPoruku($"[{DateTime.Now.ToString("t")}]{ImeBudale}: {msg}");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine($"[{DateTime.Now.ToString("t")}]{ImeBudale}: {msg}");
                            Console.ResetColor();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now.ToString("t")}]{ImeBudale} Diskonektovan!");
                    Console.ResetColor();
                    Program.PrikaziDiskonektovanje(UID.ToString());
                    BudalinSoket.Close();
                    break;
                }
            }
        }

    }
}
