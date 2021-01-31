using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.DataProcessor
{
	public class CompressChunkProcessor : BaseChunkProcessor
	{
		public CompressChunkProcessor(
			IReader<IChunk> reader,
			IWriter<IChunk> writer,
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