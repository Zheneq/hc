using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Martyr Protect Ally";
		}
		this.m_syncComponent = base.GetComponent<Martyr_SyncComponent>();
		this.SetCachedFields();
		this.SetupTargeter();
		base.ResetTooltipAndTargetingNumbers();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.GetTargetingShape(), this.GetPenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, true, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = !this.m_targetData.IsNullOrEmpty<TargetData>();
	}

	private void SetCachedFields()
	{
		this.m_cachedAllyHitEffect = this.m_allyHitEffect;
		this.m_cachedEffectOnSelf = this.m_effectOnSelf;
		this.m_cachedThornsEffect = this.m_thornsEffect;
		this.m_cachedReturnEffectOnEnemy = this.m_returnEffectOnEnemy;
	}

	public float GetDamageReductionOnTarget()
	{
		return this.m_damageReductionOnTarget;
	}

	public float GetDamageRedirectToCaster()
	{
		return this.m_damageRedirectToCaster;
	}

	public int GetTechPointGainPerRedirect()
	{
		return this.m_techPointGainPerRedirect;
	}

	public AbilityAreaShape GetTargetingShape()
	{
		return this.m_targetingShape;
	}

	public bool GetPenetrateLos()
	{
		return this.m_penetrateLos;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
		{
			result = this.m_cachedAllyHitEffect;
		}
		else
		{
			result = this.m_allyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnSelf != null)
		{
			result = this.m_cachedEffectOnSelf;
		}
		else
		{
			result = this.m_effectOnSelf;
		}
		return result;
	}

	public StandardEffectInfo GetThornsEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedThornsEffect != null)
		{
			result = this.m_cachedThornsEffect;
		}
		else
		{
			result = this.m_thornsEffect;
		}
		return result;
	}

	public StandardEffectInfo GetReturnEffectOnEnemy()
	{
		StandardEffectInfo result;
		if (this.m_cachedReturnEffectOnEnemy != null)
		{
			result = this.m_cachedReturnEffectOnEnemy;
		}
		else
		{
			result = this.m_returnEffectOnEnemy;
		}
		return result;
	}

	public int GetAbsorbAmountPerCrystalSpent()
	{
		return this.m_absorbPerCrystalSpent;
	}

	public int GetBaseAbsorbAmount()
	{
		return this.m_baseAbsorb;
	}

	public int GetAbsorbOnAllyPerCrystalSpent()
	{
		return this.m_absorbOnAllyPerCrystalSpent;
	}

	public int GetBaseAbsorbOnAlly()
	{
		return this.m_baseAbsorbOnAlly;
	}

	public int GetThornsDamagePerHit()
	{
		return this.m_thornsDamagePerHit;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyHitEffect, "AllyHitEffect", this.m_allyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnSelf, "EffectOnSelf", this.m_effectOnSelf, true);
		tokens.Add(new TooltipTokenInt("BaseAbsorb", "Absorb with no crystal bonus", this.GetBaseAbsorbAmount()));
		tokens.Add(new TooltipTokenInt("AbsorbPerCrystal", "Absorb added per crystal spent", this.GetAbsorbAmountPerCrystalSpent()));
		tokens.Add(new TooltipTokenInt("BaseAbsorbOnAlly", "Absorb on targeted ally with no crystal bonus", this.GetBaseAbsorbOnAlly()));
		tokens.Add(new TooltipTokenInt("AbsorbOnAllyPerCrystal", "Absorb on targeted ally added per crystal spent", this.GetAbsorbOnAllyPerCrystalSpent()));
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		this.GetEffectOnSelf().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = base.CalculateNameplateTargetingNumbers();
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Ally, 1);
		this.GetEffectOnSelf().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Self, 1);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor == base.ActorData)
		{
			int currentAbsorb = this.GetCurrentAbsorb(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, currentAbsorb, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self);
		}
		else
		{
			int currentAbsorbForAlly = this.GetCurrentAbsorbForAlly(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, currentAbsorbForAlly, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Ally);
		}
		return result;
	}

	private int GetBonusAbsorbForCurrentThreshold(ActorData caster, bool forAlly)
	{
		int result = 0;
		if (this.m_syncComponent != null)
		{
			if (this.m_syncComponent.IsBonusActive(caster))
			{
				int num = this.m_syncComponent.SpentDamageCrystals(caster);
				for (int i = 0; i < this.m_thresholdBasedCrystalBonuses.Count; i++)
				{
					if (num >= this.m_thresholdBasedCrystalBonuses[i].m_crystalThreshold)
					{
						if (forAlly)
						{
							result = this.m_thresholdBasedCrystalBonuses[i].m_additionalAbsorbOnAlly;
						}
						else
						{
							result = this.m_thresholdBasedCrystalBonuses[i].m_additionalAbsorb;
						}
					}
				}
			}
		}
		return result;
	}

	private int GetCurrentAbsorb(ActorData caster)
	{
		return this.GetBaseAbsorbAmount() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetAbsorbAmountPerCrystalSpent() + this.GetBonusAbsorbForCurrentThreshold(caster, false);
	}

	private int GetCurrentAbsorbForAlly(ActorData caster)
	{
		return this.GetBaseAbsorbOnAlly() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetAbsorbOnAllyPerCrystalSpent() + this.GetBonusAbsorbForCurrentThreshold(caster, true);
	}
}
