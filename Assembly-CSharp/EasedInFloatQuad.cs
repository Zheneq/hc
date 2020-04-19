using System;
using UnityEngine;

internal class EasedInFloatQuad : Eased<float>
{
	internal EasedInFloatQuad(float startValue) : base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.QuadEaseIn(Time.time - this.m_startTime, this.m_startValue, this.m_endValue - this.m_startValue, this.m_duration);
	}
}
