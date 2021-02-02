using GzipRoundRobin.Interface;

namespace GzipRoundRobin.Fabric
{
	public interface IProcessorChain
	{
		IReader Reader { get; set; }
		IWriter Writer { get; set; }
		IDataProcessor Processor { get; set; }
		void StartProcessing();
	}
}