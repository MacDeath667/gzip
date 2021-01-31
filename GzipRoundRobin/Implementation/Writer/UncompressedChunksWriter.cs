using System.IO;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Writer
{
	public class UncompressedChunksWriter : BaseChunkWriter
	{
		public UncompressedChunksWriter(AutoThreadingPreferences settings) : base(settings)
		{
		}

		public override void StartWrite(string filepath)
		{
			using (var filestream = File.Create(filepath))
			using (var binaryWriter = new BinaryWriter(filestream))
			{
				Write(binaryWriter);
			}
		}

		protected override void WriteChunk(BinaryWriter binaryWriter, IChunk chunk)
		{
			binaryWriter.Write(chunk.Data, 0, chunk.Size);
		}
	}
}