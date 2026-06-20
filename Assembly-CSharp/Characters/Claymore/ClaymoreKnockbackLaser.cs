// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreKnockbackLaser : Ability
{
	[Header("-- Only As Indicator For Passive? (If true, the ability cannot be cast)")]
	public bool m_onlyAsIndicatorForPassive;
	[Header("-- Laser Targeting")]
	public float m_laserRange = 4f;
	public float m_laserFullWidth = 5f;
	public float m_laserMiddleWidth = 1f;
	public bool m_penetrateLos;
	public bool m_lengthIgnoreWorldGeo;
	[Header("-- Knockback Params")]
	public KnockbackType m_knockbackType = KnockbackType.PerpendicularAwayFromAimDir;
	public float m_knockbackDistance = 2f;
	[Header("-- Middle Hit Damage/Effect")]
	public int m_middleHitDamage = 20;
	public StandardEffectInfo m_middleEnemyHitEffect;
	[Header("-- Knockback Hit Damage/Effect")]
	public int m_knockbackDamage = 10;
	public StandardEffectInfo m_knockbackEnemyHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Way of the Waste";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_ClaymoreKnockbackLaser(
			this,
			GetLaserFullWidth(),
			GetLaserRange(),
			m_penetrateLos,
			m_lengthIgnoreWorldGeo,
			0,
			GetLaserMiddleWidth(),
			GetKnockbackDistance(),
			m_knockbackType);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_middleHitDamage);
		m_middleEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_knockbackDamage);
		m_knockbackEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return !m_onlyAsIndicatorForPassive;
	}

	private float GetLaserRange()
	{
		return m_laserRange;
	}

	private float GetLaserFullWidth()
	{
		return m_laserFullWidth;
	}

	private float GetLaserMiddleWidth()
	{
		return m_laserMiddleWidth;
	}

	private float GetKnockbackDistance()
	{
		return m_knockbackDistance;
	}

	private int GetMiddleHitDamage()
	{
		return m_middleHitDamage;
	}

	private StandardEffectInfo GetMiddleEnemyHitEffect()
	{
		return m_middleEnemyHitEffect;
	}

	private int GetKnockbackDamage()
	{
		return m_knockbackDamage;
	}

	private StandardEffectInfo GetKnockbackEnemyHieEffect()
	{
		return m_knockbackEnemyHitEffect;
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
		GetHitActors(targets, caster, out var middleHitActors, out var knockbackHitActors, out Vector3 _, nonActorTargetInfo);
		foreach (ActorData target in middleHitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, caster.GetFreePos()));
			actorHitResults.SetBaseDamage(GetMiddleHitDamage());
			actorHitResults.AddStandardEffectInfo(GetMiddleEnemyHitEffect());
			abilityResults.StoreActorHit(actorHitResults);
		}
		foreach (ActorData target in knockbackHitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, caster.GetFreePos()));
			actorHitResults.SetBaseDamage(GetKnockbackDamage());
			actorHitResults.AddStandardEffectInfo(GetKnockbackEnemyHieEffect());
			if (GetKnockbackDistance() > 0f)
			{
				KnockbackHitData knockbackData = new KnockbackHitData(target, caster, m_knockbackType, targets[0].AimDirection, caster.GetFreePos(), GetKnockbackDistance());
				actorHitResults.AddKnockbackData(knockbackData);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private void GetHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<ActorData> middleHitActors,
		out List<ActorData> knockbackHitActors,
		out Vector3 laserEndPos,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> otherTeams = caster.GetOtherTeams();
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInFullWidth = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			GetLaserRange(),
			GetLaserFullWidth(),
			caster,
			otherTeams,
			m_penetrateLos,
			-1,
			m_lengthIgnoreWorldGeo,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		middleHitActors = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			GetLaserRange(),
			GetLaserMiddleWidth(),
			caster,
			otherTeams,
			m_penetrateLos,
			-1,
			m_lengthIgnoreWorldGeo,
			true,
			out laserCoords.end,
			null);
		knockbackHitActors = new List<ActorData>();
		foreach (ActorData item in actorsInFullWidth)
		{
			if (!AreaEffectUtils.GetActorsInLaser(
				    laserCoords.start,
				    targets[0].AimDirection,
				    GetLaserRange(),
				    GetLaserMiddleWidth(),
				    caster,
				    otherTeams,
				    m_penetrateLos,
				    -1,
				    m_lengthIgnoreWorldGeo,
				    true,
				    out laserCoords.end,
				    null).Contains(item))
			{
				knockbackHitActors.Add(item);
			}
		}
		laserEndPos = laserCoords.end;
	}
#endif
}
