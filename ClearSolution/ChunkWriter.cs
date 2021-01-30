using System;
using System.IO;
using System.Threading;

namespace ClearSolution
{
	public abstract class ChunkWriter<T>
	{
		private static int count=0;
		private MultithreadingQueue<Chunk> _queue;
		private CountdownEvent _countdownEvent;

		public ChunkWriter(MultithreadingQueue<Chunk> queue, CountdownEvent countdownEvent)
		{
			_queue = queue;
			_countdownEvent = countdownEvent;
		}

		public void Start(string filepath)
		{
			
			new Thread(() => WriteChunks(filepath)).Start();
		}
		public void StartInSuchThread(string filepath)
		{
			
			new Thread(() => WriteChunks(filepath)).Start();
		}

		private void WriteChunks(string filepath)
		{
			using (var filestream = File.OpenWrite(filepath))
			{
				while (!_countdownEvent.IsSet || !_queue.IsEmpty)
				{
					if (_queue.TryDequeue(out var chunk))
					{
						var binaryWriter = new BinaryWriter(filestream);
						binaryWriter.Write(chunk.Size);
						binaryWriter.Write(chunk.Data);
						//Thread.Sleep(150);
						//Console.WriteLine( " - write bytes");
					}
				}
				
				Console.WriteLine("Write done");
				Console.WriteLine("Write done");
			}
		}
	}
}