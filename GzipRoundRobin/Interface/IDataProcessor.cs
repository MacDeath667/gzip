namespace GzipRoundRobin.Interface
{
	public interface IDataProcessor
	{
		IReader Reader { get; set; }
		IWriter Writer { get; set; }

		void Start(int threadsCount);
	}
}