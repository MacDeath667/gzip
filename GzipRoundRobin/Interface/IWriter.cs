using System.Threading;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Interface
{
	public interface IWriter
	{
		CountdownEvent Reset { get; set; }
		MultithreadingQueue<IChunk>[] Queues { get; set; }
		void Start(string filepath);
	}
}