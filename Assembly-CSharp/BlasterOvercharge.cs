// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class BlasterOvercharge : Ability
{
	[Header("-- For managing Overcharge effect, please use on hit effect for gameplay")]
	public StandardActorEffectData m_overchargeEffectData;
	[Header("-- How many stacks are allowed")]
	public int m_maxCastCount = 1;
	
	// removed in rogues
	[Header("-- Extra damage added for all attacks except Lurker Mine")]
	public int m_extraDamage = 10;
	// end removed in rogues
	
	[Header("-- Extra Damage for Lurker Mine")]
	public int m_extraDamageForDelayedLaser;
	[Header("-- Extra Damage for multiple stacks")]
	public int m_extraDamageForMultiCast;
	
	// removed in rogues
	[Header("-- Count - Number of times extra damage applies")]
	public int m_extraDamageCount = 1;
	// end removed in rogues

	[Header("-- On Cast")]
	public StandardEffectInfo m_effectOnSelfOnCast;
	[Header("-- Extra Effects for other abilities")]
	
	// reactor
	public StandardEffectInfo m_extraEffectOnOtherAbilities;
	public List<AbilityData.ActionType> m_extraEffectActionTypes;
	// rogues
	// public StandardEffectInfo m_extraEffectForStretchingCone;
	// public StandardEffectInfo m_extraEffectForDashAndBlast;
	
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_BlasterOvercharge m_abilityMod;
	private Blaster_SyncComponent m_syncComp;
	private BlasterKnockbackCone m_ultAbility;
	private StandardEffectInfo m_cachedEffectOnSelfOnCast;
	
	// reactor
	private StandardEffectInfo m_cachedExtraEffectOnOtherAbilities;
	// rogues
	// private StandardEffectInfo m_cachedExtraEffectForStretchingCone;
	// private StandardEffectInfo m_cachedExtraEffectForDashAndBlast;

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
		// reactor
		m_cachedExtraEffectOnOtherAbilities = m_abilityMod != null
			? m_abilityMod.m_extraEffectOnOtherAbilitiesMod.GetModifiedValue(m_extraEffectOnOtherAbilities)
			: m_extraEffectOnOtherAbilities;
		// rogues
		// m_cachedExtraEffectForStretchingCone = m_abilityMod != null
		// 	? m_abilityMod.m_extraEffectForStretchingConeMod.GetModifiedValue(m_extraEffectForStretchingCone)
		// 	: m_extraEffectForStretchingCone;
		// m_cachedExtraEffectForDashAndBlast = m_abilityMod != null
		// 	? m_abilityMod.m_extraEffectForDashAndBlastMod.GetModifiedValue(m_extraEffectForDashAndBlast)
		// 	: m_extraEffectForDashAndBlast;
	}

	public int GetMaxCastCount()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_maxCastCountMod.GetModifiedValue(m_maxCastCount) 
			: m_maxCastCount;
	}

	// removed in rogues
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

	// removed in rogues
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

	// reactor
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
	// rogues
	// public StandardEffectInfo GetExtraEffectForStretchingCone()
	// {
	// 	return m_cachedExtraEffectForStretchingCone ?? m_extraEffectForStretchingCone;
	// }
	//
	// public StandardEffectInfo GetExtraEffectForDashAndBlast()
	// {
	// 	return m_cachedExtraEffectForDashAndBlast ?? m_extraEffectForDashAndBlast;
	// }

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
		// reactor
		return maxCastCount > 0
			? m_syncComp.m_overchargeBuffs < maxCastCount
			: m_syncComp.m_overchargeBuffs <= 0;
		// rogues
		// return maxCastCount > 0
		// 	? m_syncComp.m_overchargeCount < maxCastCount
		// 	: m_syncComp.m_overchargeCount <= 0;
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
		// reactor
		AbilityMod.AddToken_EffectInfo(tokens, m_extraEffectOnOtherAbilities, "ExtraEffectOnOtherAbilities", m_extraEffectOnOtherAbilities);
		AddTokenInt(tokens, "OverchargeExtraDamage", string.Empty, abilityMod_BlasterOvercharge != null
			? abilityMod_BlasterOvercharge.m_extraDamageMod.GetModifiedValue(m_extraDamage)
			: m_extraDamage);
		// rogues
		// AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterOvercharge != null
		// 	? abilityMod_BlasterOvercharge.m_extraEffectForStretchingConeMod.GetModifiedValue(m_extraEffectForStretchingCone)
		// 	: m_extraEffectForStretchingCone, "ExtraEffectForStretchingCone", m_extraEffectForStretchingCone);
		// AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterOvercharge != null
		// 	? abilityMod_BlasterOvercharge.m_extraEffectForDashAndBlastMod.GetModifiedValue(m_extraEffectForDashAndBlast)
		// 	: m_extraEffectForDashAndBlast, "ExtraEffectForDashAndBlast", m_extraEffectForDashAndBlast);
		// BlasterStretchingCone component = GetComponent<BlasterStretchingCone>();
		// if (component != null)
		// {
		// 	int num = component.m_damageAmountOvercharged - component.m_damageAmountNormal;
		// 	if (num != 0)
		// 	{
		// 		AddTokenInt(tokens, "OverchargeExtraDamage", string.Empty, Mathf.Abs(num));
		// 	}
		// }
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
	
#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_syncComp != null)
		{
			m_syncComp.Networkm_lastUltCastTurn = -1;
		}
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(m_castSequencePrefab, caster.GetFreePos(), additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		StandardActorEffectData overchargeEffectData = m_overchargeEffectData;
		List<Effect> effectsOnTargetByCaster = ServerEffectManager.Get().GetEffectsOnTargetByCaster(caster, caster, typeof(BlasterOverchargeEffect));
		if (effectsOnTargetByCaster.Count > 0)
		{
			List<MiscHitEventEffectUpdateParams> updateParams = new List<MiscHitEventEffectUpdateParams>
			{
				new BlasterOverchargeEffect.CastCountUpdateParam()
			};
			actorHitResults.AddMiscHitEvent(new MiscHitEventData_UpdateEffect(effectsOnTargetByCaster[0].GetEffectGuid(), updateParams));
		}
		else
		{
			BlasterOverchargeEffect effect = new BlasterOverchargeEffect(AsEffectSource(), caster.GetCurrentBoardSquare(), caster, caster, overchargeEffectData);
			actorHitResults.AddEffect(effect);
		}
		actorHitResults.AddStandardEffectInfo(GetEffectOnSelfOnCast());
		abilityResults.StoreActorHit(actorHitResults);
	}
#endif
}
