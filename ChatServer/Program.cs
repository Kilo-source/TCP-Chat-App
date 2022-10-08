using System;
using System.Net.Sockets;
using System.Net;
using ChatServer;
using ChatClient.NET.IO;

class Program
{
    static TcpListener _listener;
    static List<Budala> _budale;
    static void Main(string[] args)
    {
        _budale = new List<Budala>();
        _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 666);
        _listener.Start();

        while (true)
        {
            var client = new Budala(_listener.AcceptTcpClient());
            _budale.Add(client);
            PrikaziKonekciju();
        }
    }
    static void PrikaziKonekciju()
    {
        foreach (var budala in _budale)
        {
            foreach (var bdl in _budale)
            {
                var Paket = new PacketBuilder();
                Paket.WriteOPCode(1);
                Paket.WriteString(bdl.ImeBudale);
                Paket.WriteString(bdl.UID.ToString());
                budala.BudalinSoket.Client.Send(Paket.GetPacketByByte());
            }
        }
        
    }
    public static void PrikaziPoruku(string msg)
    {
        foreach (var budala in _budale)
        {
            var msgPacket = new PacketBuilder();
            msgPacket.WriteOPCode(5);
            msgPacket.WriteString(msg);
            budala.BudalinSoket.Client.Send(msgPacket.GetPacketByByte());
        }
    }
    public static void PrikaziDiskonektovanje(string uid)
    {
        var diskonektovan = _budale.Where(x => x.UID.ToString() == uid).FirstOrDefault();
        _budale.Remove(diskonektovan);

        foreach (var budala in _budale)
        {
            var Diskonekt = new PacketBuilder();
            Diskonekt.WriteOPCode(10);
            Diskonekt.WriteString(uid);
            budala.BudalinSoket.Client.Send(Diskonekt.GetPacketByByte());
        }
        PrikaziPoruku($"{diskonektovan.ImeBudale} => je napustio server u {DateTime.Now.ToShortTimeString()}!");
    }
}