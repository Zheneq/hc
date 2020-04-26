using UnityEngine;

internal class EasedOutFloatQuart : Eased<float>
{
	internal EasedOutFloatQuart(float startValue)
		: base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.QuartEaseOut(Time.time - m_startTime, m_startValue, m_endValue - m_startValue, m_duration);
	}
}
