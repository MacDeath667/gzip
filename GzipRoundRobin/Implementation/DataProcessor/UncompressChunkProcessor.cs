using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.DataProcessor
{
	public class UncompressChunkProcessor : BaseChunkProcessor
	{
		public UncompressChunkProcessor(
			IReader<IChunk> reader,
			IWriter<IChunk> writer,
			GzipWorker gzipWorker) : base(
			reader,
			writer,
			gzipWorker)
		{
		}

		protected override IChunk ProcessChunkData(IChunk result)
		{
			var processedChunk = GzipWorker.UncompressChunk(result);
			return processedChunk;
		}
	}
}