using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_ChargeAoE : AbilityUtil_Targeter
{
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

	public AbilityUtil_Targeter_ChargeAoE.ShouldAddActorDelegate m_shouldAddTargetDelegate;

	public AbilityUtil_Targeter_ChargeAoE.ShouldAddCasterDelegate m_shouldAddCasterDelegate;

	public List<ActorData> OrderedHitActors = new List<ActorData>();

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_ChargeAoE(Ability ability, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS) : base(ability)
	{
		this.m_radiusAroundStart = radiusAroundStart;
		this.m_radiusAroundEnd = radiusAroundEnd;
		this.m_rangeFromLine = rangeFromDir;
		this.m_penetrateLoS = penetrateLoS;
		this.m_maxTargets = maxTargets;
		this.m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
		bool shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
		{
			shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		}
		else
		{
			shouldShowActorRadius = true;
		}
		this.m_shouldShowActorRadius = shouldShowActorRadius;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	protected virtual bool UseRadiusAroundLine(AbilityTarget currentTarget)
	{
		return this.m_rangeFromLine > 0f;
	}

	protected virtual bool UseRadiusAroundStart(AbilityTarget currentTarget)
	{
		return this.m_radiusAroundStart > 0f;
	}

	protected virtual bool UseRadiusAroundEnd(AbilityTarget currentTarget)
	{
		return this.m_radiusAroundEnd > 0f;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		BoardSquare boardSquare = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		base.ClearActorsInRange();
		this.OrderedHitActors.Clear();
		BoardSquarePathInfo boardSquarePathInfo = null;
		float startRadiusInSquares = (!this.UseRadiusAroundStart(currentTarget)) ? 0f : this.m_radiusAroundStart;
		float num;
		if (this.UseRadiusAroundEnd(currentTarget))
		{
			num = this.m_radiusAroundEnd;
		}
		else
		{
			num = 0f;
		}
		float endRadiusInSquares = num;
		float num2 = (!this.UseRadiusAroundLine(currentTarget)) ? 0f : this.m_rangeFromLine;
		BoardSquare boardSquare2 = null;
		if (boardSquare != null)
		{
			if (currentTargetIndex != 0 && targets != null)
			{
				if (this.IsUsingMultiTargetUpdate())
				{
					goto IL_C4;
				}
			}
			boardSquare2 = targetingActor.GetCurrentBoardSquare();
			goto IL_FA;
		}
		IL_C4:
		if (boardSquare != null)
		{
			boardSquare2 = Board.Get().GetBoardSquareSafe(targets[currentTargetIndex - 1].GridPos);
		}
		IL_FA:
		List<Team> affectedTeams = base.GetAffectedTeams();
		if (boardSquare2 != null && boardSquare != null)
		{
			if (this.TrimPathOnTargetHit)
			{
				if (num2 > 0f)
				{
					bool flag;
					Vector3 vector;
					Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(targetingActor, boardSquare2.ToVector3(), boardSquare.ToVector3(), out flag, out vector, null);
					if (flag)
					{
						boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(boardSquare2.ToVector3(), abilityLineEndpoint, false, false, float.MaxValue);
					}
				}
			}
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, boardSquare2, this.AllowChargeThroughInvalidSquares);
		}
		if (boardSquarePathInfo != null)
		{
			if (boardSquarePathInfo.next != null)
			{
				if (this.TrimPathOnTargetHit)
				{
					if (num2 > 0f)
					{
						BoardSquare boardSquare3;
						TargetSelect_ChargeAoE.TrimChargePathOnActorHit(boardSquarePathInfo, boardSquare2, num2, targetingActor, affectedTeams, false, out boardSquare3);
					}
				}
			}
		}
		if (!this.SkipEvadeMovementLines)
		{
			if (this.ShowTeleportLines)
			{
				base.InstantiateTeleportPathUIEffect();
				base.UpdateEffectOnCaster(currentTarget, targetingActor);
				base.UpdateTargetAreaEffect(currentTarget, targetingActor);
			}
			else if (this.UseLineColorOverride)
			{
				base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, this.LineColorOverride, 0, false);
			}
			else
			{
				base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
			}
		}
		List<ActorData> list = null;
		if (this.m_shouldAddCasterDelegate != null)
		{
			list = new List<ActorData>();
		}
		if (boardSquarePathInfo != null)
		{
			List<Vector3> list2 = KnockbackUtils.BuildDrawablePath(boardSquarePathInfo, true);
			if (list2.Count >= 2)
			{
				Vector3 vector2 = list2[0];
				Vector3 vector3 = list2[list2.Count - 1];
				bool flag2 = this.UseRadiusAroundLine(currentTarget);
				bool flag3 = this.UseRadiusAroundStart(currentTarget);
				bool flag4 = this.UseRadiusAroundEnd(currentTarget);
				float widthInSquares = this.m_rangeFromLine * 2f;
				if (this.m_highlights.Count == 0)
				{
					GameObject item = TargeterUtils.CreateLaserBoxHighlight(vector2, vector3, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
					this.m_highlights.Add(item);
					GameObject item2 = TargeterUtils.CreateCircleHighlight(vector2, this.m_radiusAroundStart, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
					this.m_highlights.Add(item2);
					GameObject item3 = TargeterUtils.CreateCircleHighlight(vector3, this.m_radiusAroundEnd, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
					this.m_highlights.Add(item3);
				}
				if (flag2)
				{
					if (vector2 == vector3)
					{
						if (this.m_highlights.Count > 0)
						{
							if (this.m_highlights[0] != null)
							{
								this.m_highlights[0].SetActive(false);
							}
						}
					}
					else
					{
						if (this.m_highlights.Count > 0)
						{
							if (this.m_highlights[0] != null)
							{
								this.m_highlights[0].SetActive(true);
							}
						}
						TargeterUtils.RefreshLaserBoxHighlight(this.m_highlights[0], vector2, vector3, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
					}
				}
				else if (this.m_highlights.Count > 0)
				{
					if (this.m_highlights[0] != null)
					{
						this.m_highlights[0].SetActive(false);
					}
				}
				if (flag3)
				{
					this.m_highlights[1].SetActive(true);
					TargeterUtils.RefreshCircleHighlight(this.m_highlights[1], vector2, TargeterUtils.HeightAdjustType.FromPathArrow);
				}
				else if (this.m_highlights.Count > 1 && this.m_highlights[1] != null)
				{
					this.m_highlights[1].SetActive(false);
				}
				if (flag4)
				{
					this.m_highlights[2].SetActive(true);
					TargeterUtils.RefreshCircleHighlight(this.m_highlights[2], vector3, TargeterUtils.HeightAdjustType.FromPathArrow);
				}
				else if (this.m_highlights.Count > 2)
				{
					if (this.m_highlights[2] != null)
					{
						this.m_highlights[2].SetActive(false);
					}
				}
				List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(vector2, vector3, startRadiusInSquares, endRadiusInSquares, num2, this.m_penetrateLoS, targetingActor, base.GetAffectedTeams(), null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadiusOfLine);
				TargeterUtils.SortActorsByDistanceToPos(ref actorsInRadiusOfLine, targetingActor.GetTravelBoardSquareWorldPosition());
				TargeterUtils.LimitActorsToMaxNumber(ref actorsInRadiusOfLine, this.m_maxTargets);
				using (List<ActorData>.Enumerator enumerator = actorsInRadiusOfLine.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (base.GetAffectsTarget(actorData, targetingActor))
						{
							if (this.m_shouldAddTargetDelegate != null)
							{
								if (!this.m_shouldAddTargetDelegate(actorData, currentTarget, actorsInRadiusOfLine, targetingActor, this.m_ability))
								{
									continue;
								}
							}
							Vector3 damageOrigin = vector2;
							if (this.UseEndPosAsDamageOriginIfOverlap)
							{
								Vector3 travelBoardSquareWorldPosition = actorData.GetTravelBoardSquareWorldPosition();
								travelBoardSquareWorldPosition.y = vector3.y;
								if ((travelBoardSquareWorldPosition - vector3).sqrMagnitude <= Mathf.Epsilon)
								{
									damageOrigin = vector3;
								}
							}
							base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
							this.OrderedHitActors.Add(actorData);
							if (this.m_shouldAddCasterDelegate != null)
							{
								list.Add(actorData);
							}
							if (this.UseRadiusAroundStart(currentTarget))
							{
								BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
								if (AreaEffectUtils.IsSquareInConeByActorRadius(currentBoardSquare, vector2, 0f, 360f, this.m_radiusAroundStart, 0f, this.m_penetrateLoS, targetingActor, false, default(Vector3)))
								{
									base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Far, true);
								}
							}
							bool flag5 = false;
							if (this.UseRadiusAroundEnd(currentTarget))
							{
								BoardSquare currentBoardSquare2 = actorData.GetCurrentBoardSquare();
								if (AreaEffectUtils.IsSquareInConeByActorRadius(currentBoardSquare2, vector3, 0f, 360f, this.m_radiusAroundEnd, 0f, this.m_penetrateLoS, targetingActor, false, default(Vector3)))
								{
									base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Far, true);
									flag5 = true;
								}
							}
							ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
							ContextVars u = actorHitContext.symbol_0015;
							int hash = ContextKeys.symbol_0004.GetHash();
							int value;
							if (flag5)
							{
								value = 1;
							}
							else
							{
								value = 0;
							}
							u.SetInt(hash, value);
						}
					}
				}
				if (targetingActor == GameFlowData.Get().activeOwnedActorData)
				{
					base.ResetSquareIndicatorIndexToUse();
					AreaEffectUtils.OperateOnSquaresInRadiusOfLine(this.m_indicatorHandler, vector2, vector3, startRadiusInSquares, endRadiusInSquares, num2, this.m_penetrateLoS, targetingActor);
					base.HideUnusedSquareIndicators();
				}
			}
		}
		if (!this.ForceAddTargetingActor)
		{
			if (this.m_shouldAddCasterDelegate == null)
			{
				return;
			}
			if (!this.m_shouldAddCasterDelegate(targetingActor, list))
			{
				return;
			}
		}
		base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self, false);
	}

	public delegate bool ShouldAddActorDelegate(ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability);

	public delegate bool ShouldAddCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);
}
