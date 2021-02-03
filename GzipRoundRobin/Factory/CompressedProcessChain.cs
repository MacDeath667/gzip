using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Implementation.DataProcessor;
using GzipRoundRobin.Implementation.Reader;
using GzipRoundRobin.Implementation.Writer;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Factory
{
	class CompressedProcessChain : ProcessChain
	{
		public CompressedProcessChain(CliParserResult cliArguments,
			AutoThreadingPreferences threadingPreferences)
			: base(cliArguments,
				threadingPreferences)
		{
			Reader = new UncompressedChunkReader(threadingPreferences);
			Writer = new CompressedChunksWriter(threadingPreferences);
			Processor = new CompressChunkProcessor(Reader, Writer, new GzipWorker(threadingPreferences));
		}

		protected sealed override IDataProcessor Processor { get; }
	}
}