using System.Threading;
using Microsoft.Extensions.Configuration;

namespace ClearSolution
{
	class Program
	{
		private static MultithreadingQueue<Chunk> _chunksQueue;
		private static MultithreadingQueue<Chunk> _processedQueue;
		private static ManualResetEventSlim _manualResetEventRead;
		private static CountdownEvent _countdownEvent;

		//private ChunkReader<Chunk> _chunkReader; //todo remove or assign to chunkReader

		static void Main(string[] args)
		{
			var cliArgs = new CliParser().CliParse(args);
			var filepath = cliArgs.FilePath;
			var outpath = cliArgs.OutPath;

			var threadingPreferences = new AutoThreadingPreferences(); // todo why exception?
			var dataProcessorThreadsCount = (int) (threadingPreferences.Threads-2);

			_chunksQueue = new MultithreadingQueue<Chunk>(dataProcessorThreadsCount);
			_processedQueue = new MultithreadingQueue<Chunk>(dataProcessorThreadsCount);
			_manualResetEventRead = new ManualResetEventSlim(false);

			_countdownEvent = new CountdownEvent(dataProcessorThreadsCount);


			new UncompressedChunkReader(_chunksQueue, _manualResetEventRead).Start(filepath);
			new QueueProcessor(_chunksQueue, 
					_processedQueue, 
					_manualResetEventRead, 
					new GzipWorker(),
					_countdownEvent)
				.Start(dataProcessorThreadsCount);
			new CompressedChunkWriter(_processedQueue, _countdownEvent).StartInSuchThread(outpath);
		}
	}
}