using UnityEngine;

internal class EasedQuaternionNoAccel : Eased<Quaternion>
{
	internal EasedQuaternionNoAccel(Quaternion startValue)
		: base(startValue)
	{
	}

	protected override Quaternion CalcValue()
	{
		float t = (Time.time - m_startTime) / m_duration;
		return Quaternion.Slerp(m_startValue, m_endValue, t);
	}

	internal void SnapTo(Quaternion value)
	{
		m_startValue = value;
		m_endValue = value;
		m_startTime = Time.time + 0.5f;
		m_duration = 0.3f;
	}
}
