using System.Threading;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Abstractions
{
	internal interface IReader
	{
		ManualResetEventSlim Reset { get; set; }
		ManualResetEventSlim StartWork { get; set; }
		BlockingQueue<IChunk>[] Queues { get; set; }
		void Start(string filepath);
	}
}