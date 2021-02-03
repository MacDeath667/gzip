using GzipRoundRobin.Abstractions;

namespace GzipRoundRobin.Implementation.Chunks
{
	internal class DataChunk : IChunk
	{
		public int Size { get; set; }
		public byte[] Data { get; set; }
	}
}