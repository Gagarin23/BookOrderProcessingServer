using Newtonsoft.Json;
using ProcessingServer.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using TransferObjects;

namespace ProcessingServer.BL.NetWork
{
    public class ConnectionForBuildOrder<T> where T : Book
    {
        /// <summary>
        ///     Отправка книг на сервер Базы Данных..
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        internal static (List<T[]>, List<T>) RunConnection(string ip, int port, string json)
        {
            using (var tcpClient = new TcpClient())
            {
                try
                {
                    tcpClient.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                    using (var ns = tcpClient.GetStream())
                    using (var bw = new BinaryWriter(ns, Encoding.UTF8))
                    using (var br = new BinaryReader(ns, Encoding.UTF8))
                    {
                        bw.Write(json);
                        var answer = br.ReadString();

                        (List<T[]> Queues, List<T> MissingBooks) returnedData = JsonConvert.DeserializeObject<(List<T[]>, List<T>)>(answer);
                        Console.WriteLine($"Очередей сформировано: {returnedData.Queues}\n" +
                            $"Книг не найдено на сервере: {returnedData.MissingBooks}");

                        return returnedData;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
                    return (new List<T[]>(), new List<T>());
                }
            }
        }
    }
}