using System;
using Microsoft.Extensions.Configuration;

namespace GzipRoundRobin.Primitives
{
	public class AutoThreadingPreferences
	{
		public AutoThreadingPreferences()
		{
			SimpleThreadingPreferences preferences = default;
			try
			{
				preferences = new ConfigurationBuilder()
					.AddJsonFile("appsettings.json", false)
					.Build()
					.GetSection("Settings")
					.Get<SimpleThreadingPreferences>();
			}
			catch (InvalidOperationException e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine($"Auto settings will be applied.");
				preferences = new SimpleThreadingPreferences();
			}
			
			Threads = (preferences.Threads is null || preferences.Threads <= 3)
				? AutoThreadsCount
				: preferences.Threads.Value;

			BufferBytes = (preferences.BufferBytes is null || preferences.BufferBytes < 1 * 1024 * 1024)
				? AutoBufferBytes
				: preferences.BufferBytes.Value;
		}

		public const int AutoBufferBytes = 2 * 1024 * 1024; // todo: set it by available RAM size

		public static readonly int AutoThreadsCount = Math.Min(Environment.ProcessorCount, 4);

		public int Threads { get; }
		public int BufferBytes { get; }

		private sealed class SimpleThreadingPreferences
		{
			public int? Threads { get; set; }
			public int? BufferBytes { get; set; }
		}
	}
}