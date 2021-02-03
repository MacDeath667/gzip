using System;
using System.IO;
using System.IO.Compression;
using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Implementation.Chunks;

namespace GzipRoundRobin.Primitives
{
	internal class GzipWorker
	{
		internal GzipWorker(AutoThreadingPreferences settings)
		{
			_settings = settings;
		}

		internal IChunk CompressChunk(IChunk data)
		{
			MemoryStream output;
			using (output = new MemoryStream())
			{
				using (var compressStream = new GZipStream(output, CompressionMode.Compress, true))
				{
					compressStream.Write(data.Data, 0, data.Size);
				}

				return new DataChunk()
				{
					Data = output.ToArray(),
					Size = (int) output.Length
				};
			}
		}

		internal IChunk UncompressChunk(IChunk data)
		{
			var destination = new byte[_settings.BufferBytes];
			int restoredBytes;

			using (var input = new MemoryStream(data.Data))
			using (var uncompressStream = new GZipStream(input, CompressionMode.Decompress))
			{
				restoredBytes = uncompressStream.Read(destination, 0, destination.Length);
				Console.WriteLine(restoredBytes);
			}

			return new DataChunk()
			{
				Data = destination,
				Size = restoredBytes
			};
		}

		private readonly AutoThreadingPreferences _settings;
	}
}