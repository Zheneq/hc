using System;
using UnityEngine;

internal class EasedQuaternionNoAccel : Eased<Quaternion>
{
	internal EasedQuaternionNoAccel(Quaternion startValue) : base(startValue)
	{
	}

	protected override Quaternion CalcValue()
	{
		float t = (Time.time - this.m_startTime) / this.m_duration;
		return Quaternion.Slerp(this.m_startValue, this.m_endValue, t);
	}

	internal void SnapTo(Quaternion value)
	{
		this.m_startValue = value;
		this.m_endValue = value;
		this.m_startTime = Time.time + 0.5f;
		this.m_duration = 0.3f;
	}
}
