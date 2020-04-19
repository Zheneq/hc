using System;
using UnityEngine;

internal class EasedVector3AnimationCurve : Eased<Vector3>
{
	protected AnimationCurve m_curve;

	internal EasedVector3AnimationCurve(Vector3 startValue) : base(startValue)
	{
	}

	protected override Vector3 CalcValue()
	{
		if (this.m_curve == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(EasedVector3AnimationCurve.CalcValue()).MethodHandle;
			}
			throw new ApplicationException("CalcValue called without curve being set.  Please use the version of EaseTo that takes a curve and not the base.");
		}
		float num = Time.time - this.m_startTime;
		float time = num / this.m_duration;
		float t = this.m_curve.Evaluate(time);
		return Vector3.Lerp(this.m_startValue, this.m_endValue, t);
	}

	internal void EaseTo(Vector3 endValue, AnimationCurve curve, float duration = 0.3f)
	{
		this.m_curve = curve;
		base.EaseTo(endValue, duration);
	}
}
