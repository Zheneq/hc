using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCommon_DualMeetingLasers
{
	public static List<Vector3> CalcStartingPositions(Vector3 centerPos, Vector3 targeterFreePos, float forwardOffset, float sideOffset)
	{
		Vector3 vector = targeterFreePos - centerPos;
		vector.y = 0f;
		if (vector.magnitude > 1E-05f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityCommon_DualMeetingLasers.CalcStartingPositions(Vector3, Vector3, float, float)).MethodHandle;
			}
			vector.Normalize();
		}
		else
		{
			vector = Vector3.forward;
		}
		Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
		Vector3 a = centerPos + forwardOffset * Board.SquareSizeStatic * vector;
		float d = sideOffset * Board.SquareSizeStatic;
		return new List<Vector3>
		{
			a + normalized * d,
			a - normalized * d
		};
	}

	public static Vector3 CalcClampedMeetingPos(Vector3 casterPos, Vector3 freePos, float minDistFromCaster, float maxDistFromCaster)
	{
		float num = Board.SquareSizeStatic * minDistFromCaster;
		float num2 = Board.SquareSizeStatic * maxDistFromCaster;
		Vector3 vector = freePos - casterPos;
		vector.y = 0f;
		float num3 = vector.magnitude;
		if (num3 <= 1E-05f)
		{
			vector = Vector3.forward;
			num3 = Board.SquareSizeStatic;
		}
		Vector3 result = freePos;
		if (num3 < num)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityCommon_DualMeetingLasers.CalcClampedMeetingPos(Vector3, Vector3, float, float)).MethodHandle;
			}
			Vector3 normalized = vector.normalized;
			result = casterPos + num * normalized;
		}
		else if (num3 > num2 && num2 > 0f)
		{
			Vector3 normalized2 = vector.normalized;
			result = casterPos + num2 * normalized2;
		}
		return result;
	}

	public static float CalcMeetingPosDistFromMin(Vector3 casterPos, Vector3 aimAtPos, float minDistFromCaster)
	{
		Vector3 vector = aimAtPos - casterPos;
		vector.y = 0f;
		float num = vector.magnitude / Board.SquareSizeStatic;
		minDistFromCaster = Mathf.Max(0f, minDistFromCaster);
		return Mathf.Max(0f, num - minDistFromCaster);
	}

	public static float CalcAoeRadius(Vector3 casterPos, Vector3 aimAtPos, float baseRadius, float minDistFromCaster, float changePerDistFromMin, float minRadius, float maxRadius)
	{
		float num = AbilityCommon_DualMeetingLasers.CalcMeetingPosDistFromMin(casterPos, aimAtPos, minDistFromCaster);
		float num2 = baseRadius + changePerDistFromMin * num;
		if (num2 < minRadius)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityCommon_DualMeetingLasers.CalcAoeRadius(Vector3, Vector3, float, float, float, float, float)).MethodHandle;
			}
			num2 = minRadius;
		}
		else if (num2 > maxRadius)
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
			if (maxRadius > 0f)
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
				num2 = maxRadius;
			}
		}
		return num2;
	}

	public unsafe static void CalcHitActors(Vector3 aimAtPos, List<Vector3> laserStartPosList, float laserWidth, float aoeRadius, float radiusMultIfPartialBlock, ActorData caster, List<Team> relevantTeams, bool includeInvisibles, List<NonActorTargetInfo> nonActorTargetInfo, out List<List<ActorData>> laserHitActorsList, out List<Vector3> laserEndPosList, out int aoeEndPosIndex, out float finalRadius, out List<ActorData> aoeHitActors)
	{
		laserHitActorsList = new List<List<ActorData>>();
		laserEndPosList = new List<Vector3>();
		aoeEndPosIndex = -1;
		Vector3 centerPos = aimAtPos;
		int num = 0;
		for (int i = 0; i < laserStartPosList.Count; i++)
		{
			Vector3 vector = laserStartPosList[i];
			Vector3 dir = aimAtPos - vector;
			dir.y = 0f;
			float magnitude = dir.magnitude;
			RaycastHit raycastHit;
			bool flag = VectorUtils.RaycastInDirection(vector, aimAtPos, out raycastHit);
			if (flag)
			{
				Vector3 point = raycastHit.point;
				magnitude = (point - vector).magnitude;
			}
			float num2 = magnitude / Board.SquareSizeStatic;
			Vector3 vector2;
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(vector, dir, num2, laserWidth, caster, relevantTeams, false, -1, false, includeInvisibles, out vector2, nonActorTargetInfo, null, true, true);
			if (laserStartPosList.Count > 1)
			{
				float num3 = (vector2 - vector).magnitude / Board.SquareSizeStatic;
				bool flag2;
				if (!flag)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityCommon_DualMeetingLasers.CalcHitActors(Vector3, List<Vector3>, float, float, float, ActorData, List<Team>, bool, List<NonActorTargetInfo>, List<List<ActorData>>*, List<Vector3>*, int*, float*, List<ActorData>*)).MethodHandle;
					}
					flag2 = (num3 < num2 - 0.1f);
				}
				else
				{
					flag2 = true;
				}
				if (!flag2)
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
					aoeEndPosIndex = i;
					num++;
				}
			}
			else
			{
				centerPos = vector2;
				aoeEndPosIndex = i;
				num = 1;
			}
			laserHitActorsList.Add(actorsInLaser);
			laserEndPosList.Add(vector2);
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
		bool flag3 = aoeEndPosIndex >= 0;
		if (flag3)
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
			finalRadius = aoeRadius;
			if (num < laserStartPosList.Count && radiusMultIfPartialBlock >= 0f)
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
				finalRadius *= radiusMultIfPartialBlock;
			}
			aoeHitActors = AreaEffectUtils.GetActorsInRadius(centerPos, finalRadius, false, caster, relevantTeams, nonActorTargetInfo, false, default(Vector3));
			if (!includeInvisibles)
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
				TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
			}
		}
		else
		{
			finalRadius = aoeRadius;
			aoeHitActors = new List<ActorData>();
		}
	}
}
