using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_DirectionCone : AbilityUtil_Targeter
{
	public float m_coneAngleDegrees;

	public float m_coneLengthRadius;

	public bool m_penetrateLoS;

	public float m_coneBackwardOffsetInSquares;

	public bool m_useCursorHighlight = true;

	public bool m_includeEnemies = true;

	public bool m_includeAllies;

	public bool m_includeCaster;

	public int m_maxTargets = -1;

	public bool m_useCasterLocationForAllMultiTargets;

	private List<GameObject> m_coneHighlights;

	private TargeterPart_Cone m_conePart;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_DirectionCone.ClampedAimDirectionDelegate m_getClampedAimDirection;

	public AbilityUtil_Targeter_DirectionCone(Ability ability, float coneAngleDegrees, float coneLengthRadius, float coneBackwardOffsetInSquares, bool penetrateLoS, bool useCursorHighlight, bool affectEnemies = true, bool affectAllies = false, bool affectCaster = false, int maxTargets = -1, bool useOnlyCasterLocation = false) : base(ability)
	{
		this.m_coneAngleDegrees = coneAngleDegrees;
		this.m_coneLengthRadius = coneLengthRadius;
		this.m_penetrateLoS = penetrateLoS;
		this.m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		this.m_useCursorHighlight = useCursorHighlight;
		this.m_coneHighlights = new List<GameObject>();
		this.m_includeEnemies = affectEnemies;
		this.m_includeAllies = affectAllies;
		this.m_includeCaster = affectCaster;
		this.m_maxTargets = maxTargets;
		this.m_useCasterLocationForAllMultiTargets = useOnlyCasterLocation;
		this.m_conePart = new TargeterPart_Cone(this.m_coneAngleDegrees, this.m_coneLengthRadius, this.m_penetrateLoS, this.m_coneBackwardOffsetInSquares);
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		base.SetAffectedGroups(this.m_includeEnemies, this.m_includeAllies, this.m_includeCaster);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
	}

	private void ClearConeHighlights()
	{
		using (List<GameObject>.Enumerator enumerator = this.m_coneHighlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				if (gameObject != null)
				{
					base.DestroyObjectAndMaterials(gameObject);
				}
			}
		}
		this.m_coneHighlights.Clear();
	}

	protected override void ClearHighlightCursors(bool clearInstantly)
	{
		base.ClearHighlightCursors(clearInstantly);
		this.ClearConeHighlights();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		Vector3 vector = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector2 = currentTarget.AimDirection;
		if (currentTargetIndex > 0 && targets != null)
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
				vector2.Normalize();
			}
			if (this.m_getClampedAimDirection != null)
			{
				vector2 = this.m_getClampedAimDirection(vector2, targets[currentTargetIndex - 1].AimDirection);
			}
		}
		float aimDir_degrees = VectorUtils.HorizontalAngle_Deg(vector2);
		if (this.m_useCursorHighlight)
		{
			if (this.m_highlights != null)
			{
				if (this.m_highlights.Count != 0)
				{
					goto IL_133;
				}
			}
			this.m_highlights = new List<GameObject>();
			this.m_highlights.Add(this.m_conePart.CreateHighlightObject(this));
			IL_133:
			this.m_conePart.AdjustHighlight(this.m_highlights[0], vector, vector2);
		}
		else
		{
			this.CreateConeHighlightsWithLines(vector, aimDir_degrees);
		}
		List<ActorData> hitActors = this.m_conePart.GetHitActors(vector, vector2, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, this.m_affectsAllies, this.m_affectsEnemies));
		if (this.m_maxTargets > 0)
		{
			TargeterUtils.SortActorsByDistanceToPos(ref hitActors, vector);
			TargeterUtils.LimitActorsToMaxNumber(ref hitActors, this.m_maxTargets);
		}
		if (this.m_includeCaster)
		{
			if (!hitActors.Contains(targetingActor))
			{
				hitActors.Add(targetingActor);
			}
		}
		for (int i = 0; i < hitActors.Count; i++)
		{
			ActorData actorData = hitActors[i];
			if (this.ShouldAddActor(actorData, targetingActor))
			{
				base.AddActorInRange(actorData, vector, targetingActor, AbilityTooltipSubject.Primary, false);
				ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
				float value = VectorUtils.HorizontalPlaneDistInSquares(vector, actorData.GetTravelBoardSquareWorldPosition());
				actorHitContext.symbol_0015.SetFloat(ContextKeys.symbol_0018.GetHash(), value);
			}
		}
		this.DrawInvalidSquareIndicators(currentTarget, targetingActor, vector, vector2);
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

	public void CreateConeHighlightsWithLines(Vector3 casterPos, float aimDir_degrees)
	{
		Vector3 a = VectorUtils.AngleDegreesToVector(aimDir_degrees);
		float d = this.m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 a2 = casterPos + new Vector3(0f, y, 0f) - a * d;
		float num = this.m_coneAngleDegrees / 2f;
		float angle = aimDir_degrees + num;
		float angle2 = aimDir_degrees - num;
		Vector3 vector = -VectorUtils.AngleDegreesToVector(angle);
		Vector3 vector2 = -VectorUtils.AngleDegreesToVector(angle2);
		if (this.m_coneHighlights.Count != 2)
		{
			this.ClearConeHighlights();
			GameObject item = HighlightUtils.Get().CreateBoundaryLine(this.m_coneLengthRadius, true, true);
			GameObject item2 = HighlightUtils.Get().CreateBoundaryLine(this.m_coneLengthRadius, true, false);
			this.m_coneHighlights.Add(item);
			this.m_coneHighlights.Add(item2);
		}
		this.m_coneHighlights[0].transform.position = a2 - vector * d;
		this.m_coneHighlights[0].transform.rotation = Quaternion.LookRotation(vector);
		this.m_coneHighlights[1].transform.position = a2 - vector2 * d;
		this.m_coneHighlights[1].transform.rotation = Quaternion.LookRotation(vector2);
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 coneStartPos, Vector3 forwardDirection)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			float forwardAngle = VectorUtils.HorizontalAngle_Deg(forwardDirection);
			this.m_conePart.ShowHiddenSquares(this.m_indicatorHandler, coneStartPos, forwardAngle, targetingActor, this.m_penetrateLoS);
			base.HideUnusedSquareIndicators();
		}
	}

	public override void DrawGizmos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		float num = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
		Vector3 a = VectorUtils.AngleDegreesToVector(num + 0.5f * this.m_coneAngleDegrees);
		Vector3 a2 = VectorUtils.AngleDegreesToVector(num - 0.5f * this.m_coneAngleDegrees);
		float d = Board.Get().squareSize * this.m_coneLengthRadius;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(travelBoardSquareWorldPosition, travelBoardSquareWorldPosition + d * a);
		Gizmos.DrawLine(travelBoardSquareWorldPosition, travelBoardSquareWorldPosition + d * a2);
	}

	public delegate Vector3 ClampedAimDirectionDelegate(Vector3 currentAimDir, Vector3 prevAimDir);
}
