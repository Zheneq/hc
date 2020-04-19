using System;
using UnityEngine;

internal class EasedOutFloatQuad : Eased<float>
{
	internal EasedOutFloatQuad(float startValue) : base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.QuadEaseOut(Time.time - this.m_startTime, this.m_startValue, this.m_endValue - this.m_startValue, this.m_duration);
	}
}
