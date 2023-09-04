using System;
using System.Collections.Generic;
using UnityEngine;

public class ClericHammerThrow : Ability
{
	[Serializable]
	public class RadiusToHitData : RadiusToDataBase
	{
		public int m_damage;
		public StandardEffectInfo m_hitEffectInfo;

		public RadiusToHitData(float radiusInSquares, int damage, StandardEffectInfo hitEffect)
		{
			m_radius = radiusInSquares;
			m_damage = damage;
			m_hitEffectInfo = hitEffect;
		}
	}

	[Separator("Targeting")]
	public float m_maxDistToRingCenter = 5.5f;
	public float m_outerRadius = 2.5f;
	public float m_innerRadius = 1f;
	public bool m_ignoreLos;
	public bool m_clampRingToCursorPos = true;
	[Separator("On Hit")]
	public int m_outerHitDamage = 15;
	public StandardEffectInfo m_outerEnemyHitEffect;
	public int m_innerHitDamage = 20;
	public StandardEffectInfo m_innerEnemyHitEffect;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;
	
	private List<RadiusToHitData> m_cachedRadiusToHitData = new List<RadiusToHitData>();
	private AbilityMod_ClericHammerThrow m_abilityMod;
	private Cleric_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedOuterEnemyHitEffect;
	private StandardEffectInfo m_cachedInnerEnemyHitEffect;
	private StandardEffectInfo m_cachedOuterEnemyHitEffectWithNoInnerHits;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ClericHammerThrow";
		}
		m_syncComp = GetComponent<Cleric_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_cachedRadiusToHitData.Clear();
		m_cachedRadiusToHitData.Add(new RadiusToHitData(GetInnerRadius(), GetInnerHitDamage(), GetInnerEnemyHitEffect()));
		m_cachedRadiusToHitData.Add(new RadiusToHitData(GetOuterRadius(), GetOuterHitDamage(), GetOuterEnemyHitEffect()));
		m_cachedRadiusToHitData.Sort();
		Targeter = new AbilityUtil_Targeter_MartyrLaser(
			this,
			0f,
			GetMaxDistToRingCenter(),
			false,
			-1,
			true,
			false,
			false,
			true,
			false,
			GetOuterRadius(),
			GetInnerRadius(),
			false,
			true,
			false);
		Targeter.SetShowArcToShape(true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "OuterHitDamage", string.Empty, m_outerHitDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_outerEnemyHitEffect, "OuterEnemyHitEffect", m_outerEnemyHitEffect);
		AddTokenInt(tokens, "InnerHitDamage", string.Empty, m_innerHitDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_innerEnemyHitEffect, "InnerEnemyHitEffect", m_innerEnemyHitEffect);
	}

	private void SetCachedFields()
	{
		m_cachedOuterEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_outerEnemyHitEffectMod.GetModifiedValue(m_outerEnemyHitEffect)
			: m_outerEnemyHitEffect;
		m_cachedInnerEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_innerEnemyHitEffectMod.GetModifiedValue(m_innerEnemyHitEffect)
			: m_innerEnemyHitEffect;
		m_cachedOuterEnemyHitEffectWithNoInnerHits = m_abilityMod != null
			? m_abilityMod.m_outerEnemyHitEffectWithNoInnerHits.GetModifiedValue(null)
			: null;
	}

	public float GetMaxDistToRingCenter()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxDistToRingCenterMod.GetModifiedValue(m_maxDistToRingCenter)
			: m_maxDistToRingCenter;
	}

	public float GetOuterRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_outerRadiusMod.GetModifiedValue(m_outerRadius)
			: m_outerRadius;
	}

	public float GetInnerRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_innerRadiusMod.GetModifiedValue(m_innerRadius)
			: m_innerRadius;
	}

	public bool IgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_ignoreLosMod.GetModifiedValue(m_ignoreLos)
			: m_ignoreLos;
	}

	public bool ClampRingToCursorPos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_clampRingToCursorPosMod.GetModifiedValue(m_clampRingToCursorPos)
			: m_clampRingToCursorPos;
	}

	public int GetOuterHitDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_outerHitDamageMod.GetModifiedValue(m_outerHitDamage)
			: m_outerHitDamage;
	}

	public StandardEffectInfo GetOuterEnemyHitEffect()
	{
		return m_cachedOuterEnemyHitEffect ?? m_outerEnemyHitEffect;
	}

	public int GetInnerHitDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_innerHitDamageMod.GetModifiedValue(m_innerHitDamage)
			: m_innerHitDamage;
	}

	public StandardEffectInfo GetInnerEnemyHitEffect()
	{
		return m_cachedInnerEnemyHitEffect ?? m_innerEnemyHitEffect;
	}

	public StandardEffectInfo GetOuterEnemyHitEffectWithNoInnerHits()
	{
		return m_cachedOuterEnemyHitEffectWithNoInnerHits;
	}

	public int GetExtraInnerDamagePerOuterHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraInnerDamagePerOuterHit.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraTPGainInAreaBuff()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraTechPointGainInAreaBuff.GetModifiedValue(0)
			: 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_outerHitDamage);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) <= 0
		    || m_cachedRadiusToHitData.Count <= 0)
		{
			return false;
		}
		RadiusToHitData bestMatchingData = AbilityCommon_LayeredRings.GetBestMatchingData(
			m_cachedRadiusToHitData,
			targetActor.GetCurrentBoardSquare(),
			(Targeter as AbilityUtil_Targeter_MartyrLaser).m_lastLaserEndPos,
			ActorData,
			true);
		if (bestMatchingData != null)
		{
			int extraDamage = 0;
			if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Tertiary) == 0)
			{
				extraDamage = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Tertiary) * GetExtraInnerDamagePerOuterHit();
			}
			results.m_damage = bestMatchingData.m_damage + extraDamage;
			return true;
		}
		return false;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		return caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ClericAreaBuff))
			? GetExtraTPGainInAreaBuff() * Targeter.GetNumActorsInRange()
			: base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClericHammerThrow))
		{
			m_abilityMod = (abilityMod as AbilityMod_ClericHammerThrow);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

    // custom
    public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        return new ServerClientUtils.SequenceStartData(
            m_sequencePrefab,
            caster.GetCurrentBoardSquare(),
            additionalData.m_abilityResults.HitActorsArray(),
            caster,
            additionalData.m_sequenceSource);
    }

	// custom
    public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
    {
		AbilityTarget target = targets[0];
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();

		// Inner
		List<ActorData> actorsInInnerRadius = AreaEffectUtils.GetActorsInRadius(target.FreePos, GetInnerRadius(), IgnoreLos(), caster, caster.GetEnemyTeam(), nonActorTargetInfo);
		foreach (ActorData targetActor in actorsInInnerRadius)
		{
			ActorHitParameters hitParams = new ActorHitParameters(targetActor, target.FreePos);
			ActorHitResults hitResults = new ActorHitResults(GetInnerHitDamage(), HitActionType.Damage, GetInnerEnemyHitEffect(), hitParams);
			abilityResults.StoreActorHit(hitResults);
		}

        // Outer

		// check for effect on no inner hits
        StandardEffectInfo effectToApply = GetOuterEnemyHitEffect();
		if (actorsInInnerRadius.Count == 0)
		{
            effectToApply = GetOuterEnemyHitEffectWithNoInnerHits();
        }
		
        List <ActorData> actorsInOuterRadius = AreaEffectUtils.GetActorsInRadius(target.FreePos, GetOuterRadius(), IgnoreLos(), caster, caster.GetEnemyTeam(), nonActorTargetInfo);
        foreach (ActorData targetActor in actorsInOuterRadius)
        {
            ActorHitParameters hitParams = new ActorHitParameters(targetActor, target.FreePos);
            ActorHitResults hitResults = new ActorHitResults(GetOuterHitDamage(), HitActionType.Damage, effectToApply, hitParams);
            abilityResults.StoreActorHit(hitResults);
        }

		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
    }
}
