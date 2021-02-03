using System;
using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Factory
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

		public IProcessChain MakeProcessChain()
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

		private IProcessChain MakeDecompressChain()
		{
			return new DecompressedProcessChain(_cliArguments, _threadingPreferences);
		}

		private IProcessChain MakeCompressChain()
		{
			return new CompressedProcessChain(_cliArguments, _threadingPreferences);
		}
	}
}