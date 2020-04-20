using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_SweepSingleClickCone : AbilityUtil_Targeter_SweepMultiClickCone
{
	public Exo_SyncComponent m_syncComponent;

	private LaserTargetingInfo m_unanchoredLaserInfo;

	public AbilityUtil_Targeter_SweepSingleClickCone(Ability ability, float minAngle, float maxAngle, float rangeInSquares, float coneBackwardOffset, float lineWidthInSquares, LaserTargetingInfo unanchoredLaserInfo, Exo_SyncComponent syncComponent)
	{
		bool penetrateLos;
		if (unanchoredLaserInfo != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SweepSingleClickCone..ctor(Ability, float, float, float, float, float, LaserTargetingInfo, Exo_SyncComponent)).MethodHandle;
			}
			penetrateLos = unanchoredLaserInfo.penetrateLos;
		}
		else
		{
			penetrateLos = false;
		}
		base..ctor(ability, minAngle, maxAngle, rangeInSquares, coneBackwardOffset, lineWidthInSquares, penetrateLos, 0);
		this.m_syncComponent = syncComponent;
		this.m_unanchoredLaserInfo = unanchoredLaserInfo;
		base.SetUseMultiTargetUpdate(this.m_unanchoredLaserInfo == null);
	}

	public bool IsInitialPlacement()
	{
		if (this.m_syncComponent != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SweepSingleClickCone.IsInitialPlacement()).MethodHandle;
			}
			if (!this.m_syncComponent.m_anchored)
			{
				return this.m_unanchoredLaserInfo != null;
			}
		}
		return false;
	}

	public override float GetLineWidth()
	{
		if (this.IsInitialPlacement())
		{
			return this.m_unanchoredLaserInfo.width;
		}
		return base.GetLineWidth();
	}

	public override float GetLineRange()
	{
		if (this.IsInitialPlacement())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SweepSingleClickCone.GetLineRange()).MethodHandle;
			}
			return this.m_unanchoredLaserInfo.range;
		}
		return base.GetLineRange();
	}

	public override int GetLineMaxTargets()
	{
		if (this.IsInitialPlacement())
		{
			return this.m_unanchoredLaserInfo.maxTargets;
		}
		return base.GetLineMaxTargets();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		AbilityTarget abilityTarget = AbilityTarget.CreateAbilityTargetFromInterface();
		abilityTarget.SetPosAndDir(default(GridPos), default(Vector3), this.m_syncComponent.m_anchoredLaserAimDirection);
		List<ActorData> list = base.UpdateHighlightLine(targetingActor, currentTarget.AimDirection, this.m_syncComponent.m_anchored, abilityTarget.AimDirection);
		using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actor = enumerator.Current;
				base.AddActorInRange(actor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Primary, true);
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SweepSingleClickCone.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
		}
	}
}
