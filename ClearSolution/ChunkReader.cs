using System;
using System.IO;
using System.Threading;

namespace ClearSolution
{
	public abstract class ChunkReader<T>
	{
		public bool IsFileEnd { get; private set; }
		private MultithreadingQueue<T> _queue;
		private ManualResetEventSlim _manualResetEvent;

		public ChunkReader(MultithreadingQueue<T> queue, ManualResetEventSlim manualResetEvent)
		{
			_queue = queue;
			_manualResetEvent = manualResetEvent;
		}

		public void Start(string filepath)
		{
			
			new Thread(() => ReadChunks(filepath)).Start();
		}

		private void ReadChunks(string filepath)
		{
			using (var filestream = File.Open(filepath, FileMode.Open))
			{
				var buffer = new byte[2 * 1024 * 1024];
				var readBytes = 0;
				
				while ((readBytes = filestream.Read(buffer)) > 0)
				{
					_queue.Enqueue(CreateChunk(buffer.Clone() as byte[], readBytes));
					Console.WriteLine(readBytes);
				}
				Console.WriteLine("File end");
				_manualResetEvent.Set();
			}
		}


		protected abstract T CreateChunk(byte[] buffer, int size);
	}
}