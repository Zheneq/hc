using System;
using UnityEngine;

internal class EasedOutFloatQuart : Eased<float>
{
	internal EasedOutFloatQuart(float startValue) : base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.QuartEaseOut(Time.time - this.m_startTime, this.m_startValue, this.m_endValue - this.m_startValue, this.m_duration);
	}
}
