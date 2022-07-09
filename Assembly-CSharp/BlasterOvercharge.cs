using System.Collections.Generic;
using UnityEngine;

public class BlasterOvercharge : Ability
{
	[Header("-- For managing Overcharge effect, please use on hit effect for gameplay")]
	public StandardActorEffectData m_overchargeEffectData;
	[Header("-- How many stacks are allowed")]
	public int m_maxCastCount = 1;
	[Header("-- Extra damage added for all attacks except Lurker Mine")]
	public int m_extraDamage = 10;
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
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Blaster_SyncComponent>();
		}
		if (m_ultAbility == null)
		{
			m_ultAbility = GetComponent<AbilityData>().GetAbilityOfType(typeof(BlasterKnockbackCone)) as BlasterKnockbackCone;
		}
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always);
		Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnSelfOnCast = m_abilityMod != null
			? m_abilityMod.m_effectOnSelfOnCastMod.GetModifiedValue(m_effectOnSelfOnCast)
			: m_effectOnSelfOnCast;
		m_cachedExtraEffectOnOtherAbilities = m_abilityMod != null
			? m_abilityMod.m_extraEffectOnOtherAbilitiesMod.GetModifiedValue(m_extraEffectOnOtherAbilities)
			: m_extraEffectOnOtherAbilities;
	}

	public int GetMaxCastCount()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_maxCastCountMod.GetModifiedValue(m_maxCastCount) 
			: m_maxCastCount;
	}

	public int GetExtraDamage()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamageMod.GetModifiedValue(m_extraDamage) 
			: m_extraDamage;
	}

	public int GetExtraDamageForDelayedLaser()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamageForDelayedLaserMod.GetModifiedValue(m_extraDamageForDelayedLaser) 
			: m_extraDamageForDelayedLaser;
	}

	public int GetExtraDamageForMultiCast()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamageForMultiCastMod.GetModifiedValue(m_extraDamageForMultiCast) 
			: m_extraDamageForMultiCast;
	}

	public int GetExtraDamageCount()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamageCountMod.GetModifiedValue(m_extraDamageCount) 
			: m_extraDamageCount;
	}

	public StandardEffectInfo GetEffectOnSelfOnCast()
	{
		return m_cachedEffectOnSelfOnCast ?? m_effectOnSelfOnCast;
	}

	public StandardEffectInfo GetExtraEffectOnOtherAbilities()
	{
		return m_cachedExtraEffectOnOtherAbilities ?? m_extraEffectOnOtherAbilities;
	}

	public List<AbilityData.ActionType> GetExtraEffectTargetActionTypes()
	{
		return m_abilityMod != null && m_abilityMod.m_useExtraEffectActionTypeOverride
			? m_abilityMod.m_extraEffectActionTypesOverride
			: m_extraEffectActionTypes;
	}

	public override bool IsFreeAction()
	{
		return m_ultAbility != null
		       && m_ultAbility.OverchargeAsFreeActionAfterCast()
		       && GameFlowData.Get() != null
		       && m_syncComp.m_lastUltCastTurn > 0
			? GameFlowData.Get().CurrentTurn > m_syncComp.m_lastUltCastTurn
			: base.IsFreeAction();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetEffectOnSelfOnCast().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_syncComp == null)
		{
			return true;
		}
		int maxCastCount = GetMaxCastCount();
		return maxCastCount > 0
			? m_syncComp.m_overchargeBuffs < maxCastCount
			: m_syncComp.m_overchargeBuffs <= 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterOvercharge abilityMod_BlasterOvercharge = modAsBase as AbilityMod_BlasterOvercharge;
		AddTokenInt(tokens, "MaxCastCount", string.Empty, abilityMod_BlasterOvercharge != null
			? abilityMod_BlasterOvercharge.m_maxCastCountMod.GetModifiedValue(m_maxCastCount)
			: m_maxCastCount);
		AddTokenInt(tokens, "ExtraDamageForLurkerMine", string.Empty, abilityMod_BlasterOvercharge != null
			? abilityMod_BlasterOvercharge.m_extraDamageForDelayedLaserMod.GetModifiedValue(m_extraDamageForDelayedLaser)
			: m_extraDamageForDelayedLaser);
		AddTokenInt(tokens, "ExtraDamageForMultiCast", string.Empty, abilityMod_BlasterOvercharge != null
			? abilityMod_BlasterOvercharge.m_extraDamageForMultiCastMod.GetModifiedValue(m_extraDamageForMultiCast)
			: m_extraDamageForMultiCast);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterOvercharge != null
			? abilityMod_BlasterOvercharge.m_effectOnSelfOnCastMod.GetModifiedValue(m_effectOnSelfOnCast)
			: m_effectOnSelfOnCast, "EffectOnSelfOnCast", m_effectOnSelfOnCast);
		AbilityMod.AddToken_EffectInfo(tokens, m_extraEffectOnOtherAbilities, "ExtraEffectOnOtherAbilities", m_extraEffectOnOtherAbilities);
		AddTokenInt(tokens, "OverchargeExtraDamage", string.Empty, abilityMod_BlasterOvercharge != null
			? abilityMod_BlasterOvercharge.m_extraDamageMod.GetModifiedValue(m_extraDamage)
			: m_extraDamage);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BlasterOvercharge))
		{
			m_abilityMod = abilityMod as AbilityMod_BlasterOvercharge;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
