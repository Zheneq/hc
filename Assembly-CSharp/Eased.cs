using System;
using UnityEngine;

internal abstract class Eased<T>
{
	protected T m_startValue;

	protected T m_endValue;

	protected float m_duration;

	protected float m_startTime;

	internal Eased(T startValue)
	{
		this.m_startValue = startValue;
		this.m_endValue = startValue;
		this.m_duration = 1f;
	}

	internal void EaseTo(T endValue, float duration = 0.3f)
	{
		if (!endValue.Equals(this.m_endValue))
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Eased.EaseTo(T, float)).MethodHandle;
			}
			this.m_startValue = this.CalcValue();
			this.m_endValue = endValue;
			this.m_startTime = Time.time;
			this.m_duration = Mathf.Max(duration, 0.0166666675f);
		}
	}

	internal bool EaseFinished()
	{
		return Time.time - this.m_startTime >= this.m_duration;
	}

	internal float CalcTime()
	{
		return Mathf.Max(0f, Time.time - this.m_startTime);
	}

	internal float CalcTimeRemaining()
	{
		return Mathf.Max(0f, this.m_startTime + this.m_duration - Time.time);
	}

	public static implicit operator T(Eased<T> ev)
	{
		return (!ev.EaseFinished()) ? ev.CalcValue() : ev.GetEndValue();
	}

	public override string ToString()
	{
		T t = this.CalcValue();
		return t.ToString();
	}

	internal T GetEndValue()
	{
		return this.m_endValue;
	}

	protected abstract T CalcValue();
}
