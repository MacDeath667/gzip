namespace GzipRoundRobin.Abstractions
{
	interface IChunk
	{
		int Size { get; }
		byte[] Data { get; }
	}
}