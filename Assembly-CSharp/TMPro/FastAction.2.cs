using System;
using System.Collections.Generic;

namespace TMPro
{
	public class FastAction<A>
	{
		private LinkedList<Action<A>> delegates = new LinkedList<Action<A>>();

		private Dictionary<Action<A>, LinkedListNode<Action<A>>> lookup = new Dictionary<Action<A>, LinkedListNode<Action<A>>>();

		public void Add(Action<A> rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FastAction.Add(Action<A>)).MethodHandle;
				}
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		public void Remove(Action<A> rhs)
		{
			LinkedListNode<Action<A>> node;
			if (this.lookup.TryGetValue(rhs, out node))
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FastAction.Remove(Action<A>)).MethodHandle;
				}
				this.lookup.Remove(rhs);
				this.delegates.Remove(node);
			}
		}

		public void Call(A a)
		{
			for (LinkedListNode<Action<A>> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value(a);
			}
		}
	}
}
