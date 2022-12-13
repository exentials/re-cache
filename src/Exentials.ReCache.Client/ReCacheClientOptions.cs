namespace Exentials.ReCache.Client
{
    public class ReCacheClientOptions
    {
        public const string ReCache = "ReCache";
        public string SslUrl { get; set; } = "https://localhost";
        public string Token { get; set; } = string.Empty;
        public bool KeepAlive { get; set; }
        public bool IgnoreSslCertificate { get; set; }
    }
}
