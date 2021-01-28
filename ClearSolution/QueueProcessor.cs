using System;
using System.Threading;

namespace ClearSolution
{
	public class QueueProcessor
	{
		private static int count=0;
		private readonly MultithreadingQueue<Chunk> _inputQueue;
		private readonly MultithreadingQueue<Chunk> _outputQueue;
		private readonly ManualResetEventSlim _inputFinished;
		private readonly ManualResetEventSlim _outputStarted;
		private readonly GzipWorker _gzipWorker;

		public QueueProcessor(
			MultithreadingQueue<Chunk> inputQueue,
			MultithreadingQueue<Chunk> outputQueue,
			ManualResetEventSlim inputFinished,
			GzipWorker gzipWorker,
			ManualResetEventSlim outputStarted)
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
			bool successReading;

			while (_inputFinished.IsSet || !_inputQueue.IsEmpty)
			{
				successReading = _inputQueue.TryDequeue(out result);
				if (!successReading)
				{
					continue;
				}

				//Console.WriteLine(result.Size + " - read bytes from queue in Thread: " + Thread.CurrentThread.ManagedThreadId);
				
				var processedChunk = _gzipWorker.CompressChunk(result);
				_outputQueue.Enqueue(processedChunk);
				Console.WriteLine($"producer count = {++count}");
				
			//	Console.WriteLine(processedChunk.Size + " - processed bytes in Thread: " + Thread.CurrentThread.ManagedThreadId);
				_outputStarted.Set();
			} 
			_outputStarted.Reset();
			Console.WriteLine("Processing Done");
		}

		public void Start()
		{
			new Thread(Dequeue).Start();
			new Thread(Dequeue).Start();
			new Thread(Dequeue).Start();
			new Thread(Dequeue).Start();
		}
	}
}