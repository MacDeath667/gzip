using System;
using System.Threading;
using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Implementation.Chunks;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Base
{
	internal abstract class BaseChunkReader : IReader
	{
		protected BaseChunkReader(AutoThreadingPreferences settings)
		{
			Queues = new BlockingQueue<IChunk>[settings.Threads];
			for (int i = 0; i < Queues.Length; i++)
			{
				Queues[i] = new BlockingQueue<IChunk>(settings.Threads);
			}
			Reset = new ManualResetEventSlim(false);
			StartWork = new ManualResetEventSlim(false);
		}

		public ManualResetEventSlim Reset { get; }
		public ManualResetEventSlim StartWork { get; }
		public BlockingQueue<IChunk>[] Queues { get; }

		public void Start(string filepath)
		{
			Console.WriteLine("reading start single thread");
			new Thread(() => ReadChunks(filepath)).Start();
		}

		private protected abstract void ReadChunks(string filepath);
		

		private protected DataChunk CreateChunk(byte[] buffer, int readBytes)
		{
			return new DataChunk()
			{
				Data = buffer,
				Size = readBytes
			};
		}
	}
}
