namespace GzipRoundRobin.Interface
{
	public interface IChunk
	{
		public int Size { get; set; }
		public byte[] Data { get; set; }
	}
}