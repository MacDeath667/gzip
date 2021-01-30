using System;
using System.Dynamic;
using System.Threading;
using GzipRoundRobin.Implementation.Chunks;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Base
{
	public abstract class BaseChunkReader : IReader<IChunk>
	{
		public BaseChunkReader(AutoThreadingPreferences settings)
		{
			Queues = new MultithreadingQueue<IChunk>[settings.Threads];
			for (int i = 0; i < Queues.Length; i++)
			{
				Queues[i] = new MultithreadingQueue<IChunk>(settings.Threads);
			}
			Reset = new ManualResetEventSlim(false);
		}
		public ManualResetEventSlim Reset { get; set; }
		public MultithreadingQueue<IChunk>[] Queues { get; set; }

		public void StartRead(string filepath)
		{
			Console.WriteLine("reading start single thread");
			new Thread(() => ReadChunks(filepath)).Start();
		}

		protected abstract void ReadChunks(string filepath);
		

		protected DataChunk CreateChunk(byte[] buffer, int readBytes)
		{
			return new DataChunk()
			{
				Data = buffer,
				Size = readBytes
			};
		}
	}
}
