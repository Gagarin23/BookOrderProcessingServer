using Newtonsoft.Json;
using ProcessingServer.Configs;
using ProcessingServer.Controllers;
using ProcessingServer.Loggers;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TransferObjects;

[assembly: InternalsVisibleTo("ControllersUnitTest")]
namespace ProcessingServer.BL.NetWork
{
    class ClientsListener
    {
        private static readonly ConfigController CfgDirs = new ConfigController();
        private readonly ServicesAdress _cfgNetwork = CfgDirs.GetServicesAdress();

        /// <summary>
        ///     Запуск прослушки запросов на обработку.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="callController"></param>
        internal async void RunConnectionAsync(string ip, int port, Func<ExcelReaderTemplate, string, string> callController)
        {
            var tcpListener = new TcpListener(IPAddress.Parse(ip), port);

            try
            {
                tcpListener.Start();

                while (true)
                    using (var client = tcpListener.AcceptTcpClient())
                    using (var ns = client.GetStream())
                    using (var bw = new BinaryWriter(ns, Encoding.UTF8))
                    using (var br = new BinaryReader(ns, Encoding.UTF8))
                    {
                        var data = br.ReadString();
                        var result = await Task.Run(() => RunLogic(data, callController));

                        Console.WriteLine(client.Client.RemoteEndPoint?.ToString() ?? $"{DateTime.Now}: Client.RemoteEndPoint был null.", result);
                        EventsLogger.WriteLog(client.Client.RemoteEndPoint?.ToString(), result);

                        var logConnection = new ConnectionForLog();
                        logConnection.RunConnection(_cfgNetwork.LogConnection.Ip, _cfgNetwork.LogConnection.Port,
                            client.Client.RemoteEndPoint?.ToString(), result);

                        bw.Write(result);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
            }
        }

        /// <summary>
        ///     Запуск компановки заказа.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="callController"></param>
        /// <returns></returns>
        private string RunLogic(string json, Func<ExcelReaderTemplate, string, string> callController)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<(ExcelReaderTemplate excelReaderTemplate, string excelPath)>(json);
                var orderName = Path.GetFileNameWithoutExtension(request.excelPath);
                Console.WriteLine($"{orderName}");

                string result = callController(request.excelReaderTemplate, request.excelPath);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
                return e.Message;
            }
        }
    }
}