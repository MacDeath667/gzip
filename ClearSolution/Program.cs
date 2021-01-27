using System.Threading;

namespace ClearSolution
{
	class Program
	{
		private static MultithreadingQueue<Chunk> _chunksQueue;
		private static MultithreadingQueue<Chunk> _compressedQueue;
		private static ManualResetEventSlim _manualResetEvent;

		private ChunkReader<Chunk> _chunkReader;

		static void Main(string[] args)
		{
			var filepath = @"C:\Temp\1.exe";
			
			_chunksQueue = new MultithreadingQueue<Chunk>();
			_compressedQueue = new MultithreadingQueue<Chunk>();
			_manualResetEvent = new ManualResetEventSlim(false);
			
			
			new UncompressedChunkReader(_chunksQueue, _manualResetEvent).Start(filepath);
			new QueueProcessor(_chunksQueue, _manualResetEvent).Start();
		}
	}
}