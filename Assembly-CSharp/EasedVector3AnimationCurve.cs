using System;
using UnityEngine;

internal class EasedVector3AnimationCurve : Eased<Vector3>
{
	protected AnimationCurve m_curve;

	internal EasedVector3AnimationCurve(Vector3 startValue)
		: base(startValue)
	{
	}

	protected override Vector3 CalcValue()
	{
		if (m_curve == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					throw new ApplicationException("CalcValue called without curve being set.  Please use the version of EaseTo that takes a curve and not the base.");
				}
			}
		}
		float num = Time.time - m_startTime;
		float time = num / m_duration;
		float t = m_curve.Evaluate(time);
		return Vector3.Lerp(m_startValue, m_endValue, t);
	}

	internal void EaseTo(Vector3 endValue, AnimationCurve curve, float duration = 0.3f)
	{
		m_curve = curve;
		EaseTo(endValue, duration);
	}
}
