using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_GremlinsBombInCone : AbilityUtil_Targeter
{
	public AbilityAreaShape m_shape;

	public bool m_penetrateLoS;

	public AbilityUtil_Targeter.AffectsActor m_affectsCaster;

	private float m_heightOffset = 0.1f;

	private float m_curSpeed;

	private bool m_showAngleIndicators;

	private float m_angleWithCenter = 15f;

	private float m_angleIndicatorLength = 15f;

	protected AbilityTooltipSubject m_enemyTooltipSubject;

	protected AbilityTooltipSubject m_allyTooltipSubject;

	public AbilityUtil_Targeter_GremlinsBombInCone.DamageOriginType m_damageOriginType;

	private GridPos m_currentGridPos = GridPos.s_invalid;

	public AbilityUtil_Targeter_GremlinsBombInCone(Ability ability, AbilityAreaShape shape, bool penetrateLoS, AbilityUtil_Targeter_GremlinsBombInCone.DamageOriginType damageOriginType = AbilityUtil_Targeter_GremlinsBombInCone.DamageOriginType.CenterOfShape, bool affectsEnemies = true, bool affectsAllies = false, AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible) : base(ability)
	{
		this.m_shape = shape;
		this.m_penetrateLoS = penetrateLoS;
		this.m_damageOriginType = damageOriginType;
		this.m_affectsCaster = affectsCaster;
		this.m_affectsEnemies = affectsEnemies;
		this.m_affectsAllies = affectsAllies;
		this.m_enemyTooltipSubject = AbilityTooltipSubject.Primary;
		this.m_allyTooltipSubject = AbilityTooltipSubject.Primary;
		this.m_showArcToShape = HighlightUtils.Get().m_showTargetingArcsForShapes;
	}

	public GridPos GetCurrentGridPos()
	{
		return this.m_currentGridPos;
	}

	public void SetAngleIndicatorConfig(bool showAngleIndicator, float angleWithCenter, float indicatorLength = 15f)
	{
		this.m_showAngleIndicators = showAngleIndicator;
		this.m_angleWithCenter = angleWithCenter;
		this.m_angleIndicatorLength = indicatorLength;
	}

	public void SetTooltipSubjectTypes(AbilityTooltipSubject enemySubject = AbilityTooltipSubject.Primary, AbilityTooltipSubject allySubject = AbilityTooltipSubject.Primary)
	{
		this.m_enemyTooltipSubject = enemySubject;
		this.m_allyTooltipSubject = allySubject;
	}

	protected BoardSquare GetGameplayRefSquare(AbilityTarget currentTarget, ActorData targetingActor)
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
		return Board.Get().GetBoardSquareSafe(gridPos);
	}

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = this.GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, currentTarget);
			centerOfShape.y = targetingActor.GetTravelBoardSquareWorldPosition().y + this.m_heightOffset;
			return centerOfShape;
		}
		return Vector3.zero;
	}

	private void SetHighlightPos(Vector3 pos)
	{
		using (List<GameObject>.Enumerator enumerator = this.m_highlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				gameObject.transform.position = pos;
			}
		}
	}

	private void MoveHighlightsTowardPos(Vector3 pos)
	{
		this.m_highlights[0].transform.position = TargeterUtils.MoveHighlightTowards(pos, this.m_highlights[0], ref this.m_curSpeed);
		this.m_highlights[1].transform.position = this.m_highlights[0].transform.position;
	}

	private bool MatchesTeam(ActorData targetActor, ActorData caster)
	{
		if (!(targetActor != caster))
		{
			if (this.m_affectsCaster == AbilityUtil_Targeter.AffectsActor.Never)
			{
				return false;
			}
		}
		if (targetActor.GetTeam() == caster.GetTeam())
		{
			return this.m_affectsAllies;
		}
		return this.m_affectsEnemies;
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (this.m_highlights.Count >= 4)
		{
			this.m_highlights[2].SetActive(false);
			this.m_highlights[3].SetActive(false);
		}
	}

	public override void UpdateHighlightPosAfterClick(AbilityTarget target, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (this.m_highlights != null)
		{
			Vector3 highlightGoalPos = this.GetHighlightGoalPos(target, targetingActor);
			this.SetHighlightPos(highlightGoalPos);
			BoardSquare gameplayRefSquare = this.GetGameplayRefSquare(target, targetingActor);
			this.UpdateAngleIndicatorHighlights(target, targetingActor, currentTargetIndex, gameplayRefSquare);
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		this.m_currentGridPos = currentTarget.GridPos;
		base.ClearActorsInRange();
		BoardSquare gameplayRefSquare = this.GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 highlightGoalPos = this.GetHighlightGoalPos(currentTarget, targetingActor);
			int num = 2;
			bool flag;
			if (this.m_showAngleIndicators)
			{
				flag = (currentTargetIndex == 0);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				num = 4;
			}
			if (this.m_highlights != null)
			{
				if (this.m_highlights.Count >= num)
				{
					this.MoveHighlightsTowardPos(highlightGoalPos);
					goto IL_174;
				}
			}
			this.m_highlights = new List<GameObject>();
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
			if (flag2)
			{
				this.m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(this.m_angleIndicatorLength, 0.2f, true, Color.cyan));
				this.m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(this.m_angleIndicatorLength, 0.2f, true, Color.cyan));
			}
			this.SetHighlightPos(highlightGoalPos);
			IL_174:
			GameObject gameObject = this.m_highlights[0];
			GameObject gameObject2 = this.m_highlights[1];
			Vector3 damageOrigin;
			if (this.m_damageOriginType == AbilityUtil_Targeter_GremlinsBombInCone.DamageOriginType.CasterPos)
			{
				damageOrigin = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			}
			else
			{
				damageOrigin = AreaEffectUtils.GetCenterOfShape(this.m_shape, currentTarget);
			}
			ActorData occupantActor = gameplayRefSquare.OccupantActor;
			if (occupantActor != null)
			{
				if (this.MatchesTeam(occupantActor, targetingActor) && occupantActor.IsVisibleToClient())
				{
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_shape, currentTarget.FreePos, gameplayRefSquare, this.m_penetrateLoS, targetingActor, base.GetAffectedTeams(), null);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
					using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData potentialTarget = enumerator.Current;
							this.HandleAddActorInShape(potentialTarget, targetingActor, currentTarget, damageOrigin);
						}
					}
					gameObject.SetActive(true);
					gameObject2.SetActive(false);
					goto IL_2A7;
				}
			}
			gameObject.SetActive(false);
			gameObject2.SetActive(true);
			IL_2A7:
			if (this.m_affectsCaster == AbilityUtil_Targeter.AffectsActor.Always)
			{
				base.AddActorInRange(targetingActor, damageOrigin, targetingActor, this.m_allyTooltipSubject, false);
			}
			this.UpdateAngleIndicatorHighlights(currentTarget, targetingActor, currentTargetIndex, gameplayRefSquare);
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	private void UpdateAngleIndicatorHighlights(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, BoardSquare targetSquare)
	{
		bool flag;
		if (this.m_showAngleIndicators)
		{
			if (currentTargetIndex == 0)
			{
				flag = (this.m_highlights.Count >= 4);
				goto IL_3E;
			}
		}
		flag = false;
		IL_3E:
		bool flag2 = flag;
		if (flag2)
		{
			GameObject gameObject = this.m_highlights[2];
			GameObject gameObject2 = this.m_highlights[3];
			Vector3 vec = targetSquare.ToVector3() - targetingActor.GetTravelBoardSquareWorldPosition();
			vec.y = 0f;
			if (vec.magnitude > 0f)
			{
				gameObject.SetActive(true);
				gameObject2.SetActive(true);
				float num = VectorUtils.HorizontalAngle_Deg(vec);
				float angle = num + this.m_angleWithCenter;
				float angle2 = num - this.m_angleWithCenter;
				Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
				travelBoardSquareWorldPosition.y = HighlightUtils.GetHighlightHeight();
				gameObject.transform.position = travelBoardSquareWorldPosition;
				gameObject2.transform.position = travelBoardSquareWorldPosition;
				gameObject.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle));
				gameObject2.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle2));
			}
			else
			{
				gameObject.SetActive(false);
				gameObject2.SetActive(false);
			}
		}
	}

	protected virtual bool HandleAddActorInShape(ActorData potentialTarget, ActorData targetingActor, AbilityTarget currentTarget, Vector3 damageOrigin)
	{
		if (!(potentialTarget != targetingActor))
		{
			if (this.m_affectsCaster == AbilityUtil_Targeter.AffectsActor.Never)
			{
				return false;
			}
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

	public enum DamageOriginType
	{
		CenterOfShape,
		CasterPos
	}
}
