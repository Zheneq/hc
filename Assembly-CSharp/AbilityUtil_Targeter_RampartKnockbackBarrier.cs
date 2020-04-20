using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_RampartKnockbackBarrier : AbilityUtil_Targeter
{
	private float m_width;

	private float m_laserRange;

	private bool m_lengthIgnoreLos;

	private float m_knockbackDistance;

	private KnockbackType m_knockbackType;

	private bool m_penetrateLos;

	private bool m_snapToBorder;

	private bool m_allowAimAtDiagonals;

	private AbilityTooltipSubject m_enemySubjectType = AbilityTooltipSubject.Primary;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_RampartKnockbackBarrier(Ability ability, float width, float laserRange, bool lengthIgnoreLos, float knockbackDistance, KnockbackType knockbackType, bool penetrateLos, bool snapToBorder, bool allowAimAtDiagonals) : base(ability)
	{
		this.m_width = width;
		this.m_laserRange = laserRange;
		this.m_lengthIgnoreLos = lengthIgnoreLos;
		this.m_knockbackDistance = knockbackDistance;
		this.m_knockbackType = knockbackType;
		this.m_penetrateLos = penetrateLos;
		this.m_snapToBorder = snapToBorder;
		this.m_allowAimAtDiagonals = allowAimAtDiagonals;
		this.m_affectsEnemies = true;
		this.m_affectsAllies = false;
		this.m_affectsTargetingActor = false;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public void SetTooltipSubjectType(AbilityTooltipSubject enemySubjectType)
	{
		this.m_enemySubjectType = enemySubjectType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		Vector3 vector = currentTarget.FreePos;
		Vector3 vector2 = vector - targetingActor.GetTravelBoardSquareWorldPosition();
		bool active = false;
		Vector3 vector3 = vector;
		BoardSquare boardSquare = null;
		if (this.m_snapToBorder)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_RampartKnockbackBarrier.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
			if (currentTargetIndex > 0)
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
				boardSquare = Board.Get().GetBoardSquareSafe(targets[currentTargetIndex - 1].GridPos);
			}
			else
			{
				boardSquare = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
			}
			if (boardSquare != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				active = true;
				vector3 = boardSquare.ToVector3();
				Vector3 b;
				vector2 = VectorUtils.GetDirectionAndOffsetToClosestSide(boardSquare, currentTarget.FreePos, this.m_allowAimAtDiagonals, out b);
				vector = vector3 + b;
			}
		}
		vector2.y = 0f;
		vector2.Normalize();
		float num = this.m_width * Board.Get().squareSize;
		float num2 = this.m_laserRange * Board.Get().squareSize;
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 2)
			{
				goto IL_1BF;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, true));
		HighlightUtils.Get().ResizeBoundaryLine(this.m_width, this.m_highlights[0]);
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(num, 1f, null));
		HighlightUtils.Get().ResizeRectangularCursor(num, num2, this.m_highlights[1]);
		IL_1BF:
		GameObject gameObject = this.m_highlights[0];
		GameObject gameObject2 = this.m_highlights[1];
		float y = 0.1f;
		Vector3 a = Vector3.Cross(vector2, Vector3.up);
		Vector3 a2 = vector - 0.5f * num * a;
		gameObject.transform.position = a2 + new Vector3(0f, 0.1f, 0f);
		gameObject.transform.rotation = Quaternion.LookRotation(-a);
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 start = (!(boardSquare != null)) ? travelBoardSquareWorldPositionForLos : boardSquare.ToVector3();
		start.y = travelBoardSquareWorldPositionForLos.y;
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = start;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, this.m_affectsAllies, this.m_affectsEnemies);
		float laserRange = this.m_laserRange;
		float num3;
		if (this.m_snapToBorder)
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
			num3 = 0.5f;
		}
		else
		{
			num3 = 0f;
		}
		float laserRangeInSquares = laserRange + num3;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector2, laserRangeInSquares, this.m_width, targetingActor, relevantTeams, this.m_penetrateLos, -1, this.m_lengthIgnoreLos, false, out laserCoords.end, null, null, true, true);
		if (actorsInLaser.Count > 0)
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
			Vector3 vector4;
			List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(laserCoords.start, -1f * vector2, 2f, this.m_width, targetingActor, relevantTeams, true, -1, true, true, out vector4, null, null, true, true);
			for (int i = actorsInLaser.Count - 1; i >= 0; i--)
			{
				if (actorsInLaser2.Contains(actorsInLaser[i]))
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
					actorsInLaser.RemoveAt(i);
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		float lengthInWorld = num2;
		if (!this.m_lengthIgnoreLos)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			Vector3 vector5 = laserCoords.end - vector;
			vector5.y = 0f;
			lengthInWorld = vector5.magnitude;
		}
		gameObject2.transform.position = vector + new Vector3(0f, y, 0f);
		gameObject2.transform.rotation = Quaternion.LookRotation(vector2);
		HighlightUtils.Get().ResizeRectangularCursor(num, lengthInWorld, gameObject2);
		int num4 = 0;
		base.EnableAllMovementArrows();
		using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				base.AddActorInRange(actorData, laserCoords.start, targetingActor, this.m_enemySubjectType, false);
				BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, vector2, laserCoords.start, this.m_knockbackDistance);
				num4 = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num4, false);
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
		base.SetMovementArrowEnabledFromIndex(num4, false);
		if (this.m_affectsTargetingActor)
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
			base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self, false);
		}
		if (this.m_snapToBorder)
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
			if (this.m_highlights.Count < 3)
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
				this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
			}
			this.m_highlights[2].transform.position = vector3;
			this.m_highlights[2].SetActive(active);
		}
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
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
			base.ResetSquareIndicatorIndexToUse();
			Vector3 a3 = laserCoords.end - laserCoords.start;
			a3.y = 0f;
			a3.Normalize();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, laserCoords.start + 0.49f * Board.SquareSizeStatic * a3, laserCoords.end, this.m_width, targetingActor, this.m_penetrateLos, null, null, false);
			base.HideUnusedSquareIndicators();
		}
	}
}
