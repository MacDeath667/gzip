namespace VeeamGZip.Interfaces.DiskWorker
{
	public interface IDiskReader
	{
		byte[] ReadChunk();
	}
}