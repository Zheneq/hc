using System;
using UnityEngine;

internal class EasedFloatCubic : Eased<float>
{
	internal EasedFloatCubic(float startValue) : base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.CubicEaseInOut(Time.time - this.m_startTime, this.m_startValue, this.m_endValue - this.m_startValue, this.m_duration);
	}
}
