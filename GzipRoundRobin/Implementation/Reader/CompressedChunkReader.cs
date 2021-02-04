using System;
using System.IO;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Primitives;
using Serilog;

namespace GzipRoundRobin.Implementation.Reader
{
	internal class CompressedChunkReader : BaseChunkReader
	{
		internal CompressedChunkReader(AutoThreadingPreferences settings) : base(settings)
		{
			_settings = settings;
		}

		private protected override void ReadChunks(string filepath)
		{
			using (var filestream = File.Open(filepath, FileMode.Open))
			{
				Reset.Set();
				StartWork.Set();
				var i = 0;
				var bufferSize = ReadBufferSizeHeader(filestream);
				Log.Debug($"Unpacking with buffersize: {bufferSize}");
				while (true)
				{
					int chunkSize = ReadCompressedDataLength(filestream);
					var buffer = new byte[chunkSize];
					if ((_readBytes = filestream.Read(buffer, 0, buffer.Length)) <= 0)
					{
						break;
					}

					var index = i % Queues.Length;
					Log.Debug($"Compressed chunk: {i}, size: {_readBytes}");
					Queues[index].Enqueue(CreateChunk(buffer.Clone() as byte[], _readBytes));
					++i;
				}

				Reset.Reset();
				Log.Information("End of file");
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
				throw new InvalidChunkContentException("Can't get size of buffer from file");
			}
			if (bufferSize>_settings.BufferBytes)
			{
				Log.Error($"BufferBytes in appsettings.json must be {bufferSize} or more");
				ExitHelper.ExitWithCode(1);
			}
			if (bufferSize<_settings.BufferBytes)
			{
				Log.Information($"Buffer will be reduced to {bufferSize} bytes");
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
		private readonly AutoThreadingPreferences _settings;
	}
}