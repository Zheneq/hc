using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Line : AbilityUtil_Targeter
{
	public float m_lineRange = 10f;

	public bool m_linePenetrateLos;

	public AbilityUtil_Targeter_Line(Ability ability, float range, bool penetrateLos) : base(ability)
	{
		this.m_lineRange = range;
		this.m_linePenetrateLos = penetrateLos;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		float maxDistanceInWorld = this.m_lineRange * Board.\u000E().squareSize;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(targetingActor.\u0015(), currentTarget.AimDirection, maxDistanceInWorld, this.m_linePenetrateLos, targetingActor, null, true);
		if (this.m_highlights != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_Line.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (this.m_highlights.Count != 0)
			{
				goto IL_B9;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, false));
		this.m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, true));
		IL_B9:
		float magnitude = (laserEndPoint - targetingActor.\u0016()).magnitude;
		Vector3 position = targetingActor.\u0016() + new Vector3(0f, 0.1f, 0f);
		using (List<GameObject>.Enumerator enumerator = this.m_highlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				HighlightUtils.Get().ResizeBoundaryLine(magnitude / Board.\u000E().squareSize, gameObject);
				gameObject.transform.position = position;
				gameObject.transform.rotation = Quaternion.LookRotation(-currentTarget.AimDirection);
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}
}
