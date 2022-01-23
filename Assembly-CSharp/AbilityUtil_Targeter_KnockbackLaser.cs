using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_KnockbackLaser : AbilityUtil_Targeter_Laser
{
	public float m_knockbackDistanceMin;

	public float m_knockbackDistanceMax;

	public KnockbackType m_knockbackType;

	public AbilityUtil_Targeter_KnockbackLaser(Ability ability, float width, float distance, bool penetrateLoS, int maxTargets, float knockbackDistanceMin, float knockbackDistanceMax, KnockbackType knockbackType, bool affectsAllies)
		: base(ability, width, distance, penetrateLoS, maxTargets, affectsAllies)
	{
		m_knockbackDistanceMin = knockbackDistanceMin;
		m_knockbackDistanceMax = knockbackDistanceMax;
		m_knockbackType = knockbackType;
		m_affectsAllies = affectsAllies;
	}

	private float GetKnockbackDist(AbilityTarget target, Vector3 casterPos, Vector3 knockbackStartPos)
	{
		Vector3 vector = target.FreePos - casterPos;
		Vector3 vector2 = knockbackStartPos - casterPos;
		vector.y = 0f;
		vector2.y = 0f;
		float num = (vector.magnitude - vector2.magnitude) / Board.SquareSizeStatic;
		float knockbackDistanceMin = m_knockbackDistanceMin;
		float knockbackDistanceMax = m_knockbackDistanceMax;
		if (num < knockbackDistanceMin)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return knockbackDistanceMin;
				}
			}
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
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetFreePos();
		float knockbackDist = GetKnockbackDist(currentTarget, travelBoardSquareWorldPosition, m_lastCalculatedLaserEndPos);
		int num = 0;
		EnableAllMovementArrows();
		List<ActorData> visibleActorsInRange = GetVisibleActorsInRange();
		using (List<ActorData>.Enumerator enumerator = visibleActorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current.GetTeam() != targetingActor.GetTeam())
				{
					BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildKnockbackPath(current, m_knockbackType, currentTarget.AimDirection, travelBoardSquareWorldPosition, knockbackDist);
					if (boardSquarePathInfo.FindMoveCostToEnd() < 0.5f)
					{
						AddActorInRange(current, travelBoardSquareWorldPosition, targetingActor, AbilityTooltipSubject.HighHP, true);
					}
					num = AddMovementArrowWithPrevious(current, boardSquarePathInfo, TargeterMovementType.Knockback, num);
				}
			}
		}
		SetMovementArrowEnabledFromIndex(num, false);
	}
}
