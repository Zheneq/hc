using UnityEngine;

internal class EasedFloatCubic : Eased<float>
{
	internal EasedFloatCubic(float startValue)
		: base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.CubicEaseInOut(Time.time - m_startTime, m_startValue, m_endValue - m_startValue, m_duration);
	}
}
