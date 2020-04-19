using System;

namespace TMPro
{
	public struct TMP_XmlTagStack<T>
	{
		public T[] itemStack;

		public int index;

		private int m_capacity;

		private T m_defaultItem;

		private const int k_defaultCapacity = 4;

		public TMP_XmlTagStack(T[] tagStack)
		{
			this.itemStack = tagStack;
			this.m_capacity = tagStack.Length;
			this.index = 0;
			this.m_defaultItem = default(T);
		}

		public void Clear()
		{
			this.index = 0;
		}

		public void SetDefault(T item)
		{
			this.itemStack[0] = item;
			this.index = 1;
		}

		public void Add(T item)
		{
			if (this.index < this.itemStack.Length)
			{
				this.itemStack[this.index] = item;
				this.index++;
			}
		}

		public T Remove()
		{
			this.index--;
			if (this.index <= 0)
			{
				this.index = 1;
				return this.itemStack[0];
			}
			return this.itemStack[this.index - 1];
		}

		public void Push(T item)
		{
			if (this.index == this.m_capacity)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_XmlTagStack.Push(T)).MethodHandle;
				}
				this.m_capacity *= 2;
				if (this.m_capacity == 0)
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
					this.m_capacity = 4;
				}
				Array.Resize<T>(ref this.itemStack, this.m_capacity);
			}
			this.itemStack[this.index] = item;
			this.index++;
		}

		public T Pop()
		{
			if (this.index == 0)
			{
				return default(T);
			}
			this.index--;
			T result = this.itemStack[this.index];
			this.itemStack[this.index] = this.m_defaultItem;
			return result;
		}

		public T CurrentItem()
		{
			if (this.index > 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_XmlTagStack.CurrentItem()).MethodHandle;
				}
				return this.itemStack[this.index - 1];
			}
			return this.itemStack[0];
		}

		public T PreviousItem()
		{
			if (this.index > 1)
			{
				return this.itemStack[this.index - 2];
			}
			return this.itemStack[0];
		}
	}
}
