using Microsoft.Extensions.DependencyInjection;

namespace Exentials.ReCache
{
	/// <summary>
	/// Extension methods for adding services to an <see cref="IServiceCollection" />.
	/// </summary>
	public static class ReCacheExtensions
	{
		/// <summary>
		/// Adds a ReCache data store provider
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddReCacheProvider(this IServiceCollection services)
		{
			services.AddSingleton<ReCacheProvider>();
			return services;
		}

	}
}
