using System.Threading;
using GzipRoundRobin.Implementation.Chunks;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Base
{
	public abstract class BaseChunkReader : IReader<IChunk>
	{
		public ManualResetEventSlim Reset { get; set; }
		public MultithreadingQueue<IChunk>[] Queues { get; set; }

		public void StartRead(string filepath)
		{
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
