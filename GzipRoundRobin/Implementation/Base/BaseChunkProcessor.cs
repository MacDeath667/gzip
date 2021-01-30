using System;
using System.Threading;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Base
{
	public abstract class BaseChunkProcessor : IDataProcessor<IChunk, IChunk>
	{
		protected BaseChunkProcessor(
			IReader<IChunk> reader,
			IWriter<IChunk> writer,
			GzipWorker gzipWorker)
		{
			Reader = reader;
			Writer = writer;
			GzipWorker = gzipWorker;
		}

		public IReader<IChunk> Reader { get; set; }
		public IWriter<IChunk> Writer { get; set; }
		internal GzipWorker GzipWorker { get; }

		private void Dequeue(int processorId)
		{
			Console.WriteLine($"Processing start in thread: {processorId}");
			var inputQueue = Reader.Queues[processorId];
			var outputQueue = Writer.Queues[processorId];

			Reader.Reset.Wait();

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
			Console.WriteLine($"Data processor: {processorId} finished.");
		}

		protected abstract IChunk ProcessChunkData(IChunk chunk);

		public void Start(int threadsCount)
		{
			Console.WriteLine($"Processing start in {threadsCount} threads");
			
			for (int i = 0; i < threadsCount; i++)
			{
				var tmp = i;
				new Thread(() => Dequeue(tmp)).Start();
			}
		
		}
	}
}