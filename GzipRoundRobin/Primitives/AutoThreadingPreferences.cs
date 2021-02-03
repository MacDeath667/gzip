using System;
using Microsoft.Extensions.Configuration;

namespace GzipRoundRobin.Primitives
{
	internal class AutoThreadingPreferences
	{
		internal static AutoThreadingPreferences Create()
		{
			return new AutoThreadingPreferences();
		}
		
		private AutoThreadingPreferences()
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

		private const int AutoBufferBytes = 2 * 1024 * 1024; // todo: set it by available RAM size

		private static readonly int AutoThreadsCount = Math.Min(Environment.ProcessorCount, 4);

		internal int Threads { get; }
		internal int BufferBytes { get; set; }

		private sealed class SimpleThreadingPreferences
		{
			internal int? Threads { get; set; }
			internal int? BufferBytes { get; set; }
		}
	}
}