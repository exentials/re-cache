using System.IO.Compression;
using System.Reflection;

namespace Exentials.ReCache.Server.Services
{
	public sealed class CacheCollectorService : IHostedService, IDisposable
	{
		private readonly string BackupFileName = "cache.json.gz";
		private readonly ReCacheProvider _cacheProvider;
		private readonly ILogger<CacheCollectorService> _logger;
		private Timer? _cleanTimer = null;
		private Timer? _backupTimer = null;
		private bool _isCleaning;
		private bool _runningBackup;
		private string? _backupFileNamePath;

		public CacheCollectorService(
			ReCacheProvider cacheProvider,
			ILogger<CacheCollectorService> logger
			)
		{
			_cacheProvider = cacheProvider;
			_logger = logger;
		}

		public Task StartAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation($"{nameof(CacheCollectorService)} running.");

			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (path is not null)
			{
				path = Path.Combine(path, "Data");
				Directory.CreateDirectory(path);
				_backupFileNamePath = Path.Combine(path, BackupFileName);
				RestoreDictionary(_backupFileNamePath);
			}

			_cleanTimer = new Timer(CleanJob, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
			_backupTimer = new Timer(BackupJob, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

			return Task.CompletedTask;
		}

		private void CleanJob(object? state)
		{
			if (!_isCleaning && !_runningBackup)
			{
				_isCleaning = true;
				// var count = Interlocked.Increment(ref executionCount);            
				_logger.LogInformation($"{nameof(CleanJob)} Started.");
				var count = _cacheProvider.CleanExpired();
				_logger.LogInformation($"{nameof(CleanJob)} cleaned {count} keys.");

				_isCleaning = false;
			}
		}

		private void RestoreDictionary(string path)
		{
			if (File.Exists(path))
			{
				using var file = File.OpenRead(path);
				using var zip = new GZipStream(file, CompressionMode.Decompress);
				try
				{
					_cacheProvider.DeserializeFrom(zip);
				}
				catch
				{
					zip.Close();
					file.Close();
					// wrong format delete
					//File.Delete(path);
				}
			}
		}

		private void BackupJob(object? state)
		{
			if (!_isCleaning && !_runningBackup)
			{
				_runningBackup = true;
				try
				{
					if (_backupFileNamePath is not null)
					{
						using var file = File.OpenWrite(_backupFileNamePath);
						using var zip = new GZipStream(file, CompressionMode.Compress);
						_cacheProvider.SerializeTo(zip);
						zip.Flush();
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message);
				}
				_runningBackup = false;

			}
		}

		public Task StopAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation($"{nameof(CacheCollectorService)} is stopping.");

			_cleanTimer?.Change(Timeout.Infinite, 0);
			_backupTimer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_cleanTimer?.Dispose();
			_backupTimer?.Dispose();
		}
	}
}
