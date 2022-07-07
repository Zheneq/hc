// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RampartMeleeBasicAttack : Ability
{
	[Header("-- Laser Targeting")]
	public float m_laserRange = 4f;
	public float m_laserWidth = 1f;
	public int m_laserMaxTargets;
	public bool m_penetrateLos;
	[Header("-- Cone Targeting")]
	public float m_coneWidthAngle = 90f;
	public float m_coneRange = 2.5f;
	[Header("-- Hit Damage/Effects")]
	public int m_laserDamage = 20;
	public StandardEffectInfo m_laserEnemyHitEffect;
	public int m_coneDamage = 10;
	public StandardEffectInfo m_coneEnemyHitEffect;
	public int m_bonusDamageForOverlap;
	[Header("-- Sequences")]
	public GameObject m_laserSequencePrefab;
	public GameObject m_coneSequencePrefab;

	private AbilityMod_RampartMeleeBasicAttack m_abilityMod;
	private StandardEffectInfo m_cachedLaserEnemyHitEffect;
	private StandardEffectInfo m_cachedConeEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Overhead Slam";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_ClaymoreSlam(this, GetLaserRange(), GetLaserWidth(), GetLaserMaxTargets(), GetConeAngle(), GetConeRange(), 0f, PenetrateLos(), true, false, false, GetBonusDamageForOverlap() > 0);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	private void SetCachedFields()
	{
		m_cachedLaserEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(m_laserEnemyHitEffect)
			: m_laserEnemyHitEffect;
		m_cachedConeEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_coneEnemyHitEffectMod.GetModifiedValue(m_coneEnemyHitEffect)
			: m_coneEnemyHitEffect;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange)
			: m_laserRange;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public int GetLaserMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets)
			: m_laserMaxTargets;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public float GetConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float GetConeRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneRangeMod.GetModifiedValue(m_coneRange)
			: m_coneRange;
	}

	public int GetLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamage)
			: m_laserDamage;
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		return m_cachedLaserEnemyHitEffect ?? m_laserEnemyHitEffect;
	}

	public int GetConeDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneDamageMod.GetModifiedValue(m_coneDamage)
			: m_coneDamage;
	}

	public StandardEffectInfo GetConeEnemyHitEffect()
	{
		return m_cachedConeEnemyHitEffect ?? m_coneEnemyHitEffect;
	}

	public int GetBonusDamageForOverlap()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bonusDamageForOverlapMod.GetModifiedValue(m_bonusDamageForOverlap)
			: m_bonusDamageForOverlap;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartMeleeBasicAttack))
		{
			m_abilityMod = abilityMod as AbilityMod_RampartMeleeBasicAttack;
		}
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetLaserDamage());
		m_laserEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		bool hasPrimary = tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary);
		bool hasSecondary = tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary);
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		if (hasPrimary && hasSecondary)
		{
			result[AbilityTooltipSymbol.Damage] = GetLaserDamage() + GetBonusDamageForOverlap();
		}
		else if (hasPrimary)
		{
			result[AbilityTooltipSymbol.Damage] = GetLaserDamage();
		}
		else if (hasSecondary)
		{
			result[AbilityTooltipSymbol.Damage] = GetConeDamage();
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartMeleeBasicAttack mod = modAsBase as AbilityMod_RampartMeleeBasicAttack;
		AddTokenInt(tokens, "LaserMaxTargets", "", mod != null
			? mod.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets)
			: m_laserMaxTargets);
		AddTokenInt(tokens, "LaserDamage", "", mod != null
			? mod.m_laserDamageMod.GetModifiedValue(m_laserDamage)
			: m_laserDamage);
		AbilityMod.AddToken_EffectInfo(tokens, mod != null
			? mod.m_laserEnemyHitEffectMod.GetModifiedValue(m_laserEnemyHitEffect)
			: m_laserEnemyHitEffect, "LaserEnemyHitEffect", m_laserEnemyHitEffect);
		AddTokenInt(tokens, "ConeDamage", "", mod != null
			? mod.m_coneDamageMod.GetModifiedValue(m_coneDamage)
			: m_coneDamage);
		AbilityMod.AddToken_EffectInfo(tokens, mod != null
			? mod.m_coneEnemyHitEffectMod.GetModifiedValue(m_coneEnemyHitEffect)
			: m_coneEnemyHitEffect, "ConeEnemyHitEffect", m_coneEnemyHitEffect);
	}

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		Vector3 hitActors = GetHitActors(targets, caster, out var laserHitActors, out var coneHitActors, null);
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_laserSequencePrefab, hitActors, laserHitActors.ToArray(), caster, additionalData.m_sequenceSource);
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
		foreach (ActorData actorData in laserHitActors)
		{
			if (AreaEffectUtils.IsSquareInCone(actorData.GetCurrentBoardSquare(), caster.GetLoSCheckPos(), coneCenterAngleDegrees, GetConeAngle(), GetConeRange(), 0f, PenetrateLos(), caster) && !coneHitActors.Contains(actorData))
			{
				coneHitActors.Add(actorData);
			}
		}
		ServerClientUtils.SequenceStartData item2 = new ServerClientUtils.SequenceStartData(m_coneSequencePrefab, targets[0].FreePos, coneHitActors.ToArray(), caster, additionalData.m_sequenceSource);
		list.Add(item2);
		list.Add(item);
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		GetHitActors(targets, caster, out var laserHitActors, out var coneHitActors, nonActorTargetInfo);
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		foreach (ActorData actorData in laserHitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, loSCheckPos));
			int damage = GetLaserDamage();
			if (GetBonusDamageForOverlap() > 0 && coneHitActors.Contains(actorData))
			{
				damage += GetBonusDamageForOverlap();
			}
			actorHitResults.SetBaseDamage(damage);
			actorHitResults.AddStandardEffectInfo(GetLaserEnemyHitEffect());
			abilityResults.StoreActorHit(actorHitResults);
		}
		foreach (ActorData actorData2 in coneHitActors)
		{
			if (!laserHitActors.Contains(actorData2))
			{
				ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(actorData2, loSCheckPos));
				actorHitResults2.SetBaseDamage(GetConeDamage());
				actorHitResults2.AddStandardEffectInfo(GetConeEnemyHitEffect());
				abilityResults.StoreActorHit(actorHitResults2);
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private Vector3 GetHitActors(List<AbilityTarget> targets, ActorData caster, out List<ActorData> laserHitActors, out List<ActorData> coneHitActors, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		laserHitActors = AreaEffectUtils.GetActorsInLaser(laserCoords.start, targets[0].AimDirection, GetLaserRange(), GetLaserWidth(), caster, caster.GetOtherTeams(), PenetrateLos(), GetLaserMaxTargets(), false, true, out laserCoords.end, nonActorTargetInfo);
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
		coneHitActors = AreaEffectUtils.GetActorsInCone(loSCheckPos, coneCenterAngleDegrees, GetConeAngle(), GetConeRange(), 0f, PenetrateLos(), caster, caster.GetOtherTeams(), nonActorTargetInfo);
		return laserCoords.end;
	}
#endif
}
