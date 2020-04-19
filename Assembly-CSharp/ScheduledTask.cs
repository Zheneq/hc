using System;
using System.Timers;

public class ScheduledTask
{
	internal readonly Action Action;

	internal Timer Timer;

	internal EventHandler TaskComplete;

	internal bool IsOneShot;

	public ScheduledTask(Action action, TimeSpan timeSpan, bool isOneShot) : this(action, (int)timeSpan.TotalMilliseconds, isOneShot)
	{
	}

	public ScheduledTask(Action action, int timeoutMs, bool isOneShot)
	{
		this.Action = action;
		this.Timer = new Timer
		{
			Interval = (double)timeoutMs
		};
		this.Timer.Elapsed += this.TimerElapsed;
		this.IsOneShot = isOneShot;
	}

	private void TimerElapsed(object sender, ElapsedEventArgs e)
	{
		this.Timer.Stop();
		if (this.IsOneShot)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScheduledTask.TimerElapsed(object, ElapsedEventArgs)).MethodHandle;
			}
			this.Timer.Elapsed -= this.TimerElapsed;
			this.Timer = null;
			if (this.TaskComplete != null)
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
				this.TaskComplete(this, EventArgs.Empty);
				this.Action();
			}
		}
		else
		{
			this.Action();
			this.Timer.Start();
		}
	}

	public void Cancel()
	{
		this.Timer.Stop();
		this.Timer.Elapsed -= this.TimerElapsed;
		this.Timer = null;
	}
}
