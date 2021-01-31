using System;
using System.IO;
using System.Threading;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Base
{
	public abstract class BaseChunkWriter : IWriter<IChunk>
	{
		protected BaseChunkWriter(AutoThreadingPreferences settings)
		{
			Reset = new CountdownEvent(settings.Threads);
			Queues = new MultithreadingQueue<IChunk>[settings.Threads];
			for (int i = 0; i < Queues.Length; i++)
			{
				Queues[i] = new MultithreadingQueue<IChunk>(settings.Threads);
			}
		}


		public CountdownEvent Reset { get; set; }
		public MultithreadingQueue<IChunk>[] Queues { get; set; }
		
		public abstract void StartWrite(string filepath);

		protected void Write(BinaryWriter binaryWriter)
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