using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_LaserWithShape : AbilityUtil_Targeter
{
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

	public AbilityUtil_Targeter_LaserWithShape(Ability ability, LaserTargetingInfo laserInfo, AbilityAreaShape shape) : base(ability)
	{
		this.m_laserInfo = laserInfo;
		this.m_shape = shape;
		base.SetAffectedGroups(this.m_laserInfo.affectsEnemies, this.m_laserInfo.affectsAllies, this.m_laserInfo.affectsCaster);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public AbilityUtil_Targeter_LaserWithShape(Ability ability, AbilityAreaShape shape, float width, float distance, bool penetrateLos, int maxTargets, bool affectsAllies = false, bool affectsCaster = false, bool affectsEnemies = true) : base(ability)
	{
		this.m_shape = shape;
		this.m_laserInfo = new LaserTargetingInfo();
		this.m_laserInfo.width = width;
		this.m_laserInfo.range = distance;
		this.m_laserInfo.penetrateLos = penetrateLos;
		this.m_laserInfo.maxTargets = maxTargets;
		this.m_laserInfo.affectsAllies = affectsAllies;
		this.m_laserInfo.affectsCaster = affectsCaster;
		this.m_laserInfo.affectsEnemies = affectsEnemies;
		base.SetAffectedGroups(affectsEnemies, affectsAllies, affectsCaster);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public List<ActorData> GetLastLaserHitActors()
	{
		return this.m_lastLaserHitActors;
	}

	public void SetClampToCursorPos(bool value)
	{
		this.m_clampToCursorPos = value;
	}

	public void SetSnapToTargetShapeCenterWhenClampRange(bool value)
	{
		this.m_snapToTargetShapeCenterWhenClampRange = value;
	}

	public void SetSnapToTargetSquareWhenClampRange(bool value)
	{
		this.m_snapToTargetSquareWhenClampRange = value;
	}

	public void SetExplodeOnEnvironmentHit(bool value)
	{
		this.m_explodeOnEnvironmentHit = value;
	}

	public void SetExplodeOnPathEnd(bool value)
	{
		this.m_explodeOnEndOfPath = value;
	}

	public void SetExplodeIfHitActor(bool value)
	{
		this.m_explodeIfHitActor = value;
	}

	public void SetAddDirectHitActorAsPrimary(bool value)
	{
		this.m_addLaserHitActorAsPrimary = value;
	}

	public bool SnapAimDirection()
	{
		bool result;
		if (!this.SnapToTargetSquare())
		{
			result = this.SnapToTargetShapeCenter();
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool SnapToTargetSquare()
	{
		if (this.m_clampToCursorPos)
		{
			if (this.m_snapToTargetSquareWhenClampRange)
			{
				return !this.m_snapToTargetShapeCenterWhenClampRange;
			}
		}
		return false;
	}

	public bool SnapToTargetShapeCenter()
	{
		bool result;
		if (this.m_clampToCursorPos)
		{
			result = this.m_snapToTargetShapeCenterWhenClampRange;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
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
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		if (this.SnapAimDirection())
		{
			if (boardSquareSafe != null)
			{
				if (boardSquareSafe != targetingActor.GetCurrentBoardSquare())
				{
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, boardSquareSafe.ToVector3(), boardSquareSafe);
					Vector3 vector2;
					if (this.SnapToTargetShapeCenter())
					{
						vector2 = centerOfShape;
					}
					else
					{
						vector2 = boardSquareSafe.ToVector3();
					}
					Vector3 vector3 = vector2;
					dir = vector3 - targetingActor.GetTravelBoardSquareWorldPosition();
					dir.y = 0f;
					dir.Normalize();
					b = vector3;
				}
			}
		}
		float num = this.m_laserInfo.range;
		if (this.m_clampToCursorPos)
		{
			float a = VectorUtils.HorizontalPlaneDistInSquares(targetingActor.GetTravelBoardSquareWorldPosition(), b);
			num = Mathf.Min(a, num);
		}
		float widthInWorld = this.m_laserInfo.width * Board.Get().squareSize;
		VectorUtils.LaserCoords adjustedCoords;
		adjustedCoords.start = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(adjustedCoords.start, dir, num, this.m_laserInfo.width, targetingActor, base.GetAffectedTeams(), this.m_laserInfo.penetrateLos, this.m_laserInfo.maxTargets, false, false, out adjustedCoords.end, null, null, false, true);
		this.m_lastLaserHitActors = actorsInLaser;
		bool flag = AreaEffectUtils.LaserHitWorldGeo(num, adjustedCoords, this.m_laserInfo.penetrateLos, actorsInLaser);
		foreach (ActorData actor in actorsInLaser)
		{
			base.AddActorInRange(actor, adjustedCoords.start, targetingActor, AbilityTooltipSubject.Primary, false);
		}
		if (this.m_laserInfo.affectsCaster)
		{
			base.AddActorInRange(targetingActor, adjustedCoords.start, targetingActor, AbilityTooltipSubject.Primary, false);
		}
		bool flag2 = false;
		if (this.m_highlights != null)
		{
			int count = this.m_highlights.Count;
			int num2;
			if (this.SnapAimDirection())
			{
				num2 = 3;
			}
			else
			{
				num2 = 2;
			}
			if (count >= num2)
			{
				goto IL_314;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		if (this.SnapAimDirection())
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		flag2 = true;
		IL_314:
		GameObject gameObject = this.m_highlights[0];
		GameObject gameObject2 = this.m_highlights[1];
		GameObject gameObject3 = null;
		if (this.SnapAimDirection())
		{
			gameObject3 = this.m_highlights[2];
		}
		bool flag3;
		if (!this.m_explodeOnEndOfPath)
		{
			if (flag)
			{
				if (this.m_explodeOnEnvironmentHit)
				{
					goto IL_39C;
				}
			}
			if (this.m_explodeIfHitActor)
			{
				flag3 = (actorsInLaser.Count > 0);
			}
			else
			{
				flag3 = false;
			}
			goto IL_39D;
		}
		IL_39C:
		flag3 = true;
		IL_39D:
		bool flag4 = flag3;
		if (flag4)
		{
			Vector3 vector4;
			AreaEffectUtils.GetEndPointForValidGameplaySquare(adjustedCoords.start, adjustedCoords.end, out vector4);
			BoardSquare boardSquare = Board.Get().GetBoardSquare(vector4);
			Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(this.m_shape, vector4, boardSquare);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_shape, centerOfShape2, boardSquare, false, targetingActor, targetingActor.GetOpposingTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
			using (List<ActorData>.Enumerator enumerator2 = actorsInShape.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actorData = enumerator2.Current;
					if (!actorsInLaser.Contains(actorData))
					{
						base.AddActorInRange(actorData, centerOfShape2, targetingActor, AbilityTooltipSubject.Secondary, true);
					}
				}
			}
			Vector3 position = centerOfShape2;
			if (this.SnapAimDirection())
			{
				position = centerOfShape2;
			}
			else if (!flag2)
			{
				position = TargeterUtils.MoveHighlightTowards(centerOfShape2, gameObject2, ref this.m_cursorSpeed);
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
		if (this.SnapAimDirection())
		{
			if (boardSquareSafe != null)
			{
				Vector3 centerOfShape3 = AreaEffectUtils.GetCenterOfShape(this.m_shape, boardSquareSafe.ToVector3(), boardSquareSafe);
				float num3 = (float)Board.Get().BaselineHeight;
				float num4;
				if (this.SnapAimDirection())
				{
					num4 = -0.05f;
				}
				else
				{
					num4 = 0.1f;
				}
				centerOfShape3.y = num3 + num4;
				gameObject3.transform.position = centerOfShape3;
				Vector3 vector5;
				if (this.SnapToTargetShapeCenter())
				{
					vector5 = centerOfShape3;
				}
				else
				{
					vector5 = boardSquareSafe.ToVector3();
				}
				a2 = vector5;
				a2.y = adjustedCoords.start.y;
			}
		}
		float magnitude = (a2 - adjustedCoords.start).magnitude;
		Vector3 normalized = (a2 - adjustedCoords.start).normalized;
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, gameObject);
		gameObject.transform.position = adjustedCoords.start + new Vector3(0f, 0.1f - BoardSquare.s_LoSHeightOffset, 0f);
		gameObject.transform.rotation = Quaternion.LookRotation(normalized);
	}

	public struct HitActorContext
	{
		public ActorData actor;

		public int hitOrderIndex;

		public float squaresFromCaster;
	}
}
