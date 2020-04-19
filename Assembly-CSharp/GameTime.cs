using System;
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
			GameTime.ResyncTime();
			return GameTime.s_time;
		}
	}

	public static float deltaTime
	{
		get
		{
			GameTime.ResyncTime();
			return GameTime.s_timeDelta;
		}
	}

	public static float smoothDeltaTime
	{
		get
		{
			GameTime.ResyncTime();
			return GameTime.s_timeSmoothDelta;
		}
	}

	public static float scale
	{
		get
		{
			return GameTime.s_scale;
		}
		set
		{
			if (GameTime.s_scale != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameTime.set_scale(float)).MethodHandle;
				}
				GameTime.s_scale = value;
				GameEventManager.Get().FireEvent(GameEventManager.EventType.GametimeScaleChange, null);
			}
		}
	}

	private static void ResyncTime()
	{
		if (!GameTime.s_initted)
		{
			GameTime.s_time = Time.time;
			GameTime.s_timeDelta = Time.deltaTime;
			GameTime.s_timeSmoothDelta = Time.smoothDeltaTime;
			GameTime.s_timeLastSeen = Time.time;
			GameTime.s_initted = true;
			GameTime.scale = 1f;
		}
		else if (GameTime.s_timeLastSeen != Time.time)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameTime.ResyncTime()).MethodHandle;
			}
			GameTime.s_time += (Time.time - GameTime.s_timeLastSeen) * GameTime.scale;
			GameTime.s_timeDelta = Time.deltaTime * GameTime.scale;
			GameTime.s_timeSmoothDelta = Time.smoothDeltaTime * GameTime.scale;
			GameTime.s_timeLastSeen = Time.time;
		}
	}
}
