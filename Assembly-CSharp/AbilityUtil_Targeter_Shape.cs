using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Shape : AbilityUtil_Targeter
{
	public AbilityAreaShape m_shape;

	public bool m_penetrateLoS;

	public AbilityUtil_Targeter.AffectsActor m_affectsCaster;

	public AbilityUtil_Targeter.AffectsActor m_affectsBestTarget;

	private float m_heightOffset = 0.1f;

	private float m_curSpeed;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	protected AbilityTooltipSubject m_enemyTooltipSubject;

	protected AbilityTooltipSubject m_allyTooltipSubject;

	protected AbilityTooltipSubject m_casterTooltipSubject;

	public AbilityUtil_Targeter_Shape.DamageOriginType m_damageOriginType;

	public ActorData m_lastCenterSquareActor;

	public AbilityUtil_Targeter_Shape.IsAffectingCasterDelegate m_affectCasterDelegate;

	public AbilityUtil_Targeter_Shape.CustomCenterPosDelegate m_customCenterPosDelegate;

	private GridPos m_currentGridPos = GridPos.s_invalid;

	public AbilityUtil_Targeter_Shape(Ability ability, AbilityAreaShape shape, bool penetrateLoS, AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, bool affectsEnemies = true, bool affectsAllies = false, AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Possible) : base(ability)
	{
		this.m_shape = shape;
		this.m_penetrateLoS = penetrateLoS;
		this.m_damageOriginType = damageOriginType;
		this.m_affectsCaster = affectsCaster;
		this.m_affectsBestTarget = affectsBestTarget;
		this.m_affectsEnemies = affectsEnemies;
		this.m_affectsAllies = affectsAllies;
		this.m_enemyTooltipSubject = AbilityTooltipSubject.Primary;
		this.m_allyTooltipSubject = AbilityTooltipSubject.Primary;
		this.UseGridPosSquarePosAsFreePos = false;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_showArcToShape = HighlightUtils.Get().m_showTargetingArcsForShapes;
	}

	public bool UseGridPosSquarePosAsFreePos { get; set; }

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

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = this.GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 vector;
			if (this.UseGridPosSquarePosAsFreePos)
			{
				vector = gameplayRefSquare.ToVector3();
			}
			else
			{
				vector = currentTarget.FreePos;
			}
			Vector3 freePos = vector;
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, freePos, gameplayRefSquare);
			centerOfShape.y = targetingActor.GetTravelBoardSquareWorldPosition().y + this.m_heightOffset;
			return centerOfShape;
		}
		return Vector3.zero;
	}

	protected BoardSquare GetGameplayRefSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare result;
		if (this.m_customCenterPosDelegate != null)
		{
			Vector3 vector2D = this.m_customCenterPosDelegate(targetingActor, currentTarget);
			result = Board.Get().GetBoardSquare(vector2D);
		}
		else
		{
			GridPos gridPos;
			if (this.GetCurrentRangeInSquares() != 0f)
			{
				gridPos = currentTarget.GridPos;
			}
			else
			{
				gridPos = targetingActor.GetGridPosWithIncrementedHeight();
			}
			result = Board.Get().GetBoardSquareSafe(gridPos);
		}
		return result;
	}

	public override void UpdateHighlightPosAfterClick(AbilityTarget target, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (base.Highlight != null)
		{
			Vector3 highlightGoalPos = this.GetHighlightGoalPos(target, targetingActor);
			base.Highlight.transform.position = highlightGoalPos;
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		this.m_currentGridPos = currentTarget.GridPos;
		base.ClearActorsInRange();
		this.m_lastCenterSquareActor = null;
		bool flag = GameFlowData.Get().activeOwnedActorData == targetingActor;
		if (flag)
		{
			base.ResetSquareIndicatorIndexToUse();
		}
		BoardSquare gameplayRefSquare = this.GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 highlightGoalPos = this.GetHighlightGoalPos(currentTarget, targetingActor);
			if (base.Highlight == null)
			{
				base.Highlight = HighlightUtils.Get().CreateShapeCursor(this.m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData);
				base.Highlight.transform.position = highlightGoalPos;
			}
			else
			{
				base.Highlight.transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, base.Highlight, ref this.m_curSpeed);
			}
			base.Highlight.SetActive(true);
			Vector3 freePos = (!this.UseGridPosSquarePosAsFreePos) ? currentTarget.FreePos : gameplayRefSquare.ToVector3();
			Vector3 damageOrigin;
			if (this.m_damageOriginType == AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos)
			{
				damageOrigin = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			}
			else
			{
				damageOrigin = AreaEffectUtils.GetCenterOfShape(this.m_shape, freePos, gameplayRefSquare);
			}
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_shape, freePos, gameplayRefSquare, this.m_penetrateLoS, targetingActor, base.GetAffectedTeams(), null);
			actorsInShape.Remove(targetingActor);
			bool flag2 = AreaEffectUtils.IsSquareInShape(targetingActor.GetCurrentBoardSquare(), this.m_shape, freePos, gameplayRefSquare, this.m_penetrateLoS, targetingActor);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
			if (this.m_affectsCaster == AbilityUtil_Targeter.AffectsActor.Possible)
			{
				bool flag3;
				if (this.m_affectCasterDelegate == null)
				{
					flag3 = flag2;
				}
				else
				{
					flag3 = this.m_affectCasterDelegate(targetingActor, actorsInShape, flag2);
				}
				bool flag4 = flag3;
				if (flag4)
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
					this.HandleAddActorInShape(potentialTarget, targetingActor, currentTarget, damageOrigin, actorData);
				}
			}
			if (this.m_affectsCaster == AbilityUtil_Targeter.AffectsActor.Always)
			{
				AbilityTooltipSubject abilityTooltipSubject = this.m_casterTooltipSubject;
				if (abilityTooltipSubject == AbilityTooltipSubject.None)
				{
					abilityTooltipSubject = this.m_allyTooltipSubject;
				}
				base.AddActorInRange(targetingActor, damageOrigin, targetingActor, abilityTooltipSubject, false);
			}
			if (this.m_affectsBestTarget == AbilityUtil_Targeter.AffectsActor.Always)
			{
				if (actorData != null)
				{
					if (actorData.GetTeam() == targetingActor.GetTeam())
					{
						base.AddActorInRange(actorData, damageOrigin, targetingActor, this.m_allyTooltipSubject, false);
					}
					else
					{
						base.AddActorInRange(actorData, damageOrigin, targetingActor, this.m_enemyTooltipSubject, false);
					}
				}
			}
			if (flag)
			{
				AreaEffectUtils.OperateOnSquaresInShape(this.m_indicatorHandler, this.m_shape, freePos, gameplayRefSquare, this.m_penetrateLoS, targetingActor, null);
			}
		}
		if (flag)
		{
			base.HideUnusedSquareIndicators();
		}
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

	public enum DamageOriginType
	{
		CenterOfShape,
		CasterPos
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape);

	public delegate Vector3 CustomCenterPosDelegate(ActorData caster, AbilityTarget currentTarget);
}
