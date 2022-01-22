using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NanoSmithBoltInfo
{
	public int damageAmount = 5;

	public float range = 5f;

	public bool penetrateLineOfSight;

	public float width = 0.5f;

	public int maxTargets = 1;

	public bool includeAllies = true;

	public bool includeEnemies = true;

	public StandardEffectInfo effectOnAllyHit;

	public StandardEffectInfo effectOnEnemyHit;

	[Header("-- For Sequences")]
	public float sequenceHeightFromGround = 0.5f;

	public List<ActorData> GetActorsHitByBolt(Vector3 boltStartPos, Vector3 boltDirection, ActorData caster, AbilityPriority boltPhase, out VectorUtils.LaserCoords endPoints, List<NonActorTargetInfo> nonActorTargetInfo, bool includeActorAtStartPos = false, bool includeCaster = false, bool startTargetingFromCaster = false)
	{
		Vector3 start = boltStartPos;
		if (startTargetingFromCaster)
		{
			start = caster.GetLoSCheckPos();
		}
		VectorUtils.LaserCoords coords = default(VectorUtils.LaserCoords);
		coords.start = start;
		List<ActorData> actorsInRange = AreaEffectUtils.GetActorsInLaser(coords.start, boltDirection, range, width, caster, TargeterUtils.GetRelevantTeams(caster, includeAllies, includeEnemies), penetrateLineOfSight, -1, false, true, out coords.end, nonActorTargetInfo);
		if (!includeActorAtStartPos)
		{
			BoardSquare boardSquare = Board.Get().GetSquare(boltStartPos);
			if (boardSquare != null)
			{
				if (boardSquare.OccupantActor != null)
				{
					if (actorsInRange.Contains(boardSquare.OccupantActor))
					{
						actorsInRange.Remove(boardSquare.OccupantActor);
					}
				}
			}
		}
		if (!includeCaster)
		{
			if (actorsInRange.Contains(caster))
			{
				actorsInRange.Remove(caster);
			}
		}
		endPoints = TargeterUtils.TrimTargetsAndGetLaserCoordsToFarthestTarget(ref actorsInRange, maxTargets, coords);
		return actorsInRange;
	}

	public List<ActorData> GetRadialBoltHitActors(ActorData caster, AbilityPriority boltPhase, Vector3 fromLosCheckPos, int count, bool relativeToAimDirection, Vector3 aimDirection, float startAngleOffset, out List<List<ActorData>> sequenceActors, out List<VectorUtils.LaserCoords> sequenceEndPoints, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		sequenceActors = new List<List<ActorData>>();
		sequenceEndPoints = new List<VectorUtils.LaserCoords>();
		if (count > 0)
		{
			float num = startAngleOffset;
			if (relativeToAimDirection)
			{
				num += VectorUtils.HorizontalAngle_Deg(aimDirection);
			}
			float num2 = 360f / (float)count;
			for (int i = 0; i < count; i++)
			{
				Vector3 boltDirection = VectorUtils.AngleDegreesToVector(num + (float)i * num2);
				VectorUtils.LaserCoords endPoints;
				List<ActorData> actorsHitByBolt = GetActorsHitByBolt(fromLosCheckPos, boltDirection, caster, boltPhase, out endPoints, nonActorTargetInfo);
				using (List<ActorData>.Enumerator enumerator = actorsHitByBolt.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						if (!list.Contains(current))
						{
							list.Add(current);
						}
					}
				}
				sequenceActors.Add(actorsHitByBolt);
				sequenceEndPoints.Add(endPoints);
			}
		}
		return list;
	}

	public List<ServerClientUtils.SequenceStartData> CreateSequenceStartDataForBolts(GameObject sequencePrefab, ActorData caster, SequenceSource source, List<List<ActorData>> sequenceActors, List<VectorUtils.LaserCoords> sequenceEndPoints)
	{
		List<ServerClientUtils.SequenceStartData> startDataList = new List<ServerClientUtils.SequenceStartData>();
		AddSequenceStartDataForBolts(sequencePrefab, caster, source, sequenceActors, sequenceEndPoints, ref startDataList);
		return startDataList;
	}

	public void AddSequenceStartDataForBolts(GameObject sequencePrefab, ActorData caster, SequenceSource source, List<List<ActorData>> sequenceActors, List<VectorUtils.LaserCoords> sequenceEndPoints, ref List<ServerClientUtils.SequenceStartData> startDataList)
	{
		if (startDataList == null)
		{
			return;
		}
		for (int i = 0; i < sequenceActors.Count; i++)
		{
			VectorUtils.LaserCoords laserCoords = sequenceEndPoints[i];
			Vector3 start = laserCoords.start;
			start.y = (float)Board.Get().BaselineHeight + sequenceHeightFromGround;
			VectorUtils.LaserCoords laserCoords2 = sequenceEndPoints[i];
			Vector3 end = laserCoords2.end;
			end.y = (float)Board.Get().BaselineHeight + sequenceHeightFromGround;
			SplineProjectileSequence.DelayedProjectileExtraParams delayedProjectileExtraParams = new SplineProjectileSequence.DelayedProjectileExtraParams();
			delayedProjectileExtraParams.useOverrideStartPos = true;
			delayedProjectileExtraParams.overrideStartPos = start;
			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(sequencePrefab, end, sequenceActors[i].ToArray(), caster, source, delayedProjectileExtraParams.ToArray());
			startDataList.Add(item);
		}
		while (true)
		{
			return;
		}
	}

	public void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemySubject, AbilityTooltipSubject allySubject = AbilityTooltipSubject.Ally)
	{
		if (includeAllies)
		{
			effectOnAllyHit.ReportAbilityTooltipNumbers(ref numbers, allySubject);
		}
		if (includeEnemies)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, enemySubject, damageAmount);
			effectOnEnemyHit.ReportAbilityTooltipNumbers(ref numbers, enemySubject);
		}
	}

	public NanoSmithBoltInfo GetShallowCopy()
	{
		return (NanoSmithBoltInfo)MemberwiseClone();
	}
}
