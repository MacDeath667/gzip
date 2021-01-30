using System.Threading;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Interface
{
	public interface IWriter<T>
	{
		CountdownEvent Reset { get; set; }
		MultithreadingQueue<T>[] Queues { get; set; }
		void StartWrite(string filepath);
	}
}