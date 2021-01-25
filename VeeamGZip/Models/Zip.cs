namespace VeeamGZip.Models
{
	class Zip
	{
		public ZipHeader Header { get; set; }
		public CompressedChunk[] CompressedChunks { get; set; }
	}
}