using System;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Fabric
{
	public class GzipCombineFabric
	{
		private readonly CliParserResult _cliArguments;
		private readonly AutoThreadingPreferences _threadingPreferences;

		public GzipCombineFabric(
			CliParserResult cliArguments,
			AutoThreadingPreferences threadingPreferences)
		{
			_cliArguments = cliArguments;
			_threadingPreferences = threadingPreferences;
		}

		public IProcessorChain MakeProcessorChain()
		{
			switch (_cliArguments.GzipActionType)
			{
				case GzipActionType.Compress:
					return MakeCompressChain();

				case GzipActionType.Decompress:
					return MakeDecompressChain();
			}

			Console.WriteLine("Unknown Gzip action type. Check");
			Environment.Exit(1);
			return default;
		}

		private IProcessorChain MakeDecompressChain()
		{
			return new DecompressedProcessorChain(_cliArguments, _threadingPreferences);
		}

		private IProcessorChain MakeCompressChain()
		{
			return new CompressedProcessorChain(_cliArguments, _threadingPreferences);
		}
	}
}