using System.Threading;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Abstractions
{
	internal interface IReader
	{
		ManualResetEventSlim Reset { get; }
		ManualResetEventSlim StartWork { get; }
		BlockingQueue<IChunk>[] Queues { get; }
		void Start(string filepath);
	}
}