using System;
using UnityEngine;

internal class EasedOutFloat : Eased<float>
{
	internal EasedOutFloat(float startValue) : base(startValue)
	{
	}

	protected override float CalcValue()
	{
		return Easing.ExpoEaseOut(Time.time - this.m_startTime, this.m_startValue, this.m_endValue - this.m_startValue, this.m_duration);
	}
}
