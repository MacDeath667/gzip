using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Implementation.DataProcessor;
using GzipRoundRobin.Implementation.Reader;
using GzipRoundRobin.Implementation.Writer;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin
{
	class Program
	{
		static void Main(string[] args)
		{
			//todo make fabric for compress/decompress 
			
			var parsedArgs = new CliParser().CliParse(args);
			var threadingPreferences = AutoThreadingPreferences.Create();

			var reader = new UncompressedChunkReader(threadingPreferences);
			var writer = new CompressedChunksWriter(threadingPreferences);
			var dataProcessor = new CompressChunkProcessor(reader, writer, new GzipWorker());
			
			reader.Start(parsedArgs.FilePath);
			dataProcessor.Start(threadingPreferences.Threads);
			writer.Start(parsedArgs.OutPath);
			
			
			// var creader = new CompressedChunkReader(threadingPreferences);
			// var cwriter = new UncompressedChunksWriter(threadingPreferences);
			// var cdataProcessor = new UncompressChunkProcessor(creader, cwriter, new GzipWorker());
			//
			// creader.StartRead(parsedArgs.OutPath);
			// cdataProcessor.Start(threadingPreferences.Threads);
			// cwriter.StartWrite(parsedArgs.OutPath+".restored.exe");
		}
	}
}