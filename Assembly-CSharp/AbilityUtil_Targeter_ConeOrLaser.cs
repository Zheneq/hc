﻿using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_ConeOrLaser : AbilityUtil_Targeter
{
	private ConeTargetingInfo m_coneInfo;

	private LaserTargetingInfo m_laserInfo;

	private float m_toConeThreshold;

	private const int c_coneHighlightIndex = 0;

	private const int c_laserHighlightIndex = 1;

	private const int c_threshHighlightIndex = 2;

	private TargeterPart_Cone m_conePart;

	private TargeterPart_Laser m_laserPart;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_ConeOrLaser.ShouldAddCasterDelegate m_customShouldAddCasterDelegate;

	public bool m_useCasterLocationForAllMultiTargets;

	public bool m_updatingWithCone;

	public AbilityUtil_Targeter_ConeOrLaser(Ability ability, ConeTargetingInfo coneInfo, LaserTargetingInfo laserInfo, float coneThresholdDist) : base(ability)
	{
		this.m_coneInfo = coneInfo;
		this.m_laserInfo = laserInfo;
		this.m_toConeThreshold = coneThresholdDist;
		base.SetAffectedGroups(coneInfo.m_affectsEnemies, coneInfo.m_affectsAllies, coneInfo.m_affectsCaster);
		this.m_conePart = new TargeterPart_Cone(this.m_coneInfo);
		this.m_laserPart = new TargeterPart_Laser(this.m_laserInfo);
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
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
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 3)
			{
				this.m_highlights[2].SetActive(false);
			}
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		this.CreateHighlights();
		Vector3 vector = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector2 = currentTarget.AimDirection;
		if (currentTargetIndex > 0)
		{
			if (targets != null)
			{
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(targets[currentTargetIndex - 1].GridPos);
				if (boardSquareSafe != null)
				{
					if (!this.m_useCasterLocationForAllMultiTargets)
					{
						vector = boardSquareSafe.GetWorldPositionForLoS();
					}
					vector2 = currentTarget.FreePos - vector;
					vector2.y = 0f;
					if (vector2.sqrMagnitude == 0f)
					{
						vector2 = Vector3.forward;
					}
					else
					{
						vector2.Normalize();
					}
				}
			}
		}
		Vector3 vector3 = currentTarget.FreePos - vector;
		vector3.y = 0f;
		float magnitude = vector3.magnitude;
		bool flag = magnitude <= this.m_toConeThreshold;
		this.m_updatingWithCone = flag;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, this.m_affectsAllies, this.m_affectsEnemies);
		GameObject gameObject = this.m_highlights[0];
		GameObject gameObject2 = this.m_highlights[1];
		GameObject gameObject3 = this.m_highlights[2];
		List<ActorData> hitActors;
		if (flag)
		{
			hitActors = this.m_conePart.GetHitActors(vector, vector2, targetingActor, relevantTeams);
			this.m_conePart.AdjustHighlight(gameObject, vector, vector2);
			gameObject.SetActive(true);
			gameObject2.SetActive(false);
			this.DrawInvalidSquareIndicators_Cone(currentTarget, targetingActor, vector, vector2);
		}
		else
		{
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = vector;
			hitActors = this.m_laserPart.GetHitActors(laserCoords.start, vector2, targetingActor, relevantTeams, out laserCoords.end);
			this.m_laserPart.AdjustHighlight(gameObject2, laserCoords.start, laserCoords.end, true);
			gameObject.SetActive(false);
			gameObject2.SetActive(true);
			this.DrawInvalidSquareIndicators_Laser(currentTarget, targetingActor, laserCoords.start, laserCoords.end);
		}
		if (gameObject3 != null)
		{
			Vector3 position = vector;
			position.y = HighlightUtils.GetHighlightHeight();
			gameObject3.transform.position = position;
			gameObject3.transform.rotation = Quaternion.LookRotation(vector2);
		}
		TargeterUtils.RemoveActorsInvisibleToClient(ref hitActors);
		using (List<ActorData>.Enumerator enumerator = hitActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				ActorData actor = actorData;
				Vector3 damageOrigin = vector;
				AbilityTooltipSubject subjectType;
				if (flag)
				{
					subjectType = AbilityTooltipSubject.Primary;
				}
				else
				{
					subjectType = AbilityTooltipSubject.Secondary;
				}
				base.AddActorInRange(actor, damageOrigin, targetingActor, subjectType, false);
				ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
				float value = VectorUtils.HorizontalPlaneDistInSquares(vector, actorData.GetTravelBoardSquareWorldPosition());
				actorHitContext.symbol_0015.SetFloat(ContextKeys.symbol_0018.GetHash(), value);
				ContextVars u = actorHitContext.symbol_0015;
				int hash = TargetSelect_ConeOrLaser.s_cvarInCone.GetHash();
				int value2;
				if (flag)
				{
					value2 = 1;
				}
				else
				{
					value2 = 0;
				}
				u.SetInt(hash, value2);
				if (actorData != targetingActor)
				{
					if (flag)
					{
						Vector3 to = actorData.GetTravelBoardSquareWorldPosition() - targetingActor.GetTravelBoardSquareWorldPosition();
						to.y = 0f;
						float value3 = Vector3.Angle(vector2, to);
						actorHitContext.symbol_0015.SetFloat(ContextKeys.symbol_001D.GetHash(), value3);
					}
				}
			}
		}
		if (this.m_affectsTargetingActor && !hitActors.Contains(targetingActor))
		{
			if (this.m_customShouldAddCasterDelegate != null)
			{
				if (!this.m_customShouldAddCasterDelegate(targetingActor, hitActors))
				{
					return;
				}
			}
			base.AddActorInRange(targetingActor, vector, targetingActor, AbilityTooltipSubject.Self, false);
		}
	}

	private void CreateHighlights()
	{
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 3)
			{
				return;
			}
		}
		this.m_highlights = new List<GameObject>();
		GameObject item = this.m_conePart.CreateHighlightObject(this);
		this.m_highlights.Add(item);
		this.m_highlights[0].SetActive(false);
		GameObject item2 = this.m_laserPart.CreateHighlightObject(this);
		this.m_highlights.Add(item2);
		this.m_highlights[0].SetActive(false);
		float num = 1.2f;
		GameObject gameObject = HighlightUtils.Get().CreateDynamicLineSegmentMesh(num, 0.5f, false, Color.cyan);
		gameObject.transform.localPosition = new Vector3(-0.5f * Board.Get().squareSize * num, 0f, this.m_toConeThreshold);
		gameObject.transform.localRotation = Quaternion.LookRotation(new Vector3(1f, 0f, 0f));
		GameObject gameObject2 = new GameObject();
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject.transform.parent = gameObject2.transform;
		this.m_highlights.Add(gameObject2);
	}

	private void DrawInvalidSquareIndicators_Cone(AbilityTarget currentTarget, ActorData targetingActor, Vector3 coneStartPos, Vector3 forwardDirection)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			this.m_conePart.ShowHiddenSquares(this.m_indicatorHandler, coneStartPos, VectorUtils.HorizontalAngle_Deg(forwardDirection), targetingActor, this.m_coneInfo.m_penetrateLos);
			base.HideUnusedSquareIndicators();
		}
	}

	private void DrawInvalidSquareIndicators_Laser(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			this.m_laserPart.ShowHiddenSquares(this.m_indicatorHandler, startPos, endPos, targetingActor, this.m_laserInfo.penetrateLos);
			base.HideUnusedSquareIndicators();
		}
	}

	public delegate bool ShouldAddCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);
}
