using System;
using System.IO;
using System.Threading;
using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Base
{
	internal abstract class BaseChunkWriter : IWriter
	{
		protected BaseChunkWriter(AutoThreadingPreferences settings)
		{
			Reset = new CountdownEvent(settings.Threads);
			Queues = new BlockingQueue<IChunk>[settings.Threads];
			for (int i = 0; i < Queues.Length; i++)
			{
				Queues[i] = new BlockingQueue<IChunk>(settings.Threads);
			}
		}


		public CountdownEvent Reset { get; }
		public BlockingQueue<IChunk>[] Queues { get; }

		public abstract void Start(string filepath);

		private protected void Write(BinaryWriter binaryWriter)
		{
			while (true)
			{
				foreach (var currentQueue in Queues)
				{
					IChunk chunk;
					while (!currentQueue.TryDequeue(out chunk))
					{
						if (Reset.IsSet && currentQueue.IsEmpty)
						{
							Console.WriteLine("Write done");
							return;
						}

						Thread.Sleep(1);
					}

					WriteChunk(binaryWriter, chunk);
				}
			}
		}

		protected abstract void WriteChunk(BinaryWriter binaryWriter, IChunk chunk);
	}
}