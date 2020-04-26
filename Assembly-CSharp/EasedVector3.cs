using UnityEngine;

internal class EasedVector3 : Eased<Vector3>
{
	internal EasedVector3(Vector3 startValue)
		: base(startValue)
	{
	}

	protected override Vector3 CalcValue()
	{
		float t = Time.time - m_startTime;
		return new Vector3(Easing.ExpoEaseInOut(t, m_startValue.x, m_endValue.x - m_startValue.x, m_duration), Easing.ExpoEaseInOut(t, m_startValue.y, m_endValue.y - m_startValue.y, m_duration), Easing.ExpoEaseInOut(t, m_startValue.z, m_endValue.z - m_startValue.z, m_duration));
	}
}
