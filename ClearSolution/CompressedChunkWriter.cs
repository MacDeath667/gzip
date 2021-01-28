using System.Threading;

namespace ClearSolution
{
	public class CompressedChunkWriter : ChunkWriter<Chunk>
	{
		public CompressedChunkWriter(
			MultithreadingQueue<Chunk> queue,
			CountdownEvent countdownEvent)
			: base(
				queue,
				countdownEvent)
		{
		}
	}
}