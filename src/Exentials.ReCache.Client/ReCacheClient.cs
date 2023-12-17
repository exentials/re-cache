using Exentials.ReCache.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using System.Net.Security;
using System.Text.Json;

namespace Exentials.ReCache.Client
{
    /// <summary>
    /// ReCache Client Service
    /// </summary>
    public class ReCacheClient : IDisposable
    {
        private readonly string _sslUrl;
        private readonly string _authenticationToken;
        private readonly CallCredentials _callCredentials;
        private readonly GrpcChannel _channel;
        private readonly RpcCacheService.RpcCacheServiceClient _client;
        private readonly HttpMessageHandler _httpHandler;

        public ReCacheClient(IOptions<ReCacheClientOptions> config)
            : this(config.Value)
        {
        }

        public ReCacheClient(ReCacheClientOptions options)
        {
            _sslUrl = options.SslUrl;
            _authenticationToken = options.Token;
            _callCredentials = CallCredentials.FromInterceptor(CredentialsInterceptor);

            _httpHandler = GetHttpHandler(options.KeepAlive, options.IgnoreSslCertificate);

            GrpcChannelOptions channelOptions = new()
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), _callCredentials),
                HttpHandler = _httpHandler,
            };

            _channel = GrpcChannel.ForAddress(_sslUrl, channelOptions);
            _client = new(_channel);
        }

        private string BearerToken => $"Bearer {_authenticationToken}";

        public string AuthenticationToken => _authenticationToken;

        public bool IsConnected => _channel.State == ConnectivityState.Ready;

        private Task CredentialsInterceptor(AuthInterceptorContext context, Metadata metadata)
        {
            metadata.Add("Authorization", BearerToken);
            return Task.CompletedTask;
        }

        private static SocketsHttpHandler GetHttpHandler(bool keepAlive, bool ignoreSslCertificate)
        {
            var httpMessageHandler = new SocketsHttpHandler();

            if (keepAlive)
            {
                httpMessageHandler.PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan;
                httpMessageHandler.KeepAlivePingDelay = TimeSpan.FromSeconds(60);
                httpMessageHandler.KeepAlivePingTimeout = TimeSpan.FromSeconds(30);
                httpMessageHandler.EnableMultipleHttp2Connections = true;
            };
            if (ignoreSslCertificate)
            {
                httpMessageHandler.SslOptions = new SslClientAuthenticationOptions
                {
                    RemoteCertificateValidationCallback = delegate { return true; }
                };
            }
            return httpMessageHandler;
        }

        /// <summary>
        /// List the cache keys
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public async ValueTask<IEnumerable<string>> ListDictionaryAsync(string? nameSpace = null)
        {
            var response = await _client.ListDictionaryAsync(new KeysRequest { NameSpace = nameSpace ?? ReCacheKey.DefaultNamespace });
            return response.Items.Select(x => x.Key).ToList();
        }

        /// <summary>
        /// List cached namespaces
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public async ValueTask<IEnumerable<string>> ListDictionaryNamespacesAsync()
        {
            var response = await _client.ListDictionaryNamespacesAsync(new());
            return response.Items.ToList();
        }


        /// <summary>
        /// Set a string value into the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpire"></param>
        /// <param name="slidingExpire"></param>
        /// <param name="nameSpace"></param>
        /// <returns>True if value has been stored, False otherwise.</returns>
        public async ValueTask<bool> SetAsync(string key, string value, DateTimeOffset? absoluteExpire = null, TimeSpan? slidingExpire = null, string? nameSpace = null)
        {
            Timestamp? timestamp = absoluteExpire.HasValue ? Timestamp.FromDateTimeOffset(absoluteExpire.Value) : null;
            Duration? duration = slidingExpire.HasValue ? Duration.FromTimeSpan(slidingExpire.Value) : null;
            Response result = await _client.SetDictionaryAsync(
                new RpcCacheData
                {
                    Key = key,
                    Value = value,
                    NameSpace = nameSpace ?? ReCacheKey.DefaultNamespace,
                    AbsoluteExpire = timestamp,
                    SlidingExpire = duration
                }
            );
            return result.Code == 1;
        }

        /// <summary>
        /// Set a boolean value into the cache
        /// </summary>
        /// <param name="key">key of the value</param>
        /// <param name="value">the value to store</param>
        /// <param name="absoluteExpire">absolute date/time value expiration</param>
        /// <param name="slidingExpire">sliding time value expiration</param>
        /// <param name="nameSpace">namespace of the key</param>
        /// <returns>True if value has been stored, False otherwise.</returns>
        public ValueTask<bool> SetAsync(string key, bool value, DateTimeOffset? absoluteExpire = null, TimeSpan? slidingExpire = null, string? nameSpace = null)
        {
            return SetAsync(key, value.ToString(), absoluteExpire, slidingExpire, nameSpace);
        }

        /// <summary>
        /// Set a decimal value into the cache
        /// </summary>
        /// <param name="key">key of the value</param>
        /// <param name="value">the value to store</param>
        /// <param name="absoluteExpire">absolute date/time value expiration</param>
        /// <param name="slidingExpire">sliding time value expiration</param>
        /// <param name="nameSpace">namespace of the key</param>
        /// <returns>True if value has been stored, False otherwise.</returns>
        public ValueTask<bool> SetAsync(string key, decimal value, DateTimeOffset? absoluteExpire = null, TimeSpan? slidingExpire = null, string? nameSpace = null)
        {
            return SetAsync(key, value.ToString(), absoluteExpire, slidingExpire, nameSpace);
        }

        /// <summary>
        /// Set an integer value into the cache
        /// </summary>
        /// <param name="key">key of the value</param>
        /// <param name="value">the value to store</param>
        /// <param name="absoluteExpire">absolute date/time value expiration</param>
        /// <param name="slidingExpire">sliding time value expiration</param>
        /// <param name="nameSpace">namespace of the key</param>
        /// <returns>True if value has been stored, False otherwise.</returns>
        public ValueTask<bool> SetAsync(string key, int value, DateTimeOffset? absoluteExpire = null, TimeSpan? slidingExpire = null, string? nameSpace = null)
        {
            return SetAsync(key, value.ToString(), absoluteExpire, slidingExpire, nameSpace);
        }

        /// <summary>
        /// Set a double value into the cache
        /// </summary>
        /// <param name="key">key of the value</param>
        /// <param name="value">the value to store</param>
        /// <param name="absoluteExpire">absolute date/time value expiration</param>
        /// <param name="slidingExpire">sliding time value expiration</param>
        /// <param name="nameSpace">namespace of the key</param>
        /// <returns>True if value has been stored, False otherwise.</returns>
        public ValueTask<bool> SetAsync(string key, double value, DateTimeOffset? absoluteExpire = null, TimeSpan? slidingExpire = null, string? nameSpace = null)
        {
            return SetAsync(key, value.ToString(), absoluteExpire, slidingExpire, nameSpace);
        }

        /// <summary>
        /// Set a double value into the cache
        /// </summary>
        /// <param name="key">key of the value</param>
        /// <param name="value">the value to store</param>
        /// <param name="absoluteExpire">absolute date/time value expiration</param>
        /// <param name="slidingExpire">sliding time value expiration</param>
        /// <param name="nameSpace">namespace of the key</param>
        /// <returns>True if value has been stored, False otherwise.</returns>
        public ValueTask<bool> SetAsync<T>(string key, T value, DateTimeOffset? absoluteExpire = null, TimeSpan? slidingExpire = null, string? nameSpace = null) where T : class
        {
            var json = JsonSerializer.Serialize(value, new JsonSerializerOptions { IgnoreReadOnlyFields = true });
            return SetAsync(key, json, absoluteExpire, slidingExpire, nameSpace);
        }

        public async ValueTask<string?> GetAsync(string key, string? nameSpace = null)
        {
            Response result = await _client.GetDictionaryAsync(new RpcKey { Key = key, NameSpace = nameSpace ?? ReCacheKey.DefaultNamespace });
            return (result.Code == 0) ? default : result.Data;
        }

        public async ValueTask<bool?> GetBoolAsync(string key, string? nameSpace = null)
        {
            var value = await GetAsync(key, nameSpace);
            return bool.TryParse(value, out bool result) ? result : null;
        }

        public async ValueTask<int?> GetIntAsync(string key, string? nameSpace = null)
        {
            var value = await GetAsync(key, nameSpace);
            return int.TryParse(value, out int result) ? result : null;
        }

        public async ValueTask<T?> GetAsync<T>(string key, string? nameSpace = null) where T : class
        {
            var json = await GetAsync(key, nameSpace);
            return json is null ? default : JsonSerializer.Deserialize<T>(json);
        }

        public async ValueTask<bool> DelAsync(string key, string? nameSpace = null)
        {
            Response result = await _client.DelDictionaryAsync(new RpcKey { Key = key, NameSpace = nameSpace ?? ReCacheKey.DefaultNamespace });
            return result.Code == 1;
        }

        /// <summary>
        /// List the cached hashset keys
        /// </summary>
        /// <param name="nameSpace">namespace of the keys</param>
        /// <returns></returns>
        public async ValueTask<IEnumerable<string>> ListHashSetAsync(string? nameSpace = null)
        {
            var response = await _client.ListHashSetAsync(new KeysRequest { NameSpace = nameSpace ?? ReCacheKey.DefaultNamespace });
            return response.Items.Select(x => x.Key).ToList();
        }

        /// <summary>
        /// List the cached namespaces
        /// </summary>
        /// <returns></returns>
        public async ValueTask<IEnumerable<string>> ListHashSetNamespacesAsync()
        {
            var response = await _client.ListHashSetNamespacesAsync(new());
            return response.Items.ToList();
        }

        public async ValueTask<bool> SetHashSetAsync(string key, string data, DateTimeOffset? absoluteExpire = null, TimeSpan? slidingExpire = null, string? nameSpace = null)
        {
            Timestamp? timestamp = absoluteExpire.HasValue ? Timestamp.FromDateTimeOffset(absoluteExpire.Value) : null;
            Duration? duration = slidingExpire.HasValue ? Duration.FromTimeSpan(slidingExpire.Value) : null;
            Response result = await _client.SetHashSetAsync(
                new RpcCacheData
                {
                    Key = key,
                    Value = data,
                    NameSpace = nameSpace ?? ReCacheKey.DefaultNamespace,
                    AbsoluteExpire = timestamp,
                    SlidingExpire = duration
                }
            );
            return result.Code == 1;
        }

        public async ValueTask<HashSet<string>?> GetHashSetAsync(string key, string? nameSpace = null)
        {
            Response result = await _client.GetHashSetAsync(new RpcKey { Key = key, NameSpace = nameSpace ?? ReCacheKey.DefaultNamespace });
            return JsonSerializer.Deserialize<HashSet<string>>(result.Data);
        }

        public async ValueTask<bool> DelHashSetAsync(string key, string data, string? nameSpace = null)
        {
            Response result = await _client.DelHashSetAsync(
                new RpcCacheData
                {
                    Key = key,
                    Value = data,
                    NameSpace = nameSpace ?? ReCacheKey.DefaultNamespace,
                    AbsoluteExpire = null,
                    SlidingExpire = null
                }
            );
            return result.Code == 1;
        }

        public async ValueTask<bool> RemoveHashSetAsync(string key, string? nameSpace = null)
        {
            Response result = await _client.RemoveHashSetAsync(
                new RpcKey
                {
                    Key = key,
                    NameSpace = nameSpace ?? ReCacheKey.DefaultNamespace,
                }
            );
            return result.Code == 1;
        }

        public async ValueTask<bool> Clear(string? nameSpace = null)
        {
            Response result = await _client.ClearAsync(new KeysRequest
            {
                NameSpace = nameSpace ?? ReCacheKey.DefaultNamespace
            });
            return result.Code == 1;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_disposed)
                {
                    _channel.Dispose();
                    _disposed = true;
                }
            }
        }
    }
}
