using System.Threading;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Interface
{
	public interface IReader<T>
	{
		ManualResetEventSlim Reset { get; set; }
		ManualResetEventSlim StartWork { get; set; }
		MultithreadingQueue<T>[] Queues { get; set; }
		void Start(string filepath);
	}
}