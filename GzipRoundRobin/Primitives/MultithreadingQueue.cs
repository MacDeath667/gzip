﻿using System.Collections.Generic;
using System.Threading;

namespace GzipRoundRobin.Primitives
{
	public class MultithreadingQueue<T>
	{
		private readonly Queue<T> _baseQueue;
		private readonly int _queueLimit;

		public MultithreadingQueue(int queueLimit)
		{
			_baseQueue = new Queue<T>();
			_queueLimit = queueLimit;
		}

		public bool IsEmpty =>
			_baseQueue.Count == 0;


		public void Enqueue(T data)
		{
			lock (_baseQueue)
			{
				while (true)
				{
					if (_baseQueue.Count < _queueLimit)
					{
						_baseQueue.Enqueue(data);
						return;
					}

					Monitor.Wait(_baseQueue);
				}
			}
		}

		public bool TryDequeue(out T data)
		{
			try
			{
				var isLocked = Monitor.TryEnter(_baseQueue, 150);
				if (isLocked && _baseQueue.Count > 0)
				{
					data = _baseQueue.Dequeue();
					Monitor.PulseAll(_baseQueue);
					return true;
				}

				data = default;
				return false;
			}
			finally
			{
				if (Monitor.IsEntered(_baseQueue))
				{
					Monitor.Exit(_baseQueue);
				}
			}
		}
	}
}