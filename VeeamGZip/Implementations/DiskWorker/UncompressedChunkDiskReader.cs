using VeeamGZip.Interfaces.DiskWorker;

namespace VeeamGZip.Implementations.DiskWorker
{
	public class UncompressedChunkDiskReader :IDiskReader
	{
		public byte[] ReadChunk()
		{
			throw new System.NotImplementedException();
		}
	}
}