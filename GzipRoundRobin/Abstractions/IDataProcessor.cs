namespace GzipRoundRobin.Abstractions
{
	internal interface IDataProcessor
	{
		void Start(int threadsCount);
	}
}