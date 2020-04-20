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

	public AbilityUtil_Targeter_BounceBomb(Ability ability, BounceBombInfo bombInfo, int bombCount, float bombAngleInBetween, bool explodeOnEndOfPath = false, bool alignBombsInLine = false, bool clampMaxRangeToFreePos = false) : base(ability)
	{
		this.m_bombInfo = bombInfo;
		this.m_bombCount = bombCount;
		this.m_bombAngleInBetween = bombAngleInBetween;
		this.m_explodeOnEndOfPath = explodeOnEndOfPath;
		this.m_alignBombInLine = alignBombsInLine;
		this.m_clampMaxRangeToFreePos = clampMaxRangeToFreePos;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		Vector3 vector;
		if (currentTarget == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BounceBomb.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 vec = vector;
		float num = this.m_bombInfo.width * Board.Get().squareSize;
		if (this.m_highlights != null)
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
			if (this.m_highlights.Count >= 2 * this.m_bombCount)
			{
				goto IL_127;
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
		for (int i = 0; i < this.m_bombCount; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateBouncingLaserCursor(Vector3.zero, new List<Vector3>
			{
				Vector3.zero
			}, num));
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int j = 0; j < this.m_bombCount; j++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_bombInfo.shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		IL_127:
		base.ClearActorsInRange();
		float maxDistancePerBounce = this.m_bombInfo.maxDistancePerBounce;
		float num2 = this.m_bombInfo.maxTotalDistance;
		if (this.m_clampMaxRangeToFreePos)
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
			float a = (targetingActor.GetTravelBoardSquareWorldPosition() - currentTarget.FreePos).magnitude / Board.Get().squareSize;
			num2 = Mathf.Min(a, num2);
		}
		float num3 = VectorUtils.HorizontalAngle_Deg(vec);
		float num4 = num3 - 0.5f * (float)(this.m_bombCount - 1) * this.m_bombAngleInBetween;
		int k = 0;
		while (k < this.m_bombCount)
		{
			int index = k + this.m_bombCount;
			float num5 = num4 + (float)k * this.m_bombAngleInBetween;
			Vector3 aimDirection = VectorUtils.AngleDegreesToVector(num5);
			float num6 = 1f;
			if (this.m_alignBombInLine)
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
				float num7 = Mathf.Abs(num5 - num3);
				if (num7 < 80f)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					num6 = 1f / Mathf.Cos(0.0174532924f * num7);
				}
			}
			List<Vector3> list;
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary = this.m_bombInfo.FindBounceHitActors(aimDirection, targetingActor, out list, null, num6 * maxDistancePerBounce, num6 * num2, false);
			Vector3 adjustedStartPosition = this.m_bombInfo.GetAdjustedStartPosition(aimDirection, targetingActor);
			Vector3 originalStart = adjustedStartPosition + new Vector3(0f, 0.1f - BoardSquare.s_LoSHeightOffset, 0f);
			UIBouncingLaserCursor component = this.m_highlights[k].GetComponent<UIBouncingLaserCursor>();
			component.OnUpdated(originalStart, list, num);
			if (dictionary.Count > 0)
			{
				goto IL_2D8;
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
			if (this.m_explodeOnEndOfPath)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					goto IL_2D8;
				}
			}
			else
			{
				this.m_highlights[index].SetActive(false);
			}
			IL_411:
			k++;
			continue;
			IL_2D8:
			Vector3 vector2 = list[list.Count - 1];
			BoardSquare boardSquare = Board.Get().GetBoardSquare(vector2);
			Vector3 vector3 = vector2;
			if (boardSquare != null && boardSquare.IsBaselineHeight())
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
				vector2 = boardSquare.ToVector3();
				vector3 = AreaEffectUtils.GetCenterOfShape(this.m_bombInfo.shape, vector2, boardSquare);
			}
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_bombInfo.shape, vector3, boardSquare, false, targetingActor, targetingActor.GetOpposingTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
			Vector3 damageOrigin = vector3;
			foreach (ActorData actor in actorsInShape)
			{
				base.AddActorInRange(actor, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, true);
			}
			vector3.y = (float)Board.Get().BaselineHeight + 0.1f;
			this.m_highlights[index].transform.position = vector3;
			this.m_highlights[index].SetActive(true);
			goto IL_411;
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
}
