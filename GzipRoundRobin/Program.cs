using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Implementation.DataProcessor;
using GzipRoundRobin.Implementation.Reader;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin
{
	class Program
	{
		static void Main(string[] args)
		{
			//todo make fabric for compress/decompress 
			
			var parsedArgs = new CliParser().CliParse(args);
			var threadingPreferences = new AutoThreadingPreferences();

			var reader = new UncompressedChunkChunkReader(threadingPreferences);
			var writer = new BaseChunkWriter(threadingPreferences);
			//var dataProcessor = new UncompressChunkProcessor(reader, writer, new GzipWorker());
			var bypassProcessor = new BypassProcessor(reader, writer, new GzipWorker());
			
			reader.StartRead(parsedArgs.FilePath);
			// dataProcessor.Start(threadingPreferences.Threads);
			bypassProcessor.Start(threadingPreferences.Threads);
			writer.StartWrite(parsedArgs.OutPath);
		}
	}
}