// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithBuffDebuffCone : Ability
{
	[Header("-- Cone Targeting")]
	public float m_coneAngle = 270f;
	public float m_coneLength = 1.5f;
	public bool m_conePenetrateLineOfSight;
	[Header("-- Hit Effects")]
	public StandardEffectInfo m_enemyHitEffect;
	public StandardEffectInfo m_allyHitEffect;
	public StandardEffectInfo m_casterHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Buff Debuff Cone";
		}
		m_sequencePrefab = m_castSequencePrefab;
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_DirectionCone(
			this,
			m_coneAngle,
			m_coneLength,
			0f,
			m_conePenetrateLineOfSight,
			true,
			m_enemyHitEffect.m_applyEffect,
			m_allyHitEffect.m_applyEffect,
			m_casterHitEffect.m_applyEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		m_allyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		m_casterHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			targets[0].FreePos,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		foreach (ActorData actorData in GetHitActors(targets, caster, nonActorTargetInfo))
		{
			ActorHitResults actorHitResults = new ActorHitResults(
				new ActorHitParameters(actorData, caster.GetFreePos()));
			if (actorData == caster)
			{
				actorHitResults.AddStandardEffectInfo(m_casterHitEffect);
			}
			else if (actorData.GetTeam() == caster.GetTeam())
			{
				actorHitResults.AddStandardEffectInfo(m_allyHitEffect);
			}
			else
			{
				actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private new List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		Vector3 aimDirection = targets[0].AimDirection;
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
			loSCheckPos,
			coneCenterAngleDegrees,
			m_coneAngle,
			m_coneLength,
			0f,
			m_conePenetrateLineOfSight,
			caster,
			null,
			nonActorTargetInfo);
		foreach (ActorData actorData in actorsInCone)
		{
			if (IsActorRelevant(actorData, caster))
			{
				list.Add(actorData);
			}
		}
		if (m_casterHitEffect.m_applyEffect && !list.Contains(caster))
		{
			list.Add(caster);
		}
		return list;
	}

	// added in rogues
	private bool IsActorRelevant(ActorData actor, ActorData caster)
	{
		bool applyEffect;
		if (actor == caster)
		{
			applyEffect = m_casterHitEffect.m_applyEffect;
		}
		else if (actor.GetTeam() == caster.GetTeam())
		{
			applyEffect = m_allyHitEffect.m_applyEffect;
		}
		else
		{
			applyEffect = m_enemyHitEffect.m_applyEffect;
		}
		return applyEffect;
	}
#endif
}
