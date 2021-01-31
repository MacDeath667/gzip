using System;
using System.Threading;
using GzipRoundRobin.Implementation.Chunks;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Base
{
	public abstract class BaseChunkReader : IReader<IChunk>
	{
		protected BaseChunkReader(AutoThreadingPreferences settings)
		{
			Queues = new MultithreadingQueue<IChunk>[settings.Threads];
			for (int i = 0; i < Queues.Length; i++)
			{
				Queues[i] = new MultithreadingQueue<IChunk>(settings.Threads); //todo limit by ram
			}
			Reset = new ManualResetEventSlim(false);
			StartWork = new ManualResetEventSlim(false);
		}
		public ManualResetEventSlim Reset { get; set; }
		public ManualResetEventSlim StartWork { get; set; }
		public MultithreadingQueue<IChunk>[] Queues { get; set; }

		public void Start(string filepath)
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
