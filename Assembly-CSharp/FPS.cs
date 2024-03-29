using System;
using System.Threading;
using UnityEngine;

public class FPS
{
	private const float c_updateInterval = 0.5f;

	private float m_timeLeft;

	internal int NumSampledFrames
	{
		get;
		private set;
	}

	internal float TimeElapsed
	{
		get;
		private set;
	}

	private Action<float> m_OnFPSChangeHolder;
	private event Action<float> m_OnFPSChange
	{
		add
		{
			Action<float> action = this.m_OnFPSChangeHolder;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnFPSChangeHolder, (Action<float>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<float> action = this.m_OnFPSChangeHolder;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnFPSChangeHolder, (Action<float>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	internal FPS()
	{
		
		this.m_OnFPSChangeHolder = delegate
			{
			};
		m_timeLeft = 0.5f;
		
	}

	internal FPS(Action<float> onChange)
	{
		
		this.m_OnFPSChangeHolder = delegate
			{
			};
		m_timeLeft = 0.5f;
		
		m_OnFPSChange += onChange;
	}

	internal float CalcForSampledFrames()
	{
		return (NumSampledFrames != 0) ? (TimeElapsed / (float)NumSampledFrames) : 0f;
	}

	internal void SampleFrame()
	{
		m_timeLeft -= Time.deltaTime;
		TimeElapsed += Time.timeScale / Time.deltaTime;
		NumSampledFrames++;
		if (this.m_OnFPSChangeHolder == null)
		{
			return;
		}
		while (true)
		{
			if ((double)m_timeLeft <= 0.0)
			{
				while (true)
				{
					float obj = CalcForSampledFrames();
					this.m_OnFPSChangeHolder(obj);
					m_timeLeft = 0.5f;
					TimeElapsed = 0f;
					NumSampledFrames = 0;
					return;
				}
			}
			return;
		}
	}
}
