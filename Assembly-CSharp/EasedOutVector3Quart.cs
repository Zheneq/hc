using System;
using UnityEngine;

internal class EasedOutVector3Quart : Eased<Vector3>
{
	internal EasedOutVector3Quart(Vector3 startValue) : base(startValue)
	{
	}

	protected override Vector3 CalcValue()
	{
		float t = Time.time - this.m_startTime;
		return new Vector3(Easing.QuartEaseOut(t, this.m_startValue.x, this.m_endValue.x - this.m_startValue.x, this.m_duration), Easing.QuartEaseOut(t, this.m_startValue.y, this.m_endValue.y - this.m_startValue.y, this.m_duration), Easing.QuartEaseOut(t, this.m_startValue.z, this.m_endValue.z - this.m_startValue.z, this.m_duration));
	}
}
