using System;
using System.Threading;
using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Primitives;
using Serilog;

namespace GzipRoundRobin.Implementation.Base
{
	internal abstract class BaseChunkProcessor : IDataProcessor
	{
		protected BaseChunkProcessor(
			IReader reader,
			IWriter writer,
			GzipWorker gzipWorker)
		{
			Reader = reader;
			Writer = writer;
			GzipWorker = gzipWorker;
		}

		public void Start(int threadsCount)
		{
			Log.Information($"Processing start in {threadsCount} threads");

			for (int i = 0; i < threadsCount; i++)
			{
				var tmp = i;
				new Thread(() => Dequeue(tmp)).Start();
			}
		}

		protected abstract IChunk ProcessChunkData(IChunk chunk);

		private void Dequeue(int processorId)
		{
			Log.Debug($"Processing start in thread: {processorId}");
			var inputQueue = Reader.Queues[processorId];
			var outputQueue = Writer.Queues[processorId];

			Reader.StartWork.Wait();

			while (Reader.Reset.IsSet || !inputQueue.IsEmpty)
			{
				var successReading = inputQueue.TryDequeue(out var chunk);
				if (!successReading)
				{
					Thread.Sleep(1);
					continue;
				}

				var processedChunk = ProcessChunkData(chunk);
				outputQueue.Enqueue(processedChunk);
			}

			Writer.Reset.Signal();
			Log.Debug($"Data processor: {processorId} finished.");
		}


		private IReader Reader { get; }
		private IWriter Writer { get; }
		internal GzipWorker GzipWorker { get; }
	}
}