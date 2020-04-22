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
		m_syncComponent = base.ActorData.GetComponent<Samurai_SyncComponent>();
		SetCachedFields();
		AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = new AbilityUtil_Targeter_StretchCone(this, GetAoeRadius(), GetAoeRadius(), 360f, 360f, AreaEffectUtils.StretchConeStyle.Linear, 0f, PenetrateLoS());
		abilityUtil_Targeter_StretchCone.InitKnockbackData(GetKnockbackDist(), KnockbackType.AwayFromSource, 0f, KnockbackType.AwayFromSource);
		abilityUtil_Targeter_StretchCone.m_includeCaster = true;
		base.Targeter = abilityUtil_Targeter_StretchCone;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedSelfBuffEffect;
		if ((bool)m_abilityMod)
		{
			cachedSelfBuffEffect = m_abilityMod.m_selfBuffEffectMod.GetModifiedValue(m_selfBuffEffect);
		}
		else
		{
			cachedSelfBuffEffect = m_selfBuffEffect;
		}
		m_cachedSelfBuffEffect = cachedSelfBuffEffect;
		StandardEffectInfo cachedGeneralEffectOnSelf;
		if ((bool)m_abilityMod)
		{
			cachedGeneralEffectOnSelf = m_abilityMod.m_generalEffectOnSelfMod.GetModifiedValue(m_generalEffectOnSelf);
		}
		else
		{
			cachedGeneralEffectOnSelf = m_generalEffectOnSelf;
		}
		m_cachedGeneralEffectOnSelf = cachedGeneralEffectOnSelf;
	}

	public StandardEffectInfo GetSelfBuffEffect()
	{
		StandardEffectInfo result;
		if (m_cachedSelfBuffEffect != null)
		{
			result = m_cachedSelfBuffEffect;
		}
		else
		{
			result = m_selfBuffEffect;
		}
		return result;
	}

	public StandardEffectInfo GetGeneralEffectOnSelf()
	{
		StandardEffectInfo result;
		if (m_cachedGeneralEffectOnSelf != null)
		{
			result = m_cachedGeneralEffectOnSelf;
		}
		else
		{
			result = m_generalEffectOnSelf;
		}
		return result;
	}

	public int GetExtraDamageIfQueued()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageIfQueuedMod.GetModifiedValue(m_extraDamageIfQueued);
		}
		else
		{
			result = m_extraDamageIfQueued;
		}
		return result;
	}

	public int GetBaseShielding()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_baseShieldingMod.GetModifiedValue(m_baseShielding);
		}
		else
		{
			result = m_baseShielding;
		}
		return result;
	}

	public int GetExtraShieldingIfOnlyAbility()
	{
		return (!m_abilityMod) ? m_extraShieldingIfOnlyAbility : m_abilityMod.m_extraShieldingIfOnlyAbilityMod.GetModifiedValue(m_extraShieldingIfOnlyAbility);
	}

	public bool SelfBuffLastsUntilYouDealDamage()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfBuffLastsUntilYouDealDamageMod.GetModifiedValue(m_selfBuffLastsUntilYouDealDamage);
		}
		else
		{
			result = m_selfBuffLastsUntilYouDealDamage;
		}
		return result;
	}

	public float GetAoeRadius()
	{
		return (!m_abilityMod) ? m_aoeRadius : m_abilityMod.m_aoeRadiusMod.GetModifiedValue(m_aoeRadius);
	}

	public float GetKnockbackDist()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackDistMod.GetModifiedValue(m_knockbackDist);
		}
		else
		{
			result = m_knockbackDist;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		return (!m_abilityMod) ? m_damageAmount : m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
	}

	public bool PenetrateLoS()
	{
		return (!m_abilityMod) ? m_penetrateLoS : m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS);
	}

	public int GetDamageIncreaseFirstHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageIncreaseFirstHitMod.GetModifiedValue(m_damageIncreaseFirstHit);
		}
		else
		{
			result = m_damageIncreaseFirstHit;
		}
		return result;
	}

	public int GetDamageIncreaseSubseqHits()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageIncreaseSubseqHitsMod.GetModifiedValue(m_damageIncreaseSubseqHits);
		}
		else
		{
			result = m_damageIncreaseSubseqHits;
		}
		return result;
	}

	public int GetTechPointGainPerIncomingHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_techPointGainPerIncomingHitMod.GetModifiedValue(m_techPointGainPerIncomingHit);
		}
		else
		{
			result = m_techPointGainPerIncomingHit;
		}
		return result;
	}

	public bool BuffInResponseToIndirectDamage()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_buffInResponseToIndirectDamageMod.GetModifiedValue(m_buffInResponseToIndirectDamage);
		}
		else
		{
			result = m_buffInResponseToIndirectDamage;
		}
		return result;
	}

	public int GetCdrIfNotHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrIfNotHitMod.GetModifiedValue(m_cdrIfNotHit);
		}
		else
		{
			result = m_cdrIfNotHit;
		}
		return result;
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
		if (targetActor == base.ActorData)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					int num = GetBaseShielding();
					if (GetExtraShieldingIfOnlyAbility() > 0 && !HasOtherQueuedAbilities())
					{
						num += GetExtraShieldingIfOnlyAbility();
					}
					results.m_absorb = num;
					return true;
				}
				}
			}
		}
		return false;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_syncComponent != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_syncComponent.m_lastSelfBuffTurn == -1;
				}
			}
		}
		return base.CustomCanCastValidation(caster);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SamuraiSelfBuff))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_SamuraiSelfBuff);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public bool HasOtherQueuedAbilities()
	{
		bool result = false;
		if (m_abilityData != null)
		{
			int num = 0;
			while (true)
			{
				if (num <= 4)
				{
					if (num != (int)m_myActionType && m_abilityData.HasQueuedAction((AbilityData.ActionType)num))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}
		return result;
	}
}
