namespace GzipRoundRobin.Interface
{
	public interface IDataProcessor<Tin, Tout>
	{
		IReader<Tin> Reader { get; set; }
		IWriter<Tout> Writer { get; set; }
	}
}