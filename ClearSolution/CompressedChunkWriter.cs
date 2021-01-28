using System.Threading;

namespace ClearSolution
{
	public class CompressedChunkWriter : ChunkWriter<Chunk>
	{
		public CompressedChunkWriter(
			MultithreadingQueue<Chunk> queue,
			ManualResetEventSlim manualResetEvent)
			: base(
				queue,
				manualResetEvent)
		{
		}
	}
}