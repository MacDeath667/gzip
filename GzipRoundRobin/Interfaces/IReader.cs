using System.Threading;
using GzipRoundRobin.Elements;

namespace GzipRoundRobin.Interfaces
{
	public interface IReader<T>
	{
		ManualResetEventSlim Reset { get; set; }
		MultithreadingQueue<T>[] Queue { get; set; }
		void StartRead(string filepath);
	}

	public interface IWriter<T>
	{
		CountdownEvent Reset { get; set; }
		MultithreadingQueue<T>[] Queue { get; set; }
		void StartWrite(string filepath);
	}

	public interface IDataProcessor<Tin, Tout>
	{
		IReader<Tin> Reader { get; set; }
		IWriter<Tout> Writer { get; set; }
	}
	
	public interface IChunk
	{
		public int Size { get; set; }
		public byte[] Data { get; set; }
	}
}