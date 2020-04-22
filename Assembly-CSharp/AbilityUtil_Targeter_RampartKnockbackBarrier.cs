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

	public AbilityUtil_Targeter_RampartKnockbackBarrier(Ability ability, float width, float laserRange, bool lengthIgnoreLos, float knockbackDistance, KnockbackType knockbackType, bool penetrateLos, bool snapToBorder, bool allowAimAtDiagonals)
		: base(ability)
	{
		m_width = width;
		m_laserRange = laserRange;
		m_lengthIgnoreLos = lengthIgnoreLos;
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
		m_penetrateLos = penetrateLos;
		m_snapToBorder = snapToBorder;
		m_allowAimAtDiagonals = allowAimAtDiagonals;
		m_affectsEnemies = true;
		m_affectsAllies = false;
		m_affectsTargetingActor = false;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public void SetTooltipSubjectType(AbilityTooltipSubject enemySubjectType)
	{
		m_enemySubjectType = enemySubjectType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		Vector3 vector = currentTarget.FreePos;
		Vector3 vector2 = vector - targetingActor.GetTravelBoardSquareWorldPosition();
		bool active = false;
		Vector3 vector3 = vector;
		BoardSquare boardSquare = null;
		if (m_snapToBorder)
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
			if (currentTargetIndex > 0)
			{
				while (true)
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
				while (true)
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
				vector2 = VectorUtils.GetDirectionAndOffsetToClosestSide(boardSquare, currentTarget.FreePos, m_allowAimAtDiagonals, out Vector3 offset);
				vector = vector3 + offset;
			}
		}
		vector2.y = 0f;
		vector2.Normalize();
		float num = m_width * Board.Get().squareSize;
		float num2 = m_laserRange * Board.Get().squareSize;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 2)
			{
				goto IL_01bf;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, true));
		HighlightUtils.Get().ResizeBoundaryLine(m_width, m_highlights[0]);
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(num, 1f));
		HighlightUtils.Get().ResizeRectangularCursor(num, num2, m_highlights[1]);
		goto IL_01bf;
		IL_01bf:
		GameObject gameObject = m_highlights[0];
		GameObject gameObject2 = m_highlights[1];
		float y = 0.1f;
		Vector3 a = Vector3.Cross(vector2, Vector3.up);
		Vector3 a2 = vector - 0.5f * num * a;
		gameObject.transform.position = a2 + new Vector3(0f, 0.1f, 0f);
		gameObject.transform.rotation = Quaternion.LookRotation(-a);
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 start = (!(boardSquare != null)) ? travelBoardSquareWorldPositionForLos : boardSquare.ToVector3();
		start.y = travelBoardSquareWorldPositionForLos.y;
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = start;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies);
		float laserRange = m_laserRange;
		float num3;
		if (m_snapToBorder)
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
			num3 = 0.5f;
		}
		else
		{
			num3 = 0f;
		}
		float laserRangeInSquares = laserRange + num3;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector2, laserRangeInSquares, m_width, targetingActor, relevantTeams, m_penetrateLos, -1, m_lengthIgnoreLos, false, out laserCoords.end, null, null, true);
		if (actorsInLaser.Count > 0)
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
			Vector3 laserEndPos;
			List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(laserCoords.start, -1f * vector2, 2f, m_width, targetingActor, relevantTeams, true, -1, true, true, out laserEndPos, null, null, true);
			for (int num4 = actorsInLaser.Count - 1; num4 >= 0; num4--)
			{
				if (actorsInLaser2.Contains(actorsInLaser[num4]))
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
					actorsInLaser.RemoveAt(num4);
				}
			}
			while (true)
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
		if (!m_lengthIgnoreLos)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			Vector3 vector4 = laserCoords.end - vector;
			vector4.y = 0f;
			lengthInWorld = vector4.magnitude;
		}
		gameObject2.transform.position = vector + new Vector3(0f, y, 0f);
		gameObject2.transform.rotation = Quaternion.LookRotation(vector2);
		HighlightUtils.Get().ResizeRectangularCursor(num, lengthInWorld, gameObject2);
		int num5 = 0;
		EnableAllMovementArrows();
		using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				AddActorInRange(current, laserCoords.start, targetingActor, m_enemySubjectType);
				BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(current, m_knockbackType, vector2, laserCoords.start, m_knockbackDistance);
				num5 = AddMovementArrowWithPrevious(current, path, TargeterMovementType.Knockback, num5);
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
		SetMovementArrowEnabledFromIndex(num5, false);
		if (m_affectsTargetingActor)
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
			AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self);
		}
		if (m_snapToBorder)
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
			if (m_highlights.Count < 3)
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
				m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
			}
			m_highlights[2].transform.position = vector3;
			m_highlights[2].SetActive(active);
		}
		if (!(GameFlowData.Get().activeOwnedActorData == targetingActor))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			ResetSquareIndicatorIndexToUse();
			Vector3 a3 = laserCoords.end - laserCoords.start;
			a3.y = 0f;
			a3.Normalize();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, laserCoords.start + 0.49f * Board.SquareSizeStatic * a3, laserCoords.end, m_width, targetingActor, m_penetrateLos, null, null, false);
			HideUnusedSquareIndicators();
			return;
		}
	}
}
