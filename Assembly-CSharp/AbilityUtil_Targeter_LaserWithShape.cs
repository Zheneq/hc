using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_LaserWithShape : AbilityUtil_Targeter
{
	public struct HitActorContext
	{
		public ActorData actor;

		public int hitOrderIndex;

		public float squaresFromCaster;
	}

	private LaserTargetingInfo m_laserInfo;

	private AbilityAreaShape m_shape;

	private float m_cursorSpeed;

	public bool m_explodeOnEndOfPath;

	public bool m_explodeOnEnvironmentHit;

	public bool m_explodeIfHitActor = true;

	public bool m_clampToCursorPos;

	public bool m_snapToTargetShapeCenterWhenClampRange;

	public bool m_snapToTargetSquareWhenClampRange;

	public bool m_addLaserHitActorAsPrimary = true;

	private List<ActorData> m_lastLaserHitActors = new List<ActorData>();

	public AbilityUtil_Targeter_LaserWithShape(Ability ability, LaserTargetingInfo laserInfo, AbilityAreaShape shape)
		: base(ability)
	{
		m_laserInfo = laserInfo;
		m_shape = shape;
		SetAffectedGroups(m_laserInfo.affectsEnemies, m_laserInfo.affectsAllies, m_laserInfo.affectsCaster);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public AbilityUtil_Targeter_LaserWithShape(Ability ability, AbilityAreaShape shape, float width, float distance, bool penetrateLos, int maxTargets, bool affectsAllies = false, bool affectsCaster = false, bool affectsEnemies = true)
		: base(ability)
	{
		m_shape = shape;
		m_laserInfo = new LaserTargetingInfo();
		m_laserInfo.width = width;
		m_laserInfo.range = distance;
		m_laserInfo.penetrateLos = penetrateLos;
		m_laserInfo.maxTargets = maxTargets;
		m_laserInfo.affectsAllies = affectsAllies;
		m_laserInfo.affectsCaster = affectsCaster;
		m_laserInfo.affectsEnemies = affectsEnemies;
		SetAffectedGroups(affectsEnemies, affectsAllies, affectsCaster);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public List<ActorData> GetLastLaserHitActors()
	{
		return m_lastLaserHitActors;
	}

	public void SetClampToCursorPos(bool value)
	{
		m_clampToCursorPos = value;
	}

	public void SetSnapToTargetShapeCenterWhenClampRange(bool value)
	{
		m_snapToTargetShapeCenterWhenClampRange = value;
	}

	public void SetSnapToTargetSquareWhenClampRange(bool value)
	{
		m_snapToTargetSquareWhenClampRange = value;
	}

	public void SetExplodeOnEnvironmentHit(bool value)
	{
		m_explodeOnEnvironmentHit = value;
	}

	public void SetExplodeOnPathEnd(bool value)
	{
		m_explodeOnEndOfPath = value;
	}

	public void SetExplodeIfHitActor(bool value)
	{
		m_explodeIfHitActor = value;
	}

	public void SetAddDirectHitActorAsPrimary(bool value)
	{
		m_addLaserHitActorAsPrimary = value;
	}

	public bool SnapAimDirection()
	{
		int result;
		if (!SnapToTargetSquare())
		{
			result = (SnapToTargetShapeCenter() ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool SnapToTargetSquare()
	{
		int result;
		if (m_clampToCursorPos)
		{
			if (m_snapToTargetSquareWhenClampRange)
			{
				result = ((!m_snapToTargetShapeCenterWhenClampRange) ? 1 : 0);
				goto IL_0039;
			}
		}
		result = 0;
		goto IL_0039;
		IL_0039:
		return (byte)result != 0;
	}

	public bool SnapToTargetShapeCenter()
	{
		int result;
		if (m_clampToCursorPos)
		{
			result = (m_snapToTargetShapeCenterWhenClampRange ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		Vector3 vector;
		if (currentTarget == null)
		{
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 dir = vector;
		Vector3 b = currentTarget.FreePos;
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		if (SnapAimDirection())
		{
			if (boardSquareSafe != null)
			{
				if (boardSquareSafe != targetingActor.GetCurrentBoardSquare())
				{
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, boardSquareSafe.ToVector3(), boardSquareSafe);
					Vector3 vector2;
					if (SnapToTargetShapeCenter())
					{
						vector2 = centerOfShape;
					}
					else
					{
						vector2 = boardSquareSafe.ToVector3();
					}
					Vector3 vector3 = vector2;
					dir = vector3 - targetingActor.GetFreePos();
					dir.y = 0f;
					dir.Normalize();
					b = vector3;
				}
			}
		}
		float num = m_laserInfo.range;
		if (m_clampToCursorPos)
		{
			float a = VectorUtils.HorizontalPlaneDistInSquares(targetingActor.GetFreePos(), b);
			num = Mathf.Min(a, num);
		}
		float widthInWorld = m_laserInfo.width * Board.Get().squareSize;
		VectorUtils.LaserCoords adjustedCoords = default(VectorUtils.LaserCoords);
		adjustedCoords.start = targetingActor.GetLoSCheckPos();
		List<ActorData> list = m_lastLaserHitActors = AreaEffectUtils.GetActorsInLaser(adjustedCoords.start, dir, num, m_laserInfo.width, targetingActor, GetAffectedTeams(), m_laserInfo.penetrateLos, m_laserInfo.maxTargets, false, false, out adjustedCoords.end, null);
		bool flag = AreaEffectUtils.LaserHitWorldGeo(num, adjustedCoords, m_laserInfo.penetrateLos, list);
		foreach (ActorData item in list)
		{
			AddActorInRange(item, adjustedCoords.start, targetingActor);
		}
		if (m_laserInfo.affectsCaster)
		{
			AddActorInRange(targetingActor, adjustedCoords.start, targetingActor);
		}
		bool flag2 = false;
		if (m_highlights != null)
		{
			int count = m_highlights.Count;
			int num2;
			if (SnapAimDirection())
			{
				num2 = 3;
			}
			else
			{
				num2 = 2;
			}
			if (count >= num2)
			{
				goto IL_0314;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		if (SnapAimDirection())
		{
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		flag2 = true;
		goto IL_0314;
		IL_039c:
		int num3 = 1;
		goto IL_039d;
		IL_0314:
		GameObject gameObject = m_highlights[0];
		GameObject gameObject2 = m_highlights[1];
		GameObject gameObject3 = null;
		if (SnapAimDirection())
		{
			gameObject3 = m_highlights[2];
		}
		if (m_explodeOnEndOfPath)
		{
			goto IL_039c;
		}
		if (flag)
		{
			if (m_explodeOnEnvironmentHit)
			{
				goto IL_039c;
			}
		}
		if (m_explodeIfHitActor)
		{
			num3 = ((list.Count > 0) ? 1 : 0);
		}
		else
		{
			num3 = 0;
		}
		goto IL_039d;
		IL_039d:
		if (num3 != 0)
		{
			AreaEffectUtils.GetEndPointForValidGameplaySquare(adjustedCoords.start, adjustedCoords.end, out Vector3 adjustedEndPoint);
			BoardSquare boardSquare = Board.Get().GetSquareFromVec3(adjustedEndPoint);
			Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(m_shape, adjustedEndPoint, boardSquare);
			List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_shape, centerOfShape2, boardSquare, false, targetingActor, targetingActor.GetEnemyTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			using (List<ActorData>.Enumerator enumerator2 = actors.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData current2 = enumerator2.Current;
					if (!list.Contains(current2))
					{
						AddActorInRange(current2, centerOfShape2, targetingActor, AbilityTooltipSubject.Secondary, true);
					}
				}
			}
			Vector3 position = centerOfShape2;
			if (SnapAimDirection())
			{
				position = centerOfShape2;
			}
			else if (!flag2)
			{
				position = TargeterUtils.MoveHighlightTowards(centerOfShape2, gameObject2, ref m_cursorSpeed);
			}
			position.y = (float)Board.Get().BaselineHeight + 0.1f;
			gameObject2.transform.position = position;
			gameObject2.SetActive(true);
		}
		else
		{
			gameObject2.SetActive(false);
		}
		Vector3 a2 = adjustedCoords.end;
		if (SnapAimDirection())
		{
			if (boardSquareSafe != null)
			{
				Vector3 centerOfShape3 = AreaEffectUtils.GetCenterOfShape(m_shape, boardSquareSafe.ToVector3(), boardSquareSafe);
				float num4 = Board.Get().BaselineHeight;
				float num5;
				if (SnapAimDirection())
				{
					num5 = -0.05f;
				}
				else
				{
					num5 = 0.1f;
				}
				centerOfShape3.y = num4 + num5;
				gameObject3.transform.position = centerOfShape3;
				Vector3 vector4;
				if (SnapToTargetShapeCenter())
				{
					vector4 = centerOfShape3;
				}
				else
				{
					vector4 = boardSquareSafe.ToVector3();
				}
				a2 = vector4;
				a2.y = adjustedCoords.start.y;
			}
		}
		float magnitude = (a2 - adjustedCoords.start).magnitude;
		Vector3 normalized = (a2 - adjustedCoords.start).normalized;
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, gameObject);
		gameObject.transform.position = adjustedCoords.start + new Vector3(0f, 0.1f - BoardSquare.s_LoSHeightOffset, 0f);
		gameObject.transform.rotation = Quaternion.LookRotation(normalized);
	}
}
