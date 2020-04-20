using System;
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

	public AbilityUtil_Targeter_TricksterBarriers(Ability ability, TricksterAfterImageNetworkBehaviour syncComp, float rangeFromLine = 0f, float lineEndOffset = 0f, float radiusAroundStart = 0f, bool penetrateLos = false, bool drawBarriers = true) : base(ability)
	{
		this.m_afterImageSyncComp = syncComp;
		this.m_rangeFromLine = rangeFromLine;
		this.m_lineEndOffset = lineEndOffset;
		this.m_radiusAroundStart = radiusAroundStart;
		this.m_penetrateLos = penetrateLos;
		this.m_drawBarriers = drawBarriers;
		bool shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_TricksterBarriers..ctor(Ability, TricksterAfterImageNetworkBehaviour, float, float, float, bool, bool)).MethodHandle;
			}
			shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		}
		else
		{
			shouldShowActorRadius = true;
		}
		this.m_shouldShowActorRadius = shouldShowActorRadius;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		int num = validAfterImages.Count + 1;
		int num2;
		if (validAfterImages.Count > 1)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_TricksterBarriers.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			num2 = validAfterImages.Count + 1;
		}
		else
		{
			num2 = validAfterImages.Count;
		}
		int num3 = num2;
		bool flag = this.m_rangeFromLine > 0f;
		bool flag2 = this.m_radiusAroundStart > 0f;
		int num4;
		if (this.m_drawBarriers)
		{
			for (;;)
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
		int num7 = num6;
		int num8;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num8 = num3 * 2;
		}
		else
		{
			num8 = 0;
		}
		int num9 = num7 + num8;
		float num10 = this.m_lineEndOffset * Board.Get().squareSize;
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count != 0)
			{
				goto IL_1E6;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_highlights = new List<GameObject>();
		for (int i = 0; i < num5; i++)
		{
			this.m_highlights.Add(UnityEngine.Object.Instantiate<GameObject>(HighlightUtils.Get().m_rectangleCursorPrefab.GetComponent<UIRectangleCursor>().m_endWidthLine));
		}
		for (;;)
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
			for (;;)
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
				this.m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, true));
				this.m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, false));
			}
		}
		if (flag2)
		{
			for (;;)
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
				this.m_highlights.Add(TargeterUtils.CreateCircleHighlight(Vector3.zero, this.m_radiusAroundStart, TargeterUtils.HeightAdjustType.DontAdjustHeight, targetingActor == GameFlowData.Get().activeOwnedActorData));
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		IL_1E6:
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
				for (;;)
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
			Vector3 vector2 = boardSquare.ToVector3() - vector * num10;
			Vector3 vector3 = boardSquare2.ToVector3() + vector * num10;
			List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(vector2, vector3, 0f, 0f, this.m_rangeFromLine, this.m_penetrateLos, targetingActor, targetingActor.GetOpposingTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadiusOfLine);
			using (List<ActorData>.Enumerator enumerator = actorsInRadiusOfLine.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actor = enumerator.Current;
					base.AddActorInRange(actor, boardSquare.ToVector3(), targetingActor, AbilityTooltipSubject.Primary, false);
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(boardSquare.ToVector3(), this.m_radiusAroundStart, this.m_penetrateLos, targetingActor, targetingActor.GetOpposingTeam(), null, false, default(Vector3));
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadius);
			using (List<ActorData>.Enumerator enumerator2 = actorsInRadius.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actor2 = enumerator2.Current;
					base.AddActorInRange(actor2, boardSquare.ToVector3(), targetingActor, AbilityTooltipSubject.Primary, false);
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (this.m_drawBarriers)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				float num11 = magnitude;
				Vector3 a = boardSquare.ToVector3() + 0.5f * num11 * vector;
				a.y = (float)Board.Get().BaselineHeight;
				Vector3 forward = Vector3.Cross(vector, Vector3.up);
				this.m_highlights[l].transform.localScale = new Vector3(num11, 1f, 1f);
				this.m_highlights[l].transform.position = a + new Vector3(0f, 0.1f, 0f);
				this.m_highlights[l].transform.rotation = Quaternion.LookRotation(forward);
			}
			if (flag)
			{
				for (;;)
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
				Vector3 vector5 = Vector3.Cross(vector, Vector3.up);
				vector5.Normalize();
				vector5 *= this.m_rangeFromLine * Board.Get().squareSize;
				float lengthInSquares = (magnitude + 2f * num10) / Board.Get().squareSize;
				GameObject gameObject = this.m_highlights[num6 + l * 2];
				GameObject gameObject2 = this.m_highlights[num6 + l * 2 + 1];
				HighlightUtils.Get().ResizeBoundaryLine(lengthInSquares, gameObject);
				HighlightUtils.Get().ResizeBoundaryLine(lengthInSquares, gameObject2);
				gameObject.transform.position = a2 + vector5;
				gameObject.transform.rotation = Quaternion.LookRotation(-vector);
				gameObject2.transform.position = a2 - vector5;
				gameObject2.transform.rotation = Quaternion.LookRotation(-vector);
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (flag2)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
			travelBoardSquareWorldPosition.y = (float)Board.Get().BaselineHeight + 0.1f;
			TargeterUtils.RefreshCircleHighlight(this.m_highlights[num9], travelBoardSquareWorldPosition, TargeterUtils.HeightAdjustType.DontAdjustHeight);
			for (int m = 0; m < validAfterImages.Count; m++)
			{
				Vector3 travelBoardSquareWorldPosition2 = validAfterImages[m].GetTravelBoardSquareWorldPosition();
				travelBoardSquareWorldPosition2.y = (float)Board.Get().BaselineHeight + 0.1f;
				TargeterUtils.RefreshCircleHighlight(this.m_highlights[num9 + m + 1], travelBoardSquareWorldPosition2, TargeterUtils.HeightAdjustType.DontAdjustHeight);
			}
		}
	}
}
