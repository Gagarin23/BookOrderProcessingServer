using Newtonsoft.Json;
using ProcessingServer.Loggers;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace ProcessingServer.BL.NetWork
{
    internal class ConnectionForLog
    {
        /// <summary>
        ///     Отправка сообщения для записи в лог файл.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="msgs"></param>
        internal void RunConnection(string ip, int port, params string[] msgs)
        {
            using (var tcpClient = new TcpClient())
            {
                try
                {
                    tcpClient.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                    using (var ns = tcpClient.GetStream())
                    using (var bw = new BinaryWriter(ns, Encoding.UTF8))
                    using (var unusedYet = new BinaryReader(ns, Encoding.UTF8))
                    {
                        var json = JsonConvert.SerializeObject(msgs);
                        bw.Write(json);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ExceptionsLogger.WriteLog(MethodBase.GetCurrentMethod()?.Name, e.HResult, e.Message);
                }
            }
        }
    }
}