using System;
using UnityEngine;

internal class EasedVector3 : Eased<Vector3>
{
	internal EasedVector3(Vector3 startValue) : base(startValue)
	{
	}

	protected override Vector3 CalcValue()
	{
		float t = Time.time - this.m_startTime;
		return new Vector3(Easing.ExpoEaseInOut(t, this.m_startValue.x, this.m_endValue.x - this.m_startValue.x, this.m_duration), Easing.ExpoEaseInOut(t, this.m_startValue.y, this.m_endValue.y - this.m_startValue.y, this.m_duration), Easing.ExpoEaseInOut(t, this.m_startValue.z, this.m_endValue.z - this.m_startValue.z, this.m_duration));
	}
}
