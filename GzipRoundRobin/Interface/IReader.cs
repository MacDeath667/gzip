using System.Threading;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Interface
{
	public interface IReader
	{
		ManualResetEventSlim Reset { get; set; }
		ManualResetEventSlim StartWork { get; set; }
		MultithreadingQueue<IChunk>[] Queues { get; set; }
		void Start(string filepath);
	}
}