using UnityEngine;

internal abstract class Eased<T>
{
	protected T m_startValue;

	protected T m_endValue;

	protected float m_duration;

	protected float m_startTime;

	internal Eased(T startValue)
	{
		m_startValue = startValue;
		m_endValue = startValue;
		m_duration = 1f;
	}

	internal void EaseTo(T endValue, float duration = 0.3f)
	{
		if (endValue.Equals(m_endValue))
		{
			return;
		}
		while (true)
		{
			m_startValue = CalcValue();
			m_endValue = endValue;
			m_startTime = Time.time;
			m_duration = Mathf.Max(duration, 0.0166666675f);
			return;
		}
	}

	internal bool EaseFinished()
	{
		return Time.time - m_startTime >= m_duration;
	}

	internal float CalcTime()
	{
		return Mathf.Max(0f, Time.time - m_startTime);
	}

	internal float CalcTimeRemaining()
	{
		return Mathf.Max(0f, m_startTime + m_duration - Time.time);
	}

	public static implicit operator T(Eased<T> ev)
	{
		return (!ev.EaseFinished()) ? ev.CalcValue() : ev.GetEndValue();
	}

	public override string ToString()
	{
		return CalcValue().ToString();
	}

	internal T GetEndValue()
	{
		return m_endValue;
	}

	protected abstract T CalcValue();
}
