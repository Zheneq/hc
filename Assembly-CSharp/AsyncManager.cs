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
		int num = ticket = lastTicket++;
		Coroutine value = StartCoroutine(InternalRoutine(coroutine, num, delay));
		runningCoroutines.Add(num, value);
	}

	private IEnumerator InternalRoutine(IEnumerator coroutine, int ticket, float delay = 0f)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		if (!coroutine.MoveNext())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					doneTickets.Add(ticket);
					yield break;
				}
			}
		}
		yield return coroutine.Current;
		/*Error: Unable to find new state assignment for yield return*/;
	}

	public void CancelAsyncOperation(int ticket)
	{
		if (runningCoroutines.TryGetValue(ticket, out Coroutine value))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					StopCoroutine(value);
					doneTickets.Add(ticket);
					return;
				}
			}
		}
		Log.Error("Failed to find async operation for ticket #" + ticket);
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
				int current = enumerator.Current;
				runningCoroutines.Remove(current);
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					goto end_IL_000e;
				}
			}
			end_IL_000e:;
		}
		doneTickets.Clear();
	}
}
