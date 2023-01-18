using System.Collections.Generic;
using UnityEngine;

public class SamuraiSelfBuff : Ability
{
	[Header("-- Buffs")]
	public bool m_selfBuffLastsUntilYouDealDamage;
	public StandardEffectInfo m_selfBuffEffect;
	[Header("-- Extra damage to other abilities if this ability is queued")]
	public int m_extraDamageIfQueued;
	[Header("-- Shielding")]
	public int m_baseShielding = 30;
	public int m_extraShieldingIfOnlyAbility;
	public StandardEffectInfo m_generalEffectOnSelf;
	[Header("-- AoE")]
	public float m_aoeRadius = 1.5f;
	public float m_knockbackDist = 2f;
	public int m_damageAmount;
	public bool m_penetrateLoS;
	[Header("-- Damage reactions")]
	public int m_damageIncreaseFirstHit = 10;
	public int m_damageIncreaseSubseqHits;
	public int m_techPointGainPerIncomingHit;
	public bool m_buffInResponseToIndirectDamage = true;
	[Space(10f)]
	public int m_cdrIfNotHit;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_hitReactionSequencePrefab;
	public GameObject m_buffStartSequencePrefab;

	private AbilityMod_SamuraiSelfBuff m_abilityMod;
	private Samurai_SyncComponent m_syncComponent;
	private AbilityData.ActionType m_myActionType;
	private AbilityData m_abilityData;
	private StandardEffectInfo m_cachedSelfBuffEffect;
	private StandardEffectInfo m_cachedGeneralEffectOnSelf;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Self Buff";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_abilityData = GetComponent<AbilityData>();
		m_myActionType = GetActionTypeOfAbility(this);
		m_syncComponent = ActorData.GetComponent<Samurai_SyncComponent>();
		SetCachedFields();
		AbilityUtil_Targeter_StretchCone targeter = new AbilityUtil_Targeter_StretchCone(
			this,
			GetAoeRadius(),
			GetAoeRadius(),
			360f,
			360f,
			AreaEffectUtils.StretchConeStyle.Linear,
			0f,
			PenetrateLoS());
		targeter.InitKnockbackData(GetKnockbackDist(), KnockbackType.AwayFromSource, 0f, KnockbackType.AwayFromSource);
		targeter.m_includeCaster = true;
		Targeter = targeter;
	}

	private void SetCachedFields()
	{
		m_cachedSelfBuffEffect = m_abilityMod != null
			? m_abilityMod.m_selfBuffEffectMod.GetModifiedValue(m_selfBuffEffect)
			: m_selfBuffEffect;
		m_cachedGeneralEffectOnSelf = m_abilityMod != null
			? m_abilityMod.m_generalEffectOnSelfMod.GetModifiedValue(m_generalEffectOnSelf)
			: m_generalEffectOnSelf;
	}

	public StandardEffectInfo GetSelfBuffEffect()
	{
		return m_cachedSelfBuffEffect ?? m_selfBuffEffect;
	}

	public StandardEffectInfo GetGeneralEffectOnSelf()
	{
		return m_cachedGeneralEffectOnSelf ?? m_generalEffectOnSelf;
	}

	public int GetExtraDamageIfQueued()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageIfQueuedMod.GetModifiedValue(m_extraDamageIfQueued)
			: m_extraDamageIfQueued;
	}

	public int GetBaseShielding()
	{
		return m_abilityMod != null
			? m_abilityMod.m_baseShieldingMod.GetModifiedValue(m_baseShielding)
			: m_baseShielding;
	}

	public int GetExtraShieldingIfOnlyAbility()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraShieldingIfOnlyAbilityMod.GetModifiedValue(m_extraShieldingIfOnlyAbility)
			: m_extraShieldingIfOnlyAbility;
	}

	public bool SelfBuffLastsUntilYouDealDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfBuffLastsUntilYouDealDamageMod.GetModifiedValue(m_selfBuffLastsUntilYouDealDamage)
			: m_selfBuffLastsUntilYouDealDamage;
	}

	public float GetAoeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeRadiusMod.GetModifiedValue(m_aoeRadius)
			: m_aoeRadius;
	}

	public float GetKnockbackDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistMod.GetModifiedValue(m_knockbackDist)
			: m_knockbackDist;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public bool PenetrateLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS)
			: m_penetrateLoS;
	}

	public int GetDamageIncreaseFirstHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageIncreaseFirstHitMod.GetModifiedValue(m_damageIncreaseFirstHit)
			: m_damageIncreaseFirstHit;
	}

	public int GetDamageIncreaseSubseqHits()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageIncreaseSubseqHitsMod.GetModifiedValue(m_damageIncreaseSubseqHits)
			: m_damageIncreaseSubseqHits;
	}

	public int GetTechPointGainPerIncomingHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainPerIncomingHitMod.GetModifiedValue(m_techPointGainPerIncomingHit)
			: m_techPointGainPerIncomingHit;
	}

	public bool BuffInResponseToIndirectDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_buffInResponseToIndirectDamageMod.GetModifiedValue(m_buffInResponseToIndirectDamage)
			: m_buffInResponseToIndirectDamage;
	}

	public int GetCdrIfNotHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfNotHitMod.GetModifiedValue(m_cdrIfNotHit)
			: m_cdrIfNotHit;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, m_selfBuffEffect, "SelfBuffEffect", m_selfBuffEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_generalEffectOnSelf, "GeneralEffectOnSelf", m_generalEffectOnSelf);
		AddTokenInt(tokens, "ExtraDamageIfQueued", string.Empty, m_extraDamageIfQueued);
		AddTokenInt(tokens, "BaseShielding", string.Empty, m_baseShielding);
		AddTokenInt(tokens, "ExtraShieldingIfOnlyAbility", string.Empty, m_extraShieldingIfOnlyAbility);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "DamageIncreaseFirstHit", string.Empty, m_damageIncreaseFirstHit);
		AddTokenInt(tokens, "DamageIncreaseSubseqHits", string.Empty, m_damageIncreaseSubseqHits);
		AddTokenInt(tokens, "TechPointGainPerIncomingHit", string.Empty, m_techPointGainPerIncomingHit);
		AddTokenInt(tokens, "CdrIfNotHit", string.Empty, m_cdrIfNotHit);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = base.CalculateAbilityTooltipNumbers();
		GetSelfBuffEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		GetGeneralEffectOnSelf().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (targetActor != ActorData)
		{
			return false;
		}
		int absorb = GetBaseShielding();
		if (GetExtraShieldingIfOnlyAbility() > 0 && !HasOtherQueuedAbilities())
		{
			absorb += GetExtraShieldingIfOnlyAbility();
		}
		results.m_absorb = absorb;
		return true;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_syncComponent != null
			? m_syncComponent.m_lastSelfBuffTurn == -1
			: base.CustomCanCastValidation(caster);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiSelfBuff))
		{
			m_abilityMod = abilityMod as AbilityMod_SamuraiSelfBuff;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public bool HasOtherQueuedAbilities()
	{
		if (m_abilityData != null)
		{
			for (int i = 0; i <= 4; i++)
			{
				if (i != (int)m_myActionType && m_abilityData.HasQueuedAction((AbilityData.ActionType)i))
				{
					return true;
				}
			}
		}
		return false;
	}
}
