using System;
using System.IO;
using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.Writer
{
	internal class UncompressedChunksWriter : BaseChunkWriter
	{
		internal UncompressedChunksWriter(AutoThreadingPreferences settings) : base(settings)
		{
		}

		public override void Start(string filepath)
		{
			using (var filestream = File.Create(filepath))
			using (var binaryWriter = new BinaryWriter(filestream))
			{
				Write(binaryWriter);
			}
		}

		protected override void WriteChunk(BinaryWriter binaryWriter, IChunk chunk)
		{
			Console.WriteLine($"Uncompressed writer: {chunk.Size}");
			binaryWriter.Write(chunk.Data, 0, chunk.Size);
		}
	}
}