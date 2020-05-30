using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_CapsuleAoE : AbilityUtil_Targeter
{
	public delegate BoardSquare StartSquareDelegate();

	private float m_radiusAroundStart = 2f;

	private float m_radiusAroundEnd = 2f;

	private float m_rangeFromLine = 2f;

	private bool m_penetrateLoS;

	public bool UseEndPosAsDamageOriginIfOverlap;

	public StartSquareDelegate GetDefaultStartSquare;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_CapsuleAoE(Ability ability, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS)
		: base(ability)
	{
		m_radiusAroundStart = radiusAroundStart;
		m_radiusAroundEnd = radiusAroundEnd;
		m_rangeFromLine = rangeFromDir;
		m_penetrateLoS = penetrateLoS;
		m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
		int shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
		{
			shouldShowActorRadius = (GameWideData.Get().UseActorRadiusForCone() ? 1 : 0);
		}
		else
		{
			shouldShowActorRadius = 1;
		}
		m_shouldShowActorRadius = ((byte)shouldShowActorRadius != 0);
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		BoardSquare boardSquare;
		if (currentTargetIndex > 0)
		{
			boardSquare = Board.Get().GetSquare(targets[currentTargetIndex - 1].GridPos);
		}
		else
		{
			boardSquare = boardSquareSafe;
		}
		BoardSquare boardSquare2 = boardSquare;
		if (m_ability.GetExpectedNumberOfTargeters() == 1)
		{
			boardSquare2 = targetingActor.GetCurrentBoardSquare();
		}
		else if (m_ability.GetExpectedNumberOfTargeters() == 0)
		{
			if (GetDefaultStartSquare != null)
			{
				boardSquare2 = GetDefaultStartSquare();
			}
		}
		Vector3 vector = boardSquare2.ToVector3();
		Vector3 vector2 = boardSquareSafe.ToVector3();
		bool flag = m_rangeFromLine > 0f;
		bool flag2 = m_radiusAroundStart > 0f;
		bool flag3 = m_radiusAroundEnd > 0f;
		if (flag)
		{
			float widthInSquares = m_rangeFromLine * 2f;
			if (m_highlights.Count == 0)
			{
				GameObject item = TargeterUtils.CreateLaserBoxHighlight(vector, vector2, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
				m_highlights.Add(item);
			}
			if (vector == vector2)
			{
				if (m_highlights.Count > 0)
				{
					if (m_highlights[0] != null)
					{
						m_highlights[0].SetActive(false);
					}
				}
			}
			else
			{
				if (m_highlights.Count > 0 && m_highlights[0] != null)
				{
					m_highlights[0].SetActive(true);
				}
				TargeterUtils.RefreshLaserBoxHighlight(m_highlights[0], vector, vector2, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
			}
		}
		if (flag2)
		{
			if (m_highlights.Count != 1)
			{
				if (m_highlights.Count != 0 || flag)
				{
					goto IL_025e;
				}
			}
			GameObject item2 = TargeterUtils.CreateCircleHighlight(vector, m_radiusAroundStart, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
			m_highlights.Add(item2);
			goto IL_025e;
		}
		goto IL_027e;
		IL_038e:
		List<ActorData> actors = AreaEffectUtils.GetActorsInRadiusOfLine(vector, vector2, m_radiusAroundStart, m_radiusAroundEnd, m_rangeFromLine, m_penetrateLoS, targetingActor, null, null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		TargeterUtils.SortActorsByDistanceToPos(ref actors, targetingActor.GetTravelBoardSquareWorldPosition());
		foreach (ActorData item4 in actors)
		{
			if (GetAffectsTarget(item4, targetingActor))
			{
				Vector3 damageOrigin = vector;
				if (UseEndPosAsDamageOriginIfOverlap)
				{
					Vector3 travelBoardSquareWorldPosition = item4.GetTravelBoardSquareWorldPosition();
					travelBoardSquareWorldPosition.y = vector2.y;
					if ((travelBoardSquareWorldPosition - vector2).sqrMagnitude <= Mathf.Epsilon)
					{
						damageOrigin = vector2;
					}
				}
				AddActorInRange(item4, damageOrigin, targetingActor);
				if (m_radiusAroundStart > 0f)
				{
					float num = VectorUtils.HorizontalPlaneDistInSquares(vector, item4.GetTravelBoardSquareWorldPosition());
					if (num <= m_radiusAroundStart * Board.Get().squareSize)
					{
						AddActorInRange(item4, damageOrigin, targetingActor, AbilityTooltipSubject.Near, true);
					}
				}
				if (m_radiusAroundEnd > 0f)
				{
					float num2 = VectorUtils.HorizontalPlaneDistInSquares(vector2, item4.GetTravelBoardSquareWorldPosition());
					if (num2 <= m_radiusAroundEnd * Board.Get().squareSize)
					{
						AddActorInRange(item4, damageOrigin, targetingActor, AbilityTooltipSubject.Far, true);
					}
				}
			}
		}
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInRadiusOfLine(m_indicatorHandler, vector, vector2, m_radiusAroundStart, m_radiusAroundEnd, m_rangeFromLine, m_penetrateLoS, targetingActor);
			HideUnusedSquareIndicators();
			return;
		}
		IL_025e:
		int index = flag ? 1 : 0;
		TargeterUtils.RefreshCircleHighlight(m_highlights[index], vector, TargeterUtils.HeightAdjustType.FromPathArrow);
		goto IL_027e;
		IL_034f:
		int num3 = 0;
		if (flag)
		{
			num3++;
		}
		if (flag2)
		{
			num3++;
		}
		TargeterUtils.RefreshCircleHighlight(m_highlights[num3], vector2, TargeterUtils.HeightAdjustType.FromPathArrow);
		goto IL_038e;
		IL_031b:
		GameObject item3 = TargeterUtils.CreateCircleHighlight(vector2, m_radiusAroundEnd, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
		m_highlights.Add(item3);
		goto IL_034f;
		IL_027e:
		if (flag3)
		{
			if (m_highlights.Count == 2)
			{
				if (flag2)
				{
					if (flag)
					{
						goto IL_031b;
					}
				}
			}
			if (m_highlights.Count == 1)
			{
				if (flag2 ^ flag)
				{
					goto IL_031b;
				}
			}
			if (m_highlights.Count == 0)
			{
				if (!flag2)
				{
					if (!flag)
					{
						goto IL_031b;
					}
				}
			}
			goto IL_034f;
		}
		goto IL_038e;
	}
}
