using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Implementation.DataProcessor;
using GzipRoundRobin.Implementation.Reader;
using GzipRoundRobin.Implementation.Writer;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Fabric
{
	class DecompressedProcessChain : ProcessChain, IProcessChain
	{
		public DecompressedProcessChain(
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