using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Angle : AbilityUtil_Targeter
{
	public float m_width = 1f;

	public float m_distance = 15f;

	public bool m_penetrateLoS;

	public bool m_affectsCaster;

	public int m_maxTargets = -1;

	private static float s_nearAcceleratingMinSpeed = 2f;

	private static float s_farAcceleratingMinSpeed = 4f;

	private static float s_brakeMinSpeed = 0.5f;

	private static float s_nearAcceleratingMaxSpeed = 4f;

	private static float s_farAcceleratingMaxSpeed = 120f;

	private static float s_brakeMaxSpeed = 2f;

	private static float s_nearAcceleration = 40f;

	private static float s_farAcceleration = 600f;

	private static float s_brakeAcceleration = 6f;

	private static float s_brakeDistance = 0.05f;

	private static float s_farDistance = 0.5f;

	private float m_curSpeed;

	private float m_curAngle;

	private static int s_numAngles = 0x20;

	private static List<AbilityUtil_Targeter_Angle.AngleEntry> s_angles;

	public AbilityUtil_Targeter_Angle(Ability ability, float width, float distance, bool penetrateLoS, int maxTargets = -1, bool affectsAllies = false, bool affectsCaster = false) : base(ability)
	{
		AbilityUtil_Targeter_Angle.InitAngles();
		this.m_width = width;
		this.m_distance = distance;
		this.m_penetrateLoS = penetrateLoS;
		this.m_maxTargets = maxTargets;
		this.m_affectsAllies = affectsAllies;
		this.m_affectsCaster = affectsCaster;
		this.m_curAngle = float.MinValue;
	}

	private static void InitAngles()
	{
		if (AbilityUtil_Targeter_Angle.s_angles == null)
		{
			AbilityUtil_Targeter_Angle.s_angles = new List<AbilityUtil_Targeter_Angle.AngleEntry>(AbilityUtil_Targeter_Angle.s_numAngles);
			float num = (float)AbilityUtil_Targeter_Angle.s_numAngles;
			for (int i = 0; i < AbilityUtil_Targeter_Angle.s_numAngles; i++)
			{
				float angle = (float)i * 6.28318548f / num;
				Vector3 vec = VectorUtils.AngleRadToVector(angle);
				vec.Normalize();
				AbilityUtil_Targeter_Angle.AngleEntry item = default(AbilityUtil_Targeter_Angle.AngleEntry);
				item.angle = angle;
				item.vec = vec;
				AbilityUtil_Targeter_Angle.s_angles.Add(item);
			}
		}
	}

	private static Vector3 GetValidDirOf(Vector3 freeDir)
	{
		float num = VectorUtils.HorizontalAngle_Rad(freeDir);
		int num2 = Mathf.RoundToInt(num * (float)AbilityUtil_Targeter_Angle.s_numAngles / 6.28318548f);
		for (;;)
		{
			if (num2 < AbilityUtil_Targeter_Angle.s_numAngles)
			{
				if (num2 >= 0)
				{
					break;
				}
			}
			if (num2 >= AbilityUtil_Targeter_Angle.s_numAngles)
			{
				num2 -= AbilityUtil_Targeter_Angle.s_numAngles;
			}
			else
			{
				num2 += AbilityUtil_Targeter_Angle.s_numAngles;
			}
		}
		return AbilityUtil_Targeter_Angle.s_angles[num2].vec;
	}

	public float RotateHighlightTowards(float goalAngle)
	{
		if (this.m_curAngle != goalAngle)
		{
			if (this.m_curAngle == -3.40282347E+38f)
			{
			}
			else
			{
				float deltaTime = Time.deltaTime;
				float num = Mathf.Abs(goalAngle - this.m_curAngle);
				if (num > 3.14159274f)
				{
					if (this.m_curAngle > goalAngle)
					{
						this.m_curAngle -= 6.28318548f;
					}
					else
					{
						this.m_curAngle += 6.28318548f;
					}
					num = Mathf.Abs(goalAngle - this.m_curAngle);
				}
				bool flag = num >= AbilityUtil_Targeter_Angle.s_farDistance;
				bool flag2 = num <= AbilityUtil_Targeter_Angle.s_brakeDistance;
				float num2;
				float min;
				float max;
				if (flag)
				{
					num2 = AbilityUtil_Targeter_Angle.s_farAcceleration;
					min = AbilityUtil_Targeter_Angle.s_farAcceleratingMinSpeed;
					max = AbilityUtil_Targeter_Angle.s_farAcceleratingMaxSpeed;
				}
				else if (!flag2)
				{
					num2 = AbilityUtil_Targeter_Angle.s_nearAcceleration;
					min = AbilityUtil_Targeter_Angle.s_nearAcceleratingMinSpeed;
					max = AbilityUtil_Targeter_Angle.s_nearAcceleratingMaxSpeed;
				}
				else
				{
					num2 = -AbilityUtil_Targeter_Angle.s_brakeAcceleration;
					min = AbilityUtil_Targeter_Angle.s_brakeMinSpeed;
					max = AbilityUtil_Targeter_Angle.s_brakeMaxSpeed;
				}
				float num3 = this.m_curSpeed + num2 * deltaTime;
				num3 = Mathf.Clamp(num3, min, max);
				float num4 = num3 * deltaTime;
				float num5 = (goalAngle - this.m_curAngle) / num;
				if (num4 >= num)
				{
					this.m_curSpeed = AbilityUtil_Targeter_Angle.s_nearAcceleratingMinSpeed;
					return goalAngle;
				}
				this.m_curSpeed = num3;
				return this.m_curAngle + num5 * num4;
			}
		}
		this.m_curSpeed = AbilityUtil_Targeter_Angle.s_nearAcceleratingMinSpeed;
		return goalAngle;
	}

	public VectorUtils.LaserCoords CurrentLaserCoordinates(AbilityTarget currentTarget, ActorData targetingActor)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector;
		if (currentTarget == null)
		{
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 freeDir = vector;
		Vector3 validDirOf = AbilityUtil_Targeter_Angle.GetValidDirOf(freeDir);
		float maxDistanceInWorld = this.m_distance * Board.Get().squareSize;
		float widthInWorld = this.m_width * Board.Get().squareSize;
		return VectorUtils.GetLaserCoordinates(travelBoardSquareWorldPositionForLos, validDirOf, maxDistanceInWorld, widthInWorld, this.m_penetrateLoS, targetingActor, null);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		VectorUtils.LaserCoords coords = this.CurrentLaserCoordinates(currentTarget, targetingActor);
		float widthInWorld = this.m_width * Board.Get().squareSize;
		base.ClearActorsInRange();
		List<ActorData> actorsInBox = AreaEffectUtils.GetActorsInBox(coords.start, coords.end, this.m_width, this.m_penetrateLoS, targetingActor, base.GetAffectedTeams());
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInBox);
		if (actorsInBox.Contains(targetingActor))
		{
			actorsInBox.Remove(targetingActor);
		}
		VectorUtils.LaserCoords points = TargeterUtils.TrimTargetsAndGetLaserCoordsToFarthestTarget(ref actorsInBox, this.m_maxTargets, coords);
		float magnitude = (points.end - points.start).magnitude;
		this.UpdateHighlight(widthInWorld, magnitude, points);
		using (List<ActorData>.Enumerator enumerator = actorsInBox.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actor = enumerator.Current;
				base.AddActorInRange(actor, points.start, targetingActor, AbilityTooltipSubject.Primary, false);
			}
		}
		if (this.m_affectsCaster)
		{
			base.AddActorInRange(targetingActor, points.start, targetingActor, AbilityTooltipSubject.Primary, false);
		}
	}

	private void UpdateHighlight(float widthInWorld, float cursorLength, VectorUtils.LaserCoords points)
	{
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		if (base.Highlight == null)
		{
			base.Highlight = HighlightUtils.Get().CreateRectangularCursor(widthInWorld, cursorLength, null);
		}
		else
		{
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, cursorLength, base.Highlight);
		}
		Vector3 normalized = (points.end - points.start).normalized;
		base.Highlight.transform.position = points.start + new Vector3(0f, y, 0f);
		float goalAngle = VectorUtils.HorizontalAngle_Rad(normalized);
		this.m_curAngle = this.RotateHighlightTowards(goalAngle);
		Vector3 forward = VectorUtils.AngleRadToVector(this.m_curAngle);
		base.Highlight.transform.rotation = Quaternion.LookRotation(forward);
	}

	public override void DrawGizmos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		VectorUtils.LaserCoords laserCoords = this.CurrentLaserCoordinates(currentTarget, targetingActor);
		float num = this.m_width * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 vector = laserCoords.start + new Vector3(0f, y, 0f);
		Vector3 vector2 = laserCoords.end + new Vector3(0f, y, 0f);
		Vector3 vector3 = vector2 - vector;
		float magnitude = vector3.magnitude;
		vector3.Normalize();
		Vector3 a = Vector3.Cross(vector3, Vector3.up);
		Vector3 a2 = (vector + vector2) * 0.5f;
		Vector3 b = a * (num / 2f);
		Vector3 b2 = vector3 * (magnitude / 2f);
		Vector3 vector4 = a2 - b - b2;
		Vector3 vector5 = a2 - b + b2;
		Vector3 vector6 = a2 + b - b2;
		Vector3 vector7 = a2 + b + b2;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(vector4, vector5);
		Gizmos.DrawLine(vector5, vector7);
		Gizmos.DrawLine(vector7, vector6);
		Gizmos.DrawLine(vector6, vector4);
		Gizmos.color = Color.yellow;
		List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(vector, vector2, this.m_width / 2f, this.m_penetrateLoS, targetingActor);
		using (List<BoardSquare>.Enumerator enumerator = squaresInBox.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				if (boardSquare.IsBaselineHeight())
				{
					Gizmos.DrawWireCube(boardSquare.ToVector3(), new Vector3(0.05f, 0.1f, 0.05f));
				}
			}
		}
	}

	private struct AngleEntry
	{
		public float angle;

		public Vector3 vec;
	}
}
