using ProcessingServer.BL.NetWork;
using ProcessingServer.Configs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingServer.Controllers
{
    //TODO: реализовать проверку на состояние тасков.
    class MainController
    {
        private static readonly ConfigController CfgDirs = new ConfigController();
        private readonly ServicesAdress _cfgNetwork = CfgDirs.GetServicesAdress();

        /// <summary>
        ///     Запуск микросервисов прослушки клиентов.
        /// </summary>
        public MainController()
        {
            var clientListener = new ClientsListener();

            Task.Run(() =>
                clientListener.RunConnectionAsync(_cfgNetwork.ListenerCoverConnection.Ip,
                    _cfgNetwork.ListenerCoverConnection.Port, Customers.OzonCoverOrder));

            Task.Run(() =>
                clientListener.RunConnectionAsync(_cfgNetwork.ListenerBlockConnection.Ip,
                    _cfgNetwork.ListenerBlockConnection.Port, Customers.OzonBlockOrder));

            Task.Run(() =>
                clientListener.RunConnectionAsync(_cfgNetwork.IzdListenerCoverConnection.Ip,
                    _cfgNetwork.IzdListenerCoverConnection.Port, Customers.IzdCoverOrder));


        }
    }
}