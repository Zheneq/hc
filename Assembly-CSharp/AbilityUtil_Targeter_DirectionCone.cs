using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_DirectionCone : AbilityUtil_Targeter
{
	public delegate Vector3 ClampedAimDirectionDelegate(Vector3 currentAimDir, Vector3 prevAimDir);

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

	public ClampedAimDirectionDelegate m_getClampedAimDirection;

	public AbilityUtil_Targeter_DirectionCone(Ability ability, float coneAngleDegrees, float coneLengthRadius, float coneBackwardOffsetInSquares, bool penetrateLoS, bool useCursorHighlight, bool affectEnemies = true, bool affectAllies = false, bool affectCaster = false, int maxTargets = -1, bool useOnlyCasterLocation = false)
		: base(ability)
	{
		m_coneAngleDegrees = coneAngleDegrees;
		m_coneLengthRadius = coneLengthRadius;
		m_penetrateLoS = penetrateLoS;
		m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		m_useCursorHighlight = useCursorHighlight;
		m_coneHighlights = new List<GameObject>();
		m_includeEnemies = affectEnemies;
		m_includeAllies = affectAllies;
		m_includeCaster = affectCaster;
		m_maxTargets = maxTargets;
		m_useCasterLocationForAllMultiTargets = useOnlyCasterLocation;
		m_conePart = new TargeterPart_Cone(m_coneAngleDegrees, m_coneLengthRadius, m_penetrateLoS, m_coneBackwardOffsetInSquares);
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		SetAffectedGroups(m_includeEnemies, m_includeAllies, m_includeCaster);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
	}

	private void ClearConeHighlights()
	{
		using (List<GameObject>.Enumerator enumerator = m_coneHighlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				if (current != null)
				{
					DestroyObjectAndMaterials(current);
				}
			}
		}
		m_coneHighlights.Clear();
	}

	protected override void ClearHighlightCursors(bool clearInstantly)
	{
		base.ClearHighlightCursors(clearInstantly);
		ClearConeHighlights();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		Vector3 vector = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector2 = currentTarget.AimDirection;
		if (currentTargetIndex > 0 && targets != null)
		{
			BoardSquare boardSquareSafe = Board.Get().GetSquare(targets[currentTargetIndex - 1].GridPos);
			if (boardSquareSafe != null)
			{
				if (!m_useCasterLocationForAllMultiTargets)
				{
					vector = boardSquareSafe.GetWorldPositionForLoS();
				}
				vector2 = currentTarget.FreePos - vector;
				vector2.y = 0f;
				vector2.Normalize();
			}
			if (m_getClampedAimDirection != null)
			{
				vector2 = m_getClampedAimDirection(vector2, targets[currentTargetIndex - 1].AimDirection);
			}
		}
		float aimDir_degrees = VectorUtils.HorizontalAngle_Deg(vector2);
		if (m_useCursorHighlight)
		{
			if (m_highlights != null)
			{
				if (m_highlights.Count != 0)
				{
					goto IL_0133;
				}
			}
			m_highlights = new List<GameObject>();
			m_highlights.Add(m_conePart.CreateHighlightObject(this));
			goto IL_0133;
		}
		CreateConeHighlightsWithLines(vector, aimDir_degrees);
		goto IL_0158;
		IL_0133:
		m_conePart.AdjustHighlight(m_highlights[0], vector, vector2);
		goto IL_0158;
		IL_0158:
		List<ActorData> actors = m_conePart.GetHitActors(vector, vector2, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies));
		if (m_maxTargets > 0)
		{
			TargeterUtils.SortActorsByDistanceToPos(ref actors, vector);
			TargeterUtils.LimitActorsToMaxNumber(ref actors, m_maxTargets);
		}
		if (m_includeCaster)
		{
			if (!actors.Contains(targetingActor))
			{
				actors.Add(targetingActor);
			}
		}
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (ShouldAddActor(actorData, targetingActor))
			{
				AddActorInRange(actorData, vector, targetingActor);
				ActorHitContext actorHitContext = m_actorContextVars[actorData];
				float value = VectorUtils.HorizontalPlaneDistInSquares(vector, actorData.GetTravelBoardSquareWorldPosition());
				actorHitContext.m_contextVars.SetValue(ContextKeys._0018.GetKey(), value);
			}
		}
		while (true)
		{
			DrawInvalidSquareIndicators(currentTarget, targetingActor, vector, vector2);
			return;
		}
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		bool result = false;
		if (actor == caster)
		{
			result = m_includeCaster;
		}
		else
		{
			if (actor.GetTeam() == caster.GetTeam())
			{
				if (m_includeAllies)
				{
					result = true;
					goto IL_008b;
				}
			}
			if (actor.GetTeam() != caster.GetTeam())
			{
				if (m_includeEnemies)
				{
					result = true;
				}
			}
		}
		goto IL_008b;
		IL_008b:
		return result;
	}

	public void CreateConeHighlightsWithLines(Vector3 casterPos, float aimDir_degrees)
	{
		Vector3 a = VectorUtils.AngleDegreesToVector(aimDir_degrees);
		float d = m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 a2 = casterPos + new Vector3(0f, y, 0f) - a * d;
		float num = m_coneAngleDegrees / 2f;
		float angle = aimDir_degrees + num;
		float angle2 = aimDir_degrees - num;
		Vector3 vector = -VectorUtils.AngleDegreesToVector(angle);
		Vector3 vector2 = -VectorUtils.AngleDegreesToVector(angle2);
		if (m_coneHighlights.Count != 2)
		{
			ClearConeHighlights();
			GameObject item = HighlightUtils.Get().CreateBoundaryLine(m_coneLengthRadius, true, true);
			GameObject item2 = HighlightUtils.Get().CreateBoundaryLine(m_coneLengthRadius, true, false);
			m_coneHighlights.Add(item);
			m_coneHighlights.Add(item2);
		}
		m_coneHighlights[0].transform.position = a2 - vector * d;
		m_coneHighlights[0].transform.rotation = Quaternion.LookRotation(vector);
		m_coneHighlights[1].transform.position = a2 - vector2 * d;
		m_coneHighlights[1].transform.rotation = Quaternion.LookRotation(vector2);
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 coneStartPos, Vector3 forwardDirection)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			float forwardAngle = VectorUtils.HorizontalAngle_Deg(forwardDirection);
			m_conePart.ShowHiddenSquares(m_indicatorHandler, coneStartPos, forwardAngle, targetingActor, m_penetrateLoS);
			HideUnusedSquareIndicators();
			return;
		}
	}

	public override void DrawGizmos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		float num = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
		Vector3 a = VectorUtils.AngleDegreesToVector(num + 0.5f * m_coneAngleDegrees);
		Vector3 a2 = VectorUtils.AngleDegreesToVector(num - 0.5f * m_coneAngleDegrees);
		float d = Board.Get().squareSize * m_coneLengthRadius;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(travelBoardSquareWorldPosition, travelBoardSquareWorldPosition + d * a);
		Gizmos.DrawLine(travelBoardSquareWorldPosition, travelBoardSquareWorldPosition + d * a2);
	}
}
