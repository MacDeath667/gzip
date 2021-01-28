using System.IO;
using System.IO.Compression;

namespace ClearSolution
{
	public class GzipWorker
	{
		public Chunk CompressChunk(Chunk data)
		{
			using (var output = new MemoryStream())
			{
				using (var compressStream = new GZipStream(output, CompressionMode.Compress))
				{
					compressStream.Write(data.Data, 0, data.Size);


					return new Chunk()
					{
						Data = output.ToArray(),
						Size = (int) output.Length
					};
				}
			}
		}
	}
}