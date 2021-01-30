using System;
using System.Threading;

namespace ClearSolution
{
	public class DataProcessor
	{
		private readonly MultithreadingQueue<Chunk> _inputQueue;
		private readonly MultithreadingQueue<Chunk> _outputQueue;
		private readonly ManualResetEventSlim _inputFinished;
		private readonly CountdownEvent _outputStarted;
		private readonly GzipWorker _gzipWorker;

		public DataProcessor(
			MultithreadingQueue<Chunk> inputQueue,
			MultithreadingQueue<Chunk> outputQueue,
			ManualResetEventSlim inputFinished,
			GzipWorker gzipWorker,
			CountdownEvent outputStarted)
		{
			_inputQueue = inputQueue;
			_outputQueue = outputQueue;
			_inputFinished = inputFinished;
			_gzipWorker = gzipWorker;
			_outputStarted = outputStarted;
		}

		private void Dequeue()
		{
			Chunk result;

			_inputFinished.Wait();

			while (_inputFinished.IsSet || !_inputQueue.IsEmpty)
			{
				var successReading = _inputQueue.TryDequeue(out result);
				if (!successReading)
				{
					continue;
				}

				var processedChunk = _gzipWorker.CompressChunk(result);
				_outputQueue.Enqueue(processedChunk);
			}

			_outputStarted.Signal();
			Console.WriteLine($"Processing done in Thread with id: {Thread.CurrentThread.ManagedThreadId}.");
		}

		public void Start(int? threadsCount)
		{
			for (int i = 0; i < threadsCount; i++)
			{
				new Thread(Dequeue).Start();
			}
		}
	}
}