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

	public AbilityUtil_Targeter_TricksterBarriers(Ability ability, TricksterAfterImageNetworkBehaviour syncComp, float rangeFromLine = 0f, float lineEndOffset = 0f, float radiusAroundStart = 0f, bool penetrateLos = false, bool drawBarriers = true)
		: base(ability)
	{
		m_afterImageSyncComp = syncComp;
		m_rangeFromLine = rangeFromLine;
		m_lineEndOffset = lineEndOffset;
		m_radiusAroundStart = radiusAroundStart;
		m_penetrateLos = penetrateLos;
		m_drawBarriers = drawBarriers;
		int shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			shouldShowActorRadius = (GameWideData.Get().UseActorRadiusForCone() ? 1 : 0);
		}
		else
		{
			shouldShowActorRadius = 1;
		}
		m_shouldShowActorRadius = ((byte)shouldShowActorRadius != 0);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		int num = validAfterImages.Count + 1;
		int num2;
		if (validAfterImages.Count > 1)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num2 = validAfterImages.Count + 1;
		}
		else
		{
			num2 = validAfterImages.Count;
		}
		int num3 = num2;
		bool flag = m_rangeFromLine > 0f;
		bool flag2 = m_radiusAroundStart > 0f;
		int num4;
		if (m_drawBarriers)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			num4 = num3;
		}
		else
		{
			num4 = 0;
		}
		int num5 = num4;
		int num6 = num5;
		int num7;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num7 = num3 * 2;
		}
		else
		{
			num7 = 0;
		}
		int num8 = num6 + num7;
		float num9 = m_lineEndOffset * Board.Get().squareSize;
		if (m_highlights != null)
		{
			if (m_highlights.Count != 0)
			{
				goto IL_01e6;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_highlights = new List<GameObject>();
		for (int i = 0; i < num5; i++)
		{
			m_highlights.Add(Object.Instantiate(HighlightUtils.Get().m_rectangleCursorPrefab.GetComponent<UIRectangleCursor>().m_endWidthLine));
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		if (flag)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int j = 0; j < num3; j++)
			{
				m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, true));
				m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, false));
			}
		}
		if (flag2)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int k = 0; k < num; k++)
			{
				m_highlights.Add(TargeterUtils.CreateCircleHighlight(Vector3.zero, m_radiusAroundStart, TargeterUtils.HeightAdjustType.DontAdjustHeight, targetingActor == GameFlowData.Get().activeOwnedActorData));
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		goto IL_01e6;
		IL_01e6:
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
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
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
			List<ActorData> actors = AreaEffectUtils.GetActorsInRadiusOfLine(vector2, vector3, 0f, 0f, m_rangeFromLine, m_penetrateLos, targetingActor, targetingActor.GetOpposingTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					AddActorInRange(current, boardSquare.ToVector3(), targetingActor);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			List<ActorData> actors2 = AreaEffectUtils.GetActorsInRadius(boardSquare.ToVector3(), m_radiusAroundStart, m_penetrateLos, targetingActor, targetingActor.GetOpposingTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors2);
			using (List<ActorData>.Enumerator enumerator2 = actors2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData current2 = enumerator2.Current;
					AddActorInRange(current2, boardSquare.ToVector3(), targetingActor);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (m_drawBarriers)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
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
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 a2 = vector2;
				a2.y = (float)Board.Get().BaselineHeight + 0.1f;
				Vector3 vector4 = vector3;
				vector4.y = (float)Board.Get().BaselineHeight + 0.1f;
				Vector3 b = Vector3.Cross(vector, Vector3.up);
				b.Normalize();
				b *= m_rangeFromLine * Board.Get().squareSize;
				float lengthInSquares = (magnitude + 2f * num9) / Board.Get().squareSize;
				GameObject gameObject = m_highlights[num6 + l * 2];
				GameObject gameObject2 = m_highlights[num6 + l * 2 + 1];
				HighlightUtils.Get().ResizeBoundaryLine(lengthInSquares, gameObject);
				HighlightUtils.Get().ResizeBoundaryLine(lengthInSquares, gameObject2);
				gameObject.transform.position = a2 + b;
				gameObject.transform.rotation = Quaternion.LookRotation(-vector);
				gameObject2.transform.position = a2 - b;
				gameObject2.transform.rotation = Quaternion.LookRotation(-vector);
			}
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (!flag2)
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
				travelBoardSquareWorldPosition.y = (float)Board.Get().BaselineHeight + 0.1f;
				TargeterUtils.RefreshCircleHighlight(m_highlights[num8], travelBoardSquareWorldPosition, TargeterUtils.HeightAdjustType.DontAdjustHeight);
				for (int m = 0; m < validAfterImages.Count; m++)
				{
					Vector3 travelBoardSquareWorldPosition2 = validAfterImages[m].GetTravelBoardSquareWorldPosition();
					travelBoardSquareWorldPosition2.y = (float)Board.Get().BaselineHeight + 0.1f;
					TargeterUtils.RefreshCircleHighlight(m_highlights[num8 + m + 1], travelBoardSquareWorldPosition2, TargeterUtils.HeightAdjustType.DontAdjustHeight);
				}
				return;
			}
		}
	}
}
