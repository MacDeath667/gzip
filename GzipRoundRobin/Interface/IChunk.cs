namespace GzipRoundRobin.Interface
{
	public interface IChunk
	{
		int Size { get; set; }
		byte[] Data { get; set; }
	}
}