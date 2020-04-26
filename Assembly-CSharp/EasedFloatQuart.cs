using UnityEngine;

internal class EasedFloatQuart : Eased<float>
{
	internal EasedFloatQuart(float startValue)
		: base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.QuartEaseInOut(Time.time - m_startTime, m_startValue, m_endValue - m_startValue, m_duration);
	}

	public float EndValue()
	{
		return m_endValue;
	}
}
