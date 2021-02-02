using GzipRoundRobin.Interface;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Fabric
{
	abstract class ProcessorChain : IProcessorChain
	{
		protected ProcessorChain(CliParserResult cliArguments,
			AutoThreadingPreferences threadingPreferences)
		{
			_cliArguments = cliArguments;
			_threadingPreferences = threadingPreferences;
		}

		public IReader Reader { get; set; }
		public IWriter Writer { get; set; }
		public abstract IDataProcessor Processor { get; set; }

		public void StartProcessing()
		{
			Reader.Start(_cliArguments.FilePath);
			Processor.Start(_threadingPreferences.Threads);
			Writer.Start(_cliArguments.OutPath);
		}


		private readonly CliParserResult _cliArguments;
		private readonly AutoThreadingPreferences _threadingPreferences;
	}
}