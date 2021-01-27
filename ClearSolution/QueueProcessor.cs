using System;
using System.Threading;

namespace ClearSolution
{
	public class QueueProcessor
	{
		private MultithreadingQueue<Chunk> _queue;
		private ManualResetEventSlim _inputFinished;
		
		public QueueProcessor( MultithreadingQueue<Chunk> queue, ManualResetEventSlim inputFinished)
		{
			_queue = queue;
			_inputFinished = inputFinished;
		}

		private void Dequeue()
		{
			Chunk result;
			bool successReading;

			while (!_inputFinished.IsSet)
			{
				if (_queue.IsEmpty)
				{
					continue;
				}
				successReading =_queue.TryDequeue(out result); //todo условие выхода из процессора (manualreset)
				Console.WriteLine(successReading);
			}
			Console.WriteLine("ReadingDone");
			Console.Beep(880, 600);
		}

		public void Start()
		{
			new Thread(Dequeue).Start();
		}
	}
}