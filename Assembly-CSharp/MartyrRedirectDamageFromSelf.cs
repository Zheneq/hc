using System;
using System.Collections.Generic;
using UnityEngine;

public class MartyrRedirectDamageFromSelf : MartyrLaserBase
{
	[Header("-- Damage reduction and redirection")]
	public float m_damageReductionOnCaster = 0.5f;

	public float m_damageRedirectToTarget = 0.5f;

	public int m_techPointGainPerRedirect = 3;

	public StandardEffectInfo m_selfHitEffect;

	public bool m_affectsEnemies = true;

	public bool m_affectsAllies;

	public bool m_penetratesLoS;

	public StandardEffectInfo m_effectOnTarget;

	[Header("-- Self protection")]
	public int m_baseAbsorb;

	public int m_absorbPerCrystalSpent = 5;

	public List<MartyrProtectAllyThreshold> m_thresholdBasedCrystalBonuses;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	public GameObject m_projectileSequence;

	public GameObject m_redirectProjectileSequence;

	private Martyr_SyncComponent m_syncComponent;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private StandardEffectInfo m_cachedEffectOnTarget;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrRedirectDamageFromSelf.Start()).MethodHandle;
			}
			this.m_abilityName = "Martyr Redirect Damage From Self";
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
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, this.GetPenetratesLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.m_affectsEnemies, this.m_affectsAllies, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Always);
		(base.Targeter as AbilityUtil_Targeter_Shape).m_affectCasterDelegate = delegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
		{
			int currentAbsorb = this.GetCurrentAbsorb(caster);
			return currentAbsorb > 0;
		};
	}

	private void SetCachedFields()
	{
		this.m_cachedSelfHitEffect = this.m_selfHitEffect;
		this.m_cachedEffectOnTarget = this.m_effectOnTarget;
	}

	public float GetDamageReductionOnCaster()
	{
		return this.m_damageReductionOnCaster;
	}

	public float GetDamageRedirectToTarget()
	{
		return this.m_damageRedirectToTarget;
	}

	public int GetTechPointGainPerRedirect()
	{
		return this.m_techPointGainPerRedirect;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedSelfHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrRedirectDamageFromSelf.GetSelfHitEffect()).MethodHandle;
			}
			result = this.m_cachedSelfHitEffect;
		}
		else
		{
			result = this.m_selfHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnTarget()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnTarget != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrRedirectDamageFromSelf.GetEffectOnTarget()).MethodHandle;
			}
			result = this.m_cachedEffectOnTarget;
		}
		else
		{
			result = this.m_effectOnTarget;
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

	public bool GetPenetratesLoS()
	{
		return this.m_penetratesLoS;
	}

	public float GetMaxRange()
	{
		return this.GetRangeInSquares(0);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_selfHitEffect, "SelfEffect", this.m_selfHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnTarget, "TargetEffect", this.m_effectOnTarget, true);
		tokens.Add(new TooltipTokenInt("BaseAbsorb", "Absorb with no crystal bonus", this.GetBaseAbsorbAmount()));
		tokens.Add(new TooltipTokenInt("AbsorbPerCrystal", "Absorb added per crystal spent", this.GetAbsorbAmountPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("WidthPerCrystal", "Width added per crystal spent", base.GetBonusWidthPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("LengthPerCrystal", "Length added per crystal spent", base.GetBonusLengthPerCrystalSpent()));
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetSelfHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = base.CalculateNameplateTargetingNumbers();
		this.GetSelfHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Self, 1);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor == base.ActorData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrRedirectDamageFromSelf.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			int currentAbsorb = this.GetCurrentAbsorb(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, base.ActorData, currentAbsorb, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self);
		}
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return base.HasTargetableActorsInDecision(caster, this.m_affectsEnemies, this.m_affectsAllies, false, Ability.ValidateCheckPath.Ignore, !this.GetPenetratesLoS(), false, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, this.m_affectsEnemies, this.m_affectsAllies, false, Ability.ValidateCheckPath.Ignore, !this.GetPenetratesLoS(), false, false);
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
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrRedirectDamageFromSelf.GetThresholdBasedCrystalBonusList()).MethodHandle;
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
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrRedirectDamageFromSelf.GetCurrentAbsorb(ActorData)).MethodHandle;
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
