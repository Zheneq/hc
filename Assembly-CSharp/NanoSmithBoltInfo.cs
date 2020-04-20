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

	public unsafe List<ActorData> GetActorsHitByBolt(Vector3 boltStartPos, Vector3 boltDirection, ActorData caster, AbilityPriority boltPhase, out VectorUtils.LaserCoords endPoints, List<NonActorTargetInfo> nonActorTargetInfo, bool includeActorAtStartPos = false, bool includeCaster = false, bool startTargetingFromCaster = false)
	{
		Vector3 start = boltStartPos;
		if (startTargetingFromCaster)
		{
			start = caster.GetTravelBoardSquareWorldPositionForLos();
		}
		VectorUtils.LaserCoords coords;
		coords.start = start;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(coords.start, boltDirection, this.range, this.width, caster, TargeterUtils.GetRelevantTeams(caster, this.includeAllies, this.includeEnemies), this.penetrateLineOfSight, -1, false, true, out coords.end, nonActorTargetInfo, null, false, true);
		if (!includeActorAtStartPos)
		{
			BoardSquare boardSquare = Board.Get().GetBoardSquare(boltStartPos);
			if (boardSquare != null)
			{
				if (boardSquare.OccupantActor != null)
				{
					if (actorsInLaser.Contains(boardSquare.OccupantActor))
					{
						actorsInLaser.Remove(boardSquare.OccupantActor);
					}
				}
			}
		}
		if (!includeCaster)
		{
			if (actorsInLaser.Contains(caster))
			{
				actorsInLaser.Remove(caster);
			}
		}
		endPoints = TargeterUtils.TrimTargetsAndGetLaserCoordsToFarthestTarget(ref actorsInLaser, this.maxTargets, coords);
		return actorsInLaser;
	}

	public unsafe List<ActorData> GetRadialBoltHitActors(ActorData caster, AbilityPriority boltPhase, Vector3 fromLosCheckPos, int count, bool relativeToAimDirection, Vector3 aimDirection, float startAngleOffset, out List<List<ActorData>> sequenceActors, out List<VectorUtils.LaserCoords> sequenceEndPoints, List<NonActorTargetInfo> nonActorTargetInfo)
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
				VectorUtils.LaserCoords item;
				List<ActorData> actorsHitByBolt = this.GetActorsHitByBolt(fromLosCheckPos, boltDirection, caster, boltPhase, out item, nonActorTargetInfo, false, false, false);
				using (List<ActorData>.Enumerator enumerator = actorsHitByBolt.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData item2 = enumerator.Current;
						if (!list.Contains(item2))
						{
							list.Add(item2);
						}
					}
				}
				sequenceActors.Add(actorsHitByBolt);
				sequenceEndPoints.Add(item);
			}
		}
		return list;
	}

	public List<ServerClientUtils.SequenceStartData> CreateSequenceStartDataForBolts(GameObject sequencePrefab, ActorData caster, SequenceSource source, List<List<ActorData>> sequenceActors, List<VectorUtils.LaserCoords> sequenceEndPoints)
	{
		List<ServerClientUtils.SequenceStartData> result = new List<ServerClientUtils.SequenceStartData>();
		this.AddSequenceStartDataForBolts(sequencePrefab, caster, source, sequenceActors, sequenceEndPoints, ref result);
		return result;
	}

	public unsafe void AddSequenceStartDataForBolts(GameObject sequencePrefab, ActorData caster, SequenceSource source, List<List<ActorData>> sequenceActors, List<VectorUtils.LaserCoords> sequenceEndPoints, ref List<ServerClientUtils.SequenceStartData> startDataList)
	{
		if (startDataList != null)
		{
			for (int i = 0; i < sequenceActors.Count; i++)
			{
				Vector3 start = sequenceEndPoints[i].start;
				start.y = (float)Board.Get().BaselineHeight + this.sequenceHeightFromGround;
				Vector3 end = sequenceEndPoints[i].end;
				end.y = (float)Board.Get().BaselineHeight + this.sequenceHeightFromGround;
				SplineProjectileSequence.DelayedProjectileExtraParams delayedProjectileExtraParams = new SplineProjectileSequence.DelayedProjectileExtraParams();
				delayedProjectileExtraParams.useOverrideStartPos = true;
				delayedProjectileExtraParams.overrideStartPos = start;
				ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(sequencePrefab, end, sequenceActors[i].ToArray(), caster, source, delayedProjectileExtraParams.ToArray());
				startDataList.Add(item);
			}
		}
	}

	public unsafe void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemySubject, AbilityTooltipSubject allySubject = AbilityTooltipSubject.Ally)
	{
		if (this.includeAllies)
		{
			this.effectOnAllyHit.ReportAbilityTooltipNumbers(ref numbers, allySubject);
		}
		if (this.includeEnemies)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, enemySubject, this.damageAmount);
			this.effectOnEnemyHit.ReportAbilityTooltipNumbers(ref numbers, enemySubject);
		}
	}

	public NanoSmithBoltInfo GetShallowCopy()
	{
		return (NanoSmithBoltInfo)base.MemberwiseClone();
	}
}
