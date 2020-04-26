using UnityEngine;

internal class EasedFloat : Eased<float>
{
	internal EasedFloat(float startValue)
		: base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.ExpoEaseInOut(Time.time - m_startTime, m_startValue, m_endValue - m_startValue, m_duration);
	}

	public float EndValue()
	{
		return m_endValue;
	}
}
