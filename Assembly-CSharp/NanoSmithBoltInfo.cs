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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBoltInfo.GetActorsHitByBolt(Vector3, Vector3, ActorData, AbilityPriority, VectorUtils.LaserCoords*, List<NonActorTargetInfo>, bool, bool, bool)).MethodHandle;
			}
			start = caster.\u0015();
		}
		VectorUtils.LaserCoords coords;
		coords.start = start;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(coords.start, boltDirection, this.range, this.width, caster, TargeterUtils.GetRelevantTeams(caster, this.includeAllies, this.includeEnemies), this.penetrateLineOfSight, -1, false, true, out coords.end, nonActorTargetInfo, null, false, true);
		if (!includeActorAtStartPos)
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
			BoardSquare boardSquare = Board.\u000E().\u000E(boltStartPos);
			if (boardSquare != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquare.OccupantActor != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (actorsInLaser.Contains(boardSquare.OccupantActor))
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
						actorsInLaser.Remove(boardSquare.OccupantActor);
					}
				}
			}
		}
		if (!includeCaster)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (actorsInLaser.Contains(caster))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBoltInfo.GetRadialBoltHitActors(ActorData, AbilityPriority, Vector3, int, bool, Vector3, float, List<List<ActorData>>*, List<VectorUtils.LaserCoords>*, List<NonActorTargetInfo>)).MethodHandle;
			}
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
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							list.Add(item2);
						}
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				sequenceActors.Add(actorsHitByBolt);
				sequenceEndPoints.Add(item);
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
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
				start.y = (float)Board.\u000E().BaselineHeight + this.sequenceHeightFromGround;
				Vector3 end = sequenceEndPoints[i].end;
				end.y = (float)Board.\u000E().BaselineHeight + this.sequenceHeightFromGround;
				SplineProjectileSequence.DelayedProjectileExtraParams delayedProjectileExtraParams = new SplineProjectileSequence.DelayedProjectileExtraParams();
				delayedProjectileExtraParams.useOverrideStartPos = true;
				delayedProjectileExtraParams.overrideStartPos = start;
				ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(sequencePrefab, end, sequenceActors[i].ToArray(), caster, source, delayedProjectileExtraParams.ToArray());
				startDataList.Add(item);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBoltInfo.AddSequenceStartDataForBolts(GameObject, ActorData, SequenceSource, List<List<ActorData>>, List<VectorUtils.LaserCoords>, List<ServerClientUtils.SequenceStartData>*)).MethodHandle;
			}
		}
	}

	public unsafe void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemySubject, AbilityTooltipSubject allySubject = AbilityTooltipSubject.Ally)
	{
		if (this.includeAllies)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBoltInfo.ReportAbilityTooltipNumbers(List<AbilityTooltipNumber>*, AbilityTooltipSubject, AbilityTooltipSubject)).MethodHandle;
			}
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
