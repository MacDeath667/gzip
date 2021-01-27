using System;

namespace VeeamGZip.Models
{
	public class ThreadingPreferences
	{
		public const int DefaultBufferSizeBytes = 1024 * 1024;

		public static readonly int MaxWorkerThreadsNumber = Math.Min(Environment.ProcessorCount, 8);

		public int? Threads { get; set; }
		public int? BufferBytes { get; set; }

		public static ThreadingPreferences Auto => new ThreadingPreferences
		{
			BufferBytes = DefaultBufferSizeBytes,
			Threads = MaxWorkerThreadsNumber
		};
	}
}