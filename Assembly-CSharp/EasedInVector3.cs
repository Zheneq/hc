using UnityEngine;

internal class EasedInVector3 : Eased<Vector3>
{
	internal EasedInVector3(Vector3 startValue)
		: base(startValue)
	{
	}

	protected override Vector3 CalcValue()
	{
		float t = Time.time - m_startTime;
		return new Vector3(Easing.ExpoEaseIn(t, m_startValue.x, m_endValue.x - m_startValue.x, m_duration), Easing.ExpoEaseIn(t, m_startValue.y, m_endValue.y - m_startValue.y, m_duration), Easing.ExpoEaseIn(t, m_startValue.z, m_endValue.z - m_startValue.z, m_duration));
	}
}
