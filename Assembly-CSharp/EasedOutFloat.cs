using UnityEngine;

internal class EasedOutFloat : Eased<float>
{
	internal EasedOutFloat(float startValue)
		: base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.ExpoEaseOut(Time.time - m_startTime, m_startValue, m_endValue - m_startValue, m_duration);
	}
}
