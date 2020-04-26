using UnityEngine;

internal class EasedInFloatCubic : Eased<float>
{
	internal EasedInFloatCubic(float startValue)
		: base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.CubicEaseIn(Time.time - m_startTime, m_startValue, m_endValue - m_startValue, m_duration);
	}
}
