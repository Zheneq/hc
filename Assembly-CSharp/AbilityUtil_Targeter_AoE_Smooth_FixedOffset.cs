using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_AoE_Smooth_FixedOffset : AbilityUtil_Targeter_AoE_Smooth
{
	public delegate bool IsSquareInLosDelegate(BoardSquare testSquare, Vector3 aoeCenterPos, ActorData caster);

	public class SquareInsideChecker_AoeFixedOffset : SquareInsideChecker_Cone
	{
		private AbilityUtil_Targeter_AoE_Smooth_FixedOffset m_targeter;

		public SquareInsideChecker_AoeFixedOffset(AbilityUtil_Targeter_AoE_Smooth_FixedOffset targeter)
		{
			m_targeter = targeter;
		}

		public override bool IsSquareInside(BoardSquare square, out bool inLos)
		{
			bool flag = base.IsSquareInside(square, out inLos);
			if (flag)
			{
				if (m_targeter.m_delegateIsSquareInLos != null)
				{
					inLos = m_targeter.m_delegateIsSquareInLos(square, m_coneStart, m_caster);
				}
			}
			return flag;
		}
	}

	public float m_minOffsetFromCaster;

	public float m_maxOffsetFromCaster;

	private float m_knockbackDist;

	private float m_connectLaserWidth;

	private TargeterPart_Laser m_laserPart;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	private SquareInsideChecker_Box m_laserChecker;

	private SquareInsideChecker_AoeFixedOffset m_coneChecker;

	public IsSquareInLosDelegate m_delegateIsSquareInLos;

	public AbilityUtil_Targeter_AoE_Smooth_FixedOffset(Ability ability, float minOffsetFromCaster, float maxOffsetFromCaster, float radius, bool penetrateLoS, float knockbackDist, KnockbackType knockbackType, float connectLaserWidth, bool affectsEnemies = true, bool affectsAllies = false, int maxTargets = -1)
		: base(ability, radius, penetrateLoS, affectsEnemies, affectsAllies, maxTargets)
	{
		m_minOffsetFromCaster = minOffsetFromCaster;
		m_maxOffsetFromCaster = maxOffsetFromCaster;
		m_knockbackDist = knockbackDist;
		m_knockbackType = knockbackType;
		m_connectLaserWidth = connectLaserWidth;
		m_laserPart = new TargeterPart_Laser(m_connectLaserWidth, 1f, false, -1);
		m_coneChecker = new SquareInsideChecker_AoeFixedOffset(this);
		m_laserChecker = new SquareInsideChecker_Box(m_connectLaserWidth);
		m_squarePosCheckerList.Add(m_coneChecker);
		if (!(m_connectLaserWidth > 0f))
		{
			return;
		}
		while (true)
		{
			m_squarePosCheckerList.Add(m_laserChecker);
			return;
		}
	}

	public static Vector3 GetClampedFreePos(Vector3 freePos, ActorData caster, float minDistInSquares, float maxDistInSquares)
	{
		float squareSize = Board.Get().squareSize;
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector = freePos - travelBoardSquareWorldPositionForLos;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		float num = minDistInSquares * squareSize;
		float num2 = maxDistInSquares * squareSize;
		if (magnitude < num)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return travelBoardSquareWorldPositionForLos + vector.normalized * num;
				}
			}
		}
		if (magnitude > num2)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return travelBoardSquareWorldPositionForLos + vector.normalized * num2;
				}
			}
		}
		return freePos;
	}

	protected override Vector3 GetRefPos(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		return GetClampedFreePos(currentTarget.FreePos, targetingActor, m_minOffsetFromCaster, m_maxOffsetFromCaster);
	}

	protected override Vector3 GetDamageOrigin(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		return targetingActor.GetTravelBoardSquareWorldPositionForLos();
	}

	public override void CreateHighlightObjectsIfNeeded(float radiusInSquares, ActorData targetingActor)
	{
		base.CreateHighlightObjectsIfNeeded(radiusInSquares, targetingActor);
		if (!(m_connectLaserWidth > 0f))
		{
			return;
		}
		while (true)
		{
			if (m_highlights.Count < 2)
			{
				while (true)
				{
					m_highlights.Add(m_laserPart.CreateHighlightObject(this));
					return;
				}
			}
			return;
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		List<ActorData> visibleActorsInRange = GetVisibleActorsInRange();
		if (m_knockbackDist > 0f)
		{
			int num = 0;
			EnableAllMovementArrows();
			Vector3 refPos = GetRefPos(currentTarget, targetingActor, 0f);
			using (List<ActorData>.Enumerator enumerator = visibleActorsInRange.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(current, m_knockbackType, currentTarget.AimDirection, refPos, m_knockbackDist);
					num = AddMovementArrowWithPrevious(current, path, TargeterMovementType.Knockback, num);
				}
			}
			SetMovementArrowEnabledFromIndex(num, false);
		}
		Vector3 refPos2 = GetRefPos(currentTarget, targetingActor, 0f);
		Vector3 laserEnd = refPos2;
		if (m_connectLaserWidth > 0f)
		{
			Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			Vector3 laserEndPos = refPos2;
			laserEndPos.y = travelBoardSquareWorldPositionForLos.y;
			Vector3 dir = laserEndPos - travelBoardSquareWorldPositionForLos;
			float num2 = dir.magnitude / Board.SquareSizeStatic;
			float num3 = num2 - m_radius;
			laserEnd = travelBoardSquareWorldPositionForLos + Board.SquareSizeStatic * num3 * dir.normalized;
			if (num3 > 0f)
			{
				List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, dir, num3, m_connectLaserWidth, targetingActor, GetAffectedTeams(), false, -1, true, false, out laserEndPos, null);
				using (List<ActorData>.Enumerator enumerator2 = actorsInLaser.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ActorData current2 = enumerator2.Current;
						if (!visibleActorsInRange.Contains(current2))
						{
							AddActorInRange(current2, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Secondary);
						}
					}
				}
				m_laserPart.AdjustHighlight(m_highlights[1], travelBoardSquareWorldPositionForLos, laserEndPos, false);
				m_highlights[1].SetActiveIfNeeded(true);
			}
			else
			{
				m_highlights[1].SetActiveIfNeeded(false);
			}
		}
		CustomHandleHiddenSquareIndicators(targetingActor, refPos2, targetingActor.GetTravelBoardSquareWorldPositionForLos(), laserEnd);
	}

	protected override void HandleHiddenSquareIndicators(ActorData targetingActor, Vector3 centerPos)
	{
	}

	private void CustomHandleHiddenSquareIndicators(ActorData targetingActor, Vector3 centerPos, Vector3 laserStart, Vector3 laserEnd)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			m_laserChecker.UpdateBoxProperties(laserStart, laserEnd, targetingActor);
			m_coneChecker.UpdateConeProperties(centerPos, 360f, m_radius, 0f, 0f, targetingActor);
			ResetSquareIndicatorIndexToUse();
			if (m_connectLaserWidth > 0f)
			{
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, laserStart, laserEnd, m_connectLaserWidth, targetingActor, GetPenetrateLoS(), null, m_squarePosCheckerList);
			}
			AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, centerPos, 0f, 360f, m_radius, 0f, targetingActor, GetPenetrateLoS(), m_squarePosCheckerList);
			HideUnusedSquareIndicators();
			return;
		}
	}
}
