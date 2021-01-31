using System.IO;
using System.IO.Compression;
using GzipRoundRobin.Implementation.Chunks;
using GzipRoundRobin.Interface;

namespace GzipRoundRobin.Primitives
{
	public class GzipWorker
	{
		private AutoThreadingPreferences _settings;

		public GzipWorker(AutoThreadingPreferences settings)
		{
			_settings = settings;
		}
		public IChunk CompressChunk(IChunk data)
		{
			using (var output = new MemoryStream())
			{
				using (var compressStream = new GZipStream(output, CompressionMode.Compress))
				{
					compressStream.Write(data.Data, 0, data.Size);
					return new DataChunk()
					{
						Data = output.ToArray(),
						Size = (int) output.Length
					};
				}
			}
		}
		
		public IChunk UncompressChunk(IChunk data)
		{
			var destination = new byte[_settings.BufferBytes];
			using (var input = new MemoryStream(data.Data))
			{
				using (var uncompressStream = new GZipStream(input, CompressionMode.Decompress))
				{
					var restoredBytes = uncompressStream.Read(destination, 0, data.Size);
					return new DataChunk()
					{
						Data = destination,
						Size = restoredBytes
					};
				}
			}
		}
	}
}