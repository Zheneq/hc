// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ArcherShieldingArrow : Ability
{
	[Header("-- Targeting Properties --")]
	public int m_laserCount = 2;
	public float m_targeterMinAngle;
	public float m_targeterMaxAngle = 100f;
	public float m_targeterMinInterpDistance = 0.5f;
	public float m_targeterMaxInterpDistance = 4f;
	public LaserTargetingInfo m_laserTargetingInfo;
	[Header("-- On Hit --")]
	public float m_laserEnergyGainPerHit;
	[Header("-- Enemy Single Hit Effect")]
	public StandardEffectInfo m_enemySingleHitEffect;
	[Header("-- Enemy Multi Hit Effect")]
	public StandardEffectInfo m_enemyMultiHitEffect;
	[Header("-- Ally Single Hit Effect")]
	public StandardEffectInfo m_allySingleHitEffect;
	[Header("-- Ally Multi Hit Effect")]
	public StandardEffectInfo m_allyMultiHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	
	private LaserTargetingInfo m_cachedLaserTargetingInfo;
	private StandardEffectInfo m_cachedEnemySingleEffect;
	private StandardEffectInfo m_cachedEnemyMultiEffect;
	private StandardEffectInfo m_cachedAllySingleEffect;
	private StandardEffectInfo m_cachedAllyMultiEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Resonance Arrow";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		m_targeterMinAngle = Mathf.Max(0f, m_targeterMinAngle);
		LaserTargetingInfo laserTargetingInfo = GetLaserTargetingInfo();
		Targeter = new AbilityUtil_Targeter_ThiefFanLaser(
			this,
			GetTargeterMinAngle(),
			GetTargeterMaxAngle(),
			m_targeterMinInterpDistance,
			m_targeterMaxInterpDistance,
			laserTargetingInfo.range,
			laserTargetingInfo.width,
			laserTargetingInfo.maxTargets,
			GetLaserCount(),
			laserTargetingInfo.penetrateLos,
			false,
			false,
			false,
			true,
			0);
		Targeter.SetAffectedGroups(laserTargetingInfo.affectsEnemies, laserTargetingInfo.affectsAllies, false);
	}

	private void SetCachedFields()
	{
		m_cachedLaserTargetingInfo = m_laserTargetingInfo;
		m_cachedEnemySingleEffect = m_enemySingleHitEffect;
		m_cachedEnemyMultiEffect = m_enemyMultiHitEffect;
		m_cachedAllySingleEffect = m_allySingleHitEffect;
		m_cachedAllyMultiEffect = m_allyMultiHitEffect;
	}

	public LaserTargetingInfo GetLaserTargetingInfo()
	{
		return m_cachedLaserTargetingInfo ?? m_laserTargetingInfo;
	}

	public StandardEffectInfo GetAllySingleHitEffect()
	{
		return m_cachedAllySingleEffect ?? m_allySingleHitEffect;
	}

	public StandardEffectInfo GetAllyMultiHitEffect()
	{
		return m_cachedAllyMultiEffect ?? m_allyMultiHitEffect;
	}

	public StandardEffectInfo GetEnemySingleHitEffect()
	{
		return m_cachedEnemySingleEffect ?? m_enemySingleHitEffect;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		return m_cachedEnemyMultiEffect ?? m_enemyMultiHitEffect;
	}

	private int GetLaserCount()
	{
		return m_laserCount;
	}

	private float GetTargeterMinAngle()
	{
		return m_targeterMinAngle;
	}

	private float GetTargeterMaxAngle()
	{
		return m_targeterMaxAngle;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		StandardEffectInfo enemySingleHitEffect = m_enemySingleHitEffect;
		StandardEffectInfo enemyMultiHitEffect = m_enemyMultiHitEffect;
		StandardEffectInfo allySingleHitEffect = m_allySingleHitEffect;
		StandardEffectInfo allyMultiHitEffect = m_allyMultiHitEffect;
		AbilityMod.AddToken_EffectInfo(tokens, enemySingleHitEffect, "Effect_EnemySingleHit");
		AbilityMod.AddToken_EffectInfo(tokens, enemyMultiHitEffect, "Effect_EnemyMultiHit");
		AbilityMod.AddToken_EffectInfo(tokens, allySingleHitEffect, "Effect_AllySingleHit");
		AbilityMod.AddToken_EffectInfo(tokens, allyMultiHitEffect, "Effect_AllyMultiHit");
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_enemySingleHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		m_enemyMultiHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		m_allySingleHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		m_allyMultiHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		int absorbAmount = m_allySingleHitEffect.m_effectData.m_absorbAmount;
		int subsequentAmount = m_allyMultiHitEffect.m_effectData.m_absorbAmount - absorbAmount;
		AddNameplateValueForOverlap(
			ref symbolToValue,
			Targeter,
			targetActor,
			currentTargeterIndex,
			absorbAmount,
			subsequentAmount,
			AbilityTooltipSymbol.Absorb);
		return symbolToValue;
	}

	private float CalculateFanAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float value = (currentTarget.FreePos - targetingActor.GetFreePos()).magnitude / Board.Get().squareSize;
		float num = Mathf.Clamp(value, m_targeterMinInterpDistance, m_targeterMaxInterpDistance) - m_targeterMinInterpDistance;
		return Mathf.Max(GetTargeterMinAngle(), GetTargeterMaxAngle() * (1f - num / (m_targeterMaxInterpDistance - m_targeterMinInterpDistance)));
	}

	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(
			fanAngleDegrees, GetTargeterMaxAngle(), m_targeterMinInterpDistance, m_targeterMaxInterpDistance);
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetHitActorsAndHitCount(targets, caster, out List<List<ActorData>> actorsForSequence, out List<Vector3> targetPosForSequences, null);
		for (int i = 0; i < actorsForSequence.Count; i++)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				targetPosForSequences[i],
				actorsForSequence[i].ToArray(),
				caster,
				additionalData.m_sequenceSource));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Dictionary<ActorData, int> hitActorsAndHitCount = GetHitActorsAndHitCount(targets, caster, out _, out _, nonActorTargetInfo);
		foreach (ActorData actorData in hitActorsAndHitCount.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			if (actorData.GetTeam() != caster.GetTeam())
			{
				StandardEffectInfo effect = hitActorsAndHitCount[actorData] < 2
					? GetEnemySingleHitEffect()
					: GetEnemyMultiHitEffect();
				actorHitResults.AddStandardEffectInfo(effect);
				if (m_laserEnergyGainPerHit > 0f)
				{
					int techPointGainOnCaster = Mathf.RoundToInt(m_laserEnergyGainPerHit * hitActorsAndHitCount[actorData]);
					actorHitResults.SetTechPointGainOnCaster(techPointGainOnCaster);
				}
			}
			else if (hitActorsAndHitCount[actorData] < 2)
			{
				actorHitResults.AddStandardEffectInfo(GetAllySingleHitEffect());
			}
			else
			{
				actorHitResults.AddStandardEffectInfo(GetAllyMultiHitEffect());
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private Dictionary<ActorData, int> GetHitActorsAndHitCount(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<List<ActorData>> actorsForSequence,
		out List<Vector3> targetPosForSequences,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		actorsForSequence = new List<List<ActorData>>();
		targetPosForSequences = new List<Vector3>();
		int laserCount = GetLaserCount();
		float angle = laserCount > 1
			? CalculateFanAngleDegrees(targets[0], caster)
			: 0f;
		float step = laserCount > 1
			? angle / (laserCount - 1)
			: 0f;
		float startAngle = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection) - 0.5f * (laserCount - 1) * step;
		LaserTargetingInfo laserTargetingInfo = GetLaserTargetingInfo();
		int maxTargets = laserTargetingInfo.maxTargets;
		for (int i = 0; i < laserCount; i++)
		{
			Vector3 dir = VectorUtils.AngleDegreesToVector(startAngle + i * step);
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = caster.GetLoSCheckPos();
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, laserTargetingInfo.affectsAllies, laserTargetingInfo.affectsEnemies);
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
				laserCoords.start,
				dir,
				laserTargetingInfo.range,
				laserTargetingInfo.width,
				caster,
				relevantTeams,
				laserTargetingInfo.penetrateLos,
				maxTargets,
				false,
				true,
				out laserCoords.end,
				nonActorTargetInfo);
			actorsForSequence.Add(new List<ActorData>());
			foreach (ActorData actorData in actorsInLaser)
			{
				if (dictionary.ContainsKey(actorData))
				{
					dictionary[actorData]++;
				}
				else
				{
					dictionary[actorData] = 1;
					actorsForSequence[i].Add(actorData);
				}
			}
			int count = actorsForSequence[i].Count;
			Vector3 targetPos = count > 0
				? actorsForSequence[i][count - 1].GetFreePos()
				: laserCoords.end;
			targetPosForSequences.Add(targetPos);
		}
		return dictionary;
	}
#endif
}
