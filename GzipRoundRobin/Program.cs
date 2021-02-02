using System;
using GzipRoundRobin.Fabric;
using GzipRoundRobin.Implementation.DataProcessor;
using GzipRoundRobin.Implementation.Reader;
using GzipRoundRobin.Implementation.Writer;
using GzipRoundRobin.Primitives;
using GzipRoundRobin.Validators;

namespace GzipRoundRobin
{
	class Program
	{
		static void Main(string[] args)
		{
			//todo make fabric for compress/decompress 
			//todo input args validator: folder access, foldername valid, action type, 

			var parsedArgs = new CliParser().CliParse(args);
			ValidateArguments(parsedArgs);
			
			var threadingPreferences = AutoThreadingPreferences.Create();

			new GzipCombineFabric(parsedArgs, threadingPreferences).MakeProcessorChain().StartProcessing();
			
			
			// var gzipworker = new GzipWorker(threadingPreferences);
			//
			// var reader = new UncompressedChunkReader(threadingPreferences);
			// var writer = new CompressedChunksWriter(threadingPreferences);
			// var dataProcessor = new CompressChunkProcessor(reader, writer, gzipworker);
			//
			// reader.Start(parsedArgs.FilePath);
			// dataProcessor.Start(threadingPreferences.Threads);
			// writer.Start(parsedArgs.OutPath);
			//
			//
			// var creader = new CompressedChunkReader(threadingPreferences);
			// var cwriter = new UncompressedChunksWriter(threadingPreferences);
			// var cdataProcessor = new UncompressChunkProcessor(creader, cwriter, gzipworker);
			//
			// creader.Start(parsedArgs.OutPath);
			// cdataProcessor.Start(threadingPreferences.Threads);
			// cwriter.Start(parsedArgs.OutPath + ".restored.exe");

			Environment.Exit(0);
		}

		private static void ValidateArguments(CliParserResult parsedArgs)
		{
			if (!ArgumentsValidateHelper.IsInputFileExists(parsedArgs.FilePath) 
			||!ArgumentsValidateHelper.IsValidOutpath(parsedArgs.OutPath)
			|| !ArgumentsValidateHelper.IsValidGzipActionType(parsedArgs.GzipActionType))
			{
				Environment.Exit(1);
			}
		}
	}
}