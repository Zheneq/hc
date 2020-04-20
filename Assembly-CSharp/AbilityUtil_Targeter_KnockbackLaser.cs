using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_KnockbackLaser : AbilityUtil_Targeter_Laser
{
	public float m_knockbackDistanceMin;

	public float m_knockbackDistanceMax;

	public KnockbackType m_knockbackType;

	public AbilityUtil_Targeter_KnockbackLaser(Ability ability, float width, float distance, bool penetrateLoS, int maxTargets, float knockbackDistanceMin, float knockbackDistanceMax, KnockbackType knockbackType, bool affectsAllies) : base(ability, width, distance, penetrateLoS, maxTargets, affectsAllies, false)
	{
		this.m_knockbackDistanceMin = knockbackDistanceMin;
		this.m_knockbackDistanceMax = knockbackDistanceMax;
		this.m_knockbackType = knockbackType;
		this.m_affectsAllies = affectsAllies;
	}

	private float GetKnockbackDist(AbilityTarget target, Vector3 casterPos, Vector3 knockbackStartPos)
	{
		Vector3 vector = target.FreePos - casterPos;
		Vector3 vector2 = knockbackStartPos - casterPos;
		vector.y = 0f;
		vector2.y = 0f;
		float num = (vector.magnitude - vector2.magnitude) / Board.SquareSizeStatic;
		float knockbackDistanceMin = this.m_knockbackDistanceMin;
		float knockbackDistanceMax = this.m_knockbackDistanceMax;
		if (num < knockbackDistanceMin)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_KnockbackLaser.GetKnockbackDist(AbilityTarget, Vector3, Vector3)).MethodHandle;
			}
			return knockbackDistanceMin;
		}
		if (num > knockbackDistanceMax)
		{
			return knockbackDistanceMax;
		}
		return num;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		float knockbackDist = this.GetKnockbackDist(currentTarget, travelBoardSquareWorldPosition, this.m_lastCalculatedLaserEndPos);
		int num = 0;
		base.EnableAllMovementArrows();
		List<ActorData> visibleActorsInRange = this.GetVisibleActorsInRange();
		using (List<ActorData>.Enumerator enumerator = visibleActorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData.GetTeam() != targetingActor.GetTeam())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_KnockbackLaser.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
					}
					BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, currentTarget.AimDirection, travelBoardSquareWorldPosition, knockbackDist);
					if (boardSquarePathInfo.FindMoveCostToEnd() < 0.5f)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						base.AddActorInRange(actorData, travelBoardSquareWorldPosition, targetingActor, AbilityTooltipSubject.HighHP, true);
					}
					num = base.AddMovementArrowWithPrevious(actorData, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
				}
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
		base.SetMovementArrowEnabledFromIndex(num, false);
	}
}
