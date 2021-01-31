using System;
using System.Configuration;
using System.IO;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Reader
{
	public class CompressedChunkReader : BaseChunkReader
	{
		public CompressedChunkReader(AutoThreadingPreferences settings) : base(settings)
		{
			_settings = settings;
		}

		protected override void ReadChunks(string filepath)
		{
			using (var filestream = File.Open(filepath, FileMode.Open))
			{
				Reset.Set();
				StartWork.Set();
				var i = 0;
				var bufferSize = ReadBufferSizeHeader(filestream);
				Console.WriteLine($"Unpacking with buffersize: {bufferSize}");
				while (true)
				{
					int chunkSize = ReadCompressedDataLength(filestream);
					var buffer = new byte[chunkSize];
					if ((_readBytes = filestream.Read(buffer, 0, buffer.Length)) <= 0)
					{
						break;
					}

					var index = i % Queues.Length;
					Console.WriteLine($"compressed chunk: {i}, size: {_readBytes}");
					Queues[index].Enqueue(CreateChunk(buffer.Clone() as byte[], _readBytes));
					++i;
				}

				Reset.Reset();
				Console.WriteLine("File end");
			}
		}

		private int ReadCompressedDataLength(FileStream fileStream)
		{
			var headerSize = ReadIntFromFilestream(fileStream);
			if (headerSize < 0)
			{
				throw new InvalidChunkContentException("Не удалось прочитать размер сжатого чанка");
			}

			return headerSize;
		}


		private int ReadBufferSizeHeader(FileStream fileStream)
		{
			var bufferSize = ReadIntFromFilestream(fileStream);
			if (bufferSize < 0)
			{
				throw new InvalidChunkContentException("Не удалось прочитать размер буфера");
			}
			if (bufferSize>_settings.BufferBytes)
			{
				Console.WriteLine($"BufferBytes in appsettings.json must be {bufferSize} or more");
				Environment.Exit(1);
			}
			if (bufferSize<_settings.BufferBytes)
			{
				Console.WriteLine($"Buffer will be reduced to {bufferSize} bytes");
				_settings.BufferBytes = bufferSize;
			}
			return bufferSize;
		}

		private int ReadIntFromFilestream(FileStream fileStream)
		{
			var buffer = new byte[4];

			var read = fileStream.Read(buffer, 0, buffer.Length);
			if (read != buffer.Length &&read !=0)
			{
				return -1;
			}

			return BitConverter.ToInt32(buffer, 0);
		}

		private int _readBytes;
		private AutoThreadingPreferences _settings;
	}
}