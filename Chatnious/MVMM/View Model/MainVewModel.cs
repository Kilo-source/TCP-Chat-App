using ChatClient.MVMM.Core;
using ChatClient.MVMM.Model;
using ChatClient.NET;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.MVMM.View_Model
{
    internal class MainVewModel
    {
        public ObservableCollection<BudalinModel> _budale { get; set; }
        public ObservableCollection<String> _poruke { get; set; }
        public ReleyCommands connectToServerCommand { get; set; }
        public ReleyCommands sendMSGCommand { get; set; }
        public ReleyCommands ExitApplication { get; set; } 
        public string _imeBudale { get; set; }

        private Server _server;

        public string Poruka { get; set; }
        public MainVewModel()
        {
            _budale = new ObservableCollection<BudalinModel>();
            _poruke = new ObservableCollection<String>();
            _server = new Server();
            _server.ConnectedEvent += BudalaSePovezala;
            _server.PorukaPrimljena += _server_PorukaPrimljena;
            _server.Diskonektovan += _server_Diskonektovan;
            connectToServerCommand = new ReleyCommands(o => _server._ConnectToTheServer(_imeBudale), o => !string.IsNullOrEmpty(_imeBudale));
            sendMSGCommand = new ReleyCommands(o => _server.SendMessageToServer(Poruka), o => !string.IsNullOrEmpty(Poruka));
            ExitApplication = new ReleyCommands(o => Environment.Exit(0));

        }

        private void _server_Diskonektovan()
        {
            var uid = _server._pr.ReadMsg();
            var user = _budale.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => _budale.Remove(user));
        }

        private void _server_PorukaPrimljena()
        {
            var msg = _server._pr.ReadMsg();
            Application.Current.Dispatcher.Invoke(() => _poruke.Add(msg));
        }

        private void BudalaSePovezala()
        {
            var budala = new BudalinModel
            {
                Ime = _server._pr.ReadMsg(),
                UID = _server._pr.ReadMsg()
            };

            if (!_budale.Any(x => x.UID == budala.UID))
            {
                Application.Current.Dispatcher.Invoke(() => _budale.Add( budala));
            }
        }
    }
}
