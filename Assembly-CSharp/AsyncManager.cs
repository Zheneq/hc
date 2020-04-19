using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncManager : MonoBehaviour
{
	private static AsyncManager s_instance;

	public static int lastTicket;

	public const int INVALID_TICKET = -1;

	private Dictionary<int, Coroutine> runningCoroutines = new Dictionary<int, Coroutine>();

	private List<int> doneTickets = new List<int>();

	public static AsyncManager Get()
	{
		if (AsyncManager.s_instance == null)
		{
			Log.Error("AsyncManager component is not present on a bootstrap singleton!", new object[0]);
		}
		return AsyncManager.s_instance;
	}

	public void StartAsyncOperation(out int ticket, IEnumerator coroutine, float delay = 0f)
	{
		int num = AsyncManager.lastTicket++;
		ticket = num;
		Coroutine value = base.StartCoroutine(this.InternalRoutine(coroutine, num, delay));
		this.runningCoroutines.Add(num, value);
	}

	private IEnumerator InternalRoutine(IEnumerator coroutine, int ticket, float delay = 0f)
	{
		if (delay > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AsyncManager.<InternalRoutine>c__Iterator0.MoveNext()).MethodHandle;
			}
			yield return new WaitForSeconds(delay);
		}
		while (coroutine.MoveNext())
		{
			object obj = coroutine.Current;
			yield return obj;
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		this.doneTickets.Add(ticket);
		yield break;
		yield break;
	}

	public void CancelAsyncOperation(int ticket)
	{
		Coroutine routine;
		if (this.runningCoroutines.TryGetValue(ticket, out routine))
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AsyncManager.CancelAsyncOperation(int)).MethodHandle;
			}
			base.StopCoroutine(routine);
			this.doneTickets.Add(ticket);
		}
		else
		{
			Log.Error("Failed to find async operation for ticket #" + ticket, new object[0]);
		}
	}

	private void Awake()
	{
		AsyncManager.s_instance = this;
	}

	private void OnDestroy()
	{
		AsyncManager.s_instance = null;
	}

	private void Update()
	{
		using (List<int>.Enumerator enumerator = this.doneTickets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int key = enumerator.Current;
				this.runningCoroutines.Remove(key);
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AsyncManager.Update()).MethodHandle;
			}
		}
		this.doneTickets.Clear();
	}
}
