using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ChargeAoE : AbilityUtil_Targeter
{
	public delegate bool ShouldAddActorDelegate(ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability);

	public delegate bool ShouldAddCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	protected float m_radiusAroundStart = 2f;

	protected float m_radiusAroundEnd = 2f;

	protected float m_rangeFromLine = 2f;

	protected bool m_penetrateLoS;

	protected int m_maxTargets;

	public bool AllowChargeThroughInvalidSquares;

	public bool ShowTeleportLines;

	public bool SkipEvadeMovementLines;

	public bool ForceAddTargetingActor;

	public bool UseEndPosAsDamageOriginIfOverlap;

	public bool TrimPathOnTargetHit;

	public bool UseLineColorOverride;

	public Color LineColorOverride = Color.white;

	public ShouldAddActorDelegate m_shouldAddTargetDelegate;

	public ShouldAddCasterDelegate m_shouldAddCasterDelegate;

	public List<ActorData> OrderedHitActors = new List<ActorData>();

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_ChargeAoE(
		Ability ability,
		float radiusAroundStart,
		float radiusAroundEnd,
		float rangeFromDir,
		int maxTargets,
		bool ignoreTargetsCover,
		bool penetrateLoS)
		: base(ability)
	{
		m_radiusAroundStart = radiusAroundStart;
		m_radiusAroundEnd = radiusAroundEnd;
		m_rangeFromLine = rangeFromDir;
		m_penetrateLoS = penetrateLoS;
		m_maxTargets = maxTargets;
		m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser() || GameWideData.Get().UseActorRadiusForCone();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	protected virtual bool UseRadiusAroundLine(AbilityTarget currentTarget)
	{
		return m_rangeFromLine > 0f;
	}

	protected virtual bool UseRadiusAroundStart(AbilityTarget currentTarget)
	{
		return m_radiusAroundStart > 0f;
	}

	protected virtual bool UseRadiusAroundEnd(AbilityTarget currentTarget)
	{
		return m_radiusAroundEnd > 0f;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
		ClearActorsInRange();
		OrderedHitActors.Clear();
		BoardSquarePathInfo boardSquarePathInfo = null;
		float startRadiusInSquares = UseRadiusAroundStart(currentTarget) ? m_radiusAroundStart : 0f;
		float endRadiusInSquares = UseRadiusAroundEnd(currentTarget) ? m_radiusAroundEnd : 0f;
		float rangeFromLineInSquares = UseRadiusAroundLine(currentTarget) ? m_rangeFromLine : 0f;
		BoardSquare startSquare = null;
		if (targetSquare == null
		    || currentTargetIndex != 0
			    && targets != null
			    && IsUsingMultiTargetUpdate())
		{
			if (targetSquare != null)
			{
				startSquare = Board.Get().GetSquare(targets[currentTargetIndex - 1].GridPos);
			}
		}
		else
		{
			startSquare = targetingActor.GetCurrentBoardSquare();
		}
		List<Team> affectedTeams = GetAffectedTeams();
		if (startSquare != null && targetSquare != null)
		{
			if (TrimPathOnTargetHit && rangeFromLineInSquares > 0f)
			{
				Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(
					targetingActor,
					startSquare.ToVector3(),
					targetSquare.ToVector3(),
					out var collision,
					out Vector3 _);
				if (collision)
				{
					targetSquare = KnockbackUtils.GetLastValidBoardSquareInLine(startSquare.ToVector3(), abilityLineEndpoint);
				}
			}
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, targetSquare, startSquare, AllowChargeThroughInvalidSquares);
		}
		if (boardSquarePathInfo != null
		    && boardSquarePathInfo.next != null
		    && TrimPathOnTargetHit
		    && rangeFromLineInSquares > 0f)
		{
			TargetSelect_ChargeAoE.TrimChargePathOnActorHit(
				boardSquarePathInfo,
				startSquare,
				rangeFromLineInSquares,
				targetingActor,
				affectedTeams,
				false,
				out BoardSquare _);
		}
		if (!SkipEvadeMovementLines)
		{
			if (ShowTeleportLines)
			{
				InstantiateTeleportPathUIEffect();
				UpdateEffectOnCaster(currentTarget, targetingActor);
				UpdateTargetAreaEffect(currentTarget, targetingActor);
			}
			else if (UseLineColorOverride)
			{
				AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, LineColorOverride, 0);
			}
			else
			{
				AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, 0);
			}
		}
		List<ActorData> hitActors = null;
		if (m_shouldAddCasterDelegate != null)
		{
			hitActors = new List<ActorData>();
		}
		if (boardSquarePathInfo != null)
		{
			List<Vector3> path = KnockbackUtils.BuildDrawablePath(boardSquarePathInfo, true);
			if (path.Count >= 2)
			{
				Vector3 start = path[0];
				Vector3 end = path[path.Count - 1];
				float widthInSquares = m_rangeFromLine * 2f;
				if (m_highlights.Count == 0)
				{
					m_highlights.Add(TargeterUtils.CreateLaserBoxHighlight(
						start,
						end,
						widthInSquares,
						TargeterUtils.HeightAdjustType.FromPathArrow));
					m_highlights.Add(TargeterUtils.CreateCircleHighlight(
						start,
						m_radiusAroundStart,
						TargeterUtils.HeightAdjustType.FromPathArrow,
						targetingActor == GameFlowData.Get().activeOwnedActorData));
					m_highlights.Add(TargeterUtils.CreateCircleHighlight(
						end,
						m_radiusAroundEnd,
						TargeterUtils.HeightAdjustType.FromPathArrow,
						targetingActor == GameFlowData.Get().activeOwnedActorData));
				}
				if (UseRadiusAroundLine(currentTarget))
				{
					if (start == end)
					{
						if (m_highlights.Count > 0 && m_highlights[0] != null)
						{
							m_highlights[0].SetActive(false);
						}
					}
					else
					{
						if (m_highlights.Count > 0 && m_highlights[0] != null)
						{
							m_highlights[0].SetActive(true);
						}
						TargeterUtils.RefreshLaserBoxHighlight(m_highlights[0], start, end, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
					}
				}
				else if (m_highlights.Count > 0)
				{
					if (m_highlights[0] != null)
					{
						m_highlights[0].SetActive(false);
					}
				}
				if (UseRadiusAroundStart(currentTarget))
				{
					m_highlights[1].SetActive(true);
					TargeterUtils.RefreshCircleHighlight(m_highlights[1], start, TargeterUtils.HeightAdjustType.FromPathArrow);
				}
				else if (m_highlights.Count > 1 && m_highlights[1] != null)
				{
					m_highlights[1].SetActive(false);
				}
				if (UseRadiusAroundEnd(currentTarget))
				{
					m_highlights[2].SetActive(true);
					TargeterUtils.RefreshCircleHighlight(m_highlights[2], end, TargeterUtils.HeightAdjustType.FromPathArrow);
				}
				else if (m_highlights.Count > 2)
				{
					if (m_highlights[2] != null)
					{
						m_highlights[2].SetActive(false);
					}
				}
				List<ActorData> actors = AreaEffectUtils.GetActorsInRadiusOfLine(
					start,
					end,
					startRadiusInSquares,
					endRadiusInSquares,
					rangeFromLineInSquares,
					m_penetrateLoS,
					targetingActor,
					GetAffectedTeams(),
					null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
				TargeterUtils.SortActorsByDistanceToPos(ref actors, targetingActor.GetFreePos());
				TargeterUtils.LimitActorsToMaxNumber(ref actors, m_maxTargets);
				foreach (ActorData current in actors)
				{
					if (GetAffectsTarget(current, targetingActor)
					    && (m_shouldAddTargetDelegate == null
					        || m_shouldAddTargetDelegate(current, currentTarget, actors, targetingActor, m_ability)))
					{
						Vector3 damageOrigin = start;
						if (UseEndPosAsDamageOriginIfOverlap)
						{
							Vector3 travelBoardSquareWorldPosition = current.GetFreePos();
							travelBoardSquareWorldPosition.y = end.y;
							if ((travelBoardSquareWorldPosition - end).sqrMagnitude <= Mathf.Epsilon)
							{
								damageOrigin = end;
							}
						}
						AddActorInRange(current, damageOrigin, targetingActor);
						OrderedHitActors.Add(current);
						if (m_shouldAddCasterDelegate != null)
						{
							hitActors.Add(current);
						}
						if (UseRadiusAroundStart(currentTarget))
						{
							BoardSquare targetBoardSquare = current.GetCurrentBoardSquare();
							bool isInRange = AreaEffectUtils.IsSquareInConeByActorRadius(
								targetBoardSquare,
								start,
								0f,
								360f,
								m_radiusAroundStart,
								0f,
								m_penetrateLoS,
								targetingActor);
							if (isInRange)
							{
								AddActorInRange(current, damageOrigin, targetingActor, AbilityTooltipSubject.Far, true);
							}
						}
						bool inEndAoE = false;
						if (UseRadiusAroundEnd(currentTarget))
						{
							BoardSquare targetBoardSquare = current.GetCurrentBoardSquare();
							bool isInRange = AreaEffectUtils.IsSquareInConeByActorRadius(
								targetBoardSquare,
								end,
								0f,
								360f,
								m_radiusAroundEnd,
								0f,
								m_penetrateLoS,
								targetingActor);
							if (isInRange)
							{
								AddActorInRange(current, damageOrigin, targetingActor, AbilityTooltipSubject.Far, true);
								inEndAoE = true;
							}
						}
						m_actorContextVars[current].m_contextVars.SetValue(ContextKeys.s_InEndAoe.GetKey(), inEndAoE ? 1 : 0);
					}
				}
				if (targetingActor == GameFlowData.Get().activeOwnedActorData)
				{
					ResetSquareIndicatorIndexToUse();
					AreaEffectUtils.OperateOnSquaresInRadiusOfLine(
						m_indicatorHandler,
						start,
						end,
						startRadiusInSquares,
						endRadiusInSquares,
						rangeFromLineInSquares,
						m_penetrateLoS,
						targetingActor);
					HideUnusedSquareIndicators();
				}
			}
		}
		if (ForceAddTargetingActor
		    || (m_shouldAddCasterDelegate != null
		        && m_shouldAddCasterDelegate(targetingActor, hitActors)))
		{
			AddActorInRange(targetingActor, targetingActor.GetFreePos(), targetingActor, AbilityTooltipSubject.Self);
		}
	}
}
