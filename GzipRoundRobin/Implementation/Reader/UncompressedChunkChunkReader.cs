using System;
using System.IO;
using GzipRoundRobin.Implementation.Base;

namespace GzipRoundRobin.Implementation
{
	public class UncompressedChunkChunkReader : BaseChunkReader
	{
		protected override void ReadChunks(string filepath)
		{
			using (var filestream = File.Open(filepath, FileMode.Open))
			{
				var buffer = new byte[2 * 1024 * 1024];
				var readBytes = 0;
				Reset.Set();
				while ((readBytes = filestream.Read(buffer)) > 0)
				{
					var i = 0;
					var index = i % Queues.Length;

					Queues[index].Enqueue(CreateChunk(buffer.Clone() as byte[], readBytes));
					++i;
					Console.WriteLine($"Reader count = {i}");
				}

				Console.WriteLine("File end");
				Reset.Reset();
			}
		}
	}
}