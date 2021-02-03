using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.DataProcessor
{
	public class BypassProcessor : BaseChunkProcessor
	{
		public BypassProcessor(
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
			return chunk;
		}
	}
}