using UnityEngine;

internal class EasedOutDegreesQuart : Eased<float>
{
	internal EasedOutDegreesQuart(float startValue)
		: base(startValue)
	{
	}

	protected override float CalcValue()
	{
		float value = Easing.QuartEaseOut(Time.time - m_startTime, 0f, 1f, m_duration);
		return Mathf.LerpAngle(m_startValue, m_endValue, Mathf.Clamp01(value));
	}

	public float EndValue()
	{
		return m_endValue;
	}
}
