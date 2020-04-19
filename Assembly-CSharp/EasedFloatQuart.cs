using System;
using UnityEngine;

internal class EasedFloatQuart : Eased<float>
{
	internal EasedFloatQuart(float startValue) : base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.QuartEaseInOut(Time.time - this.m_startTime, this.m_startValue, this.m_endValue - this.m_startValue, this.m_duration);
	}

	public float EndValue()
	{
		return this.m_endValue;
	}
}
