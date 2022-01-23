using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_SweepSingleClickCone : AbilityUtil_Targeter_SweepMultiClickCone
{
	public Exo_SyncComponent m_syncComponent;

	private LaserTargetingInfo m_unanchoredLaserInfo;

	public AbilityUtil_Targeter_SweepSingleClickCone(Ability ability, float minAngle, float maxAngle, float rangeInSquares, float coneBackwardOffset, float lineWidthInSquares, LaserTargetingInfo unanchoredLaserInfo, Exo_SyncComponent syncComponent)
	: base(ability, minAngle, maxAngle, rangeInSquares, coneBackwardOffset, lineWidthInSquares, unanchoredLaserInfo?.penetrateLos ?? false, 0)
	{
		m_syncComponent = syncComponent;
		m_unanchoredLaserInfo = unanchoredLaserInfo;
		SetUseMultiTargetUpdate(m_unanchoredLaserInfo == null);
	}

	public bool IsInitialPlacement()
	{
		int result;
		if (m_syncComponent != null)
		{
			if (!m_syncComponent.m_anchored)
			{
				result = ((m_unanchoredLaserInfo != null) ? 1 : 0);
				goto IL_003f;
			}
		}
		result = 0;
		goto IL_003f;
		IL_003f:
		return (byte)result != 0;
	}

	public override float GetLineWidth()
	{
		if (IsInitialPlacement())
		{
			return m_unanchoredLaserInfo.width;
		}
		return base.GetLineWidth();
	}

	public override float GetLineRange()
	{
		if (IsInitialPlacement())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_unanchoredLaserInfo.range;
				}
			}
		}
		return base.GetLineRange();
	}

	public override int GetLineMaxTargets()
	{
		if (IsInitialPlacement())
		{
			return m_unanchoredLaserInfo.maxTargets;
		}
		return base.GetLineMaxTargets();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		AbilityTarget abilityTarget = AbilityTarget.CreateAbilityTargetFromInterface();
		abilityTarget.SetPosAndDir(default(GridPos), default(Vector3), m_syncComponent.m_anchoredLaserAimDirection);
		List<ActorData> list = UpdateHighlightLine(targetingActor, currentTarget.AimDirection, m_syncComponent.m_anchored, abilityTarget.AimDirection);
		using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				AddActorInRange(current, targetingActor.GetFreePos(), targetingActor, AbilityTooltipSubject.Primary, true);
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
	}
}
