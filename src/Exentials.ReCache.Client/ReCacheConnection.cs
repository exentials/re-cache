using Exentials.ReCache.Models;
using System.Net.Http.Json;
using System.Net.Security;

namespace Exentials.ReCache.Client;

public class ReCacheConnection
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
                RemoteCertificateValidationCallback = delegate { return true; }
            }
        };


        try
        {
            using HttpClient httpClient = new(handler)
            {
                BaseAddress = new Uri(SslUrl),
                DefaultRequestVersion = new Version(2, 0)
            };

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Authenticate/login", new LoginModel
            {
                Username = username,
                Password = password
            }, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                AuthenticationInfo? authenticationInfo = await response.Content.ReadFromJsonAsync<AuthenticationInfo>(cancellationToken: cancellationToken);
                if (authenticationInfo is not null)
                {
                    _client = new ReCacheClient(new ReCacheClientOptions
                    {
                        SslUrl = SslUrl,
                        Token = authenticationInfo.Token,
                        KeepAlive = true,
                        IgnoreSslCertificate = true
                    });
                    IsConnected = true;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

            }
        }
        catch (HttpRequestException)
        {

        }
        catch (UriFormatException ex)
        {
            Console.WriteLine(ex.Message);
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
