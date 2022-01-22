using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ChainLightningLaser : AbilityUtil_Targeter
{
	private float m_width = 1f;

	public float m_distance = 15f;

	private bool m_penetrateLos;

	private int m_maxTargets = -1;

	private int m_maxChainHits = -1;

	private float m_chainRadius = 3f;

	private int m_numChainHighlights = 3;

	private float m_chainHighlightWidthInSquares = 0.1f;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_ChainLightningLaser(Ability ability, float width, float distance, bool penetrateLos, int maxTargets, bool affectsAllies, int maxChainHits, float chainRadius)
		: base(ability)
	{
		m_width = width;
		m_distance = distance;
		m_penetrateLos = penetrateLos;
		m_maxTargets = maxTargets;
		m_affectsAllies = affectsAllies;
		m_maxChainHits = maxChainHits;
		m_chainRadius = chainRadius;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_squarePosCheckerList.Add(new SquareInsideChecker_Box(m_width));
		for (int i = 0; i < m_maxChainHits; i++)
		{
			m_squarePosCheckerList.Add(new SquareInsideChecker_Box(m_chainHighlightWidthInSquares));
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		AllocateHighlights();
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, true);
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = targetingActor.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, currentTarget.AimDirection, m_distance, m_width, targetingActor, relevantTeams, m_penetrateLos, m_maxTargets, false, false, out laserCoords.end, null);
		GameObject highlightObj = m_highlights[0];
		SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[0] as SquareInsideChecker_Box;
		squareInsideChecker_Box.UpdateBoxProperties(laserCoords.start, laserCoords.end, targetingActor);
		AdjustLaserHighlight(highlightObj, laserCoords.start, laserCoords.end, m_width);
		List<ActorData> list = new List<ActorData>();
		using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				AddActorInRange(current, laserCoords.start, targetingActor);
				list.Add(current);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					goto end_IL_00cd;
				}
			}
			end_IL_00cd:;
		}
		int num = 0;
		if (actorsInLaser.Count > 0)
		{
			ActorData actorData = actorsInLaser[actorsInLaser.Count - 1];
			while (actorData != null)
			{
				if (m_maxChainHits <= 0 || num < m_maxChainHits)
				{
					ActorData actorData2 = FindChainHitActor(actorData, targetingActor, list);
					if (actorData2 != null)
					{
						AddActorInRange(actorData2, actorData.GetLoSCheckPos(), targetingActor, AbilityTooltipSubject.Secondary);
						list.Add(actorData2);
						if (num + 1 < m_highlights.Count)
						{
							GameObject gameObject = m_highlights[1 + num];
							AdjustLaserHighlight(gameObject, actorData.GetTravelBoardSquareWorldPosition(), actorData2.GetTravelBoardSquareWorldPosition(), m_chainHighlightWidthInSquares);
							gameObject.SetActive(true);
							SquareInsideChecker_Box squareInsideChecker_Box2 = m_squarePosCheckerList[1 + num] as SquareInsideChecker_Box;
							squareInsideChecker_Box2.UpdateBoxProperties(actorData.GetTravelBoardSquareWorldPosition(), actorData2.GetTravelBoardSquareWorldPosition(), targetingActor);
						}
						num++;
					}
					actorData = actorData2;
					continue;
				}
				break;
			}
		}
		for (int i = num + 1; i < m_highlights.Count; i++)
		{
			m_highlights[i].SetActive(false);
		}
		while (true)
		{
			for (int j = num + 1; j < m_squarePosCheckerList.Count; j++)
			{
				SquareInsideChecker_Box squareInsideChecker_Box3 = m_squarePosCheckerList[j] as SquareInsideChecker_Box;
				squareInsideChecker_Box3.UpdateBoxProperties(squareInsideChecker_Box.GetStartPos(), squareInsideChecker_Box.GetEndPos(), targetingActor);
			}
			if (!(GameFlowData.Get().activeOwnedActorData == targetingActor))
			{
				return;
			}
			while (true)
			{
				ResetSquareIndicatorIndexToUse();
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, laserCoords.start, laserCoords.end, m_width, targetingActor, m_penetrateLos, null, m_squarePosCheckerList);
				for (int k = 0; k < num; k++)
				{
					SquareInsideChecker_Box squareInsideChecker_Box4 = m_squarePosCheckerList[1 + k] as SquareInsideChecker_Box;
					AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, squareInsideChecker_Box4.GetStartPos(), squareInsideChecker_Box4.GetEndPos(), m_chainHighlightWidthInSquares, targetingActor, true, null, m_squarePosCheckerList);
				}
				while (true)
				{
					HideUnusedSquareIndicators();
					return;
				}
			}
		}
	}

	private ActorData FindChainHitActor(ActorData fromActor, ActorData caster, List<ActorData> actorsAddedSoFar)
	{
		ActorData result = null;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, m_affectsAllies, true);
		Vector3 travelBoardSquareWorldPositionForLos = fromActor.GetLoSCheckPos();
		List<ActorData> actors = AreaEffectUtils.GetActorsInRadius(travelBoardSquareWorldPositionForLos, m_chainRadius, m_penetrateLos, caster, relevantTeams, null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		TargeterUtils.SortActorsByDistanceToPos(ref actors, travelBoardSquareWorldPositionForLos);
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (!actorsAddedSoFar.Contains(current))
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return current;
						}
					}
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	private void AllocateHighlights()
	{
		float widthInWorld = m_width * Board.Get().squareSize;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= m_numChainHighlights + 1)
			{
				return;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(widthInWorld, 1f));
		for (int i = 0; i < m_numChainHighlights; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(m_chainHighlightWidthInSquares, 1f));
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void AdjustLaserHighlight(GameObject highlightObj, Vector3 startPos, Vector3 endPos, float widthInSquares)
	{
		startPos.y = HighlightUtils.GetHighlightHeight();
		endPos.y = startPos.y;
		float widthInWorld = widthInSquares * Board.Get().squareSize;
		Vector3 vector = endPos - startPos;
		float magnitude = vector.magnitude;
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, highlightObj);
		Vector3 normalized = vector.normalized;
		highlightObj.transform.position = startPos;
		highlightObj.transform.rotation = Quaternion.LookRotation(normalized);
	}
}
