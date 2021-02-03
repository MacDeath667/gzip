using System;
using System.IO;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Reader
{
	internal class UncompressedChunkReader : BaseChunkReader
	{
		internal UncompressedChunkReader(AutoThreadingPreferences settings) : base(settings)
		{
			_buffer = new byte[settings.BufferBytes];
		}

		private protected override void ReadChunks(string filepath)
		{
			using (var filestream = File.Open(filepath, FileMode.Open))
			{
				int readBytes;
				var i = 0;
				Reset.Set();
				StartWork.Set();
				while ((readBytes = filestream.Read(_buffer, 0,_buffer.Length)) > 0)
				{
					var index = i % Queues.Length;
					Queues[index].Enqueue(CreateChunk(_buffer.Clone() as byte[], readBytes));
					Console.WriteLine($"reader: {readBytes}");
					++i;
				}
			}
			
			Console.WriteLine("File end");
			Reset.Reset();
		}

		private readonly byte[] _buffer;
	}
}