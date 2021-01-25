namespace VeeamGZip.Models
{
	public class CompressedChunk
	{
		public int ChunkNumber { get; set; }
		public int ChunkSize { get; set; }
		public byte[] DataBytes { get; set; }
		
	}
}