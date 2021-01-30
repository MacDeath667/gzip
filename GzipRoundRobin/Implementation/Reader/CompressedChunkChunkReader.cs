using System;
using System.IO;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation
{
	public class CompressedChunkChunkReader : BaseChunkReader
	{
		public CompressedChunkChunkReader(AutoThreadingPreferences settings) : base(settings)
		{
		}

		protected override void ReadChunks(string filepath)
		{
			using (var filestream = File.Open(filepath, FileMode.Open))
			{
				Reset.Set();
				var i = 0;
				while (true)
				{
					var chunkSize = ReadHeader(filepath);
					var buffer = new byte[chunkSize];
					if ((_readBytes = filestream.Read(buffer)) <= 0)
					{
						break;
					}
					
					var index = i % Queues.Length;

					Queues[index].Enqueue(CreateChunk(buffer.Clone() as byte[], _readBytes));
					++i;
					//Console.WriteLine($"Reader count = {i}");
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
				if (_readBytes != buffer.Length)
				{
					throw new InvalidCastException("Неожиданное окончание заголовка с размером чанка");
				}

				return BitConverter.ToInt32(buffer, 0);
			}
		}

		private int _readBytes;
	}
}