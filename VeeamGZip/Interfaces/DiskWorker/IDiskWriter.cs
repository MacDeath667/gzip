namespace VeeamGZip.Interfaces.DiskWorker
{
	public interface IDiskWriter
	{
		byte[] WriteChunk();
	}
}