// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// empty in rogues
public class ClaymoreSlam : Ability
{
	[Header("-- Laser Targeting")]
	public float m_laserRange = 4f;
	public float m_midLaserWidth = 1f;
	public float m_fullLaserWidth = 2f;
	public int m_laserMaxTargets;
	public bool m_penetrateLos;
	public bool m_laserLengthIgnoreWorldGeo = true;
	[Header("-- Normal Hit Damage/Effects")]
	public int m_middleDamage = 20;
	public StandardEffectInfo m_middleEnemyHitEffect;
	public int m_sideDamage = 10;
	public StandardEffectInfo m_sideEnemyHitEffect;
	[Header("-- Extra Damage on Side")]
	public int m_extraSideDamagePerMiddleHit;
	[Header("-- Extra Damage from Target Health Threshold (0 to 1) --")]
	public int m_extraDamageOnLowHealthTarget;
	public float m_lowHealthThreshold;
	[Header("-- Energy Damage")]
	public int m_energyLossOnMidHit;
	public int m_energyLossOnSideHit;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ClaymoreSlam m_abilityMod;
	private Claymore_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedMiddleEnemyHitEffect;
	private StandardEffectInfo m_cachedSideEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Overhead Slam";
		}
		Setup();
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
		m_cachedMiddleEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_middleEnemyHitEffectMod.GetModifiedValue(m_middleEnemyHitEffect)
			: m_middleEnemyHitEffect;
		m_cachedSideEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_sideEnemyHitEffectMod.GetModifiedValue(m_sideEnemyHitEffect)
			: m_sideEnemyHitEffect;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange) 
			: m_laserRange;
	}

	public float GetMidLaserWidth()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_midLaserWidthMod.GetModifiedValue(m_midLaserWidth) 
			: m_midLaserWidth;
	}

	public float GetFullLaserWidth()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_fullLaserWidthMod.GetModifiedValue(m_fullLaserWidth) 
			: m_fullLaserWidth;
	}

	public int GetLaserMaxTargets()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets) 
			: m_laserMaxTargets;
	}

	public bool GetPenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public int GetMiddleDamage()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_middleDamageMod.GetModifiedValue(m_middleDamage) 
			: m_middleDamage;
	}

	public StandardEffectInfo GetMiddleEnemyHitEffect()
	{
		return m_cachedMiddleEnemyHitEffect ?? m_middleEnemyHitEffect;
	}

	public int GetSideDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_sideDamageMod.GetModifiedValue(m_sideDamage)
			: m_sideDamage;
	}

	public StandardEffectInfo GetSideEnemyHitEffect()
	{
		return m_cachedSideEnemyHitEffect ?? m_sideEnemyHitEffect;
	}

	public int GetExtraSideDamagePerMiddleHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraSideDamagePerMiddleHitMod.GetModifiedValue(m_extraSideDamagePerMiddleHit) 
			: m_extraSideDamagePerMiddleHit;
	}

	public int GetExtraDamageOnLowHealthTarget()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamageOnLowHealthTargetMod.GetModifiedValue(m_extraDamageOnLowHealthTarget) 
			: m_extraDamageOnLowHealthTarget;
	}

	public float GetLowHealthThreshold()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_lowHealthThresholdMod.GetModifiedValue(m_lowHealthThreshold) 
			: m_lowHealthThreshold;
	}

	public int GetEnergyLossOnMidHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_energyLossOnMidHitMod.GetModifiedValue(m_energyLossOnMidHit) 
			: m_energyLossOnMidHit;
	}

	public int GetEnergyLossOnSideHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_energyLossOnSideHitMod.GetModifiedValue(m_energyLossOnSideHit) 
			: m_energyLossOnSideHit;
	}

	public int GetHealPerMidHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_healPerMidHit.GetModifiedValue(0) 
			: 0;
	}

	public int GetHealPerSideHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_healPerSideHit.GetModifiedValue(0) 
			: 0;
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Claymore_SyncComponent>();
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_ClaymoreKnockbackLaser(
			this,
			GetFullLaserWidth(),
			GetLaserRange(),
			GetPenetrateLos(),
			m_laserLengthIgnoreWorldGeo,
			0,
			GetMidLaserWidth(),
			0f,
			KnockbackType.AwayFromSource);
		bool affectsCaster = GetHealPerMidHit() > 0 || GetHealPerSideHit() > 0;
		Targeter.SetAffectedGroups(true, false, affectsCaster);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreSlam abilityMod_ClaymoreSlam = modAsBase as AbilityMod_ClaymoreSlam;
		AddTokenInt(tokens, "LaserMaxTargets", string.Empty, abilityMod_ClaymoreSlam != null
			? abilityMod_ClaymoreSlam.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets)
			: m_laserMaxTargets);
		AddTokenInt(tokens, "MiddleDamage", string.Empty, abilityMod_ClaymoreSlam != null
			? abilityMod_ClaymoreSlam.m_middleDamageMod.GetModifiedValue(m_middleDamage)
			: m_middleDamage);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreSlam != null
			? abilityMod_ClaymoreSlam.m_middleEnemyHitEffectMod.GetModifiedValue(m_middleEnemyHitEffect)
			: m_middleEnemyHitEffect, "MiddleEnemyHitEffect", m_middleEnemyHitEffect);
		AddTokenInt(tokens, "SideDamage", string.Empty, abilityMod_ClaymoreSlam != null
			? abilityMod_ClaymoreSlam.m_sideDamageMod.GetModifiedValue(m_sideDamage)
			: m_sideDamage);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreSlam != null
			? abilityMod_ClaymoreSlam.m_sideEnemyHitEffectMod.GetModifiedValue(m_sideEnemyHitEffect)
			: m_sideEnemyHitEffect, "SideEnemyHitEffect", m_sideEnemyHitEffect);
		AddTokenInt(tokens, "ExtraSideDamagePerMiddleHit", string.Empty, abilityMod_ClaymoreSlam != null
			? abilityMod_ClaymoreSlam.m_extraSideDamagePerMiddleHitMod.GetModifiedValue(m_extraSideDamagePerMiddleHit)
			: m_extraSideDamagePerMiddleHit);
		AddTokenInt(tokens, "ExtraDamageOnLowHealthTarget", string.Empty, abilityMod_ClaymoreSlam != null
			? abilityMod_ClaymoreSlam.m_extraDamageOnLowHealthTargetMod.GetModifiedValue(m_extraDamageOnLowHealthTarget)
			: m_extraDamageOnLowHealthTarget);
		AddTokenInt(tokens, "EnergyLossOnMidHit", string.Empty, abilityMod_ClaymoreSlam != null
			? abilityMod_ClaymoreSlam.m_energyLossOnMidHitMod.GetModifiedValue(m_energyLossOnMidHit)
			: m_energyLossOnMidHit);
		AddTokenInt(tokens, "EnergyLossOnSideHit", string.Empty, abilityMod_ClaymoreSlam != null
			? abilityMod_ClaymoreSlam.m_energyLossOnSideHitMod.GetModifiedValue(m_energyLossOnSideHit)
			: m_energyLossOnSideHit);
		AddTokenInt(tokens, "HealPerMidHit", string.Empty, abilityMod_ClaymoreSlam != null
			? abilityMod_ClaymoreSlam.m_healPerMidHit.GetModifiedValue(0)
			: 0);
		AddTokenInt(tokens, "HealPerSideHit", string.Empty, abilityMod_ClaymoreSlam != null
			? abilityMod_ClaymoreSlam.m_healPerSideHit.GetModifiedValue(0)
			: 0);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetMiddleDamage());
		m_middleEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Primary, -1 * GetEnergyLossOnMidHit());
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetSideDamage());
		m_sideEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Secondary, -1 * GetEnergyLossOnSideHit());
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, 0));
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int numPrimary = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Primary);
		int numSecondary = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Secondary);
		int damage = 0;
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy)
		    && targetActor.GetHitPointPercent() < GetLowHealthThreshold())
		{
			damage = GetExtraDamageOnLowHealthTarget();
		}
		dictionary[AbilityTooltipSymbol.Damage] = damage;
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			dictionary[AbilityTooltipSymbol.Damage] += GetMiddleDamage();
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
		{
			int sideDamage = GetSideDamage();
			if (GetExtraSideDamagePerMiddleHit() > 0)
			{
				sideDamage += numPrimary * GetExtraSideDamagePerMiddleHit();
			}
			dictionary[AbilityTooltipSymbol.Damage] += sideDamage;
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			int healing = GetHealPerMidHit() * numPrimary + GetHealPerSideHit() * numSecondary;
			if (healing > 0)
			{
				dictionary[AbilityTooltipSymbol.Healing] = healing;
			}
		}
		return dictionary;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return m_syncComp != null
			? m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, ActorData)
			: null;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreSlam))
		{
			m_abilityMod = abilityMod as AbilityMod_ClaymoreSlam;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
	
#if SERVER
	// custom
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

	// custom
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		AbilityTarget currentTarget = targets[0];
		VectorUtils.LaserCoords laserCoords = new VectorUtils.LaserCoords
		{
			start = caster.GetLoSCheckPos()
		};
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			currentTarget.AimDirection,
			GetLaserRange(),
			GetFullLaserWidth(),
			caster,
			caster.GetOtherTeams(),
			GetPenetrateLos(),
			0,
			m_laserLengthIgnoreWorldGeo,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		List<ActorData> actorsInMiddleLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			currentTarget.AimDirection,
			GetLaserRange(),
			GetMidLaserWidth(),
			caster,
			caster.GetOtherTeams(),
			GetPenetrateLos(),
			0,
			m_laserLengthIgnoreWorldGeo,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		List<ActorData> actorsInOuterLaser = new List<ActorData>();
		foreach (ActorData item in actorsInLaser)
		{
			if (!actorsInMiddleLaser.Contains(item))
			{
				actorsInOuterLaser.Add(item);
			}
		}
		int numPrimary = actorsInMiddleLaser.Count;
		int numSecondary = actorsInOuterLaser.Count;
		foreach (ActorData targetActor in actorsInLaser)
		{
			int damage = 0;
			StandardEffectInfo effect;
			if (targetActor.GetHitPointPercent() < GetLowHealthThreshold())
			{
				damage = GetExtraDamageOnLowHealthTarget();
			}
			if (actorsInMiddleLaser.Contains(targetActor))
			{
				damage += GetMiddleDamage();
				effect = GetMiddleEnemyHitEffect();
			}
			else
			{
				int sideDamage = GetSideDamage();
				if (GetExtraSideDamagePerMiddleHit() > 0)
				{
					sideDamage += numPrimary * GetExtraSideDamagePerMiddleHit();
				}
				damage += sideDamage;
				effect = GetSideEnemyHitEffect();
			}
			Vector3 damageOrigin = caster.GetFreePos();
			ActorHitParameters hitParams = new ActorHitParameters(targetActor, damageOrigin);
			ActorHitResults hitResults = new ActorHitResults(damage, HitActionType.Damage, effect, hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
		if (GetHealPerMidHit() > 0 || GetHealPerSideHit() > 0)
		{
			int healing = GetHealPerMidHit() * numPrimary + GetHealPerSideHit() * numSecondary;
			ActorHitParameters hitParams = new ActorHitParameters(caster, caster.GetFreePos());
			ActorHitResults hitResults = new ActorHitResults(healing, HitActionType.Healing, (StandardEffectInfo) null, hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
