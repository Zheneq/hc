using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro
{
	internal class TMP_ObjectPool<T> where T : new()
	{
		private readonly Stack<T> m_Stack = new Stack<T>();

		private readonly UnityAction<T> m_ActionOnGet;

		private readonly UnityAction<T> m_ActionOnRelease;

		public int countAll
		{
			get;
			private set;
		}

		public int countActive => countAll - countInactive;

		public int countInactive => m_Stack.Count;

		public TMP_ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
		{
			m_ActionOnGet = actionOnGet;
			m_ActionOnRelease = actionOnRelease;
		}

		public T Get()
		{
			T val;
			if (m_Stack.Count == 0)
			{
				while (true)
				{
					switch (3)
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
				val = new T();
				countAll++;
			}
			else
			{
				val = m_Stack.Pop();
			}
			if (m_ActionOnGet != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				m_ActionOnGet(val);
			}
			return val;
		}

		public void Release(T element)
		{
			if (m_Stack.Count > 0 && object.ReferenceEquals(m_Stack.Peek(), element))
			{
				while (true)
				{
					switch (1)
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
				Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
			}
			if (m_ActionOnRelease != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				m_ActionOnRelease(element);
			}
			m_Stack.Push(element);
		}
	}
}
