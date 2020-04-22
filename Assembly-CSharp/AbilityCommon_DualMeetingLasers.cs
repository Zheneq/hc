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
			while (true)
			{
				switch (5)
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
			vector.Normalize();
		}
		else
		{
			vector = Vector3.forward;
		}
		Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
		Vector3 a = centerPos + forwardOffset * Board.SquareSizeStatic * vector;
		float d = sideOffset * Board.SquareSizeStatic;
		List<Vector3> list = new List<Vector3>();
		list.Add(a + normalized * d);
		list.Add(a - normalized * d);
		return list;
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
		float num = CalcMeetingPosDistFromMin(casterPos, aimAtPos, minDistFromCaster);
		float num2 = baseRadius + changePerDistFromMin * num;
		if (num2 < minRadius)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num2 = minRadius;
		}
		else if (num2 > maxRadius)
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
			if (maxRadius > 0f)
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
				num2 = maxRadius;
			}
		}
		return num2;
	}

	public static void CalcHitActors(Vector3 aimAtPos, List<Vector3> laserStartPosList, float laserWidth, float aoeRadius, float radiusMultIfPartialBlock, ActorData caster, List<Team> relevantTeams, bool includeInvisibles, List<NonActorTargetInfo> nonActorTargetInfo, out List<List<ActorData>> laserHitActorsList, out List<Vector3> laserEndPosList, out int aoeEndPosIndex, out float finalRadius, out List<ActorData> aoeHitActors)
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
			RaycastHit hit;
			bool flag = VectorUtils.RaycastInDirection(vector, aimAtPos, out hit);
			if (flag)
			{
				Vector3 point = hit.point;
				magnitude = (point - vector).magnitude;
			}
			float num2 = magnitude / Board.SquareSizeStatic;
			Vector3 laserEndPos;
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(vector, dir, num2, laserWidth, caster, relevantTeams, false, -1, false, includeInvisibles, out laserEndPos, nonActorTargetInfo, null, true);
			if (laserStartPosList.Count > 1)
			{
				float num3 = (laserEndPos - vector).magnitude / Board.SquareSizeStatic;
				int num4;
				if (!flag)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					num4 = ((num3 < num2 - 0.1f) ? 1 : 0);
				}
				else
				{
					num4 = 1;
				}
				if (num4 == 0)
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
					aoeEndPosIndex = i;
					num++;
				}
			}
			else
			{
				centerPos = laserEndPos;
				aoeEndPosIndex = i;
				num = 1;
			}
			laserHitActorsList.Add(actorsInLaser);
			laserEndPosList.Add(laserEndPos);
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (aoeEndPosIndex >= 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						finalRadius = aoeRadius;
						if (num < laserStartPosList.Count && radiusMultIfPartialBlock >= 0f)
						{
							while (true)
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
						aoeHitActors = AreaEffectUtils.GetActorsInRadius(centerPos, finalRadius, false, caster, relevantTeams, nonActorTargetInfo);
						if (!includeInvisibles)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
									return;
								}
							}
						}
						return;
					}
				}
			}
			finalRadius = aoeRadius;
			aoeHitActors = new List<ActorData>();
			return;
		}
	}
}
