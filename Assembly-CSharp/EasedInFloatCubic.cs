using System;
using UnityEngine;

internal class EasedInFloatCubic : Eased<float>
{
	internal EasedInFloatCubic(float startValue) : base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.CubicEaseIn(Time.time - this.m_startTime, this.m_startValue, this.m_endValue - this.m_startValue, this.m_duration);
	}
}
