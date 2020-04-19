using System;
using UnityEngine;

internal class EasedInVector3 : Eased<Vector3>
{
	internal EasedInVector3(Vector3 startValue) : base(startValue)
	{
	}

	protected override Vector3 CalcValue()
	{
		float t = Time.time - this.m_startTime;
		return new Vector3(Easing.ExpoEaseIn(t, this.m_startValue.x, this.m_endValue.x - this.m_startValue.x, this.m_duration), Easing.ExpoEaseIn(t, this.m_startValue.y, this.m_endValue.y - this.m_startValue.y, this.m_duration), Easing.ExpoEaseIn(t, this.m_startValue.z, this.m_endValue.z - this.m_startValue.z, this.m_duration));
	}
}
