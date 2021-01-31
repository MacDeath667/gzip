using System.IO;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Writer
{
	public class CompressedChunksWriter : BaseChunkWriter
	{
		public CompressedChunksWriter(AutoThreadingPreferences settings) : base(settings)
		{
			_bufferBytes = settings.BufferBytes;
		}

		public override void StartWrite(string filepath)
		{
			using (var filestream = File.Create(filepath))
			using (var binaryWriter = new BinaryWriter(filestream))
			{
				binaryWriter.Write(_bufferBytes);
				Write(binaryWriter);
			}
		}

		protected override void WriteChunk(BinaryWriter binaryWriter, IChunk chunk)
		{
			binaryWriter.Write(chunk.Size);
			binaryWriter.Write(chunk.Data, 0, chunk.Size);
		}

		private readonly int _bufferBytes;
	}
}