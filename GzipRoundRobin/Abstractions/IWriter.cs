using System.Threading;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Abstractions
{
	internal interface IWriter
	{
		CountdownEvent Reset { get; }
		BlockingQueue<IChunk>[] Queues { get; }
		void Start(string filepath);
	}
}