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
			itemStack = tagStack;
			m_capacity = tagStack.Length;
			index = 0;
			m_defaultItem = default(T);
		}

		public void Clear()
		{
			index = 0;
		}

		public void SetDefault(T item)
		{
			itemStack[0] = item;
			index = 1;
		}

		public void Add(T item)
		{
			if (index < itemStack.Length)
			{
				itemStack[index] = item;
				index++;
			}
		}

		public T Remove()
		{
			index--;
			if (index <= 0)
			{
				index = 1;
				return itemStack[0];
			}
			return itemStack[index - 1];
		}

		public void Push(T item)
		{
			if (index == m_capacity)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_capacity *= 2;
				if (m_capacity == 0)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					m_capacity = 4;
				}
				Array.Resize(ref itemStack, m_capacity);
			}
			itemStack[index] = item;
			index++;
		}

		public T Pop()
		{
			if (index == 0)
			{
				return default(T);
			}
			index--;
			T result = itemStack[index];
			itemStack[index] = m_defaultItem;
			return result;
		}

		public T CurrentItem()
		{
			if (index > 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return itemStack[index - 1];
					}
				}
			}
			return itemStack[0];
		}

		public T PreviousItem()
		{
			if (index > 1)
			{
				return itemStack[index - 2];
			}
			return itemStack[0];
		}
	}
}
