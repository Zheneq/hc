using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_TricksterBarriers : AbilityUtil_Targeter
{
	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;
	private float m_rangeFromLine;
	private float m_lineEndOffset;
	private float m_radiusAroundStart;
	private bool m_penetrateLos;
	private bool m_drawBarriers = true;

	public AbilityUtil_Targeter_TricksterBarriers(
		Ability ability,
		TricksterAfterImageNetworkBehaviour syncComp,
		float rangeFromLine = 0f,
		float lineEndOffset = 0f,
		float radiusAroundStart = 0f,
		bool penetrateLos = false,
		bool drawBarriers = true)
		: base(ability)
	{
		m_afterImageSyncComp = syncComp;
		m_rangeFromLine = rangeFromLine;
		m_lineEndOffset = lineEndOffset;
		m_radiusAroundStart = radiusAroundStart;
		m_penetrateLos = penetrateLos;
		m_drawBarriers = drawBarriers;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser() || GameWideData.Get().UseActorRadiusForCone();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		int num = validAfterImages.Count + 1;
		int num3 = validAfterImages.Count > 1 ? validAfterImages.Count + 1 : validAfterImages.Count;
		bool flag = m_rangeFromLine > 0f;
		bool flag2 = m_radiusAroundStart > 0f;
		int num5 = m_drawBarriers ? num3 : 0;
		int num7 = flag ? num3 * 2 : 0;
		int num8 = num5 + num7;
		float num9 = m_lineEndOffset * Board.Get().squareSize;
		if (m_highlights == null || m_highlights.Count == 0)
		{
			m_highlights = new List<GameObject>();
			for (int i = 0; i < num5; i++)
			{
				m_highlights.Add(Object.Instantiate(HighlightUtils.Get().m_rectangleCursorPrefab.GetComponent<UIRectangleCursor>().m_endWidthLine));
			}
			if (flag)
			{
				for (int j = 0; j < num3; j++)
				{
					m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, true));
					m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, false));
				}
			}
			if (flag2)
			{
				for (int k = 0; k < num; k++)
				{
					m_highlights.Add(TargeterUtils.CreateCircleHighlight(
						Vector3.zero,
						m_radiusAroundStart,
						TargeterUtils.HeightAdjustType.DontAdjustHeight,
						targetingActor == GameFlowData.Get().activeOwnedActorData));
				}
			}
		}

		for (int l = 0; l < num3; l++)
		{
			BoardSquare boardSquare = null;
			BoardSquare boardSquare2 = null;
			if (l == 0)
			{
				boardSquare = targetingActor.GetCurrentBoardSquare();
				boardSquare2 = validAfterImages[l].GetCurrentBoardSquare();
			}
			else if (l == validAfterImages.Count)
			{
				boardSquare = validAfterImages[l - 1].GetCurrentBoardSquare();
				boardSquare2 = targetingActor.GetCurrentBoardSquare();
			}
			else if (l < validAfterImages.Count)
			{
				boardSquare = validAfterImages[l - 1].GetCurrentBoardSquare();
				boardSquare2 = validAfterImages[l].GetCurrentBoardSquare();
			}
			Vector3 vector = boardSquare2.ToVector3() - boardSquare.ToVector3();
			vector.y = 0f;
			float magnitude = vector.magnitude;
			vector.Normalize();
			Vector3 vector2 = boardSquare.ToVector3() - vector * num9;
			Vector3 vector3 = boardSquare2.ToVector3() + vector * num9;
			List<ActorData> actorsInLine = AreaEffectUtils.GetActorsInRadiusOfLine(
				vector2,
				vector3,
				0f,
				0f,
				m_rangeFromLine,
				m_penetrateLos,
				targetingActor,
				targetingActor.GetEnemyTeam(),
				null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInLine);
			foreach (ActorData actor in actorsInLine)
			{
				AddActorInRange(actor, boardSquare.ToVector3(), targetingActor);
			}
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
				boardSquare.ToVector3(),
				m_radiusAroundStart,
				m_penetrateLos,
				targetingActor,
				targetingActor.GetEnemyTeam(),
				null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadius);
			foreach (ActorData actor in actorsInRadius)
			{
				AddActorInRange(actor, boardSquare.ToVector3(), targetingActor);
			}
			if (m_drawBarriers)
			{
				float num10 = magnitude;
				Vector3 a = boardSquare.ToVector3() + 0.5f * num10 * vector;
				a.y = Board.Get().BaselineHeight;
				Vector3 forward = Vector3.Cross(vector, Vector3.up);
				m_highlights[l].transform.localScale = new Vector3(num10, 1f, 1f);
				m_highlights[l].transform.position = a + new Vector3(0f, 0.1f, 0f);
				m_highlights[l].transform.rotation = Quaternion.LookRotation(forward);
			}
			if (flag)
			{
				Vector3 a2 = vector2;
				a2.y = Board.Get().BaselineHeight + 0.1f;
				Vector3 vector4 = vector3;
				vector4.y = Board.Get().BaselineHeight + 0.1f;
				Vector3 b = Vector3.Cross(vector, Vector3.up);
				b.Normalize();
				b *= m_rangeFromLine * Board.Get().squareSize;
				float lengthInSquares = (magnitude + 2f * num9) / Board.Get().squareSize;
				GameObject gameObject = m_highlights[num5 + l * 2];
				GameObject gameObject2 = m_highlights[num5 + l * 2 + 1];
				HighlightUtils.Get().ResizeBoundaryLine(lengthInSquares, gameObject);
				HighlightUtils.Get().ResizeBoundaryLine(lengthInSquares, gameObject2);
				gameObject.transform.position = a2 + b;
				gameObject.transform.rotation = Quaternion.LookRotation(-vector);
				gameObject2.transform.position = a2 - b;
				gameObject2.transform.rotation = Quaternion.LookRotation(-vector);
			}
		}
		if (flag2)
		{
			Vector3 travelBoardSquareWorldPosition = targetingActor.GetFreePos();
			travelBoardSquareWorldPosition.y = Board.Get().BaselineHeight + 0.1f;
			TargeterUtils.RefreshCircleHighlight(m_highlights[num8], travelBoardSquareWorldPosition, TargeterUtils.HeightAdjustType.DontAdjustHeight);
			for (int m = 0; m < validAfterImages.Count; m++)
			{
				Vector3 travelBoardSquareWorldPosition2 = validAfterImages[m].GetFreePos();
				travelBoardSquareWorldPosition2.y = Board.Get().BaselineHeight + 0.1f;
				TargeterUtils.RefreshCircleHighlight(m_highlights[num8 + m + 1], travelBoardSquareWorldPosition2, TargeterUtils.HeightAdjustType.DontAdjustHeight);
			}
		}
	}
}
