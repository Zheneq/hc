using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_NinjaConeOrSquare : AbilityUtil_Targeter
{
	private Ninja_SyncComponent m_syncComp;

	public ConeTargetingInfo m_coneInfo;

	public AbilityAreaShape m_shape;

	public bool m_penetrateLoS;

	public AbilityUtil_Targeter.AffectsActor m_affectsCaster;

	public AbilityUtil_Targeter.AffectsActor m_affectsBestTarget;

	private bool m_dashOnlyMode;

	private float m_heightOffset = 0.1f;

	private float m_curSpeed;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	protected AbilityTooltipSubject m_enemyTooltipSubject;

	protected AbilityTooltipSubject m_allyTooltipSubject;

	protected AbilityTooltipSubject m_casterTooltipSubject;

	public ActorData m_lastCenterSquareActor;

	public AbilityUtil_Targeter_NinjaConeOrSquare.IsAffectingCasterDelegate m_affectCasterDelegate;

	private GridPos m_currentGridPos = GridPos.s_invalid;

	public AbilityUtil_Targeter_NinjaConeOrSquare(Ability ability, Ninja_SyncComponent syncComp, ConeTargetingInfo coneInfo, AbilityAreaShape shape, bool penetrateLoS, bool dashOnlyMode, bool affectsEnemies = true, bool affectsAllies = false, AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Possible) : base(ability)
	{
		this.m_syncComp = syncComp;
		this.m_coneInfo = coneInfo;
		this.m_shape = shape;
		this.m_penetrateLoS = penetrateLoS;
		this.m_dashOnlyMode = dashOnlyMode;
		this.m_affectsCaster = affectsCaster;
		this.m_affectsBestTarget = affectsBestTarget;
		this.m_affectsEnemies = affectsEnemies;
		this.m_affectsAllies = affectsAllies;
		this.m_enemyTooltipSubject = AbilityTooltipSubject.Primary;
		this.m_allyTooltipSubject = AbilityTooltipSubject.Primary;
		this.m_showArcToShape = false;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public bool ShowTeleportLines { get; set; }

	public GridPos GetCurrentGridPos()
	{
		return this.m_currentGridPos;
	}

	public void SetTooltipSubjectTypes(AbilityTooltipSubject enemySubject = AbilityTooltipSubject.Primary, AbilityTooltipSubject allySubject = AbilityTooltipSubject.Primary, AbilityTooltipSubject casterSubject = AbilityTooltipSubject.None)
	{
		this.m_enemyTooltipSubject = enemySubject;
		this.m_allyTooltipSubject = allySubject;
		this.m_casterTooltipSubject = casterSubject;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		this.m_currentGridPos = currentTarget.GridPos;
		base.ClearActorsInRange();
		this.CreateHighlightObjects(targetingActor);
		GameObject gameObject = this.m_highlights[0];
		GameObject gameObject2 = this.m_highlights[1];
		this.m_lastCenterSquareActor = null;
		if (this.m_syncComp == null)
		{
			return;
		}
		if (!this.m_syncComp.m_shurikenDashingThisTurn)
		{
			if (!this.m_dashOnlyMode)
			{
				gameObject2.SetActive(true);
				gameObject.SetActive(false);
				Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
				float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
				List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, this.m_coneInfo.m_widthAngleDeg, this.m_coneInfo.m_radiusInSquares, this.m_coneInfo.m_backwardsOffset, this.m_penetrateLoS, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, this.m_affectsAllies, this.m_affectsEnemies), null, false, default(Vector3));
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
				if (this.m_affectsTargetingActor && !actorsInCone.Contains(targetingActor))
				{
					actorsInCone.Add(targetingActor);
				}
				for (int i = 0; i < actorsInCone.Count; i++)
				{
					ActorData actor = actorsInCone[i];
					base.AddActorInRange(actor, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Primary, false);
				}
				float d = this.m_coneInfo.m_backwardsOffset * Board.Get().squareSize;
				Vector3 position = travelBoardSquareWorldPositionForLos - currentTarget.AimDirection * d;
				position.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = position;
				gameObject2.transform.rotation = Quaternion.LookRotation(currentTarget.AimDirection);
				base.SetMovementArrowEnabledFromIndex(0, false);
				return;
			}
		}
		base.ResetSquareIndicatorIndexToUse();
		base.HideUnusedSquareIndicators();
		int fromIndex = 0;
		base.EnableAllMovementArrows();
		BoardSquare gameplayRefSquare = this.GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 highlightGoalPos = this.GetHighlightGoalPos(currentTarget, targetingActor);
			gameObject.transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, base.Highlight, ref this.m_curSpeed);
			gameObject.SetActive(true);
			gameObject2.SetActive(false);
			Vector3 freePos = currentTarget.FreePos;
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, freePos, gameplayRefSquare);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_shape, freePos, gameplayRefSquare, this.m_penetrateLoS, targetingActor, base.GetAffectedTeams(), null);
			actorsInShape.Remove(targetingActor);
			bool flag = AreaEffectUtils.IsSquareInShape(targetingActor.GetCurrentBoardSquare(), this.m_shape, freePos, gameplayRefSquare, this.m_penetrateLoS, targetingActor);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
			if (this.m_affectsCaster == AbilityUtil_Targeter.AffectsActor.Possible)
			{
				bool flag2 = (this.m_affectCasterDelegate != null) ? this.m_affectCasterDelegate(targetingActor, actorsInShape, flag) : flag;
				if (flag2)
				{
					actorsInShape.Add(targetingActor);
				}
			}
			ActorData actorData = currentTarget.GetCurrentBestActorTarget();
			if (actorData != null)
			{
				if (!actorData.IsVisibleToClient())
				{
					actorData = null;
				}
			}
			this.m_lastCenterSquareActor = actorData;
			using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData potentialTarget = enumerator.Current;
					this.HandleAddActorInShape(potentialTarget, targetingActor, currentTarget, centerOfShape, actorData);
				}
			}
			if (this.m_affectsCaster == AbilityUtil_Targeter.AffectsActor.Always)
			{
				AbilityTooltipSubject abilityTooltipSubject = this.m_casterTooltipSubject;
				if (abilityTooltipSubject == AbilityTooltipSubject.None)
				{
					abilityTooltipSubject = this.m_allyTooltipSubject;
				}
				base.AddActorInRange(targetingActor, centerOfShape, targetingActor, abilityTooltipSubject, false);
			}
			if (this.m_affectsBestTarget == AbilityUtil_Targeter.AffectsActor.Always)
			{
				if (actorData != null)
				{
					if (actorData.GetTeam() == targetingActor.GetTeam())
					{
						base.AddActorInRange(actorData, centerOfShape, targetingActor, this.m_allyTooltipSubject, false);
					}
					else
					{
						base.AddActorInRange(actorData, centerOfShape, targetingActor, this.m_enemyTooltipSubject, false);
					}
				}
			}
			if (this.ShowTeleportLines)
			{
				base.InstantiateTeleportPathUIEffect();
				base.UpdateEffectOnCaster(currentTarget, targetingActor);
				base.UpdateTargetAreaEffect(currentTarget, targetingActor);
			}
			else
			{
				BoardSquare currentBoardSquare = targetingActor.GetCurrentBoardSquare();
				BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, gameplayRefSquare, currentBoardSquare, true);
				fromIndex = base.AddMovementArrowWithPrevious(targetingActor, path, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
			}
		}
		base.SetMovementArrowEnabledFromIndex(fromIndex, false);
	}

	protected virtual bool HandleAddActorInShape(ActorData potentialTarget, ActorData targetingActor, AbilityTarget currentTarget, Vector3 damageOrigin, ActorData bestTarget)
	{
		bool flag;
		if (!(potentialTarget != targetingActor))
		{
			flag = (this.m_affectsCaster == AbilityUtil_Targeter.AffectsActor.Possible);
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		bool flag3 = potentialTarget != bestTarget || this.m_affectsBestTarget == AbilityUtil_Targeter.AffectsActor.Possible;
		if (flag2)
		{
			if (flag3)
			{
				if (potentialTarget == targetingActor)
				{
					AbilityTooltipSubject abilityTooltipSubject = this.m_casterTooltipSubject;
					if (abilityTooltipSubject == AbilityTooltipSubject.None)
					{
						abilityTooltipSubject = this.m_allyTooltipSubject;
					}
					base.AddActorInRange(potentialTarget, damageOrigin, targetingActor, abilityTooltipSubject, false);
				}
				if (potentialTarget.GetTeam() == targetingActor.GetTeam())
				{
					base.AddActorInRange(potentialTarget, damageOrigin, targetingActor, this.m_allyTooltipSubject, false);
				}
				else
				{
					base.AddActorInRange(potentialTarget, damageOrigin, targetingActor, this.m_enemyTooltipSubject, false);
				}
				return true;
			}
		}
		return false;
	}

	private void CreateHighlightObjects(ActorData targetingActor)
	{
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 2)
			{
				return;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		this.m_highlights.Add(HighlightUtils.Get().CreateConeCursor(this.m_coneInfo.m_radiusInSquares * Board.Get().squareSize, this.m_coneInfo.m_widthAngleDeg));
	}

	protected BoardSquare GetGameplayRefSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
	}

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = this.GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 freePos = currentTarget.FreePos;
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, freePos, gameplayRefSquare);
			centerOfShape.y = targetingActor.GetTravelBoardSquareWorldPosition().y + this.m_heightOffset;
			return centerOfShape;
		}
		return Vector3.zero;
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 coneStartPos, Vector3 forwardDirection)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(forwardDirection);
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, coneStartPos, coneCenterAngleDegrees, this.m_coneInfo.m_widthAngleDeg, this.m_coneInfo.m_radiusInSquares, this.m_coneInfo.m_backwardsOffset, targetingActor, this.m_penetrateLoS, null);
			base.HideUnusedSquareIndicators();
		}
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape);
}
