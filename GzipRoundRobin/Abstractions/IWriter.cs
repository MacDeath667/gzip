using System.Threading;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Abstractions
{
	internal interface IWriter
	{
		CountdownEvent Reset { get; set; }
		BlockingQueue<IChunk>[] Queues { get; set; }
		void Start(string filepath);
	}
}