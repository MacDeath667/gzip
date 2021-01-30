using System;
using System.IO;
using GzipRoundRobin.Implementation.Base;

namespace GzipRoundRobin.Implementation
{
	public class CompressedChunkChunkReader : BaseChunkReader
	{
		int _readBytes;

		protected override void ReadChunks(string filepath)
		{
			int _readBytes;
			using (var filestream = File.Open(filepath, FileMode.Open))
			{
				Reset.Set();

				while (true)
				{
					var chunkSize = ReadHeader(filepath);
					var buffer = new byte[chunkSize];
					if ((_readBytes = filestream.Read(buffer)) <= 0)
					{
						break;
					}
					var i = 0;
					var index = i % Queues.Length;

					Queues[index].Enqueue(CreateChunk(buffer.Clone() as byte[], _readBytes));
					++i;
					Console.WriteLine($"Reader count = {i}");
				}

				Console.WriteLine("File end");
				Reset.Reset();
			}
		}

		private int ReadHeader(string filepath)
		{
			//read first int32 with bufferSize
			using (var filestream = File.Open(filepath, FileMode.Open))
			{
				var buffer = new byte[4];
				Reset.Set();
				_readBytes = filestream.Read(buffer);
				if (_readBytes!=buffer.Length)
				{
					throw new InvalidCastException("Неожиданное окончание заголовка с размером чанка");
				}
				return BitConverter.ToInt32(buffer, 0);
			}
		}
	}
}