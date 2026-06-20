// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MartyrProtectAoE : Ability
{
	[Header("-- Targeting")]
	public AbilityAreaShape m_targetingShape = AbilityAreaShape.Five_x_Five_NoCorners;
	public bool m_penetrateLos = true;
	[Header("-- Damage reduction and redirection")]
	public float m_damageReductionOnTarget = 0.5f;
	public float m_damageRedirectToCaster = 0.5f;
	public int m_techPointGainPerRedirect = 3;
	public StandardEffectInfo m_allyHitEffect;
	[Header("-- Thorns effect on protected ally")]
	public StandardEffectInfo m_thornsEffect;
	public StandardEffectInfo m_returnEffectOnEnemy;
	public int m_thornsDamagePerHit;
	[Header("-- Absorb & Crystal Bonuses")]
	public StandardEffectInfo m_effectOnSelf;
	public int m_baseAbsorb;
	public int m_absorbPerCrystalSpent = 5;
	public int m_baseAbsorbOnAlly;
	public int m_absorbOnAllyPerCrystalSpent = 5;
	public List<MartyrProtectAllyThreshold> m_thresholdBasedCrystalBonuses;
	[Header("-- Sequences")]
	public GameObject m_selfHitSequence;
	public GameObject m_aoeHitSequence;
	public GameObject m_redirectProjectilePrefab;
	public GameObject m_thornsProjectilePrefab;

	private Martyr_SyncComponent m_syncComponent;
	private StandardEffectInfo m_cachedAllyHitEffect;
	private StandardEffectInfo m_cachedEffectOnSelf;
	private StandardEffectInfo m_cachedThornsEffect;
	private StandardEffectInfo m_cachedReturnEffectOnEnemy;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Martyr Protect Ally";
		}
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		SetCachedFields();
		SetupTargeter();
		ResetTooltipAndTargetingNumbers();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_Shape(this, GetTargetingShape(), GetPenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, true, AbilityUtil_Targeter.AffectsActor.Always)
		{
			ShowArcToShape = !m_targetData.IsNullOrEmpty()
		};
	}

	private void SetCachedFields()
	{
		m_cachedAllyHitEffect = m_allyHitEffect;
		m_cachedEffectOnSelf = m_effectOnSelf;
		m_cachedThornsEffect = m_thornsEffect;
		m_cachedReturnEffectOnEnemy = m_returnEffectOnEnemy;
	}

	public float GetDamageReductionOnTarget()
	{
		return m_damageReductionOnTarget;
	}

	public float GetDamageRedirectToCaster()
	{
		return m_damageRedirectToCaster;
	}

	public int GetTechPointGainPerRedirect()
	{
		return m_techPointGainPerRedirect;
	}

	public AbilityAreaShape GetTargetingShape()
	{
		return m_targetingShape;
	}

	public bool GetPenetrateLos()
	{
		return m_penetrateLos;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		return m_cachedEffectOnSelf ?? m_effectOnSelf;
	}

	public StandardEffectInfo GetThornsEffect()
	{
		return m_cachedThornsEffect ?? m_thornsEffect;
	}

	public StandardEffectInfo GetReturnEffectOnEnemy()
	{
		return m_cachedReturnEffectOnEnemy ?? m_returnEffectOnEnemy;
	}

	public int GetAbsorbAmountPerCrystalSpent()
	{
		return m_absorbPerCrystalSpent;
	}

	public int GetBaseAbsorbAmount()
	{
		return m_baseAbsorb;
	}

	public int GetAbsorbOnAllyPerCrystalSpent()
	{
		return m_absorbOnAllyPerCrystalSpent;
	}

	public int GetBaseAbsorbOnAlly()
	{
		return m_baseAbsorbOnAlly;
	}

	public int GetThornsDamagePerHit()
	{
		return m_thornsDamagePerHit;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnSelf, "EffectOnSelf", m_effectOnSelf);
		tokens.Add(new TooltipTokenInt("BaseAbsorb", "Absorb with no crystal bonus", GetBaseAbsorbAmount()));
		tokens.Add(new TooltipTokenInt("AbsorbPerCrystal", "Absorb added per crystal spent", GetAbsorbAmountPerCrystalSpent()));
		tokens.Add(new TooltipTokenInt("BaseAbsorbOnAlly", "Absorb on targeted ally with no crystal bonus", GetBaseAbsorbOnAlly()));
		tokens.Add(new TooltipTokenInt("AbsorbOnAllyPerCrystal", "Absorb on targeted ally added per crystal spent", GetAbsorbOnAllyPerCrystalSpent()));
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		GetEffectOnSelf().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = base.CalculateNameplateTargetingNumbers();
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Ally, 1);
		GetEffectOnSelf().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor == ActorData)
		{
			int currentAbsorb = GetCurrentAbsorb(ActorData);
			AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, currentAbsorb, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self);
		}
		else
		{
			int currentAbsorbForAlly = GetCurrentAbsorbForAlly(ActorData);
			AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, currentAbsorbForAlly, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Ally);
		}
		return symbolToValue;
	}

	private int GetBonusAbsorbForCurrentThreshold(ActorData caster, bool forAlly)
	{
		if (m_syncComponent == null || !m_syncComponent.IsBonusActive(caster))
		{
			return 0;
		}
		int result = 0;
		int spentDamageCrystals = m_syncComponent.SpentDamageCrystals(caster);
		foreach (MartyrProtectAllyThreshold bonus in m_thresholdBasedCrystalBonuses)
		{
			if (spentDamageCrystals >= bonus.m_crystalThreshold)
			{
				result = forAlly
					? bonus.m_additionalAbsorbOnAlly
					: bonus.m_additionalAbsorb;
			}
		}
		return result;
	}

	private int GetCurrentAbsorb(ActorData caster)
	{
		return GetBaseAbsorbAmount()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetAbsorbAmountPerCrystalSpent()
			+ GetBonusAbsorbForCurrentThreshold(caster, false);
	}

	private int GetCurrentAbsorbForAlly(ActorData caster)
	{
		return GetBaseAbsorbOnAlly()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetAbsorbOnAllyPerCrystalSpent()
			+ GetBonusAbsorbForCurrentThreshold(caster, true);
	}

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		List<ActorData> hitActors = GetHitActors(targets, caster);
		list.Add(new ServerClientUtils.SequenceStartData(m_aoeHitSequence, AreaEffectUtils.GetCenterOfShape(GetTargetingShape(), targets[0]), hitActors.ToArray(), caster, additionalData.m_sequenceSource, new Sequence.IExtraSequenceParams[0]));
		if (GetEffectOnSelf().m_applyEffect || GetCurrentAbsorb(caster) > 0)
		{
			list.Add(new ServerClientUtils.SequenceStartData(m_selfHitSequence, caster.GetFreePos(), new ActorData[]
			{
				caster
			}, caster, additionalData.m_sequenceSource, new Sequence.IExtraSequenceParams[0]));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<ActorData> hitActors = GetHitActors(targets, caster);
		foreach (ActorData hitActor in hitActors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(hitActor, caster.GetFreePos());
			StandardActorEffectData allyAbsorbEffect = GetAllyHitEffect().m_effectData.GetShallowCopy();
			allyAbsorbEffect.m_absorbAmount = GetCurrentAbsorbForAlly(caster);
			ActorHitResults actorHitResults = new ActorHitResults(new MartyrDamageRedirectEffect(AsEffectSource(), hitActor.GetCurrentBoardSquare(), hitActor, caster, true, new List<ActorData>
			{
				caster
			}, allyAbsorbEffect, GetDamageReductionOnTarget(), GetDamageRedirectToCaster(), GetTechPointGainPerRedirect(), 0f, null, m_redirectProjectilePrefab), hitParams);
			StandardEffectInfo thornsEffect = GetThornsEffect();
			if (thornsEffect != null && thornsEffect.m_applyEffect)
			{
				BattleMonkThornsEffect effect = new BattleMonkThornsEffect(AsEffectSource(), caster.GetCurrentBoardSquare(), hitActor, caster, thornsEffect.m_effectData, GetThornsDamagePerHit(), GetReturnEffectOnEnemy(), m_thornsProjectilePrefab);
				actorHitResults.AddEffect(effect);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		int currentAbsorb = GetCurrentAbsorb(caster);
		if (GetEffectOnSelf().m_applyEffect || currentAbsorb > 0)
		{
			StandardEffectInfo effectOnSelf = GetEffectOnSelf().GetShallowCopy();
			effectOnSelf.m_effectData = effectOnSelf.m_effectData.GetShallowCopy();
			effectOnSelf.m_effectData.m_absorbAmount = currentAbsorb;
			effectOnSelf.m_applyEffect = true;
			ActorHitParameters hitParams = new ActorHitParameters(caster, caster.GetFreePos());
			ActorHitResults hitResults = new ActorHitResults(effectOnSelf, hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
	}

	// added in rogues
	private List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster)
	{
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(GetTargetingShape(), targets[0], GetPenetrateLos(), caster, caster.GetTeam(), null);
		actorsInShape.Remove(caster);
		return actorsInShape;
	}
#endif
}
