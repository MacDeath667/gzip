using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace VeeamGZip
{
	public class MultithreadingQueue<T>
	{
		private Queue<T> _baseQueue;

		public MultithreadingQueue()
		{
			_baseQueue = new Queue<T>();
		}

		public void Enqueue(T data)
		{
			lock (_baseQueue)
			{
				while (true)
				{
					if (_baseQueue.Count < 8)
					{
						_baseQueue.Enqueue(data);
						Monitor.Pulse(_baseQueue);
						return;
					}
					Monitor.Wait(_baseQueue);
				}
			}
		}

		public bool TryDequeue(out T data)
		{
			lock (_baseQueue)
			{
				if (_baseQueue.Count > 0)
				{
					data = _baseQueue.Dequeue();
					Monitor.Pulse(_baseQueue);
					return true;
				}

				data = default;
				return false;
			}
		}
	}
}