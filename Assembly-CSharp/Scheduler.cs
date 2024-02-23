using System;
using System.Collections.Generic;

public class Scheduler
{
	private readonly Dictionary<Action, ScheduledTask> m_scheduledTasks = new Dictionary<Action, ScheduledTask>();

	public int Count
	{
		get
		{
			lock (m_scheduledTasks)
			{
				return m_scheduledTasks.Count;
			}
		}
	}

	public void AddTask(Action action, TimeSpan timeSpan, bool isOneShot = true)
	{
		if (!(timeSpan != TimeSpan.Zero))
		{
			return;
		}
		while (true)
		{
			AddTask(action, (int)timeSpan.TotalMilliseconds, isOneShot);
			return;
		}
	}

	public void AddTask(Action action, DateTime datetime, bool isOneShot = true)
	{
		if (DateTime.UtcNow < datetime)
		{
			AddTask(action, (int)(datetime - DateTime.UtcNow).TotalMilliseconds, isOneShot);
		}
	}

	public void AddTask(Action action, int timeoutMs, bool isOneShot = true)
	{
		lock (m_scheduledTasks)
		{
			if (!m_scheduledTasks.ContainsKey(action))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						ScheduledTask scheduledTask = new ScheduledTask(action, timeoutMs, isOneShot);
						scheduledTask.TaskComplete = (EventHandler)Delegate.Combine(scheduledTask.TaskComplete, new EventHandler(TaskComplete));
						m_scheduledTasks.Add(action, scheduledTask);
						scheduledTask.Timer.Start();
						return;
					}
					}
				}
			}
		}
	}

	public void RemoveTask(Action action)
	{
		lock (m_scheduledTasks)
		{
			ScheduledTask value;
			if (m_scheduledTasks.TryGetValue(action, out value))
			{
				value.Cancel();
				TaskComplete(value, EventArgs.Empty);
			}
		}
	}

	public void Reset()
	{
		lock (m_scheduledTasks)
		{
			using (Dictionary<Action, ScheduledTask>.ValueCollection.Enumerator enumerator = m_scheduledTasks.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ScheduledTask current = enumerator.Current;
					current.Cancel();
					current.TaskComplete = (EventHandler)Delegate.Remove(current.TaskComplete, new EventHandler(TaskComplete));
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						goto end_IL_0020;
					}
				}
				end_IL_0020:;
			}
			m_scheduledTasks.Clear();
		}
	}

	private void TaskComplete(object sender, EventArgs e)
	{
		lock (m_scheduledTasks)
		{
			ScheduledTask scheduledTask = (ScheduledTask)sender;
			scheduledTask.TaskComplete = (EventHandler)Delegate.Remove(scheduledTask.TaskComplete, new EventHandler(TaskComplete));
			m_scheduledTasks.Remove(scheduledTask.Action);
		}
	}
}
