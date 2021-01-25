using System;
using System.Collections.Generic;
using System.Threading;

namespace ClearSolution
{
	public class QueueProcessor
	{
		private MultithreadingQueue<Chunk> _queue;
		private ManualResetEvent _manualResetEvent;
		
		public QueueProcessor( MultithreadingQueue<Chunk> queue, ManualResetEvent manualResetEvent)
		{
			_queue = queue;
			_manualResetEvent = manualResetEvent;
		}

		private void Dequeue()
		{
			Chunk result;
			while (true)
			{
				var isQueueEmpty =_queue.TryDequeue(out result); //todo условие выхода из процессора (manualreset)
				Console.WriteLine(isQueueEmpty);
				if (isQueueEmpty)
					_manualResetEvent.WaitOne();
			}
		}

		public void Start()
		{
			new Thread(Dequeue).Start();
		}
	}
}