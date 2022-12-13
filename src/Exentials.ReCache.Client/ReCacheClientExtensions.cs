using Exentials.ReCache.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding services to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ReCacheClientExtensions
    {
        /// <summary>
        /// Adds a ReCache client service
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddReCacheClient(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            return services.AddSingleton<ReCacheClient>();
        }

        /// <summary>
        /// Adds a ReCache client service
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
        /// <param name="options">Client configuration options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddReCacheClient(this IServiceCollection services, Action<ReCacheClientOptions> options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            ReCacheClientOptions opt = new();
            options.Invoke(opt);
            return services.AddSingleton(new ReCacheClient(opt));
        }
    }
}
