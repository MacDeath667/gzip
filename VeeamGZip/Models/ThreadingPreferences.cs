using System;

namespace VeeamGZip.Models
{
	public class ThreadingPreferences
	{
		public const int DefaultBufferSizeBytes = 1024 * 1024;

		public static readonly int MaxWorkerThreadsNumber = Math.Min(Environment.ProcessorCount, 8);

		public int? WorkerThreadsNumber { get; set; }
		public int? BufferSizeBytes { get; set; }

		public static ThreadingPreferences Default => new ThreadingPreferences
		{
			BufferSizeBytes = DefaultBufferSizeBytes,
			WorkerThreadsNumber = MaxWorkerThreadsNumber
		};
	}
}