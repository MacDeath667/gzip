using System;
using System.IO;
using System.Threading;

namespace ClearSolution
{
	public abstract class ChunkWriter<T>
	{
		private static int count=0;
		private MultithreadingQueue<T> _queue;
		private ManualResetEventSlim _manualResetEvent;

		public ChunkWriter(MultithreadingQueue<T> queue, ManualResetEventSlim manualResetEvent)
		{
			_queue = queue;
			_manualResetEvent = manualResetEvent;
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
			_manualResetEvent.Wait();
			using (var filestream = File.OpenWrite(filepath))
			{
				while (_manualResetEvent.IsSet || !_queue.IsEmpty)
				{
					if (_queue.TryDequeue(out var chunk))
					{
						Console.WriteLine($"writer count = {++count}");
						//Console.WriteLine( " - write bytes");
					}
				}
				Console.WriteLine("Write done");
			}
		}
	}
}