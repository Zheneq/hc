using UnityEngine;

internal class EasedOutVector3Quart : Eased<Vector3>
{
	internal EasedOutVector3Quart(Vector3 startValue)
		: base(startValue)
	{
	}

	protected override Vector3 CalcValue()
	{
		float t = Time.time - m_startTime;
		return new Vector3(Easing.QuartEaseOut(t, m_startValue.x, m_endValue.x - m_startValue.x, m_duration), Easing.QuartEaseOut(t, m_startValue.y, m_endValue.y - m_startValue.y, m_duration), Easing.QuartEaseOut(t, m_startValue.z, m_endValue.z - m_startValue.z, m_duration));
	}
}
