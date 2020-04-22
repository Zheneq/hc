using System.Collections.Generic;
using UnityEngine;

public class AbilityCommon_FanLaser
{
	public static AbilityUtil_Targeter CreateTargeter_SingleClick(Ability ability, int numLasers, LaserTargetingInfo laserInfo, float angleInBetweenFixed, bool changeAngleByCursorDist, float minAngle, float maxAngle, float minInterpDist, float maxInterpDist)
	{
		if (changeAngleByCursorDist)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = new AbilityUtil_Targeter_ThiefFanLaser(ability, minAngle, maxAngle, minInterpDist, maxInterpDist, laserInfo.range, laserInfo.width, laserInfo.maxTargets, numLasers, laserInfo.penetrateLos, false, false, false, true, 0);
					abilityUtil_Targeter_ThiefFanLaser.SetAffectedGroups(laserInfo.affectsEnemies, laserInfo.affectsAllies, false);
					return abilityUtil_Targeter_ThiefFanLaser;
				}
				}
			}
		}
		AbilityUtil_Targeter_LaserMultiple abilityUtil_Targeter_LaserMultiple = new AbilityUtil_Targeter_LaserMultiple(ability, laserInfo, numLasers, angleInBetweenFixed);
		abilityUtil_Targeter_LaserMultiple.SetAffectedGroups(laserInfo.affectsEnemies, laserInfo.affectsAllies, false);
		return abilityUtil_Targeter_LaserMultiple;
	}

	public static Dictionary<ActorData, int> GetHitActorsAndHitCount(List<AbilityTarget> targets, ActorData caster, LaserTargetingInfo laserInfo, int numLasers, float angleInBetweenFixed, bool changeAngleByCursorDist, float minAngle, float maxAngle, float minInterpDist, float maxInterpDist, out List<List<ActorData>> actorsForSequence, out List<Vector3> targetPosForSequences, out int numLasersWithHits, List<NonActorTargetInfo> nonActorTargetInfo, bool stopEndPosOnHitActor, float interpStep = 0f, float startAngleOffset = 0f)
	{
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		actorsForSequence = new List<List<ActorData>>();
		targetPosForSequences = new List<Vector3>();
		numLasersWithHits = 0;
		float num = angleInBetweenFixed;
		if (changeAngleByCursorDist)
		{
			float num2;
			if (numLasers > 1)
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
				num2 = CalculateFanAngleDegrees(targets[0], caster, minAngle, maxAngle, minInterpDist, maxInterpDist, interpStep);
			}
			else
			{
				num2 = 0f;
			}
			float num3 = num2;
			float num4;
			if (numLasers > 1)
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
				num4 = num3 / (float)(numLasers - 1);
			}
			else
			{
				num4 = 0f;
			}
			num = num4;
		}
		float num5 = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection) + startAngleOffset;
		float num6 = num5 - 0.5f * (float)(numLasers - 1) * num;
		int maxTargets = laserInfo.maxTargets;
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		for (int i = 0; i < numLasers; i++)
		{
			Vector3 dir = VectorUtils.AngleDegreesToVector(num6 + (float)i * num);
			laserCoords.start = caster.GetTravelBoardSquareWorldPositionForLos();
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, laserInfo.affectsAllies, laserInfo.affectsEnemies);
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, dir, laserInfo.range, laserInfo.width, caster, relevantTeams, laserInfo.penetrateLos, maxTargets, false, true, out laserCoords.end, nonActorTargetInfo);
			if (i == 0)
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
				if (laserInfo.affectsCaster)
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
					actorsInLaser.Add(caster);
				}
			}
			actorsForSequence.Add(actorsInLaser);
			using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (dictionary.ContainsKey(current))
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
						dictionary[current]++;
					}
					else
					{
						dictionary[current] = 1;
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
			if (actorsInLaser.Count > 0)
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
				numLasersWithHits++;
			}
			int count = actorsForSequence[i].Count;
			if (count > 0)
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
				if (stopEndPosOnHitActor)
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
					targetPosForSequences.Add(actorsForSequence[i][count - 1].GetTravelBoardSquareWorldPosition());
					continue;
				}
			}
			targetPosForSequences.Add(laserCoords.end);
		}
		return dictionary;
	}

	public static float CalculateFanAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor, float minAngle, float maxAngle, float minInterpDist, float maxInterpDist, float interpStep)
	{
		minAngle = Mathf.Max(minAngle, 0f);
		maxAngle = Mathf.Max(maxAngle, 1f);
		float value = (currentTarget.FreePos - targetingActor.GetTravelBoardSquareWorldPosition()).magnitude / Board.Get().squareSize;
		float num = Mathf.Clamp(value, minInterpDist, maxInterpDist) - minInterpDist;
		if (interpStep > 0f)
		{
			float num2 = num % interpStep;
			num -= num2;
		}
		return Mathf.Max(minAngle, maxAngle * (1f - num / (maxInterpDist - minInterpDist)));
	}

	public static float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees, float maxAngle, float minInterpDist, float maxInterpDist)
	{
		if (float.IsNaN(fanAngleDegrees))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			fanAngleDegrees = 0f;
		}
		maxAngle = Mathf.Max(maxAngle, 1f);
		float num = 1f - fanAngleDegrees / maxAngle;
		float num2 = minInterpDist + (maxInterpDist - minInterpDist) * num;
		return num2 * Board.Get().squareSize;
	}
}
