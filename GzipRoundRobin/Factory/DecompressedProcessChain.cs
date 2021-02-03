using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Implementation.DataProcessor;
using GzipRoundRobin.Implementation.Reader;
using GzipRoundRobin.Implementation.Writer;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Factory
{
	class DecompressedProcessChain : ProcessChain
	{
		protected internal DecompressedProcessChain(
			CliParserResult cliArguments,
			AutoThreadingPreferences threadingPreferences)
			: base(cliArguments,
				threadingPreferences)
		{
			Reader = new CompressedChunkReader(threadingPreferences);
			Writer = new UncompressedChunksWriter(threadingPreferences);
			Processor = new UncompressChunkProcessor(Reader, Writer, new GzipWorker(threadingPreferences));
		}

		protected sealed override IDataProcessor Processor { get; }
	}
}