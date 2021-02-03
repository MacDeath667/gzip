using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Factory
{
	abstract class ProcessChain : IProcessChain
	{
		protected ProcessChain(
			CliParserResult cliArguments,
			AutoThreadingPreferences threadingPreferences)
		{
			_cliArguments = cliArguments;
			_threadingPreferences = threadingPreferences;
		}

		public void StartProcessing()
		{
			Reader.Start(_cliArguments.FilePath);
			Processor.Start(_threadingPreferences.Threads);
			Writer.Start(_cliArguments.OutPath);
		}

		protected IReader Reader { get; set; }
		protected IWriter Writer { get; set; }
		protected abstract IDataProcessor Processor { get; }
		
		private readonly CliParserResult _cliArguments;
		private readonly AutoThreadingPreferences _threadingPreferences;
	}
}