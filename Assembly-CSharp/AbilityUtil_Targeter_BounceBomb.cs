using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BounceBomb : AbilityUtil_Targeter
{
	public BounceBombInfo m_bombInfo;

	public int m_bombCount;

	public float m_bombAngleInBetween;

	public bool m_explodeOnEndOfPath;

	public bool m_alignBombInLine;

	public bool m_clampMaxRangeToFreePos;

	public AbilityUtil_Targeter_BounceBomb(Ability ability, BounceBombInfo bombInfo, int bombCount, float bombAngleInBetween, bool explodeOnEndOfPath = false, bool alignBombsInLine = false, bool clampMaxRangeToFreePos = false)
		: base(ability)
	{
		m_bombInfo = bombInfo;
		m_bombCount = bombCount;
		m_bombAngleInBetween = bombAngleInBetween;
		m_explodeOnEndOfPath = explodeOnEndOfPath;
		m_alignBombInLine = alignBombsInLine;
		m_clampMaxRangeToFreePos = clampMaxRangeToFreePos;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
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
		float num = m_bombInfo.width * Board.Get().squareSize;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 2 * m_bombCount)
			{
				goto IL_0127;
			}
		}
		m_highlights = new List<GameObject>();
		for (int i = 0; i < m_bombCount; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateBouncingLaserCursor(Vector3.zero, new List<Vector3>
			{
				Vector3.zero
			}, num));
		}
		for (int j = 0; j < m_bombCount; j++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_bombInfo.shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		goto IL_0127;
		IL_0127:
		ClearActorsInRange();
		float maxDistancePerBounce = m_bombInfo.maxDistancePerBounce;
		float num2 = m_bombInfo.maxTotalDistance;
		if (m_clampMaxRangeToFreePos)
		{
			float a = (targetingActor.GetTravelBoardSquareWorldPosition() - currentTarget.FreePos).magnitude / Board.Get().squareSize;
			num2 = Mathf.Min(a, num2);
		}
		float num3 = VectorUtils.HorizontalAngle_Deg(vec);
		float num4 = num3 - 0.5f * (float)(m_bombCount - 1) * m_bombAngleInBetween;
		for (int k = 0; k < m_bombCount; k++)
		{
			int index = k + m_bombCount;
			float num5 = num4 + (float)k * m_bombAngleInBetween;
			Vector3 aimDirection = VectorUtils.AngleDegreesToVector(num5);
			float num6 = 1f;
			if (m_alignBombInLine)
			{
				float num7 = Mathf.Abs(num5 - num3);
				if (num7 < 80f)
				{
					num6 = 1f / Mathf.Cos((float)Math.PI / 180f * num7);
				}
			}
			List<Vector3> bounceEndPoints;
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary = m_bombInfo.FindBounceHitActors(aimDirection, targetingActor, out bounceEndPoints, null, num6 * maxDistancePerBounce, num6 * num2, false);
			Vector3 adjustedStartPosition = m_bombInfo.GetAdjustedStartPosition(aimDirection, targetingActor);
			Vector3 originalStart = adjustedStartPosition + new Vector3(0f, 0.1f - BoardSquare.s_LoSHeightOffset, 0f);
			UIBouncingLaserCursor component = m_highlights[k].GetComponent<UIBouncingLaserCursor>();
			component.OnUpdated(originalStart, bounceEndPoints, num);
			if (dictionary.Count <= 0)
			{
				if (!m_explodeOnEndOfPath)
				{
					m_highlights[index].SetActive(false);
					continue;
				}
			}
			Vector3 vector2 = bounceEndPoints[bounceEndPoints.Count - 1];
			BoardSquare boardSquare = Board.Get().GetSquare(vector2);
			Vector3 vector3 = vector2;
			if (boardSquare != null && boardSquare.IsValidForGameplay())
			{
				vector2 = boardSquare.ToVector3();
				vector3 = AreaEffectUtils.GetCenterOfShape(m_bombInfo.shape, vector2, boardSquare);
			}
			List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_bombInfo.shape, vector3, boardSquare, false, targetingActor, targetingActor.GetEnemyTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			Vector3 damageOrigin = vector3;
			foreach (ActorData item in actors)
			{
				AddActorInRange(item, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, true);
			}
			vector3.y = (float)Board.Get().BaselineHeight + 0.1f;
			m_highlights[index].transform.position = vector3;
			m_highlights[index].SetActive(true);
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
