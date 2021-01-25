﻿using System.Threading;

namespace ClearSolution
{
	public class UncompressedChunkReader : ChunkReader<Chunk>
	{
		public UncompressedChunkReader(
			MultithreadingQueue<Chunk> queue,
			ManualResetEvent manualResetEvent) 
			: base(
				queue, 
				manualResetEvent)
		{
		}

		protected override Chunk CreateChunk(byte[] buffer, int size)
		{
			return new Chunk()
			{
				Data = buffer,
				Size = size
			};
		}
	}
}