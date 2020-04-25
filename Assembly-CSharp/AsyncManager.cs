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
		if (s_instance == null)
		{
			Log.Error("AsyncManager component is not present on a bootstrap singleton!");
		}
		return s_instance;
	}

	public void StartAsyncOperation(out int ticket, IEnumerator coroutine, float delay = 0f)
	{
		int num = lastTicket++;
		ticket = num;
		Coroutine value = StartCoroutine(InternalRoutine(coroutine, num, delay));
		runningCoroutines.Add(num, value);
	}

	private IEnumerator InternalRoutine(IEnumerator coroutine, int ticket, float delay = 0f)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		while (coroutine.MoveNext())
		{
			yield return coroutine.Current;
		}
		this.doneTickets.Add(ticket);
		yield break;
	}

	public void CancelAsyncOperation(int ticket)
	{
		if (runningCoroutines.TryGetValue(ticket, out Coroutine routine))
		{
			StopCoroutine(routine);
			doneTickets.Add(ticket);
		}
		else
		{
			Log.Error("Failed to find async operation for ticket #" + ticket);
		}
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Update()
	{
		using (List<int>.Enumerator enumerator = doneTickets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int ticket = enumerator.Current;
				runningCoroutines.Remove(ticket);
			}
		}
		doneTickets.Clear();
	}
}
