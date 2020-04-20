using System;
using System.Collections.Generic;
using UnityEngine;

public class BlasterOvercharge : Ability
{
	[Header("-- For managing Overcharge effect, please use on hit effect for gameplay")]
	public StandardActorEffectData m_overchargeEffectData;

	[Header("-- How many stacks are allowed")]
	public int m_maxCastCount = 1;

	[Header("-- Extra damage added for all attacks except Lurker Mine")]
	public int m_extraDamage = 0xA;

	[Header("-- Extra Damage for Lurker Mine")]
	public int m_extraDamageForDelayedLaser;

	[Header("-- Extra Damage for multiple stacks")]
	public int m_extraDamageForMultiCast;

	[Header("-- Count - Number of times extra damage applies")]
	public int m_extraDamageCount = 1;

	[Header("-- On Cast")]
	public StandardEffectInfo m_effectOnSelfOnCast;

	[Header("-- Extra Effects for other abilities")]
	public StandardEffectInfo m_extraEffectOnOtherAbilities;

	public List<AbilityData.ActionType> m_extraEffectActionTypes;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_BlasterOvercharge m_abilityMod;

	private Blaster_SyncComponent m_syncComp;

	private BlasterKnockbackCone m_ultAbility;

	private StandardEffectInfo m_cachedEffectOnSelfOnCast;

	private StandardEffectInfo m_cachedExtraEffectOnOtherAbilities;

	private void Start()
	{
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.m_syncComp == null)
		{
			this.m_syncComp = base.GetComponent<Blaster_SyncComponent>();
		}
		if (this.m_ultAbility == null)
		{
			this.m_ultAbility = (base.GetComponent<AbilityData>().GetAbilityOfType(typeof(BlasterKnockbackCone)) as BlasterKnockbackCone);
		}
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnSelfOnCast;
		if (this.m_abilityMod)
		{
			cachedEffectOnSelfOnCast = this.m_abilityMod.m_effectOnSelfOnCastMod.GetModifiedValue(this.m_effectOnSelfOnCast);
		}
		else
		{
			cachedEffectOnSelfOnCast = this.m_effectOnSelfOnCast;
		}
		this.m_cachedEffectOnSelfOnCast = cachedEffectOnSelfOnCast;
		StandardEffectInfo cachedExtraEffectOnOtherAbilities;
		if (this.m_abilityMod != null)
		{
			cachedExtraEffectOnOtherAbilities = this.m_abilityMod.m_extraEffectOnOtherAbilitiesMod.GetModifiedValue(this.m_extraEffectOnOtherAbilities);
		}
		else
		{
			cachedExtraEffectOnOtherAbilities = this.m_extraEffectOnOtherAbilities;
		}
		this.m_cachedExtraEffectOnOtherAbilities = cachedExtraEffectOnOtherAbilities;
	}

	public int GetMaxCastCount()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxCastCountMod.GetModifiedValue(this.m_maxCastCount);
		}
		else
		{
			result = this.m_maxCastCount;
		}
		return result;
	}

	public int GetExtraDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageMod.GetModifiedValue(this.m_extraDamage);
		}
		else
		{
			result = this.m_extraDamage;
		}
		return result;
	}

	public int GetExtraDamageForDelayedLaser()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageForDelayedLaserMod.GetModifiedValue(this.m_extraDamageForDelayedLaser);
		}
		else
		{
			result = this.m_extraDamageForDelayedLaser;
		}
		return result;
	}

	public int GetExtraDamageForMultiCast()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageForMultiCastMod.GetModifiedValue(this.m_extraDamageForMultiCast);
		}
		else
		{
			result = this.m_extraDamageForMultiCast;
		}
		return result;
	}

	public int GetExtraDamageCount()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_extraDamageCountMod.GetModifiedValue(this.m_extraDamageCount);
		}
		else
		{
			result = this.m_extraDamageCount;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelfOnCast()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnSelfOnCast != null)
		{
			result = this.m_cachedEffectOnSelfOnCast;
		}
		else
		{
			result = this.m_effectOnSelfOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEffectOnOtherAbilities()
	{
		StandardEffectInfo result;
		if (this.m_cachedExtraEffectOnOtherAbilities != null)
		{
			result = this.m_cachedExtraEffectOnOtherAbilities;
		}
		else
		{
			result = this.m_extraEffectOnOtherAbilities;
		}
		return result;
	}

	public List<AbilityData.ActionType> GetExtraEffectTargetActionTypes()
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_useExtraEffectActionTypeOverride)
			{
				return this.m_abilityMod.m_extraEffectActionTypesOverride;
			}
		}
		return this.m_extraEffectActionTypes;
	}

	public override bool IsFreeAction()
	{
		if (this.m_ultAbility != null && this.m_ultAbility.OverchargeAsFreeActionAfterCast())
		{
			if (GameFlowData.Get() != null)
			{
				if (this.m_syncComp.m_lastUltCastTurn > 0)
				{
					return GameFlowData.Get().CurrentTurn > this.m_syncComp.m_lastUltCastTurn;
				}
			}
		}
		return base.IsFreeAction();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetEffectOnSelfOnCast().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (!(this.m_syncComp != null))
		{
			return true;
		}
		int maxCastCount = this.GetMaxCastCount();
		if (maxCastCount > 0)
		{
			return this.m_syncComp.m_overchargeBuffs < maxCastCount;
		}
		return this.m_syncComp.m_overchargeBuffs <= 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterOvercharge abilityMod_BlasterOvercharge = modAsBase as AbilityMod_BlasterOvercharge;
		string name = "MaxCastCount";
		string empty = string.Empty;
		int val;
		if (abilityMod_BlasterOvercharge)
		{
			val = abilityMod_BlasterOvercharge.m_maxCastCountMod.GetModifiedValue(this.m_maxCastCount);
		}
		else
		{
			val = this.m_maxCastCount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "ExtraDamageForLurkerMine";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_BlasterOvercharge)
		{
			val2 = abilityMod_BlasterOvercharge.m_extraDamageForDelayedLaserMod.GetModifiedValue(this.m_extraDamageForDelayedLaser);
		}
		else
		{
			val2 = this.m_extraDamageForDelayedLaser;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "ExtraDamageForMultiCast";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_BlasterOvercharge)
		{
			val3 = abilityMod_BlasterOvercharge.m_extraDamageForMultiCastMod.GetModifiedValue(this.m_extraDamageForMultiCast);
		}
		else
		{
			val3 = this.m_extraDamageForMultiCast;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_BlasterOvercharge)
		{
			effectInfo = abilityMod_BlasterOvercharge.m_effectOnSelfOnCastMod.GetModifiedValue(this.m_effectOnSelfOnCast);
		}
		else
		{
			effectInfo = this.m_effectOnSelfOnCast;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnSelfOnCast", this.m_effectOnSelfOnCast, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_extraEffectOnOtherAbilities, "ExtraEffectOnOtherAbilities", this.m_extraEffectOnOtherAbilities, true);
		string name4 = "OverchargeExtraDamage";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_BlasterOvercharge)
		{
			val4 = abilityMod_BlasterOvercharge.m_extraDamageMod.GetModifiedValue(this.m_extraDamage);
		}
		else
		{
			val4 = this.m_extraDamage;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BlasterOvercharge))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_BlasterOvercharge);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
