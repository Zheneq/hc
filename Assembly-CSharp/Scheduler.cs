using System;
using System.Collections.Generic;

public class Scheduler
{
	private readonly Dictionary<Action, ScheduledTask> m_scheduledTasks = new Dictionary<Action, ScheduledTask>();

	public int Count
	{
		get
		{
			object scheduledTasks = this.m_scheduledTasks;
			int count;
			lock (scheduledTasks)
			{
				count = this.m_scheduledTasks.Count;
			}
			return count;
		}
	}

	public void AddTask(Action action, TimeSpan timeSpan, bool isOneShot = true)
	{
		if (timeSpan != TimeSpan.Zero)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Scheduler.AddTask(Action, TimeSpan, bool)).MethodHandle;
			}
			this.AddTask(action, (int)timeSpan.TotalMilliseconds, isOneShot);
		}
	}

	public void AddTask(Action action, DateTime datetime, bool isOneShot = true)
	{
		if (DateTime.UtcNow < datetime)
		{
			this.AddTask(action, (int)(datetime - DateTime.UtcNow).TotalMilliseconds, isOneShot);
		}
	}

	public void AddTask(Action action, int timeoutMs, bool isOneShot = true)
	{
		object scheduledTasks = this.m_scheduledTasks;
		lock (scheduledTasks)
		{
			if (!this.m_scheduledTasks.ContainsKey(action))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Scheduler.AddTask(Action, int, bool)).MethodHandle;
				}
				ScheduledTask scheduledTask = new ScheduledTask(action, timeoutMs, isOneShot);
				ScheduledTask scheduledTask2 = scheduledTask;
				scheduledTask2.TaskComplete = (EventHandler)Delegate.Combine(scheduledTask2.TaskComplete, new EventHandler(this.TaskComplete));
				this.m_scheduledTasks.Add(action, scheduledTask);
				scheduledTask.Timer.Start();
			}
		}
	}

	public void RemoveTask(Action action)
	{
		object scheduledTasks = this.m_scheduledTasks;
		lock (scheduledTasks)
		{
			ScheduledTask scheduledTask;
			if (this.m_scheduledTasks.TryGetValue(action, out scheduledTask))
			{
				scheduledTask.Cancel();
				this.TaskComplete(scheduledTask, EventArgs.Empty);
			}
		}
	}

	public void Reset()
	{
		object scheduledTasks = this.m_scheduledTasks;
		lock (scheduledTasks)
		{
			using (Dictionary<Action, ScheduledTask>.ValueCollection.Enumerator enumerator = this.m_scheduledTasks.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ScheduledTask scheduledTask = enumerator.Current;
					scheduledTask.Cancel();
					ScheduledTask scheduledTask2 = scheduledTask;
					scheduledTask2.TaskComplete = (EventHandler)Delegate.Remove(scheduledTask2.TaskComplete, new EventHandler(this.TaskComplete));
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Scheduler.Reset()).MethodHandle;
				}
			}
			this.m_scheduledTasks.Clear();
		}
	}

	private void TaskComplete(object sender, EventArgs e)
	{
		object scheduledTasks = this.m_scheduledTasks;
		lock (scheduledTasks)
		{
			ScheduledTask scheduledTask = (ScheduledTask)sender;
			ScheduledTask scheduledTask2 = scheduledTask;
			scheduledTask2.TaskComplete = (EventHandler)Delegate.Remove(scheduledTask2.TaskComplete, new EventHandler(this.TaskComplete));
			this.m_scheduledTasks.Remove(scheduledTask.Action);
		}
	}
}
