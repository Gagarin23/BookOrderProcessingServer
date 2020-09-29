using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ProcessingServerUnitTest
{
    internal class TcpRequestForTest
    {
        internal void RunConnection(string ip, int port, string json)
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
                        Console.WriteLine(answer);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}