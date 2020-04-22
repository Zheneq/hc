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
		base.Targeter = new AbilityUtil_Targeter_Shape(this, GetTargetingShape(), GetPenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, true, AbilityUtil_Targeter.AffectsActor.Always);
		base.Targeter.ShowArcToShape = !m_targetData.IsNullOrEmpty();
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
		StandardEffectInfo result;
		if (m_cachedAllyHitEffect != null)
		{
			result = m_cachedAllyHitEffect;
		}
		else
		{
			result = m_allyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnSelf != null)
		{
			result = m_cachedEffectOnSelf;
		}
		else
		{
			result = m_effectOnSelf;
		}
		return result;
	}

	public StandardEffectInfo GetThornsEffect()
	{
		StandardEffectInfo result;
		if (m_cachedThornsEffect != null)
		{
			result = m_cachedThornsEffect;
		}
		else
		{
			result = m_thornsEffect;
		}
		return result;
	}

	public StandardEffectInfo GetReturnEffectOnEnemy()
	{
		StandardEffectInfo result;
		if (m_cachedReturnEffectOnEnemy != null)
		{
			result = m_cachedReturnEffectOnEnemy;
		}
		else
		{
			result = m_returnEffectOnEnemy;
		}
		return result;
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
		if (targetActor == base.ActorData)
		{
			int currentAbsorb = GetCurrentAbsorb(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, currentAbsorb, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self);
		}
		else
		{
			int currentAbsorbForAlly = GetCurrentAbsorbForAlly(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, currentAbsorbForAlly, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Ally);
		}
		return symbolToValue;
	}

	private int GetBonusAbsorbForCurrentThreshold(ActorData caster, bool forAlly)
	{
		int result = 0;
		if (m_syncComponent != null)
		{
			if (m_syncComponent.IsBonusActive(caster))
			{
				int num = m_syncComponent.SpentDamageCrystals(caster);
				for (int i = 0; i < m_thresholdBasedCrystalBonuses.Count; i++)
				{
					if (num >= m_thresholdBasedCrystalBonuses[i].m_crystalThreshold)
					{
						result = ((!forAlly) ? m_thresholdBasedCrystalBonuses[i].m_additionalAbsorb : m_thresholdBasedCrystalBonuses[i].m_additionalAbsorbOnAlly);
					}
				}
			}
		}
		return result;
	}

	private int GetCurrentAbsorb(ActorData caster)
	{
		return GetBaseAbsorbAmount() + m_syncComponent.SpentDamageCrystals(caster) * GetAbsorbAmountPerCrystalSpent() + GetBonusAbsorbForCurrentThreshold(caster, false);
	}

	private int GetCurrentAbsorbForAlly(ActorData caster)
	{
		return GetBaseAbsorbOnAlly() + m_syncComponent.SpentDamageCrystals(caster) * GetAbsorbOnAllyPerCrystalSpent() + GetBonusAbsorbForCurrentThreshold(caster, true);
	}
}
