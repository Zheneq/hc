using UnityEngine;

internal class EasedOutFloatQuad : Eased<float>
{
	internal EasedOutFloatQuad(float startValue)
		: base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.QuadEaseOut(Time.time - m_startTime, m_startValue, m_endValue - m_startValue, m_duration);
	}
}
