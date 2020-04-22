using System;
using System.Timers;

public class ScheduledTask
{
	internal readonly Action Action;

	internal Timer Timer;

	internal EventHandler TaskComplete;

	internal bool IsOneShot;

	public ScheduledTask(Action action, TimeSpan timeSpan, bool isOneShot)
		: this(action, (int)timeSpan.TotalMilliseconds, isOneShot)
	{
	}

	public ScheduledTask(Action action, int timeoutMs, bool isOneShot)
	{
		Action = action;
		Timer = new Timer
		{
			Interval = timeoutMs
		};
		Timer.Elapsed += TimerElapsed;
		IsOneShot = isOneShot;
	}

	private void TimerElapsed(object sender, ElapsedEventArgs e)
	{
		Timer.Stop();
		if (IsOneShot)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Timer.Elapsed -= TimerElapsed;
					Timer = null;
					if (TaskComplete != null)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								TaskComplete(this, EventArgs.Empty);
								Action();
								return;
							}
						}
					}
					return;
				}
			}
		}
		Action();
		Timer.Start();
	}

	public void Cancel()
	{
		Timer.Stop();
		Timer.Elapsed -= TimerElapsed;
		Timer = null;
	}
}
