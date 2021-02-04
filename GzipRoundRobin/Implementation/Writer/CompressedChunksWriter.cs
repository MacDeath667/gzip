using System.IO;
using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Primitives;
using Serilog;

namespace GzipRoundRobin.Implementation.Writer
{
	internal class CompressedChunksWriter : BaseChunkWriter
	{
		internal CompressedChunksWriter(AutoThreadingPreferences settings) : base(settings)
		{
			_bufferBytes = settings.BufferBytes;
		}

		public override void Start(string filepath)
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
			Log.Debug($"Compressed writer: {chunk.Size}");
			binaryWriter.Write(chunk.Size);
			binaryWriter.Write(chunk.Data, 0, chunk.Size);
		}

		private readonly int _bufferBytes;
	}
}