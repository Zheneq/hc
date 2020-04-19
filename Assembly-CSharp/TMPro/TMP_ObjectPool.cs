using System;
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

		public TMP_ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
		{
			this.m_ActionOnGet = actionOnGet;
			this.m_ActionOnRelease = actionOnRelease;
		}

		public int countAll { get; private set; }

		public int countActive
		{
			get
			{
				return this.countAll - this.countInactive;
			}
		}

		public int countInactive
		{
			get
			{
				return this.m_Stack.Count;
			}
		}

		public T Get()
		{
			T t;
			if (this.m_Stack.Count == 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_ObjectPool.Get()).MethodHandle;
				}
				t = Activator.CreateInstance<T>();
				this.countAll++;
			}
			else
			{
				t = this.m_Stack.Pop();
			}
			if (this.m_ActionOnGet != null)
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
				this.m_ActionOnGet(t);
			}
			return t;
		}

		public void Release(T element)
		{
			if (this.m_Stack.Count > 0 && object.ReferenceEquals(this.m_Stack.Peek(), element))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_ObjectPool.Release(T)).MethodHandle;
				}
				Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
			}
			if (this.m_ActionOnRelease != null)
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
				this.m_ActionOnRelease(element);
			}
			this.m_Stack.Push(element);
		}
	}
}
