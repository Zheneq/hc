using System;
using System.Collections.Generic;
using UnityEngine;

public class MartyrProtectAlly : MartyrLaserBase
{
	[Header("-- Damage reduction and redirection")]
	public float m_damageReductionOnTarget = 0.5f;

	public float m_damageRedirectToCaster = 0.5f;

	public int m_techPointGainPerRedirect = 3;

	public StandardEffectInfo m_laserHitEffect;

	[Space(10f)]
	public bool m_affectsEnemies;

	public bool m_affectsAllies = true;

	public bool m_penetratesLoS;

	[Header("-- Thorns effect on protected ally")]
	public StandardEffectInfo m_thornsEffect;

	public StandardEffectInfo m_returnEffectOnEnemy;

	public int m_thornsDamagePerHit;

	[Header("-- Absorb & Crystal Bonuses, Self")]
	public StandardEffectInfo m_effectOnSelf;

	public int m_baseAbsorb;

	public int m_absorbPerCrystalSpent = 5;

	[Header("-- Absorb on Ally")]
	public int m_baseAbsorbOnAlly;

	public int m_absorbOnAllyPerCrystalSpent = 5;

	public List<MartyrProtectAllyThreshold> m_thresholdBasedCrystalBonuses;

	[Header("-- Extra Energy per damage redirect")]
	public float m_extraEnergyPerRedirectDamage;

	[Header("-- Heal per damage redirect on next turn")]
	public float m_healOnTurnStartPerRedirectDamage;

	[Header("-- Sequences")]
	public GameObject m_allyShieldSequence;

	public GameObject m_projectileSequence;

	public GameObject m_redirectProjectileSequence;

	public GameObject m_thornsProjectileSequence;

	[Tooltip("Ignored if no effect or absorb is applied on the caster")]
	public GameObject m_selfShieldSequence;

	private Martyr_SyncComponent m_syncComponent;

	private AbilityMod_MartyrProtectAlly m_abilityMod;

	private StandardEffectInfo m_cachedLaserHitEffect;

	private StandardEffectInfo m_cachedThornsEffect;

	private StandardEffectInfo m_cachedReturnEffectOnEnemy;

