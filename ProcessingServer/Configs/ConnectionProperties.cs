namespace ProcessingServer.Configs
{
    public struct ConnectionProperties
    {
        public string Ip { get; set; }
        public int Port { get; set; }

        public ConnectionProperties(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }
    }
}