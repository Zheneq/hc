using System;
using System.Collections.Generic;

namespace TMPro
{
	public class FastAction<A, B, C>
	{
		private LinkedList<Action<A, B, C>> delegates = new LinkedList<Action<A, B, C>>();

		private Dictionary<Action<A, B, C>, LinkedListNode<Action<A, B, C>>> lookup = new Dictionary<Action<A, B, C>, LinkedListNode<Action<A, B, C>>>();

		public void Add(Action<A, B, C> rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FastAction.Add(Action<A, B, C>)).MethodHandle;
				}
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		public void Remove(Action<A, B, C> rhs)
		{
			LinkedListNode<Action<A, B, C>> node;
			if (this.lookup.TryGetValue(rhs, out node))
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FastAction.Remove(Action<A, B, C>)).MethodHandle;
				}
				this.lookup.Remove(rhs);
				this.delegates.Remove(node);
			}
		}

		public void Call(A a, B b, C c)
		{
			for (LinkedListNode<Action<A, B, C>> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value(a, b, c);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FastAction.Call(A, B, C)).MethodHandle;
			}
		}
	}
}
