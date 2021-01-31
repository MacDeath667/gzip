using System;

namespace GzipRoundRobin.Primitives
{
	public class InvalidChunkContentException : Exception
	{
		public InvalidChunkContentException(string errorMessage) : base(errorMessage)
		{
		}
	}
}