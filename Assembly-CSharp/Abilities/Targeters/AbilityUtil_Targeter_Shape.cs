using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Shape : AbilityUtil_Targeter
{
	public enum DamageOriginType
	{
		CenterOfShape,
		CasterPos
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape);
	public delegate Vector3 CustomCenterPosDelegate(ActorData caster, AbilityTarget currentTarget);

	public AbilityAreaShape m_shape;
	public bool m_penetrateLoS;
	public AffectsActor m_affectsCaster;
	public AffectsActor m_affectsBestTarget;

	private float m_heightOffset = 0.1f;
	private float m_curSpeed;
	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;
	protected AbilityTooltipSubject m_enemyTooltipSubject;
	protected AbilityTooltipSubject m_allyTooltipSubject;
	protected AbilityTooltipSubject m_casterTooltipSubject;

	public DamageOriginType m_damageOriginType;
	public ActorData m_lastCenterSquareActor;
	public IsAffectingCasterDelegate m_affectCasterDelegate;
	public CustomCenterPosDelegate m_customCenterPosDelegate;

	private GridPos m_currentGridPos = GridPos.s_invalid;

	public bool UseGridPosSquarePosAsFreePos { get; set; }

	public AbilityUtil_Targeter_Shape(
		Ability ability,
		AbilityAreaShape shape,
		bool penetrateLoS,
		DamageOriginType damageOriginType = DamageOriginType.CenterOfShape,
		bool affectsEnemies = true,
		bool affectsAllies = false,
		AffectsActor affectsCaster = AffectsActor.Possible,
		AffectsActor affectsBestTarget = AffectsActor.Possible)
		: base(ability)
	{
		m_shape = shape;
		m_penetrateLoS = penetrateLoS;
		m_damageOriginType = damageOriginType;
		m_affectsCaster = affectsCaster;
		m_affectsBestTarget = affectsBestTarget;
		m_affectsEnemies = affectsEnemies;
		m_affectsAllies = affectsAllies;
		m_enemyTooltipSubject = AbilityTooltipSubject.Primary;
		m_allyTooltipSubject = AbilityTooltipSubject.Primary;
		UseGridPosSquarePosAsFreePos = false;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_showArcToShape = HighlightUtils.Get().m_showTargetingArcsForShapes;
	}

	public GridPos GetCurrentGridPos()
	{
		return m_currentGridPos;
	}

	public void SetTooltipSubjectTypes(
		AbilityTooltipSubject enemySubject = AbilityTooltipSubject.Primary,
		AbilityTooltipSubject allySubject = AbilityTooltipSubject.Primary,
		AbilityTooltipSubject casterSubject = AbilityTooltipSubject.None)
	{
		m_enemyTooltipSubject = enemySubject;
		m_allyTooltipSubject = allySubject;
		m_casterTooltipSubject = casterSubject;
	}

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare == null)
		{
			return Vector3.zero;
		}
		Vector3 freePos = UseGridPosSquarePosAsFreePos
			? gameplayRefSquare.ToVector3()
			: currentTarget.FreePos;
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, freePos, gameplayRefSquare);
		centerOfShape.y = targetingActor.GetFreePos().y + m_heightOffset;
		return centerOfShape;
	}

	protected BoardSquare GetGameplayRefSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (m_customCenterPosDelegate != null)
		{
			Vector3 vector2D = m_customCenterPosDelegate(targetingActor, currentTarget);
			return Board.Get().GetSquareFromVec3(vector2D);
		}
		else
		{
			GridPos gridPos = GetCurrentRangeInSquares() != 0f
				? currentTarget.GridPos
				: targetingActor.GetGridPos();
			return Board.Get().GetSquare(gridPos);
		}
	}

	public override void UpdateHighlightPosAfterClick(AbilityTarget target, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (Highlight != null)
		{
			Vector3 highlightGoalPos = GetHighlightGoalPos(target, targetingActor);
			Highlight.transform.position = highlightGoalPos;
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		m_currentGridPos = currentTarget.GridPos;
		ClearActorsInRange();
		m_lastCenterSquareActor = null;
		bool casterIsActive = GameFlowData.Get().activeOwnedActorData == targetingActor;
		if (casterIsActive)
		{
			ResetSquareIndicatorIndexToUse();
		}
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor);
			if (Highlight == null)
			{
				Highlight = HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData);
				Highlight.transform.position = highlightGoalPos;
			}
			else
			{
				Highlight.transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, Highlight, ref m_curSpeed);
			}
			Highlight.SetActive(true);
			Vector3 freePos = UseGridPosSquarePosAsFreePos
				? gameplayRefSquare.ToVector3()
				: currentTarget.FreePos;
			Vector3 damageOrigin = m_damageOriginType == DamageOriginType.CasterPos
				? targetingActor.GetLoSCheckPos()
				: AreaEffectUtils.GetCenterOfShape(m_shape, freePos, gameplayRefSquare);
			List<ActorData> actors = AreaEffectUtils.GetActorsInShape(
				m_shape,
				freePos,
				gameplayRefSquare,
				m_penetrateLoS,
				targetingActor,
				GetAffectedTeams(),
				null);
			actors.Remove(targetingActor);
			bool isCasterInShape = AreaEffectUtils.IsSquareInShape(
				targetingActor.GetCurrentBoardSquare(),
				m_shape,
				freePos, 
				gameplayRefSquare,
				m_penetrateLoS,
				targetingActor);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			if (m_affectsCaster == AffectsActor.Possible)
			{
				bool includeCaster = m_affectCasterDelegate?.Invoke(targetingActor, actors, isCasterInShape) ?? isCasterInShape;
				if (includeCaster)
				{
					actors.Add(targetingActor);
				}
			}
			ActorData actorData = currentTarget.GetCurrentBestActorTarget();
			if (actorData != null && !actorData.IsActorVisibleToClient())
			{
				actorData = null;
			}
			m_lastCenterSquareActor = actorData;
			foreach (ActorData current in actors)
			{
				HandleAddActorInShape(current, targetingActor, currentTarget, damageOrigin, actorData);
			}
			if (m_affectsCaster == AffectsActor.Always)
			{
				AbilityTooltipSubject abilityTooltipSubject = m_casterTooltipSubject;
				if (abilityTooltipSubject == AbilityTooltipSubject.None)
				{
					abilityTooltipSubject = m_allyTooltipSubject;
				}
				AddActorInRange(targetingActor, damageOrigin, targetingActor, abilityTooltipSubject);
			}
			if (m_affectsBestTarget == AffectsActor.Always && actorData != null)
			{
				AddActorInRange(
					actorData,
					damageOrigin,
					targetingActor,
					actorData.GetTeam() == targetingActor.GetTeam() ? m_allyTooltipSubject : m_enemyTooltipSubject);
			}
			if (casterIsActive)
			{
				AreaEffectUtils.OperateOnSquaresInShape(m_indicatorHandler, m_shape, freePos, gameplayRefSquare, m_penetrateLoS, targetingActor);
			}
		}
		if (casterIsActive)
		{
			HideUnusedSquareIndicators();
		}
	}

	protected virtual bool HandleAddActorInShape(
		ActorData potentialTarget,
		ActorData targetingActor,
		AbilityTarget currentTarget,
		Vector3 damageOrigin,
		ActorData bestTarget)
	{
		if (potentialTarget == targetingActor && m_affectsCaster != AffectsActor.Possible
		    || potentialTarget == bestTarget && m_affectsBestTarget != AffectsActor.Possible)
		{
			return false;
		}
		if (potentialTarget == targetingActor)
		{
			AbilityTooltipSubject abilityTooltipSubject = m_casterTooltipSubject;
			if (abilityTooltipSubject == AbilityTooltipSubject.None)
			{
				abilityTooltipSubject = m_allyTooltipSubject;
			}
			AddActorInRange(potentialTarget, damageOrigin, targetingActor, abilityTooltipSubject);
		}
		AddActorInRange(
			potentialTarget,
			damageOrigin,
			targetingActor,
			potentialTarget.GetTeam() == targetingActor.GetTeam() ? m_allyTooltipSubject : m_enemyTooltipSubject);
		return true;
	}
}
