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
			bool isInside = base.IsSquareInside(square, out inLos);
			if (isInside && m_targeter.m_delegateIsSquareInLos != null)
			{
				inLos = m_targeter.m_delegateIsSquareInLos(square, m_coneStart, m_caster);
			}
			return isInside;
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

	public AbilityUtil_Targeter_AoE_Smooth_FixedOffset(
		Ability ability,
		float minOffsetFromCaster,
		float maxOffsetFromCaster,
		float radius,
		bool penetrateLoS,
		float knockbackDist,
		KnockbackType knockbackType,
		float connectLaserWidth,
		bool affectsEnemies = true,
		bool affectsAllies = false,
		int maxTargets = -1)
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
		if (m_connectLaserWidth > 0f)
		{
			m_squarePosCheckerList.Add(m_laserChecker);
		}
	}

	public static Vector3 GetClampedFreePos(Vector3 freePos, ActorData caster, float minDistInSquares, float maxDistInSquares)
	{
		float squareSize = Board.Get().squareSize;
		Vector3 casterPos = caster.GetLoSCheckPos();
		Vector3 vector = freePos - casterPos;
		vector.y = 0f;
		float dist = vector.magnitude;
		float minDist = minDistInSquares * squareSize;
		float maxDist = maxDistInSquares * squareSize;
		if (dist < minDist)
		{
			return casterPos + vector.normalized * minDist;
		}
		if (dist > maxDist)
		{
			return casterPos + vector.normalized * maxDist;
		}
		return freePos;
	}

	protected override Vector3 GetRefPos(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		return GetClampedFreePos(currentTarget.FreePos, targetingActor, m_minOffsetFromCaster, m_maxOffsetFromCaster);
	}

	protected override Vector3 GetDamageOrigin(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		return targetingActor.GetLoSCheckPos();
	}

	public override void CreateHighlightObjectsIfNeeded(float radiusInSquares, ActorData targetingActor)
	{
		base.CreateHighlightObjectsIfNeeded(radiusInSquares, targetingActor);
		if (m_connectLaserWidth > 0f && m_highlights.Count < 2)
		{
			m_highlights.Add(m_laserPart.CreateHighlightObject(this));
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		List<ActorData> visibleActorsInRange = GetVisibleActorsInRange();
		if (m_knockbackDist > 0f)
		{
			int arrowIndex = 0;
			EnableAllMovementArrows();
			Vector3 refPos = GetRefPos(currentTarget, targetingActor, 0f);
			foreach (ActorData target in visibleActorsInRange)
			{
				BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(
					target,
					m_knockbackType,
					currentTarget.AimDirection,
					refPos,
					m_knockbackDist);
				arrowIndex = AddMovementArrowWithPrevious(target, path, TargeterMovementType.Knockback, arrowIndex);
			}
			SetMovementArrowEnabledFromIndex(arrowIndex, false);
		}
		Vector3 refPos2 = GetRefPos(currentTarget, targetingActor, 0f);
		Vector3 laserEnd = refPos2;
		if (m_connectLaserWidth > 0f)
		{
			Vector3 casterPos = targetingActor.GetLoSCheckPos();
			Vector3 laserEndPos = refPos2;
			laserEndPos.y = casterPos.y;
			Vector3 dir = laserEndPos - casterPos;
			float num2 = dir.magnitude / Board.SquareSizeStatic;
			float num3 = num2 - m_radius;
			laserEnd = casterPos + Board.SquareSizeStatic * num3 * dir.normalized;
			if (num3 > 0f)
			{
				List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
					casterPos,
					dir,
					num3,
					m_connectLaserWidth,
					targetingActor,
					GetAffectedTeams(),
					false,
					-1,
					true,
					false,
					out laserEndPos,
					null);
				foreach (ActorData target in actorsInLaser)
				{
					if (!visibleActorsInRange.Contains(target))
					{
						AddActorInRange(target, casterPos, targetingActor, AbilityTooltipSubject.Secondary);
					}
				}
				m_laserPart.AdjustHighlight(m_highlights[1], casterPos, laserEndPos, false);
				m_highlights[1].SetActiveIfNeeded(true);
			}
			else
			{
				m_highlights[1].SetActiveIfNeeded(false);
			}
		}
		CustomHandleHiddenSquareIndicators(targetingActor, refPos2, targetingActor.GetLoSCheckPos(), laserEnd);
	}

	protected override void HandleHiddenSquareIndicators(ActorData targetingActor, Vector3 centerPos)
	{
	}

	private void CustomHandleHiddenSquareIndicators(ActorData targetingActor, Vector3 centerPos, Vector3 laserStart, Vector3 laserEnd)
	{
		if (targetingActor != GameFlowData.Get().activeOwnedActorData)
		{
			return;
		}
		m_laserChecker.UpdateBoxProperties(laserStart, laserEnd, targetingActor);
		m_coneChecker.UpdateConeProperties(centerPos, 360f, m_radius, 0f, 0f, targetingActor);
		ResetSquareIndicatorIndexToUse();
		if (m_connectLaserWidth > 0f)
		{
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(
				m_indicatorHandler,
				laserStart,
				laserEnd,
				m_connectLaserWidth,
				targetingActor,
				GetPenetrateLoS(),
				null,
				m_squarePosCheckerList);
		}
		AreaEffectUtils.OperateOnSquaresInCone(
			m_indicatorHandler,
			centerPos,
			0f,
			360f,
			m_radius,
			0f,
			targetingActor,
			GetPenetrateLoS(),
			m_squarePosCheckerList);
		HideUnusedSquareIndicators();
	}
}
