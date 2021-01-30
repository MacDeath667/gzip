using GzipRoundRobin.Interface;

namespace GzipRoundRobin.Implementation.Chunks
{
	public class DataChunk : IChunk
	{
		public int Size { get; set; }
		public byte[] Data { get; set; }
	}
}