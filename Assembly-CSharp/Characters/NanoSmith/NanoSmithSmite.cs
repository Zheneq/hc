// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithSmite : Ability
{
	public float m_coneWidthAngle = 270f;
	public float m_coneLength = 1.5f;
	public float m_coneBackwardOffset;
	public int m_coneDamageAmount = 10;
	public int m_coneMaxTargets;
	public bool m_conePenetrateLineOfSight;
	public StandardEffectInfo m_coneEffectOnEnemyHit;
	[Header("-- additional bolt info now specified in NanoSmithBoltInfoComponent")]
	public float m_boltAngle = 45f;
	public int m_boltCount = 3;

	private NanoSmithBoltInfo m_boltInfo;

	[Header("-- Sequences -----------------------------------")]
	public GameObject m_coneSequencePrefab;
	public GameObject m_boltSequencePrefab;
	[TextArea(1, 5)]
	public string m_sequenceNotes;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Smite";
		}
		NanoSmithBoltInfoComponent component = GetComponent<NanoSmithBoltInfoComponent>();
		if (component != null)
		{
			m_boltInfo = component.m_boltInfo.GetShallowCopy();
			if (component.m_smiteRangeOverride > 0f)
			{
				m_boltInfo.range = component.m_smiteRangeOverride;
			}
		}
		else
		{
			Debug.LogError("No bolt info component found for NanoSmith ability");
			m_boltInfo = new NanoSmithBoltInfo();
		}
		ResetTooltipAndTargetingNumbers();
		Targeter = new AbilityUtil_Targeter_Smite(
			this,
			m_coneWidthAngle,
			m_coneLength,
			m_coneBackwardOffset,
			m_conePenetrateLineOfSight,
			m_boltInfo,
			m_boltAngle,
			m_boltCount);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_coneDamageAmount);
		if (m_boltCount > 0 && m_boltInfo != null)
		{
			m_boltInfo.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		}
		return numbers;
	}

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		List<ActorData> hitActors = FindHitActorsHitByCone(targets, caster, null);
		FindHitActorsHitByBolt(
			targets,
			caster,
			out List<List<ActorData>> sequenceActors,
			out List<VectorUtils.LaserCoords> sequenceEndPoints,
			null);
		BoardSquare squareFromVec = Board.Get().GetSquareFromVec3(loSCheckPos);
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
			m_coneSequencePrefab, squareFromVec, hitActors.ToArray(), caster, additionalData.m_sequenceSource);
		list.Add(item);
		m_boltInfo.AddSequenceStartDataForBolts(
			m_boltSequencePrefab, caster, additionalData.m_sequenceSource, sequenceActors, sequenceEndPoints, ref list);
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActorsByCone = FindHitActorsHitByCone(targets, caster, nonActorTargetInfo);
		List<ActorData> hitActorsByBolt = FindHitActorsHitByBolt(targets, caster, out _, out _, nonActorTargetInfo);
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		foreach (ActorData target in hitActorsByCone)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, loSCheckPos));
			actorHitResults.SetBaseDamage(m_coneDamageAmount);
			actorHitResults.AddStandardEffectInfo(m_coneEffectOnEnemyHit);
			abilityResults.StoreActorHit(actorHitResults);
		}
		foreach (ActorData actorData in hitActorsByBolt)
		{
			if (!hitActorsByCone.Contains(actorData))
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, loSCheckPos));
				if (actorData.GetTeam() != caster.GetTeam())
				{
					actorHitResults.SetBaseDamage(m_boltInfo.damageAmount);
					actorHitResults.AddStandardEffectInfo(m_boltInfo.effectOnEnemyHit);
				}
				else
				{
					actorHitResults.AddStandardEffectInfo(m_boltInfo.effectOnAllyHit);
				}
				abilityResults.StoreActorHit(actorHitResults);
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> FindHitActorsHitByCone(
		List<AbilityTarget> targets,
		ActorData caster,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 aimDirection = targets[0].AimDirection;
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
			loSCheckPos,
			coneCenterAngleDegrees,
			m_coneWidthAngle,
			m_coneLength,
			m_coneBackwardOffset,
			m_conePenetrateLineOfSight,
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		if (m_coneMaxTargets > 0)
		{
			TargeterUtils.SortActorsByDistanceToPos(ref actorsInCone, loSCheckPos);
			TargeterUtils.LimitActorsToMaxNumber(ref actorsInCone, m_coneMaxTargets);
		}
		return actorsInCone;
	}

	// added in rogues
	private List<ActorData> FindHitActorsHitByBolt(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<List<ActorData>> sequenceActors,
		out List<VectorUtils.LaserCoords> sequenceEndPoints,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		sequenceActors = new List<List<ActorData>>();
		sequenceEndPoints = new List<VectorUtils.LaserCoords>();
		Vector3 aimDirection = targets[0].AimDirection;
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		float squareSize = Board.Get().squareSize;
		Vector3 vector = Quaternion.AngleAxis(-0.5f * (m_boltCount - 1) * m_boltAngle, Vector3.up) * aimDirection;
		for (int i = 0; i < m_boltCount; i++)
		{
			Vector3 vector2 = Quaternion.AngleAxis(i * m_boltAngle, Vector3.up) * vector;
			Vector3 boltStartPos = loSCheckPos
				+ (m_coneLength + m_coneBackwardOffset) * squareSize * vector2
				- (m_coneBackwardOffset + 0.25f) * squareSize * aimDirection;
			List<ActorData> actorsHitByBolt = m_boltInfo.GetActorsHitByBolt(
				boltStartPos, 
				vector2,
				caster,
				RunPriority,
				out VectorUtils.LaserCoords item,
				nonActorTargetInfo,
				true,
				false,
				true);
			foreach (ActorData hitActor in actorsHitByBolt)
			{
				list.Add(hitActor);
			}
			sequenceActors.Add(actorsHitByBolt);
			sequenceEndPoints.Add(item);
		}
		return list;
	}
#endif
}
