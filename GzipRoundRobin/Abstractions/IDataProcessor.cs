namespace GzipRoundRobin.Abstractions
{
	internal interface IDataProcessor
	{
		IReader Reader { get; set; }
		IWriter Writer { get; set; }

		void Start(int threadsCount);
	}
}