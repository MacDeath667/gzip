using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Primitives;
using Serilog;

namespace GzipRoundRobin.Factory
{
	internal class GzipCombineFabric
	{
		private readonly CliParserResult _cliArguments;
		private readonly AutoThreadingPreferences _threadingPreferences;

		internal GzipCombineFabric(
			CliParserResult cliArguments,
			AutoThreadingPreferences threadingPreferences)
		{
			_cliArguments = cliArguments;
			_threadingPreferences = threadingPreferences;
		}

		internal IProcessChain MakeProcessChain()
		{
			switch (_cliArguments.GzipActionType)
			{
				case GzipActionType.Compress:
					return MakeCompressChain();

				case GzipActionType.Decompress:
					return MakeDecompressChain();
			}

			Log.Error("Unknown Gzip action type. Check it");
			ExitHelper.ExitWithCode(1);
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