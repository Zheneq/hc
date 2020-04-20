﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_StretchCone : AbilityUtil_Targeter
{
	public float m_minLengthSquares;

	public float m_maxLengthSquares;

	public float m_minAngleDegrees;

	public float m_maxAngleDegrees;

	public float m_coneBackwardOffsetInSquares;

	public bool m_penetrateLoS;

	public bool m_includeEnemies = true;

	public bool m_includeAllies;

	public bool m_includeCaster;

	public AreaEffectUtils.StretchConeStyle m_stretchStyle;

	public float m_knockbackDistance;

	public KnockbackType m_knockbackType;

	public float m_extraKnockbackDist;

	public float m_knockbackDistanceOnSelf;

	public KnockbackType m_knockbackTypeOnSelf;

	public float m_interpMinDistOverride = -1f;

	public float m_interpRangeOverride = -1f;

	public bool m_discreteWidthAngleChange;

	public int m_numDiscreteWidthChanges;

	private TargeterPart_Cone m_conePart;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_StretchCone.UseExtraKnockbackDistDelegate m_useExtraKnockbackDistDelegate;

	public AbilityUtil_Targeter_StretchCone.ConeLengthSquaresOverrideDelegate m_coneLengthSquaresOverrideDelegate;

	public bool ForceHideSides;

	public AbilityUtil_Targeter_StretchCone(Ability ability, float minLengthInSquares, float maxLengthInSquares, float minAngleDegrees, float maxAngleDegrees, AreaEffectUtils.StretchConeStyle stretchStyle, float coneBackwardOffsetInSquares, bool penetrateLoS) : base(ability)
	{
		this.m_minLengthSquares = minLengthInSquares;
		this.m_maxLengthSquares = maxLengthInSquares;
		this.m_minAngleDegrees = minAngleDegrees;
		this.m_maxAngleDegrees = maxAngleDegrees;
		this.m_stretchStyle = stretchStyle;
		this.m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		this.m_penetrateLoS = penetrateLoS;
		this.m_knockbackDistance = 0f;
		this.m_knockbackType = KnockbackType.ForwardAlongAimDir;
		this.m_conePart = new TargeterPart_Cone(this.m_maxAngleDegrees, this.m_maxLengthSquares, this.m_penetrateLoS, this.m_coneBackwardOffsetInSquares);
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		this.LastConeAngle = 0f;
		this.LastConeRadiusInSquares = 0f;
	}

	public float LastConeAngle { get; set; }

	public float LastConeRadiusInSquares { get; set; }

	public void InitKnockbackData(float knockbackDistance, KnockbackType knockbackType, float knockbackDistanceOnSelf, KnockbackType knockbackTypeOnSelf)
	{
		this.m_knockbackDistance = knockbackDistance;
		this.m_knockbackType = knockbackType;
		this.m_knockbackDistanceOnSelf = knockbackDistanceOnSelf;
		this.m_knockbackTypeOnSelf = knockbackTypeOnSelf;
	}

	public void SetExtraKnockbackDist(float extraDist)
	{
		this.m_extraKnockbackDist = extraDist;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare currentBoardSquare = targetingActor.GetCurrentBoardSquare();
		this.UpdateTargetingAsIfFromSquare(currentTarget, targetingActor, currentBoardSquare);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		BoardSquare coneStartSquare;
		if (targets.Count >= 1 && currentTargetIndex >= 1)
		{
			if (targets[currentTargetIndex - 1] != null)
			{
				AbilityTarget abilityTarget = targets[currentTargetIndex - 1];
				coneStartSquare = Board.Get().GetBoardSquareSafe(abilityTarget.GridPos);
				goto IL_66;
			}
		}
		coneStartSquare = targetingActor.GetCurrentBoardSquare();
		IL_66:
		this.UpdateTargetingAsIfFromSquare(currentTarget, targetingActor, coneStartSquare);
	}

	public void UpdateTargetingAsIfFromSquare(AbilityTarget currentTarget, ActorData targetingActor, BoardSquare coneStartSquare)
	{
		base.ClearActorsInRange();
		Vector3 worldPositionForLoS = coneStartSquare.GetWorldPositionForLoS();
		Vector3 freePos = currentTarget.FreePos;
		Vector3 vector = freePos - worldPositionForLoS;
		vector.y = 0f;
		vector.Normalize();
		float forwardDir_degrees = VectorUtils.HorizontalAngle_Deg(vector);
		float num;
		float num2;
		AreaEffectUtils.GatherStretchConeDimensions(freePos, worldPositionForLoS, this.m_minLengthSquares, this.m_maxLengthSquares, this.m_minAngleDegrees, this.m_maxAngleDegrees, this.m_stretchStyle, out num, out num2, this.m_discreteWidthAngleChange, this.m_numDiscreteWidthChanges, this.m_interpMinDistOverride, this.m_interpRangeOverride);
		if (this.m_coneLengthSquaresOverrideDelegate != null)
		{
			num = this.m_coneLengthSquaresOverrideDelegate();
		}
		this.LastConeAngle = num2;
		this.LastConeRadiusInSquares = num;
		this.m_conePart.UpdateDimensions(num2, num);
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 1)
			{
				goto IL_10E;
			}
		}
		this.m_highlights = new List<GameObject>();
		GameObject item = this.m_conePart.CreateHighlightObject(this);
		this.m_highlights.Add(item);
		IL_10E:
		this.m_conePart.AdjustHighlight(this.m_highlights[0], worldPositionForLoS, vector);
		List<ActorData> hitActors = this.m_conePart.GetHitActors(worldPositionForLoS, vector, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, this.m_includeAllies, this.m_includeEnemies));
		if (this.m_knockbackDistance <= 0f)
		{
			if (this.m_knockbackDistanceOnSelf <= 0f)
			{
				goto IL_2AA;
			}
		}
		int num3 = 0;
		base.EnableAllMovementArrows();
		Vector3 sourcePos = worldPositionForLoS;
		if (this.m_knockbackDistance > 0f)
		{
			float num4 = this.m_knockbackDistance;
			if (this.m_extraKnockbackDist > 0f)
			{
				if (this.m_useExtraKnockbackDistDelegate != null)
				{
					if (this.m_useExtraKnockbackDistDelegate(targetingActor))
					{
						num4 += this.m_extraKnockbackDist;
					}
				}
			}
			using (List<ActorData>.Enumerator enumerator = hitActors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					if (actorData.GetTeam() != targetingActor.GetTeam())
					{
						BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, vector, sourcePos, num4);
						num3 = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num3, false);
					}
				}
			}
		}
		if (this.m_knockbackDistanceOnSelf > 0f)
		{
			BoardSquarePathInfo path2 = KnockbackUtils.BuildKnockbackPath(targetingActor, this.m_knockbackTypeOnSelf, vector, sourcePos, this.m_knockbackDistanceOnSelf);
			num3 = base.AddMovementArrowWithPrevious(targetingActor, path2, AbilityUtil_Targeter.TargeterMovementType.Knockback, num3, false);
		}
		base.SetMovementArrowEnabledFromIndex(num3, false);
		IL_2AA:
		if (this.m_includeCaster)
		{
			if (!hitActors.Contains(targetingActor))
			{
				hitActors.Add(targetingActor);
			}
		}
		for (int i = 0; i < hitActors.Count; i++)
		{
			ActorData actor = hitActors[i];
			if (this.ShouldAddActor(actor, targetingActor))
			{
				base.AddActorInRange(actor, worldPositionForLoS, targetingActor, AbilityTooltipSubject.Primary, false);
			}
		}
		this.DrawInvalidSquareIndicators(targetingActor, worldPositionForLoS, forwardDir_degrees, num, num2);
	}

	private void DrawInvalidSquareIndicators(ActorData targetingActor, Vector3 coneStartPos, float forwardDir_degrees, float coneLengthSquares, float coneWidthDegrees)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			this.m_conePart.ShowHiddenSquares(this.m_indicatorHandler, coneStartPos, forwardDir_degrees, targetingActor, this.m_penetrateLoS);
			base.HideUnusedSquareIndicators();
		}
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		bool result = false;
		if (actor == caster)
		{
			result = this.m_includeCaster;
		}
		else
		{
			if (actor.GetTeam() == caster.GetTeam())
			{
				if (this.m_includeAllies)
				{
					return true;
				}
			}
			if (actor.GetTeam() != caster.GetTeam())
			{
				if (this.m_includeEnemies)
				{
					result = true;
				}
			}
		}
		return result;
	}

	public delegate bool UseExtraKnockbackDistDelegate(ActorData caster);

	public delegate float ConeLengthSquaresOverrideDelegate();
}
