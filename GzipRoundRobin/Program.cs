using GzipRoundRobin.Implementation;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Implementation.DataProcessor;
using GzipRoundRobin.Primitives;
using Microsoft.Extensions.Configuration;

namespace GzipRoundRobin
{
	class Program
	{
		static void Main(string[] args)
		{
			//todo make fabric for compress/decompress 
			
			var parsedArgs = new CliParser().CliParse(args);
			var threadingPreferences = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false)
				.Build()
				.GetSection("Settings")
				.Get<ThreadingPreferences>(); // todo why exception?
			
			if (threadingPreferences==null)
			{
				threadingPreferences=ThreadingPreferences.Auto;
			}

			var reader = new UncompressedChunkChunkReader();
			var writer = new BaseChunkWriter();
			var dataProcessor = new UncompressChunkProcessor(reader, writer, new GzipWorker());
		}
	}
}