using Exentials.ReCache.Models;
using System.Net.Http.Json;
using System.Net.Security;

namespace Exentials.ReCache.Client
{
    internal class ReCacheHelpers
    {

        public async Task<AuthenticationInfo?> GetClientToken(string sslUrl, string username, string password)
        {
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
                BaseAddress = new Uri(sslUrl),
                DefaultRequestVersion = new Version(2, 0)
            };


            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Authenticate/login", new LoginModel
            {
                Username = username,
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                AuthenticationInfo? authInfo = await response.Content.ReadFromJsonAsync<AuthenticationInfo>();
                return authInfo;
            }
            return null;
        }

    }
}
