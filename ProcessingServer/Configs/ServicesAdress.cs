namespace ProcessingServer.Configs
{
    /// <summary>
    ///     Адреса сервисов.
    /// </summary>
    internal class ServicesAdress
    {
        public ConnectionProperties LogConnection { get; set; }

        public ConnectionProperties ListenerCoverConnection { get; set; }

        public ConnectionProperties ListenerBlockConnection { get; set; }
        public ConnectionProperties IzdListenerCoverConnection { get; set; }

        public ConnectionProperties OzonBlockConnection { get; set; }

        public ConnectionProperties OzonCoverConnection { get; set; }

        public ConnectionProperties IzdatelskieCoverConnection { get; set; }
    }
}