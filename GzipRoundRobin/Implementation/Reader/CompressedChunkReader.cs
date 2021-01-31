using System;
using System.IO;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Reader
{
	public class CompressedChunkReader : BaseChunkReader
	{
		public CompressedChunkReader(AutoThreadingPreferences settings) : base(settings)
		{
		}

		protected override void ReadChunks(string filepath)
		{
			using (var filestream = File.Open(filepath, FileMode.Open))
			{
				Reset.Set();
				var i = 0;
				var bufferSize = ReadBufferSizeHeader(filestream);
				while (true)
				{
					int chunkSize = ReadCompressedDataLength(filestream);
					var buffer = new byte[chunkSize];
					if ((_readBytes = filestream.Read(buffer, 0, buffer.Length)) <= 0)
					{
						break;
					}
					
					var index = i % Queues.Length;

					Queues[index].Enqueue(CreateChunk(buffer.Clone() as byte[], _readBytes));
					++i;
				}

				Reset.Reset();
				Console.WriteLine("File end");
			}
		}

		private int ReadCompressedDataLength(FileStream fileStream)
		{
			var buffer = new byte[4];
			var read = fileStream.Read(buffer, 0, buffer.Length);
			if (read !=buffer.Length)
			{
				throw new InvalidCastException("Не удалось прочитать размер сжатого чанка");
			}
			return BitConverter.ToInt32(buffer, 0);
		}

		private int ReadBufferSizeHeader(FileStream fileStream)
		{
			var buffer = new byte[4];
			var read = fileStream.Read(buffer, 0, buffer.Length);
			if (read !=buffer.Length)
			{
				throw new InvalidCastException("Не удалось прочитать размер буффера");
			}
			return BitConverter.ToInt32(buffer, 0);
		}
		private int _readBytes;
	}
}