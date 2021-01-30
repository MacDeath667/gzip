using System;
using System.IO;
using System.Threading;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Base
{
	public class BaseChunkWriter : IWriter<IChunk>
	{
		public CountdownEvent Reset { get; set; }
		public MultithreadingQueue<IChunk>[] Queues { get; set; }
		
		public void StartWrite(string filepath)
		{
			WriteChunks(filepath);
		}
		
		private void WriteChunks(string filepath)
		{
			using (var filestream = File.OpenWrite(filepath))
			{
				var queueIndex = 0;
				while (!Reset.IsSet)
				{
					for (int i = 0; i < Queues.Length; i++)
					{
						if (Queues[i].TryDequeue(out var chunk))
						{
							var binaryWriter = new BinaryWriter(filestream);
							binaryWriter.Write(chunk.Size);
							binaryWriter.Write(chunk.Data);
						}
					}
				}
				Console.WriteLine("Write done");
			}
		}
	}
}
