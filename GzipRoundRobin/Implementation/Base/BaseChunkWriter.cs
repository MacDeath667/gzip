using System;
using System.IO;
using System.Threading;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Base
{
	public class BaseChunkWriter : IWriter<IChunk>
	{
		public BaseChunkWriter(AutoThreadingPreferences settings)
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

		public void StartWrite(string filepath)
		{
			WriteChunks(filepath);
		}

		private void WriteChunks(string filepath)
		{
			using (var filestream = File.Create(filepath))
			using (var binaryWriter = new BinaryWriter(filestream))
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
						//binaryWriter.Write(chunk.Size);
						binaryWriter.Write(chunk.Data,0,chunk.Size);
					}
				}

				// var queueIndex = 0;
				// while (!Reset.IsSet)
				// {
				// 	for (int i = 0; i < Queues.Length; i++)
				// 	{
				// 		if (Queues[i].TryDequeue(out var chunk))
				// 		{
				// 			var binaryWriter = new BinaryWriter(filestream);
				// 			//binaryWriter.Write(chunk.Size);
				// 			binaryWriter.Write(chunk.Data,0,chunk.Size);
				// 		}
				// 	}
				// }
				// Console.WriteLine("Write done");
			}
		}
	}
}