using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Microsoft.Extensions.Configuration;
using VeeamGZip.Models;

namespace VeeamGZip
{
	class Program
	{
		static void Main(string[] args)
		{
			var parsedArgs = new CliParser().CliParse(args);
			var threadingPreferences = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false)
				.Build()
				.GetSection("Settings")
				.Get<ThreadingPreferences>(); // todo why exception?
			
			if (threadingPreferences==null)
			{
				threadingPreferences=ThreadingPreferences.Auto;
			}

			var bufferSize = 2 * 1024 * 1024;
			var filepath = parsedArgs.FilePath;
			var outPath = parsedArgs.OutPath;
			long chunksCount;

			using (var fileStream = new FileStream(filepath, FileMode.Open))
			{
				chunksCount = fileStream.Length % bufferSize > 0
					? fileStream.Length / bufferSize + 1
					: fileStream.Length / bufferSize;
			}

			var writeQueue = new MultithreadingQueue<byte[]>();
			for (int i = 0; i < chunksCount; i++)
			{
				var chunkFromDisk = ReadChunkFromDisk(filepath, bufferSize, 0);
				var compressedChunk = Compress(chunkFromDisk);
				writeQueue.Enqueue(compressedChunk);
				//Console.WriteLine(writeQueue.Count);
			}

			using (var fileStream = new FileStream(outPath, FileMode.Create))
			{
				var zipChunk = new ZipHeader()
					{BufferSize = bufferSize};
				new BinaryWriter(fileStream).Write(zipChunk.BufferSize);
			}

			while (writeQueue.TryDequeue(out var chank))
			{
				var cnt = 0;
				using (var fileStream = new FileStream(outPath, FileMode.Create))
				{
					var binaryWriter = new BinaryWriter(fileStream);
					var zipChunk = new CompressedChunk()
						{ChunkNumber = cnt++, ChunkSize = chank.Length, DataBytes = chank};
					for (int i = 0; i < chunksCount; i++)
					{
						binaryWriter.Write(zipChunk.ChunkNumber);
						binaryWriter.Write(zipChunk.ChunkSize);
						binaryWriter.Write(zipChunk.DataBytes);
					}
				}
			}
		}

		public static byte[] ReadChunkFromDisk(string filePath, int bufferSize, int chunkNumber)
		{
			using (var output = new MemoryStream())
			{
				var buffer = new byte[bufferSize];
				using (var fileStream = new FileStream(filePath, FileMode.Open))
				{
					var offset = bufferSize * chunkNumber;
					fileStream.Read(buffer, offset, bufferSize);
				}

				return buffer;
			}
		}

		public static byte[] Compress(byte[] data)
		{
			using (var output = new MemoryStream())
			{
				using (var compressStream = new GZipStream(output, CompressionMode.Compress))
				{
					compressStream.Write(data, 0, data.Length);
				}

				return output.ToArray();
			}
		}

		public static byte[] Decompress(byte[] data)
		{
			using (var output = new MemoryStream())
			{
				using (var decompressStream = new GZipStream(output, CompressionMode.Decompress))
				{
					decompressStream.Read(data, 0, data.Length);
				}

				return output.ToArray();
			}
		}
	}
}