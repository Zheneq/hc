using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Blindfire : AbilityUtil_Targeter
{
	public float m_coneAngleDegrees;

	public float m_coneLengthRadiusInSquares;

	public float m_coneBackwardOffsetInSquares;

	public bool m_penetrateLoS;

	public bool m_restrictWithinCover;

	public bool m_includeTargetsInCover = true;

	public int m_maxTargets;

	private List<GameObject> m_boundsHighlights;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_Blindfire(
		Ability ability,
		float coneAngleDegrees,
		float coneLengthRadiusInSquares,
		float coneBackwardOffsetInSquares,
		bool penetrateLoS,
		bool restrictWithinCover,
		bool includeTargetsInCover,
		int maxTargets)
		: base(ability)
	{
		m_coneAngleDegrees = coneAngleDegrees;
		m_coneLengthRadiusInSquares = coneLengthRadiusInSquares;
		m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		m_penetrateLoS = penetrateLoS;
		m_restrictWithinCover = restrictWithinCover;
		m_includeTargetsInCover = includeTargetsInCover;
		m_maxTargets = maxTargets;
		m_boundsHighlights = new List<GameObject>();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		SetAffectedGroups(true, false, false);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
	}

	private void ClearBoundsHighlights()
	{
		using (List<GameObject>.Enumerator enumerator = m_boundsHighlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				if (current != null)
				{
					Object.Destroy(current);
				}
			}
		}
		m_boundsHighlights.Clear();
	}

	protected override void ClearHighlightCursors(bool clearInstantly)
	{
		ClearBoundsHighlights();
		base.ClearHighlightCursors(clearInstantly);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		Vector3 casterPos = targetingActor.GetLoSCheckPos();
		ActorCover component = targetingActor.GetComponent<ActorCover>();
		if (m_restrictWithinCover)
		{
			CreateBoundsHighlights(casterPos, component);
		}
		Vector3 aimDirection = currentTarget?.AimDirection ?? targetingActor.transform.forward;
		if (component.IsDirInCover(aimDirection) || !m_restrictWithinCover)
		{
			float num = VectorUtils.HorizontalAngle_Deg(aimDirection);
			float newDirAngleDegrees;
			if (m_restrictWithinCover)
			{
				component.ClampConeToValidCover(num, m_coneAngleDegrees, out newDirAngleDegrees, out Vector3 _);
			}
			else
			{
				newDirAngleDegrees = num;
				Vector3 newConeDir = aimDirection;
			}
			CreateConeHighlights(casterPos, newDirAngleDegrees);
			List<Team> affectedTeams = GetAffectedTeams();
			List<ActorData> actors = AreaEffectUtils.GetActorsInCone(
				casterPos,
				newDirAngleDegrees,
				m_coneAngleDegrees,
				m_coneLengthRadiusInSquares,
				m_coneBackwardOffsetInSquares,
				m_penetrateLoS,
				targetingActor,
				affectedTeams,
				null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			if (!m_includeTargetsInCover)
			{
				actors.RemoveAll(actor => actor.GetActorCover() != null && actor.GetActorCover().IsInCoverWrt(casterPos));
			}
			if (m_maxTargets > 0)
			{
				TargeterUtils.SortActorsByDistanceToPos(ref actors, casterPos);
				TargeterUtils.LimitActorsToMaxNumber(ref actors, m_maxTargets);
			}
			foreach (ActorData item in actors)
			{
				AddActorInRange(item, casterPos, targetingActor);
			}
		}
		else
		{
			ClearHighlightCursors(true);
		}
		DrawInvalidSquareIndicators(currentTarget, targetingActor);
	}

	public void CreateConeHighlights(Vector3 casterPos, float aimDir_degrees)
	{
		Vector3 vector = VectorUtils.AngleDegreesToVector(aimDir_degrees);
		float d = m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = casterPos + new Vector3(0f, y, 0f) - vector * d;
		if (Highlight == null)
		{
			float radiusInWorld = (m_coneLengthRadiusInSquares + m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
			Highlight = HighlightUtils.Get().CreateConeCursor(radiusInWorld, m_coneAngleDegrees);
		}
		Highlight.transform.position = position;
		Highlight.transform.rotation = Quaternion.LookRotation(vector);
	}

	public void CreateBoundsHighlights(Vector3 casterPos, ActorCover actorCover)
	{
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = casterPos + new Vector3(0f, y, 0f);
		List<CoverRegion> coveredRegions = actorCover.GetCoveredRegions();
		int num = coveredRegions.Count * 2;
		if (m_boundsHighlights.Count != num)
		{
			ClearBoundsHighlights();
			for (int i = 0; i < coveredRegions.Count; i++)
			{
				GameObject item = HighlightUtils.Get().CreateBoundaryLine(m_coneLengthRadiusInSquares, false, true);
				GameObject item2 = HighlightUtils.Get().CreateBoundaryLine(m_coneLengthRadiusInSquares, false, false);
				m_boundsHighlights.Add(item);
				m_boundsHighlights.Add(item2);
			}
		}
		for (int j = 0; j < coveredRegions.Count; j++)
		{
			int num2 = j * 2;
			int index = num2 + 1;
			Vector3 forward = -VectorUtils.AngleDegreesToVector(coveredRegions[j].m_endAngle);
			m_boundsHighlights[num2].transform.position = position;
			m_boundsHighlights[num2].transform.rotation = Quaternion.LookRotation(forward);
			Vector3 forward2 = -VectorUtils.AngleDegreesToVector(coveredRegions[j].m_startAngle);
			m_boundsHighlights[index].transform.position = position;
			m_boundsHighlights[index].transform.rotation = Quaternion.LookRotation(forward2);
		}
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		ResetSquareIndicatorIndexToUse();
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetLoSCheckPos();
		Vector3 vector;
		if (currentTarget == null)
		{
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 vec = vector;
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
		AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, m_coneAngleDegrees, m_coneLengthRadiusInSquares, m_coneBackwardOffsetInSquares, targetingActor, m_penetrateLoS);
		HideUnusedSquareIndicators();
	}
}
