using System;

namespace GzipRoundRobin.Primitives
{
	internal class InvalidChunkContentException : Exception
	{
		internal InvalidChunkContentException(string errorMessage) : base(errorMessage)
		{
		}
	}
}