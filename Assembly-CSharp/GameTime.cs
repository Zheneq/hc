using UnityEngine;

public static class GameTime
{
	private static float s_time;

	private static float s_timeDelta;

	private static float s_timeSmoothDelta;

	private static float s_scale = 1f;

	private static float s_timeLastSeen;

	private static bool s_initted;

	public static float time
	{
		get
		{
			ResyncTime();
			return s_time;
		}
	}

	public static float deltaTime
	{
		get
		{
			ResyncTime();
			return s_timeDelta;
		}
	}

	public static float smoothDeltaTime
	{
		get
		{
			ResyncTime();
			return s_timeSmoothDelta;
		}
	}

	public static float scale
	{
		get
		{
			return s_scale;
		}
		set
		{
			if (s_scale == value)
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				s_scale = value;
				GameEventManager.Get().FireEvent(GameEventManager.EventType.GametimeScaleChange, null);
				return;
			}
		}
	}

	private static void ResyncTime()
	{
		if (!s_initted)
		{
			s_time = Time.time;
			s_timeDelta = Time.deltaTime;
			s_timeSmoothDelta = Time.smoothDeltaTime;
			s_timeLastSeen = Time.time;
			s_initted = true;
			scale = 1f;
		}
		else
		{
			if (s_timeLastSeen == Time.time)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				s_time += (Time.time - s_timeLastSeen) * scale;
				s_timeDelta = Time.deltaTime * scale;
				s_timeSmoothDelta = Time.smoothDeltaTime * scale;
				s_timeLastSeen = Time.time;
				return;
			}
		}
	}
}
