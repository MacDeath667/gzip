﻿using GzipRoundRobin.Implementation.DataProcessor;
using GzipRoundRobin.Implementation.Reader;
using GzipRoundRobin.Implementation.Writer;
using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Fabric
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