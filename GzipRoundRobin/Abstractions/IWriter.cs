using System.Threading;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Abstractions
{
	public interface IWriter
	{
		CountdownEvent Reset { get; set; }
		MultithreadingQueue<IChunk>[] Queues { get; set; }
		void Start(string filepath);
	}
}