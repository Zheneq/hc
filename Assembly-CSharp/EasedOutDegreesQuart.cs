using System;
using UnityEngine;

internal class EasedOutDegreesQuart : Eased<float>
{
	internal EasedOutDegreesQuart(float startValue) : base(startValue)
	{
	}

	protected override float CalcValue()
	{
		float value = Easing.QuartEaseOut(Time.time - this.m_startTime, 0f, 1f, this.m_duration);
		return Mathf.LerpAngle(this.m_startValue, this.m_endValue, Mathf.Clamp01(value));
	}

	public float EndValue()
	{
		return this.m_endValue;
	}
}
