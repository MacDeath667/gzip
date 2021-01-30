using System.IO;
using System.IO.Compression;
using GzipRoundRobin.Implementation.Chunks;
using GzipRoundRobin.Interface;

namespace GzipRoundRobin.Primitives
{
	public class GzipWorker
	{
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
			using (var output = new MemoryStream())
			{
				using (var compressStream = new GZipStream(output, CompressionMode.Decompress))
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
	}
}