namespace GzipRoundRobin.Abstractions
{
	interface IChunk
	{
		int Size { get; set; }
		byte[] Data { get; set; }
	}
}