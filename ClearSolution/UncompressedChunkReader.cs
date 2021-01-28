using System.Threading;

namespace ClearSolution
{
	public class UncompressedChunkReader : ChunkReader<Chunk>
	{
		public UncompressedChunkReader(
			MultithreadingQueue<Chunk> queue,
			ManualResetEventSlim manualResetEvent) 
			: base(
				queue, 
				manualResetEvent)
		{
		}

		protected override Chunk CreateChunk(byte[] buffer, int readBytes, int order)
		{
			return new Chunk()
			{
				Data = buffer,
				Size = readBytes,
				Order = order
			};
		}
	}
}