	private StandardEffectInfo m_cachedEffectOnSelf;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.Start()).MethodHandle;
			}
			this.m_abilityName = "Martyr Protect Ally";
		}
		this.Setup();
		base.ResetTooltipAndTargetingNumbers();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return this.m_syncComponent;
	}

	protected void Setup()
	{
		this.m_syncComponent = base.GetComponent<Martyr_SyncComponent>();
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, this.PenetratesLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.AffectsEnemies(), this.AffectsAllies(), AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Always);
		(base.Targeter as AbilityUtil_Targeter_Shape).m_affectCasterDelegate = delegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
		{
			int currentAbsorb = this.GetCurrentAbsorb(caster);
			return currentAbsorb > 0;
		};
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedLaserHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.SetCachedFields()).MethodHandle;
			}
			cachedLaserHitEffect = this.m_abilityMod.m_laserHitEffectMod.GetModifiedValue(this.m_laserHitEffect);
		}
		else
		{
			cachedLaserHitEffect = this.m_laserHitEffect;
		}
		this.m_cachedLaserHitEffect = cachedLaserHitEffect;
		StandardEffectInfo cachedThornsEffect;
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
			cachedThornsEffect = this.m_abilityMod.m_thornsEffectMod.GetModifiedValue(this.m_thornsEffect);
		}
		else
		{
			cachedThornsEffect = this.m_thornsEffect;
		}
		this.m_cachedThornsEffect = cachedThornsEffect;
		this.m_cachedReturnEffectOnEnemy = ((!this.m_abilityMod) ? this.m_returnEffectOnEnemy : this.m_abilityMod.m_returnEffectOnEnemyMod.GetModifiedValue(this.m_returnEffectOnEnemy));
		StandardEffectInfo cachedEffectOnSelf;
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
			cachedEffectOnSelf = this.m_abilityMod.m_effectOnSelfMod.GetModifiedValue(this.m_effectOnSelf);
		}
		else
		{
			cachedEffectOnSelf = this.m_effectOnSelf;
		}
		this.m_cachedEffectOnSelf = cachedEffectOnSelf;
	}

	public float GetDamageReductionOnTarget()
	{
		return (!this.m_abilityMod) ? this.m_damageReductionOnTarget : this.m_abilityMod.m_damageReductionOnTargetMod.GetModifiedValue(this.m_damageReductionOnTarget);
	}

	public float GetDamageRedirectToCaster()
	{
		return (!this.m_abilityMod) ? this.m_damageRedirectToCaster : this.m_abilityMod.m_damageRedirectToCasterMod.GetModifiedValue(this.m_damageRedirectToCaster);
	}

	public int GetTechPointGainPerRedirect()
	{
		int result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetTechPointGainPerRedirect()).MethodHandle;
			}
			result = this.m_abilityMod.m_techPointGainPerRedirectMod.GetModifiedValue(this.m_techPointGainPerRedirect);
		}
		else
		{
			result = this.m_techPointGainPerRedirect;
		}
		return result;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedLaserHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetLaserHitEffect()).MethodHandle;
			}
			result = this.m_cachedLaserHitEffect;
		}
		else
		{
			result = this.m_laserHitEffect;
		}
		return result;
	}

	public bool AffectsEnemies()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.AffectsEnemies()).MethodHandle;
			}
			result = this.m_abilityMod.m_affectsEnemiesMod.GetModifiedValue(this.m_affectsEnemies);
		}
		else
		{
			result = this.m_affectsEnemies;
		}
		return result;
	}

	public bool AffectsAllies()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.AffectsAllies()).MethodHandle;
			}
			result = this.m_abilityMod.m_affectsAlliesMod.GetModifiedValue(this.m_affectsAllies);
		}
		else
		{
			result = this.m_affectsAllies;
		}
		return result;
	}

	public bool PenetratesLoS()
	{
		return (!this.m_abilityMod) ? this.m_penetratesLoS : this.m_abilityMod.m_penetratesLoSMod.GetModifiedValue(this.m_penetratesLoS);
	}

	public StandardEffectInfo GetThornsEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedThornsEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetThornsEffect()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetReturnEffectOnEnemy()).MethodHandle;
			}
			result = this.m_cachedReturnEffectOnEnemy;
		}
		else
		{
			result = this.m_returnEffectOnEnemy;
		}
		return result;
	}

	public int GetThornsDamagePerHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetThornsDamagePerHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_thornsDamagePerHitMod.GetModifiedValue(this.m_thornsDamagePerHit);
		}
		else
		{
			result = this.m_thornsDamagePerHit;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnSelf != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetEffectOnSelf()).MethodHandle;
			}
			result = this.m_cachedEffectOnSelf;
		}
		else
		{
			result = this.m_effectOnSelf;
		}
		return result;
	}

	public int GetBaseAbsorb()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetBaseAbsorb()).MethodHandle;
			}
			result = this.m_abilityMod.m_baseAbsorbMod.GetModifiedValue(this.m_baseAbsorb);
		}
		else
		{
			result = this.m_baseAbsorb;
		}
		return result;
	}

	public int GetAbsorbPerCrystalSpent()
	{
		return (!this.m_abilityMod) ? this.m_absorbPerCrystalSpent : this.m_abilityMod.m_absorbPerCrystalSpentMod.GetModifiedValue(this.m_absorbPerCrystalSpent);
	}

	public int GetBaseAbsorbOnAlly()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetBaseAbsorbOnAlly()).MethodHandle;
			}
			result = this.m_abilityMod.m_baseAbsorbOnAllyMod.GetModifiedValue(this.m_baseAbsorbOnAlly);
		}
		else
		{
			result = this.m_baseAbsorbOnAlly;
		}
		return result;
	}

	public int GetAbsorbOnAllyPerCrystalSpent()
	{
		return (!this.m_abilityMod) ? this.m_absorbOnAllyPerCrystalSpent : this.m_abilityMod.m_absorbOnAllyPerCrystalSpentMod.GetModifiedValue(this.m_absorbOnAllyPerCrystalSpent);
	}

	public float GetExtraEnergyPerRedirectDamage()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetExtraEnergyPerRedirectDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraEnergyPerRedirectDamageMod.GetModifiedValue(this.m_extraEnergyPerRedirectDamage);
		}
		else
		{
			result = this.m_extraEnergyPerRedirectDamage;
		}
		return result;
	}

	public float GetHealOnTurnStartPerRedirectDamage()
	{
		return (!this.m_abilityMod) ? this.m_healOnTurnStartPerRedirectDamage : this.m_abilityMod.m_healOnTurnStartPerRedirectDamageMod.GetModifiedValue(this.m_healOnTurnStartPerRedirectDamage);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetCurrentAbsorb(ActorData)).MethodHandle;
			}
			num = martyrProtectAllyThreshold.m_additionalAbsorb;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return this.GetBaseAbsorb() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetAbsorbPerCrystalSpent() + num2;
	}

	private int GetCurrentAbsorbForAlly(ActorData caster)
	{
		MartyrProtectAllyThreshold martyrProtectAllyThreshold = base.GetCurrentPowerEntry(caster) as MartyrProtectAllyThreshold;
		int num;
		if (martyrProtectAllyThreshold != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetCurrentAbsorbForAlly(ActorData)).MethodHandle;
			}
			num = martyrProtectAllyThreshold.m_additionalAbsorbOnAlly;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return this.GetBaseAbsorbOnAlly() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetAbsorbOnAllyPerCrystalSpent() + num2;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_MartyrProtectAlly abilityMod_MartyrProtectAlly = modAsBase as AbilityMod_MartyrProtectAlly;
		string name = "DamageReductionOnTarget_Pct";
		string empty = string.Empty;
		float val;
		if (abilityMod_MartyrProtectAlly)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_MartyrProtectAlly.m_damageReductionOnTargetMod.GetModifiedValue(this.m_damageReductionOnTarget);
		}
		else
		{
			val = this.m_damageReductionOnTarget;
		}
		base.AddTokenFloatAsPct(tokens, name, empty, val, false);
		string name2 = "DamageRedirectToCaster_Pct";
		string empty2 = string.Empty;
		float val2;
		if (abilityMod_MartyrProtectAlly)
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
			val2 = abilityMod_MartyrProtectAlly.m_damageRedirectToCasterMod.GetModifiedValue(this.m_damageRedirectToCaster);
		}
		else
		{
			val2 = this.m_damageRedirectToCaster;
		}
		base.AddTokenFloatAsPct(tokens, name2, empty2, val2, false);
		string name3 = "TechPointGainPerRedirect";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_MartyrProtectAlly)
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
			val3 = abilityMod_MartyrProtectAlly.m_techPointGainPerRedirectMod.GetModifiedValue(this.m_techPointGainPerRedirect);
		}
		else
		{
			val3 = this.m_techPointGainPerRedirect;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_MartyrProtectAlly) ? this.m_laserHitEffect : abilityMod_MartyrProtectAlly.m_laserHitEffectMod.GetModifiedValue(this.m_laserHitEffect), "LaserHitEffect", this.m_laserHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_MartyrProtectAlly) ? this.m_thornsEffect : abilityMod_MartyrProtectAlly.m_thornsEffectMod.GetModifiedValue(this.m_thornsEffect), "ThornsEffect", this.m_thornsEffect, true);
		StandardEffectInfo effectInfo;
		if (abilityMod_MartyrProtectAlly)
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
			effectInfo = abilityMod_MartyrProtectAlly.m_returnEffectOnEnemyMod.GetModifiedValue(this.m_returnEffectOnEnemy);
		}
		else
		{
			effectInfo = this.m_returnEffectOnEnemy;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "ReturnEffectOnEnemy", this.m_returnEffectOnEnemy, true);
		string name4 = "ThornsDamagePerHit";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_MartyrProtectAlly)
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
			val4 = abilityMod_MartyrProtectAlly.m_thornsDamagePerHitMod.GetModifiedValue(this.m_thornsDamagePerHit);
		}
		else
		{
			val4 = this.m_thornsDamagePerHit;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_MartyrProtectAlly)
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
			effectInfo2 = abilityMod_MartyrProtectAlly.m_effectOnSelfMod.GetModifiedValue(this.m_effectOnSelf);
		}
		else
		{
			effectInfo2 = this.m_effectOnSelf;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnSelf", this.m_effectOnSelf, true);
		string name5 = "BaseAbsorb";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_MartyrProtectAlly)
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
			val5 = abilityMod_MartyrProtectAlly.m_baseAbsorbMod.GetModifiedValue(this.m_baseAbsorb);
		}
		else
		{
			val5 = this.m_baseAbsorb;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
		base.AddTokenInt(tokens, "AbsorbPerCrystalSpent", string.Empty, (!abilityMod_MartyrProtectAlly) ? this.m_absorbPerCrystalSpent : abilityMod_MartyrProtectAlly.m_absorbPerCrystalSpentMod.GetModifiedValue(this.m_absorbPerCrystalSpent), false);
		string name6 = "BaseAbsorbOnAlly";
		string empty6 = string.Empty;
		int val6;
		if (abilityMod_MartyrProtectAlly)
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
			val6 = abilityMod_MartyrProtectAlly.m_baseAbsorbOnAllyMod.GetModifiedValue(this.m_baseAbsorbOnAlly);
		}
		else
		{
			val6 = this.m_baseAbsorbOnAlly;
		}
		base.AddTokenInt(tokens, name6, empty6, val6, false);
		string name7 = "AbsorbOnAllyPerCrystalSpent";
		string empty7 = string.Empty;
		int val7;
		if (abilityMod_MartyrProtectAlly)
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
			val7 = abilityMod_MartyrProtectAlly.m_absorbOnAllyPerCrystalSpentMod.GetModifiedValue(this.m_absorbOnAllyPerCrystalSpent);
		}
		else
		{
			val7 = this.m_absorbOnAllyPerCrystalSpent;
		}
		base.AddTokenInt(tokens, name7, empty7, val7, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetLaserHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		this.GetEffectOnSelf().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = base.CalculateNameplateTargetingNumbers();
		this.GetLaserHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
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

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return base.HasTargetableActorsInDecision(caster, this.AffectsEnemies(), this.AffectsAllies(), false, Ability.ValidateCheckPath.Ignore, !this.PenetratesLoS(), false, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, this.AffectsEnemies(), this.AffectsAllies(), false, Ability.ValidateCheckPath.Ignore, !this.PenetratesLoS(), false, false);
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
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.GetThresholdBasedCrystalBonusList()).MethodHandle;
			}
		}
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrProtectAlly))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrProtectAlly.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_MartyrProtectAlly);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
