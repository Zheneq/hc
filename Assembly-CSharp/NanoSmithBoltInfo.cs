﻿// ROGUES
// SERVER
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

	public List<ActorData> GetActorsHitByBolt(
		Vector3 boltStartPos,
		Vector3 boltDirection,
		ActorData caster,
		AbilityPriority boltPhase,
		out VectorUtils.LaserCoords endPoints,
		List<NonActorTargetInfo> nonActorTargetInfo,
		bool includeActorAtStartPos = false,
		bool includeCaster = false,
		bool startTargetingFromCaster = false)
	{
		Vector3 start = boltStartPos;
		if (startTargetingFromCaster)
		{
			start = caster.GetLoSCheckPos();
		}
		VectorUtils.LaserCoords coords = default(VectorUtils.LaserCoords);
		coords.start = start;
		List<ActorData> actorsInRange = AreaEffectUtils.GetActorsInLaser(
			coords.start,
			boltDirection,
			range,
			width,
			caster,
			TargeterUtils.GetRelevantTeams(caster, includeAllies, includeEnemies),
			penetrateLineOfSight,
			-1,
			false,
			true,
			out coords.end,
			nonActorTargetInfo);
		
		// added in rogues
		// if (boltPhase == AbilityPriority.Evasion)  
		// {
		// 	ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInRange);
		// }
		// end added in rogues

		if (!includeActorAtStartPos)
		{
			BoardSquare boardSquare = Board.Get().GetSquareFromVec3(boltStartPos);
			if (boardSquare != null && boardSquare.OccupantActor != null && actorsInRange.Contains(boardSquare.OccupantActor))
			{
				actorsInRange.Remove(boardSquare.OccupantActor);
			}
		}
		if (!includeCaster && actorsInRange.Contains(caster))
		{
			actorsInRange.Remove(caster);
		}
		endPoints = TargeterUtils.TrimTargetsAndGetLaserCoordsToFarthestTarget(ref actorsInRange, maxTargets, coords);
		return actorsInRange;
	}

	public List<ActorData> GetRadialBoltHitActors(
		ActorData caster,
		AbilityPriority boltPhase,
		Vector3 fromLosCheckPos,
		int count,
		bool relativeToAimDirection,
		Vector3 aimDirection,
		float startAngleOffset,
		out List<List<ActorData>> sequenceActors,
		out List<VectorUtils.LaserCoords> sequenceEndPoints,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		sequenceActors = new List<List<ActorData>>();
		sequenceEndPoints = new List<VectorUtils.LaserCoords>();
		if (count <= 0)
		{
			return list;
		}
		float startAngle = startAngleOffset;
		if (relativeToAimDirection)
		{
			startAngle += VectorUtils.HorizontalAngle_Deg(aimDirection);
		}
		float angleStep = 360f / count;
		for (int i = 0; i < count; i++)
		{
			Vector3 boltDirection = VectorUtils.AngleDegreesToVector(startAngle + i * angleStep);
			VectorUtils.LaserCoords endPoints;
			List<ActorData> actorsHitByBolt = GetActorsHitByBolt(
				fromLosCheckPos,
				boltDirection,
				caster,
				boltPhase,
				out endPoints,
				nonActorTargetInfo);
			foreach (ActorData hitActor in actorsHitByBolt)
			{
				if (!list.Contains(hitActor))
				{
					list.Add(hitActor);
				}
			}
			sequenceActors.Add(actorsHitByBolt);
			sequenceEndPoints.Add(endPoints);
		}
		return list;
	}

	public List<ServerClientUtils.SequenceStartData> CreateSequenceStartDataForBolts(
		GameObject sequencePrefab,
		ActorData caster,
		SequenceSource source,
		List<List<ActorData>> sequenceActors,
		List<VectorUtils.LaserCoords> sequenceEndPoints)
	{
		List<ServerClientUtils.SequenceStartData> startDataList = new List<ServerClientUtils.SequenceStartData>();
		AddSequenceStartDataForBolts(sequencePrefab, caster, source, sequenceActors, sequenceEndPoints, ref startDataList);
		return startDataList;
	}

	public void AddSequenceStartDataForBolts(
		GameObject sequencePrefab,
		ActorData caster,
		SequenceSource source,
		List<List<ActorData>> sequenceActors,
		List<VectorUtils.LaserCoords> sequenceEndPoints,
		ref List<ServerClientUtils.SequenceStartData> startDataList)
	{
		if (startDataList == null)
		{
			return;
		}
		for (int i = 0; i < sequenceActors.Count; i++)
		{
			VectorUtils.LaserCoords laserCoords = sequenceEndPoints[i];
			Vector3 start = laserCoords.start;
			start.y = Board.Get().BaselineHeight + sequenceHeightFromGround;
			VectorUtils.LaserCoords laserCoords2 = sequenceEndPoints[i];
			Vector3 end = laserCoords2.end;
			end.y = Board.Get().BaselineHeight + sequenceHeightFromGround;
			SplineProjectileSequence.DelayedProjectileExtraParams delayedProjectileExtraParams =
				new SplineProjectileSequence.DelayedProjectileExtraParams
				{
					useOverrideStartPos = true,
					overrideStartPos = start
				};
			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
				sequencePrefab, end, sequenceActors[i].ToArray(), caster, source, delayedProjectileExtraParams.ToArray());
			startDataList.Add(item);
		}
	}

	public void ReportAbilityTooltipNumbers(
		ref List<AbilityTooltipNumber> numbers,
		AbilityTooltipSubject enemySubject,
		AbilityTooltipSubject allySubject = AbilityTooltipSubject.Ally)
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
