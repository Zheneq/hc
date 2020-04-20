using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCommon_FanLaser
{
	public static AbilityUtil_Targeter CreateTargeter_SingleClick(Ability ability, int numLasers, LaserTargetingInfo laserInfo, float angleInBetweenFixed, bool changeAngleByCursorDist, float minAngle, float maxAngle, float minInterpDist, float maxInterpDist)
	{
		if (changeAngleByCursorDist)
		{
			AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = new AbilityUtil_Targeter_ThiefFanLaser(ability, minAngle, maxAngle, minInterpDist, maxInterpDist, laserInfo.range, laserInfo.width, laserInfo.maxTargets, numLasers, laserInfo.penetrateLos, false, false, false, true, 0, 0f, 0f);
			abilityUtil_Targeter_ThiefFanLaser.SetAffectedGroups(laserInfo.affectsEnemies, laserInfo.affectsAllies, false);
			return abilityUtil_Targeter_ThiefFanLaser;
		}
		AbilityUtil_Targeter_LaserMultiple abilityUtil_Targeter_LaserMultiple = new AbilityUtil_Targeter_LaserMultiple(ability, laserInfo, numLasers, angleInBetweenFixed);
		abilityUtil_Targeter_LaserMultiple.SetAffectedGroups(laserInfo.affectsEnemies, laserInfo.affectsAllies, false);
		return abilityUtil_Targeter_LaserMultiple;
	}

	public unsafe static Dictionary<ActorData, int> GetHitActorsAndHitCount(List<AbilityTarget> targets, ActorData caster, LaserTargetingInfo laserInfo, int numLasers, float angleInBetweenFixed, bool changeAngleByCursorDist, float minAngle, float maxAngle, float minInterpDist, float maxInterpDist, out List<List<ActorData>> actorsForSequence, out List<Vector3> targetPosForSequences, out int numLasersWithHits, List<NonActorTargetInfo> nonActorTargetInfo, bool stopEndPosOnHitActor, float interpStep = 0f, float startAngleOffset = 0f)
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
				num2 = AbilityCommon_FanLaser.CalculateFanAngleDegrees(targets[0], caster, minAngle, maxAngle, minInterpDist, maxInterpDist, interpStep);
			}
			else
			{
				num2 = 0f;
			}
			float num3 = num2;
			float num4;
			if (numLasers > 1)
			{
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
		int i = 0;
		while (i < numLasers)
		{
			Vector3 dir = VectorUtils.AngleDegreesToVector(num6 + (float)i * num);
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = caster.GetTravelBoardSquareWorldPositionForLos();
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, laserInfo.affectsAllies, laserInfo.affectsEnemies);
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, dir, laserInfo.range, laserInfo.width, caster, relevantTeams, laserInfo.penetrateLos, maxTargets, false, true, out laserCoords.end, nonActorTargetInfo, null, false, true);
			if (i == 0)
			{
				if (laserInfo.affectsCaster)
				{
					actorsInLaser.Add(caster);
				}
			}
			actorsForSequence.Add(actorsInLaser);
			using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					if (dictionary.ContainsKey(actorData))
					{
						Dictionary<ActorData, int> dictionary2;
						ActorData key;
						(dictionary2 = dictionary)[key = actorData] = dictionary2[key] + 1;
					}
					else
					{
						dictionary[actorData] = 1;
					}
				}
			}
			if (actorsInLaser.Count > 0)
			{
				numLasersWithHits++;
			}
			int count = actorsForSequence[i].Count;
			if (count <= 0)
			{
				goto IL_232;
			}
			if (!stopEndPosOnHitActor)
			{
				goto IL_232;
			}
			targetPosForSequences.Add(actorsForSequence[i][count - 1].GetTravelBoardSquareWorldPosition());
			IL_241:
			i++;
			continue;
			IL_232:
			targetPosForSequences.Add(laserCoords.end);
			goto IL_241;
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
			fanAngleDegrees = 0f;
		}
		maxAngle = Mathf.Max(maxAngle, 1f);
		float num = 1f - fanAngleDegrees / maxAngle;
		float num2 = minInterpDist + (maxInterpDist - minInterpDist) * num;
		return num2 * Board.Get().squareSize;
	}
}
