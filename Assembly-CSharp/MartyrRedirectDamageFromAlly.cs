using System;
using System.Collections.Generic;
using UnityEngine;

public class MartyrRedirectDamageFromAlly : MartyrLaserBase
{
	[Header("-- Targeting")]
	public bool m_canTargetEnemies;

	public bool m_canTargetAllies = true;

	public bool m_canTargetSelf = true;

	public bool m_penetratesLoS;

	public float m_enemyHitRadius = 3f;

	public int m_maxEnemyTargets = 4;

	[Header("-- Damage reduction and redirection")]
	public float m_damageReductionOnAlly = 0.5f;

	public float m_damageRedirectToEnemy = 0.5f;

	public int m_techPointGainPerRedirect = 3;

	public StandardEffectInfo m_allyHitEffect;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Absorb & Crystal Bonuses")]
	public int m_baseAbsorb;

	public int m_absorbPerCrystalSpent = 5;

	public List<MartyrProtectAllyThreshold> m_thresholdBasedCrystalBonuses;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	public GameObject m_allyHitSequence;

	public GameObject m_enemyHitSequence;

	public GameObject m_reactionHitProjectilePrefab;

	private Martyr_SyncComponent m_syncComponent;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Martyr Redirect Damage From Ally";
		}
		this.m_syncComponent = base.GetComponent<Martyr_SyncComponent>();
		this.SetCachedFields();
		this.SetupTargeter();
		base.ResetTooltipAndTargetingNumbers();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return this.m_syncComponent;
	}

	protected void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_AoE_AroundActor(this, this.GetEnemyHitRadius(), this.GetPenetratesLoS(), true, false, this.GetMaxEnemyTargets(), false, true, true);
	}

	private void SetCachedFields()
	{
		this.m_cachedAllyHitEffect = this.m_allyHitEffect;
		this.m_cachedEnemyHitEffect = this.m_enemyHitEffect;
	}

	public float GetDamageReductionOnAlly()
	{
		return this.m_damageReductionOnAlly;
	}

	public float GetDamageRedirectToEnemy()
	{
		return this.m_damageRedirectToEnemy;
	}

	public int GetTechPointGainPerRedirect()
	{
		return this.m_techPointGainPerRedirect;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return (this.m_cachedAllyHitEffect == null) ? this.m_allyHitEffect : this.m_cachedAllyHitEffect;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return (this.m_cachedEnemyHitEffect == null) ? this.m_enemyHitEffect : this.m_cachedEnemyHitEffect;
	}

	public int GetAbsorbAmountPerCrystalSpent()
	{
		return this.m_absorbPerCrystalSpent;
	}

	public int GetBaseAbsorbAmount()
	{
		return this.m_baseAbsorb;
	}

	public bool GetPenetratesLoS()
	{
		return this.m_penetratesLoS;
	}

	public float GetEnemyHitRadius()
	{
		return this.m_enemyHitRadius;
	}

	public int GetMaxEnemyTargets()
	{
		return this.m_maxEnemyTargets;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyHitEffect, "AllyEffect", this.m_allyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyEffect", this.m_enemyHitEffect, true);
		tokens.Add(new TooltipTokenInt("BaseAbsorb", "Absorb with no crystal bonus", this.GetBaseAbsorbAmount()));
		tokens.Add(new TooltipTokenInt("AbsorbPerCrystal", "Absorb added per crystal spent", this.GetAbsorbAmountPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("WidthPerCrystal", "Width added per crystal spent", base.GetBonusWidthPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("LengthPerCrystal", "Length added per crystal spent", base.GetBonusLengthPerCrystalSpent()));
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = base.CalculateNameplateTargetingNumbers();
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Ally, 1);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor.\u000E() == base.ActorData.\u000E())
		{
			int currentAbsorb = this.GetCurrentAbsorb(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, currentAbsorb, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Ally);
		}
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return base.HasTargetableActorsInDecision(caster, this.m_canTargetEnemies, this.m_canTargetAllies, this.m_canTargetSelf, Ability.ValidateCheckPath.Ignore, !this.GetPenetratesLoS(), false, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, this.m_canTargetEnemies, this.m_canTargetAllies, this.m_canTargetSelf, Ability.ValidateCheckPath.Ignore, !this.GetPenetratesLoS(), false, false);
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		using (List<MartyrProtectAllyThreshold>.Enumerator enumerator = this.m_thresholdBasedCrystalBonuses.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MartyrProtectAllyThreshold item = enumerator.Current;
				list.Add(item);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrRedirectDamageFromAlly.GetThresholdBasedCrystalBonusList()).MethodHandle;
			}
		}
		return list;
	}

	private int GetCurrentAbsorb(ActorData caster)
	{
		MartyrProtectAllyThreshold martyrProtectAllyThreshold = base.GetCurrentPowerEntry(caster) as MartyrProtectAllyThreshold;
		int num;
		if (martyrProtectAllyThreshold != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrRedirectDamageFromAlly.GetCurrentAbsorb(ActorData)).MethodHandle;
			}
			num = martyrProtectAllyThreshold.m_additionalAbsorb;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return this.GetBaseAbsorbAmount() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetAbsorbAmountPerCrystalSpent() + num2;
	}
}
