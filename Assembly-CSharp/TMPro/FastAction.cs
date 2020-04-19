using System;
using System.Collections.Generic;

namespace TMPro
{
	public class FastAction
	{
		private LinkedList<Action> delegates = new LinkedList<Action>();

		private Dictionary<Action, LinkedListNode<Action>> lookup = new Dictionary<Action, LinkedListNode<Action>>();

		public void Add(Action rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FastAction.Add(Action)).MethodHandle;
				}
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		public void Remove(Action rhs)
		{
			LinkedListNode<Action> node;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FastAction.Remove(Action)).MethodHandle;
				}
				this.lookup.Remove(rhs);
				this.delegates.Remove(node);
			}
		}

		public void Call()
		{
			for (LinkedListNode<Action> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value();
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FastAction.Call()).MethodHandle;
			}
		}
	}
}
