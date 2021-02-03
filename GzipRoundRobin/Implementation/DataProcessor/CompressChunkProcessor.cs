using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.DataProcessor
{
	internal class CompressChunkProcessor : BaseChunkProcessor
	{
		internal CompressChunkProcessor(
			IReader reader,
			IWriter writer,
			GzipWorker gzipWorker) : base(
			reader,
			writer,
			gzipWorker)
		{
		}

		protected override IChunk ProcessChunkData(IChunk chunk)
		{
			var processedChunk = GzipWorker.CompressChunk(chunk);
			return processedChunk;
		}
	}
}