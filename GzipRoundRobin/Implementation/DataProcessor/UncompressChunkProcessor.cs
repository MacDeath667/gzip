﻿using GzipRoundRobin.Abstractions;
using GzipRoundRobin.Implementation.Base;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Implementation.DataProcessor
{
	public class UncompressChunkProcessor : BaseChunkProcessor
	{
		public UncompressChunkProcessor(
			IReader reader,
			IWriter writer,
			GzipWorker gzipWorker) : base(
			reader,
			writer,
			gzipWorker)
		{
		}

		protected override IChunk ProcessChunkData(IChunk chunk)
		{
			var processedChunk = GzipWorker.UncompressChunk(chunk);
			return processedChunk;
		}
	}
}