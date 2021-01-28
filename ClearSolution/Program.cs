using System.Threading;

namespace ClearSolution
{
	class Program
	{
		private static MultithreadingQueue<Chunk> _chunksQueue;
		private static MultithreadingQueue<Chunk> _processedQueue;
		private static ManualResetEventSlim _manualResetEventRead;
		private static ManualResetEventSlim _manualResetEventWrite;

		//private ChunkReader<Chunk> _chunkReader; //todo remove or assign to chunkReader

		static void Main(string[] args)
		{
			var filepath = @"C:\Temp\1.exe";
			var outpath = @"C:\Temp\1.exe.gzip";

			_chunksQueue = new MultithreadingQueue<Chunk>();
			_processedQueue = new MultithreadingQueue<Chunk>();
			_manualResetEventRead = new ManualResetEventSlim(false);
			_manualResetEventWrite = new ManualResetEventSlim(false);


			new UncompressedChunkReader(_chunksQueue, _manualResetEventRead).Start(filepath);
			new QueueProcessor(_chunksQueue, _processedQueue, _manualResetEventRead, new GzipWorker(), _manualResetEventWrite).Start();
			new CompressedChunkWriter(_processedQueue, _manualResetEventWrite).StartInSuchThread(outpath);
		}
	}
}