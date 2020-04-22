using UnityEngine;

internal class EasedInFloatQuad : Eased<float>
{
	internal EasedInFloatQuad(float startValue)
		: base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.QuadEaseIn(Time.time - m_startTime, m_startValue, m_endValue - m_startValue, m_duration);
	}
}
