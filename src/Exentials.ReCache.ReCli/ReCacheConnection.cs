using Exentials.ReCache.Client;
using Exentials.ReCache.Models;
using System.Net.Http.Json;
using System.Net.Security;

namespace Exentials.ReCache.ReCli
{
    internal class ReCacheConnection
    {

        public string Host { get; private set; } = "localhost";
        public int Port { get; private set; } = 443;
        public string? Route { get; set; }
        public string SslUrl { get => $"https://{Host}:{Port}/{Route ?? string.Empty}".TrimEnd('/'); }
        public bool IsConnected { get; private set; }

        private ReCacheClient? _client;
        public ReCacheClient? Client { get => IsConnected ? _client : null; }

        public void Invoke(Action<ReCacheClient> action)
        {
            if (_client is not null)
            {
                action(_client);
            }
        }

        public ValueTask InvokeAsync(Func<ReCacheClient, ValueTask> action)
        {
            return (_client is null)
                ? ValueTask.CompletedTask
                : action(_client);
        }

        public async Task<bool> Connect(string host, int port, string? username, string? password, string? route, CancellationToken cancellationToken)
        {
            Host = host;
            Port = port;
            Route = route;

            var handler = new SocketsHttpHandler()
            {
                SslOptions = new SslClientAuthenticationOptions
                {
                    // Leave certs unvalidated for debugging
                    RemoteCertificateValidationCallback = delegate { return true; }
                }
            };

            HttpClient httpClient = new(handler)
            {
                BaseAddress = new Uri(SslUrl),
                DefaultRequestVersion = new Version(2, 0)
            };

            try
            {

                HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Authenticate/login", new LoginModel
                {
                    Username = username,
                    Password = password
                }, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    AuthenticationInfo? authInfo = await response.Content.ReadFromJsonAsync<AuthenticationInfo>(cancellationToken: cancellationToken);
                    if (authInfo is not null)
                    {
                        _client = new ReCacheClient(new ReCacheClientOptions
                        {
                            SslUrl = SslUrl,
                            Token = authInfo.Token,
                            KeepAlive = true,
                            IgnoreSslCertificate = true
                        });
                        IsConnected = true;
                    }
                }
            }
            catch (HttpRequestException)
            {

            }
            return IsConnected;
        }

        public bool Close()
        {
            if (IsConnected)
            {
                _client?.Dispose();
                _client = null;
                IsConnected = false;
            }
            return IsConnected;
        }
    }
}
