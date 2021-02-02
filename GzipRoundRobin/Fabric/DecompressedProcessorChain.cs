using GzipRoundRobin.Implementation.DataProcessor;
using GzipRoundRobin.Implementation.Reader;
using GzipRoundRobin.Implementation.Writer;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Fabric
{
	class DecompressedProcessorChain : ProcessorChain
	{
		public DecompressedProcessorChain(
			CliParserResult cliArguments,
			AutoThreadingPreferences threadingPreferences)
			: base(cliArguments,
				threadingPreferences)
		{
			Reader = new CompressedChunkReader(threadingPreferences);
			Writer = new UncompressedChunksWriter(threadingPreferences);
			Processor = new UncompressChunkProcessor(Reader, Writer, new GzipWorker(threadingPreferences));
		}

		public sealed override IDataProcessor Processor { get; set; }
	}
}