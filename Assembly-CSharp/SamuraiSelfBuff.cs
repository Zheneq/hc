using System;
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
	public int m_baseShielding = 0x1E;

	public int m_extraShieldingIfOnlyAbility;

	public StandardEffectInfo m_generalEffectOnSelf;

	[Header("-- AoE")]
	public float m_aoeRadius = 1.5f;

	public float m_knockbackDist = 2f;

	public int m_damageAmount;

	public bool m_penetrateLoS;

	[Header("-- Damage reactions")]
	public int m_damageIncreaseFirstHit = 0xA;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Self Buff";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.m_abilityData = base.GetComponent<AbilityData>();
		this.m_myActionType = base.GetActionTypeOfAbility(this);
		this.m_syncComponent = base.ActorData.GetComponent<Samurai_SyncComponent>();
		this.SetCachedFields();
		AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = new AbilityUtil_Targeter_StretchCone(this, this.GetAoeRadius(), this.GetAoeRadius(), 360f, 360f, AreaEffectUtils.StretchConeStyle.Linear, 0f, this.PenetrateLoS());
		abilityUtil_Targeter_StretchCone.InitKnockbackData(this.GetKnockbackDist(), KnockbackType.AwayFromSource, 0f, KnockbackType.AwayFromSource);
		abilityUtil_Targeter_StretchCone.m_includeCaster = true;
		base.Targeter = abilityUtil_Targeter_StretchCone;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedSelfBuffEffect;
		if (this.m_abilityMod)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.SetCachedFields()).MethodHandle;
			}
			cachedSelfBuffEffect = this.m_abilityMod.m_selfBuffEffectMod.GetModifiedValue(this.m_selfBuffEffect);
		}
		else
		{
			cachedSelfBuffEffect = this.m_selfBuffEffect;
		}
		this.m_cachedSelfBuffEffect = cachedSelfBuffEffect;
		StandardEffectInfo cachedGeneralEffectOnSelf;
		if (this.m_abilityMod)
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
			cachedGeneralEffectOnSelf = this.m_abilityMod.m_generalEffectOnSelfMod.GetModifiedValue(this.m_generalEffectOnSelf);
		}
		else
		{
			cachedGeneralEffectOnSelf = this.m_generalEffectOnSelf;
		}
		this.m_cachedGeneralEffectOnSelf = cachedGeneralEffectOnSelf;
	}

	public StandardEffectInfo GetSelfBuffEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedSelfBuffEffect != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.GetSelfBuffEffect()).MethodHandle;
			}
			result = this.m_cachedSelfBuffEffect;
		}
		else
		{
			result = this.m_selfBuffEffect;
		}
		return result;
	}

	public StandardEffectInfo GetGeneralEffectOnSelf()
	{
		StandardEffectInfo result;
		if (this.m_cachedGeneralEffectOnSelf != null)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.GetGeneralEffectOnSelf()).MethodHandle;
			}
			result = this.m_cachedGeneralEffectOnSelf;
		}
		else
		{
			result = this.m_generalEffectOnSelf;
		}
		return result;
	}

	public int GetExtraDamageIfQueued()
	{
		int result;
		if (this.m_abilityMod)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.GetExtraDamageIfQueued()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageIfQueuedMod.GetModifiedValue(this.m_extraDamageIfQueued);
		}
		else
		{
			result = this.m_extraDamageIfQueued;
		}
		return result;
	}

	public int GetBaseShielding()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.GetBaseShielding()).MethodHandle;
			}
			result = this.m_abilityMod.m_baseShieldingMod.GetModifiedValue(this.m_baseShielding);
		}
		else
		{
			result = this.m_baseShielding;
		}
		return result;
	}

	public int GetExtraShieldingIfOnlyAbility()
	{
		return (!this.m_abilityMod) ? this.m_extraShieldingIfOnlyAbility : this.m_abilityMod.m_extraShieldingIfOnlyAbilityMod.GetModifiedValue(this.m_extraShieldingIfOnlyAbility);
	}

	public bool SelfBuffLastsUntilYouDealDamage()
	{
		bool result;
		if (this.m_abilityMod)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.SelfBuffLastsUntilYouDealDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfBuffLastsUntilYouDealDamageMod.GetModifiedValue(this.m_selfBuffLastsUntilYouDealDamage);
		}
		else
		{
			result = this.m_selfBuffLastsUntilYouDealDamage;
		}
		return result;
	}

	public float GetAoeRadius()
	{
		return (!this.m_abilityMod) ? this.m_aoeRadius : this.m_abilityMod.m_aoeRadiusMod.GetModifiedValue(this.m_aoeRadius);
	}

	public float GetKnockbackDist()
	{
		float result;
		if (this.m_abilityMod)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.GetKnockbackDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_knockbackDistMod.GetModifiedValue(this.m_knockbackDist);
		}
		else
		{
			result = this.m_knockbackDist;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_damageAmount : this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
	}

	public bool PenetrateLoS()
	{
		return (!this.m_abilityMod) ? this.m_penetrateLoS : this.m_abilityMod.m_penetrateLoSMod.GetModifiedValue(this.m_penetrateLoS);
	}

	public int GetDamageIncreaseFirstHit()
	{
		int result;
		if (this.m_abilityMod)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.GetDamageIncreaseFirstHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageIncreaseFirstHitMod.GetModifiedValue(this.m_damageIncreaseFirstHit);
		}
		else
		{
			result = this.m_damageIncreaseFirstHit;
		}
		return result;
	}

	public int GetDamageIncreaseSubseqHits()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.GetDamageIncreaseSubseqHits()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageIncreaseSubseqHitsMod.GetModifiedValue(this.m_damageIncreaseSubseqHits);
		}
		else
		{
			result = this.m_damageIncreaseSubseqHits;
		}
		return result;
	}

	public int GetTechPointGainPerIncomingHit()
	{
		int result;
		if (this.m_abilityMod)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.GetTechPointGainPerIncomingHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_techPointGainPerIncomingHitMod.GetModifiedValue(this.m_techPointGainPerIncomingHit);
		}
		else
		{
			result = this.m_techPointGainPerIncomingHit;
		}
		return result;
	}

	public bool BuffInResponseToIndirectDamage()
	{
		bool result;
		if (this.m_abilityMod)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.BuffInResponseToIndirectDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_buffInResponseToIndirectDamageMod.GetModifiedValue(this.m_buffInResponseToIndirectDamage);
		}
		else
		{
			result = this.m_buffInResponseToIndirectDamage;
		}
		return result;
	}

	public int GetCdrIfNotHit()
	{
		int result;
		if (this.m_abilityMod)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.GetCdrIfNotHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrIfNotHitMod.GetModifiedValue(this.m_cdrIfNotHit);
		}
		else
		{
			result = this.m_cdrIfNotHit;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, this.m_selfBuffEffect, "SelfBuffEffect", this.m_selfBuffEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_generalEffectOnSelf, "GeneralEffectOnSelf", this.m_generalEffectOnSelf, true);
		base.AddTokenInt(tokens, "ExtraDamageIfQueued", string.Empty, this.m_extraDamageIfQueued, false);
		base.AddTokenInt(tokens, "BaseShielding", string.Empty, this.m_baseShielding, false);
		base.AddTokenInt(tokens, "ExtraShieldingIfOnlyAbility", string.Empty, this.m_extraShieldingIfOnlyAbility, false);
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		base.AddTokenInt(tokens, "DamageIncreaseFirstHit", string.Empty, this.m_damageIncreaseFirstHit, false);
		base.AddTokenInt(tokens, "DamageIncreaseSubseqHits", string.Empty, this.m_damageIncreaseSubseqHits, false);
		base.AddTokenInt(tokens, "TechPointGainPerIncomingHit", string.Empty, this.m_techPointGainPerIncomingHit, false);
		base.AddTokenInt(tokens, "CdrIfNotHit", string.Empty, this.m_cdrIfNotHit, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = base.CalculateAbilityTooltipNumbers();
		this.GetSelfBuffEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		this.GetGeneralEffectOnSelf().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamageAmount());
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (targetActor == base.ActorData)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			int num = this.GetBaseShielding();
			if (this.GetExtraShieldingIfOnlyAbility() > 0 && !this.HasOtherQueuedAbilities())
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
				num += this.GetExtraShieldingIfOnlyAbility();
			}
			results.m_absorb = num;
			return true;
		}
		return false;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_syncComponent != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			return this.m_syncComponent.m_lastSelfBuffTurn == -1;
		}
		return base.CustomCanCastValidation(caster);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiSelfBuff))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SamuraiSelfBuff);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public bool HasOtherQueuedAbilities()
	{
		bool result = false;
		if (this.m_abilityData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SamuraiSelfBuff.HasOtherQueuedAbilities()).MethodHandle;
			}
			for (int i = 0; i <= 4; i++)
			{
				if (i != (int)this.m_myActionType && this.m_abilityData.HasQueuedAction((AbilityData.ActionType)i))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					return true;
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
		return result;
	}
}
