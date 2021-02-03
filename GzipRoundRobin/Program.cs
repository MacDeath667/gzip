using GzipRoundRobin.Factory;
using GzipRoundRobin.Primitives;
using GzipRoundRobin.Validators;

namespace GzipRoundRobin
{
	static class Program
	{
		static void Main(string[] args)
		{
			if (!new CliParser().TryCliParse(out var parsedArgs, args) 
			    || !parsedArgs.Validate())
			{
				ExitHelper.ExitWithCode(1);
			}
			
			var threadingPreferences = AutoThreadingPreferences.Create();

			new GzipCombineFabric(parsedArgs, threadingPreferences)
				.MakeProcessChain()
				.StartProcessing();

			ExitHelper.ExitWithCode(0);
		}
	}
}