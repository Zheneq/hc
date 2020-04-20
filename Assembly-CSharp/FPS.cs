using System;
using System.Threading;
using UnityEngine;

public class FPS
{
	private const float c_updateInterval = 0.5f;

	private float m_timeLeft;

	internal FPS()
	{
		
		this.m_OnFPSChange = delegate(float A_0)
			{
			};
		this.m_timeLeft = 0.5f;
		
	}

	internal FPS(Action<float> onChange)
	{
		
		this.m_OnFPSChange = delegate(float A_0)
			{
			};
		this.m_timeLeft = 0.5f;
		
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
			if ((double)this.m_timeLeft <= 0.0)
			{
				float obj = this.CalcForSampledFrames();
				this.m_OnFPSChange(obj);
				this.m_timeLeft = 0.5f;
				this.TimeElapsed = 0f;
				this.NumSampledFrames = 0;
			}
		}
	}
}
