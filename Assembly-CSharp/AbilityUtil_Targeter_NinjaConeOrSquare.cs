using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_NinjaConeOrSquare : AbilityUtil_Targeter
{
	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape);

	private Ninja_SyncComponent m_syncComp;

	public ConeTargetingInfo m_coneInfo;

	public AbilityAreaShape m_shape;

	public bool m_penetrateLoS;

	public AffectsActor m_affectsCaster;

	public AffectsActor m_affectsBestTarget;

	private bool m_dashOnlyMode;

	private float m_heightOffset = 0.1f;

	private float m_curSpeed;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	protected AbilityTooltipSubject m_enemyTooltipSubject;

	protected AbilityTooltipSubject m_allyTooltipSubject;

	protected AbilityTooltipSubject m_casterTooltipSubject;

	public ActorData m_lastCenterSquareActor;

	public IsAffectingCasterDelegate m_affectCasterDelegate;

	private GridPos m_currentGridPos = GridPos.s_invalid;

	public bool ShowTeleportLines
	{
		get;
		set;
	}

	public AbilityUtil_Targeter_NinjaConeOrSquare(Ability ability, Ninja_SyncComponent syncComp, ConeTargetingInfo coneInfo, AbilityAreaShape shape, bool penetrateLoS, bool dashOnlyMode, bool affectsEnemies = true, bool affectsAllies = false, AffectsActor affectsCaster = AffectsActor.Possible, AffectsActor affectsBestTarget = AffectsActor.Possible)
		: base(ability)
	{
		m_syncComp = syncComp;
		m_coneInfo = coneInfo;
		m_shape = shape;
		m_penetrateLoS = penetrateLoS;
		m_dashOnlyMode = dashOnlyMode;
		m_affectsCaster = affectsCaster;
		m_affectsBestTarget = affectsBestTarget;
		m_affectsEnemies = affectsEnemies;
		m_affectsAllies = affectsAllies;
		m_enemyTooltipSubject = AbilityTooltipSubject.Primary;
		m_allyTooltipSubject = AbilityTooltipSubject.Primary;
		m_showArcToShape = false;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public GridPos GetCurrentGridPos()
	{
		return m_currentGridPos;
	}

	public void SetTooltipSubjectTypes(AbilityTooltipSubject enemySubject = AbilityTooltipSubject.Primary, AbilityTooltipSubject allySubject = AbilityTooltipSubject.Primary, AbilityTooltipSubject casterSubject = AbilityTooltipSubject.None)
	{
		m_enemyTooltipSubject = enemySubject;
		m_allyTooltipSubject = allySubject;
		m_casterTooltipSubject = casterSubject;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		m_currentGridPos = currentTarget.GridPos;
		ClearActorsInRange();
		CreateHighlightObjects(targetingActor);
		GameObject gameObject = m_highlights[0];
		GameObject gameObject2 = m_highlights[1];
		m_lastCenterSquareActor = null;
		if (m_syncComp == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (!m_syncComp.m_shurikenDashingThisTurn)
		{
			if (!m_dashOnlyMode)
			{
				gameObject2.SetActive(true);
				gameObject.SetActive(false);
				Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetLoSCheckPos();
				float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
				List<ActorData> actors = AreaEffectUtils.GetActorsInCone(travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, m_coneInfo.m_widthAngleDeg, m_coneInfo.m_radiusInSquares, m_coneInfo.m_backwardsOffset, m_penetrateLoS, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies), null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
				if (m_affectsTargetingActor && !actors.Contains(targetingActor))
				{
					actors.Add(targetingActor);
				}
				for (int i = 0; i < actors.Count; i++)
				{
					ActorData actor = actors[i];
					AddActorInRange(actor, travelBoardSquareWorldPositionForLos, targetingActor);
				}
				float d = m_coneInfo.m_backwardsOffset * Board.Get().squareSize;
				Vector3 position = travelBoardSquareWorldPositionForLos - currentTarget.AimDirection * d;
				position.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = position;
				gameObject2.transform.rotation = Quaternion.LookRotation(currentTarget.AimDirection);
				SetMovementArrowEnabledFromIndex(0, false);
				return;
			}
		}
		ResetSquareIndicatorIndexToUse();
		HideUnusedSquareIndicators();
		int fromIndex = 0;
		EnableAllMovementArrows();
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor);
			gameObject.transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, base.Highlight, ref m_curSpeed);
			gameObject.SetActive(true);
			gameObject2.SetActive(false);
			Vector3 freePos = currentTarget.FreePos;
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, freePos, gameplayRefSquare);
			List<ActorData> actors2 = AreaEffectUtils.GetActorsInShape(m_shape, freePos, gameplayRefSquare, m_penetrateLoS, targetingActor, GetAffectedTeams(), null);
			actors2.Remove(targetingActor);
			bool flag = AreaEffectUtils.IsSquareInShape(targetingActor.GetCurrentBoardSquare(), m_shape, freePos, gameplayRefSquare, m_penetrateLoS, targetingActor);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors2);
			if (m_affectsCaster == AffectsActor.Possible)
			{
				if ((m_affectCasterDelegate != null) ? m_affectCasterDelegate(targetingActor, actors2, flag) : flag)
				{
					actors2.Add(targetingActor);
				}
			}
			ActorData actorData = currentTarget.GetCurrentBestActorTarget();
			if (actorData != null)
			{
				if (!actorData.IsActorVisibleToClient())
				{
					actorData = null;
				}
			}
			m_lastCenterSquareActor = actorData;
			using (List<ActorData>.Enumerator enumerator = actors2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					HandleAddActorInShape(current, targetingActor, currentTarget, centerOfShape, actorData);
				}
			}
			if (m_affectsCaster == AffectsActor.Always)
			{
				AbilityTooltipSubject abilityTooltipSubject = m_casterTooltipSubject;
				if (abilityTooltipSubject == AbilityTooltipSubject.None)
				{
					abilityTooltipSubject = m_allyTooltipSubject;
				}
				AddActorInRange(targetingActor, centerOfShape, targetingActor, abilityTooltipSubject);
			}
			if (m_affectsBestTarget == AffectsActor.Always)
			{
				if (actorData != null)
				{
					if (actorData.GetTeam() == targetingActor.GetTeam())
					{
						AddActorInRange(actorData, centerOfShape, targetingActor, m_allyTooltipSubject);
					}
					else
					{
						AddActorInRange(actorData, centerOfShape, targetingActor, m_enemyTooltipSubject);
					}
				}
			}
			if (ShowTeleportLines)
			{
				InstantiateTeleportPathUIEffect();
				UpdateEffectOnCaster(currentTarget, targetingActor);
				UpdateTargetAreaEffect(currentTarget, targetingActor);
			}
			else
			{
				BoardSquare currentBoardSquare = targetingActor.GetCurrentBoardSquare();
				BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, gameplayRefSquare, currentBoardSquare, true);
				fromIndex = AddMovementArrowWithPrevious(targetingActor, path, TargeterMovementType.Movement, 0);
			}
		}
		SetMovementArrowEnabledFromIndex(fromIndex, false);
	}

	protected virtual bool HandleAddActorInShape(ActorData potentialTarget, ActorData targetingActor, AbilityTarget currentTarget, Vector3 damageOrigin, ActorData bestTarget)
	{
		int num;
		if (!(potentialTarget != targetingActor))
		{
			num = ((m_affectsCaster == AffectsActor.Possible) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		bool flag2 = potentialTarget != bestTarget || m_affectsBestTarget == AffectsActor.Possible;
		if (flag)
		{
			if (flag2)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (potentialTarget == targetingActor)
						{
							AbilityTooltipSubject abilityTooltipSubject = m_casterTooltipSubject;
							if (abilityTooltipSubject == AbilityTooltipSubject.None)
							{
								abilityTooltipSubject = m_allyTooltipSubject;
							}
							AddActorInRange(potentialTarget, damageOrigin, targetingActor, abilityTooltipSubject);
						}
						if (potentialTarget.GetTeam() == targetingActor.GetTeam())
						{
							AddActorInRange(potentialTarget, damageOrigin, targetingActor, m_allyTooltipSubject);
						}
						else
						{
							AddActorInRange(potentialTarget, damageOrigin, targetingActor, m_enemyTooltipSubject);
						}
						return true;
					}
				}
			}
		}
		return false;
	}

	private void CreateHighlightObjects(ActorData targetingActor)
	{
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 2)
			{
				return;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		m_highlights.Add(HighlightUtils.Get().CreateConeCursor(m_coneInfo.m_radiusInSquares * Board.Get().squareSize, m_coneInfo.m_widthAngleDeg));
	}

	protected BoardSquare GetGameplayRefSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return Board.Get().GetSquare(currentTarget.GridPos);
	}

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 freePos = currentTarget.FreePos;
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, freePos, gameplayRefSquare);
			Vector3 travelBoardSquareWorldPosition = targetingActor.GetFreePos();
			centerOfShape.y = travelBoardSquareWorldPosition.y + m_heightOffset;
			return centerOfShape;
		}
		return Vector3.zero;
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 coneStartPos, Vector3 forwardDirection)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(forwardDirection);
			AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, coneStartPos, coneCenterAngleDegrees, m_coneInfo.m_widthAngleDeg, m_coneInfo.m_radiusInSquares, m_coneInfo.m_backwardsOffset, targetingActor, m_penetrateLoS);
			HideUnusedSquareIndicators();
		}
	}
}
