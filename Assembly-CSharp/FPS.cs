using System;
using System.Threading;
using UnityEngine;

public class FPS
{
	private const float c_updateInterval = 0.5f;

	private float m_timeLeft;

	internal FPS()
	{
		if (FPS.<>f__am$cache0 == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FPS..ctor()).MethodHandle;
			}
			FPS.<>f__am$cache0 = delegate(float A_0)
			{
			};
		}
		this.m_OnFPSChange = FPS.<>f__am$cache0;
		this.m_timeLeft = 0.5f;
		base..ctor();
	}

	internal FPS(Action<float> onChange)
	{
		if (FPS.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FPS..ctor(Action<float>)).MethodHandle;
			}
			FPS.<>f__am$cache0 = delegate(float A_0)
			{
			};
		}
		this.m_OnFPSChange = FPS.<>f__am$cache0;
		this.m_timeLeft = 0.5f;
		base..ctor();
		this.m_OnFPSChange += onChange;
	}

	private event Action<float> m_OnFPSChange
	{
		add
		{
			Action<float> action = this.m_OnFPSChange;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<float>>(ref this.m_OnFPSChange, (Action<float>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<float> action = this.m_OnFPSChange;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<float>>(ref this.m_OnFPSChange, (Action<float>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FPS.remove_m_OnFPSChange(Action<float>)).MethodHandle;
			}
		}
	}

	internal int NumSampledFrames { get; private set; }

	internal float TimeElapsed { get; private set; }

	internal float CalcForSampledFrames()
	{
		return (this.NumSampledFrames != 0) ? (this.TimeElapsed / (float)this.NumSampledFrames) : 0f;
	}

	internal void SampleFrame()
	{
		this.m_timeLeft -= Time.deltaTime;
		this.TimeElapsed += Time.timeScale / Time.deltaTime;
		this.NumSampledFrames++;
		if (this.m_OnFPSChange != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FPS.SampleFrame()).MethodHandle;
			}
			if ((double)this.m_timeLeft <= 0.0)
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
				float obj = this.CalcForSampledFrames();
				this.m_OnFPSChange(obj);
				this.m_timeLeft = 0.5f;
				this.TimeElapsed = 0f;
				this.NumSampledFrames = 0;
			}
		}
	}
}
