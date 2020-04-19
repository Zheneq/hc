using System;
using System.Collections.Generic;
using UnityEngine;

public class MartyrSpendCrystals : Ability
{
	public MartyrSpendCrystals.TargetingMode m_targetingMode;

	[Header("-- Self Healing & Absorb")]
	public StandardEffectInfo m_spentCrystalsEffect;

	[Header("-- Self Healing")]
	public int m_selfHealBase;

	public int m_selfHealPerCrystalSpent;

	public int m_selfHealPerEnemyHit;

	public bool m_selfHealIsOverTime = true;

	[Space(10f)]
	public int m_extraSelfHealPerTurnAtMaxEnergy;

	public int m_maxExtraSelfHealForMaxEnergy;

	[Header("-- Self Absorb")]
	public int m_selfAbsorbBase;

	public int m_selfAbsorbPerCrystalSpent;

	[Header("-- Enemy Hit (if using AoE targeting)")]
	public float m_aoeRadiusBase = 2f;

	public float m_aoeRadiuePerCrystal = 0.5f;

	public bool m_penetrateLos;

	public int m_damageBase;

	public int m_damagePerCrystal;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Ally Hit (if using AoE targeting")]
	public int m_allyHealBase;

	public int m_allyHealPerCrystal;

	public int m_allyHealPerEnemyHit;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- Energy Use --")]
	public bool m_clearEnergyOnCast = true;

	public int m_selfEnergyGainOnCast;

	[Header("-- Cooldown Reduction on other abilities")]
	public AbilityData.ActionType m_protectAllyActionType = AbilityData.ActionType.INVALID_ACTION;

	public int m_cdrOnProtectAllyAbility;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	private Martyr_SyncComponent m_syncComponent;

	private AbilityMod_MartyrSpendCrystals m_abilityMod;

	private StandardEffectInfo m_cachedSpentCrystalsEffect;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.Start()).MethodHandle;
			}
			this.m_abilityName = "Martyr Spend Crystals";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		this.m_syncComponent = base.GetComponent<Martyr_SyncComponent>();
		if (this.m_targetingMode == MartyrSpendCrystals.TargetingMode.OnSelf)
		{
			base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
			base.Targeter.ShowArcToShape = false;
		}
		else
		{
			AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = new AbilityUtil_Targeter_AoE_Smooth(this, 1f, this.PenetrateLos(), this.IncludeEnemies(), this.IncludeAllies(), -1);
			abilityUtil_Targeter_AoE_Smooth.SetAffectedGroups(this.IncludeEnemies(), this.IncludeAllies(), true);
			abilityUtil_Targeter_AoE_Smooth.m_customRadiusDelegate = new AbilityUtil_Targeter_AoE_Smooth.GetRadiusDelegate(this.GetTargeterRadius);
			base.Targeter = abilityUtil_Targeter_AoE_Smooth;
		}
	}

	private float GetTargeterRadius(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return this.GetCurrentAoeRadius(targetingActor);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedSpentCrystalsEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.SetCachedFields()).MethodHandle;
			}
			cachedSpentCrystalsEffect = this.m_abilityMod.m_spentCrystalsEffectMod.GetModifiedValue(this.m_spentCrystalsEffect);
		}
		else
		{
			cachedSpentCrystalsEffect = this.m_spentCrystalsEffect;
		}
		this.m_cachedSpentCrystalsEffect = cachedSpentCrystalsEffect;
		StandardEffectInfo cachedEnemyHitEffect;
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
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		this.m_cachedAllyHitEffect = ((!this.m_abilityMod) ? this.m_allyHitEffect : this.m_abilityMod.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect));
	}

	public StandardEffectInfo GetSpentCrystalsEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedSpentCrystalsEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetSpentCrystalsEffect()).MethodHandle;
			}
			result = this.m_cachedSpentCrystalsEffect;
		}
		else
		{
			result = this.m_spentCrystalsEffect;
		}
		return result;
	}

	public int GetSelfHealBase()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetSelfHealBase()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealBaseMod.GetModifiedValue(this.m_selfHealBase);
		}
		else
		{
			result = this.m_selfHealBase;
		}
		return result;
	}

	public int GetSelfHealPerCrystalSpent()
	{
		return (!this.m_abilityMod) ? this.m_selfHealPerCrystalSpent : this.m_abilityMod.m_selfHealPerCrystalSpentMod.GetModifiedValue(this.m_selfHealPerCrystalSpent);
	}

	public int GetSelfHealPerEnemyHit()
	{
		return (!this.m_abilityMod) ? this.m_selfHealPerEnemyHit : this.m_abilityMod.m_selfHealPerEnemyHitMod.GetModifiedValue(this.m_selfHealPerEnemyHit);
	}

	public bool SelfHealIsOverTime()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.SelfHealIsOverTime()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealIsOverTimeMod.GetModifiedValue(this.m_selfHealIsOverTime);
		}
		else
		{
			result = this.m_selfHealIsOverTime;
		}
		return result;
	}

	public int GetExtraSelfHealPerTurnAtMaxEnergy()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetExtraSelfHealPerTurnAtMaxEnergy()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraSelfHealPerTurnAtMaxEnergyMod.GetModifiedValue(this.m_extraSelfHealPerTurnAtMaxEnergy);
		}
		else
		{
			result = this.m_extraSelfHealPerTurnAtMaxEnergy;
		}
		return result;
	}

	public int GetMaxExtraSelfHealForMaxEnergy()
	{
		return (!this.m_abilityMod) ? this.m_maxExtraSelfHealForMaxEnergy : this.m_abilityMod.m_maxExtraSelfHealForMaxEnergyMod.GetModifiedValue(this.m_maxExtraSelfHealForMaxEnergy);
	}

	public int GetSelfAbsorbBase()
	{
		return (!this.m_abilityMod) ? this.m_selfAbsorbBase : this.m_abilityMod.m_selfAbsorbBaseMod.GetModifiedValue(this.m_selfAbsorbBase);
	}

	public int GetSelfAbsorbPerCrystalSpent()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetSelfAbsorbPerCrystalSpent()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfAbsorbPerCrystalSpentMod.GetModifiedValue(this.m_selfAbsorbPerCrystalSpent);
		}
		else
		{
			result = this.m_selfAbsorbPerCrystalSpent;
		}
		return result;
	}

	public float GetAoeRadiusBase()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetAoeRadiusBase()).MethodHandle;
			}
			result = this.m_abilityMod.m_aoeRadiusBaseMod.GetModifiedValue(this.m_aoeRadiusBase);
		}
		else
		{
			result = this.m_aoeRadiusBase;
		}
		return result;
	}

	public float GetAoeRadiuePerCrystal()
	{
		return (!this.m_abilityMod) ? this.m_aoeRadiuePerCrystal : this.m_abilityMod.m_aoeRadiuePerCrystalMod.GetModifiedValue(this.m_aoeRadiuePerCrystal);
	}

	public bool PenetrateLos()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.PenetrateLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
		}
		else
		{
			result = this.m_penetrateLos;
		}
		return result;
	}

	public int GetDamageBase()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetDamageBase()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageBaseMod.GetModifiedValue(this.m_damageBase);
		}
		else
		{
			result = this.m_damageBase;
		}
		return result;
	}

	public int GetDamagePerCrystal()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetDamagePerCrystal()).MethodHandle;
			}
			result = this.m_abilityMod.m_damagePerCrystalMod.GetModifiedValue(this.m_damagePerCrystal);
		}
		else
		{
			result = this.m_damagePerCrystal;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public int GetAllyHealBase()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetAllyHealBase()).MethodHandle;
			}
			result = this.m_abilityMod.m_allyHealBaseMod.GetModifiedValue(this.m_allyHealBase);
		}
		else
		{
			result = this.m_allyHealBase;
		}
		return result;
	}

	public int GetAllyHealPerCrystal()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetAllyHealPerCrystal()).MethodHandle;
			}
			result = this.m_abilityMod.m_allyHealPerCrystalMod.GetModifiedValue(this.m_allyHealPerCrystal);
		}
		else
		{
			result = this.m_allyHealPerCrystal;
		}
		return result;
	}

	public int GetAllyHealPerEnemyHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetAllyHealPerEnemyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_allyHealPerEnemyHitMod.GetModifiedValue(this.m_allyHealPerEnemyHit);
		}
		else
		{
			result = this.m_allyHealPerEnemyHit;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetAllyHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllyHitEffect;
		}
		else
		{
			result = this.m_allyHitEffect;
		}
		return result;
	}

	public bool ClearEnergyOnCast()
	{
		return (!this.m_abilityMod) ? this.m_clearEnergyOnCast : this.m_abilityMod.m_clearEnergyOnCastMod.GetModifiedValue(this.m_clearEnergyOnCast);
	}

	public int GetSelfEnergyGainOnCast()
	{
		return (!this.m_abilityMod) ? this.m_selfEnergyGainOnCast : this.m_abilityMod.m_selfEnergyGainOnCastMod.GetModifiedValue(this.m_selfEnergyGainOnCast);
	}

	public int GetCdrOnProtectAllyAbility()
	{
		return (!this.m_abilityMod) ? this.m_cdrOnProtectAllyAbility : this.m_abilityMod.m_cdrOnProtectAllyAbilityMod.GetModifiedValue(this.m_cdrOnProtectAllyAbility);
	}

	public float GetCurrentAoeRadius(ActorData caster)
	{
		float num = 0f;
		if (this.m_syncComponent != null)
		{
			num = this.GetAoeRadiuePerCrystal() * (float)this.m_syncComponent.SpentDamageCrystals(caster);
		}
		return this.GetAoeRadiusBase() + num;
	}

	public bool IncludeEnemies()
	{
		if (this.GetDamageBase() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.IncludeEnemies()).MethodHandle;
			}
			if (this.GetDamagePerCrystal() <= 0)
			{
				return this.GetEnemyHitEffect().m_applyEffect;
			}
		}
		return true;
	}

	public bool IncludeAllies()
	{
		bool result;
		if (this.GetAllyHealBase() <= 0 && this.GetAllyHealPerCrystal() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.IncludeAllies()).MethodHandle;
			}
			result = this.GetAllyHitEffect().m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	private int GetCurrentAbsorbOnSelf(ActorData caster)
	{
		return this.GetSelfAbsorbBase() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetSelfAbsorbPerCrystalSpent();
	}

	private int GetCurrentHealingOnSelf(ActorData caster, int numEnemiesHit)
	{
		int num = 0;
		if (this.GetSelfHealPerEnemyHit() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetCurrentHealingOnSelf(ActorData, int)).MethodHandle;
			}
			num += this.GetSelfHealPerEnemyHit() * numEnemiesHit;
		}
		if (this.GetExtraSelfHealPerTurnAtMaxEnergy() > 0)
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
			if (this.m_syncComponent != null)
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
				if (this.m_syncComponent.m_syncNumTurnsAtFullEnergy > 1)
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
					int num2 = this.GetExtraSelfHealPerTurnAtMaxEnergy() * (this.m_syncComponent.m_syncNumTurnsAtFullEnergy - 1);
					if (this.GetMaxExtraSelfHealForMaxEnergy() > 0)
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
						num2 = Mathf.Min(this.GetMaxExtraSelfHealForMaxEnergy(), num2);
					}
					num += num2;
				}
			}
		}
		return this.GetSelfHealBase() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetSelfHealPerCrystalSpent() + num;
	}

	private int GetCurrentHealingOnAlly(ActorData caster, int numEnemiesHit)
	{
		int num = 0;
		if (this.GetSelfHealPerEnemyHit() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetCurrentHealingOnAlly(ActorData, int)).MethodHandle;
			}
			num = this.GetAllyHealPerEnemyHit() * numEnemiesHit;
		}
		return this.GetAllyHealBase() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetAllyHealPerCrystal() + num;
	}

	private int GetCurrentDamage(ActorData caster)
	{
		return this.GetDamageBase() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetDamagePerCrystal();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_MartyrSpendCrystals abilityMod_MartyrSpendCrystals = modAsBase as AbilityMod_MartyrSpendCrystals;
		StandardEffectInfo effectInfo;
		if (abilityMod_MartyrSpendCrystals)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			effectInfo = abilityMod_MartyrSpendCrystals.m_spentCrystalsEffectMod.GetModifiedValue(this.m_spentCrystalsEffect);
		}
		else
		{
			effectInfo = this.m_spentCrystalsEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "SpentCrystalsEffect", this.m_spentCrystalsEffect, true);
		string name = "SelfHealBase";
		string empty = string.Empty;
		int val;
		if (abilityMod_MartyrSpendCrystals)
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
			val = abilityMod_MartyrSpendCrystals.m_selfHealBaseMod.GetModifiedValue(this.m_selfHealBase);
		}
		else
		{
			val = this.m_selfHealBase;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "SelfHealPerCrystalSpent";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_MartyrSpendCrystals)
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
			val2 = abilityMod_MartyrSpendCrystals.m_selfHealPerCrystalSpentMod.GetModifiedValue(this.m_selfHealPerCrystalSpent);
		}
		else
		{
			val2 = this.m_selfHealPerCrystalSpent;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "SelfHealPerEnemyHit", string.Empty, (!abilityMod_MartyrSpendCrystals) ? this.m_selfHealPerEnemyHit : abilityMod_MartyrSpendCrystals.m_selfHealPerEnemyHitMod.GetModifiedValue(this.m_selfHealPerEnemyHit), false);
		string name3 = "ExtraSelfHealPerTurnAtMaxEnergy";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_MartyrSpendCrystals)
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
			val3 = abilityMod_MartyrSpendCrystals.m_extraSelfHealPerTurnAtMaxEnergyMod.GetModifiedValue(this.m_extraSelfHealPerTurnAtMaxEnergy);
		}
		else
		{
			val3 = this.m_extraSelfHealPerTurnAtMaxEnergy;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "MaxExtraSelfHealForMaxEnergy";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_MartyrSpendCrystals)
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
			val4 = abilityMod_MartyrSpendCrystals.m_maxExtraSelfHealForMaxEnergyMod.GetModifiedValue(this.m_maxExtraSelfHealForMaxEnergy);
		}
		else
		{
			val4 = this.m_maxExtraSelfHealForMaxEnergy;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		string name5 = "SelfAbsorbBase";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_MartyrSpendCrystals)
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
			val5 = abilityMod_MartyrSpendCrystals.m_selfAbsorbBaseMod.GetModifiedValue(this.m_selfAbsorbBase);
		}
		else
		{
			val5 = this.m_selfAbsorbBase;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
		string name6 = "SelfAbsorbPerCrystalSpent";
		string empty6 = string.Empty;
		int val6;
		if (abilityMod_MartyrSpendCrystals)
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
			val6 = abilityMod_MartyrSpendCrystals.m_selfAbsorbPerCrystalSpentMod.GetModifiedValue(this.m_selfAbsorbPerCrystalSpent);
		}
		else
		{
			val6 = this.m_selfAbsorbPerCrystalSpent;
		}
		base.AddTokenInt(tokens, name6, empty6, val6, false);
		string name7 = "AoeRadiusBase";
		string empty7 = string.Empty;
		float val7;
		if (abilityMod_MartyrSpendCrystals)
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
			val7 = abilityMod_MartyrSpendCrystals.m_aoeRadiusBaseMod.GetModifiedValue(this.m_aoeRadiusBase);
		}
		else
		{
			val7 = this.m_aoeRadiusBase;
		}
		base.AddTokenFloat(tokens, name7, empty7, val7, false);
		string name8 = "AoeRadiuePerCrystal";
		string empty8 = string.Empty;
		float val8;
		if (abilityMod_MartyrSpendCrystals)
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
			val8 = abilityMod_MartyrSpendCrystals.m_aoeRadiuePerCrystalMod.GetModifiedValue(this.m_aoeRadiuePerCrystal);
		}
		else
		{
			val8 = this.m_aoeRadiuePerCrystal;
		}
		base.AddTokenFloat(tokens, name8, empty8, val8, false);
		string name9 = "DamageBase";
		string empty9 = string.Empty;
		int val9;
		if (abilityMod_MartyrSpendCrystals)
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
			val9 = abilityMod_MartyrSpendCrystals.m_damageBaseMod.GetModifiedValue(this.m_damageBase);
		}
		else
		{
			val9 = this.m_damageBase;
		}
		base.AddTokenInt(tokens, name9, empty9, val9, false);
		string name10 = "DamagePerCrystal";
		string empty10 = string.Empty;
		int val10;
		if (abilityMod_MartyrSpendCrystals)
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
			val10 = abilityMod_MartyrSpendCrystals.m_damagePerCrystalMod.GetModifiedValue(this.m_damagePerCrystal);
		}
		else
		{
			val10 = this.m_damagePerCrystal;
		}
		base.AddTokenInt(tokens, name10, empty10, val10, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_MartyrSpendCrystals)
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
			effectInfo2 = abilityMod_MartyrSpendCrystals.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo2 = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EnemyHitEffect", this.m_enemyHitEffect, true);
		string name11 = "AllyHealBase";
		string empty11 = string.Empty;
		int val11;
		if (abilityMod_MartyrSpendCrystals)
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
			val11 = abilityMod_MartyrSpendCrystals.m_allyHealBaseMod.GetModifiedValue(this.m_allyHealBase);
		}
		else
		{
			val11 = this.m_allyHealBase;
		}
		base.AddTokenInt(tokens, name11, empty11, val11, false);
		string name12 = "AllyHealPerCrystal";
		string empty12 = string.Empty;
		int val12;
		if (abilityMod_MartyrSpendCrystals)
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
			val12 = abilityMod_MartyrSpendCrystals.m_allyHealPerCrystalMod.GetModifiedValue(this.m_allyHealPerCrystal);
		}
		else
		{
			val12 = this.m_allyHealPerCrystal;
		}
		base.AddTokenInt(tokens, name12, empty12, val12, false);
		string name13 = "AllyHealPerEnemyHit";
		string empty13 = string.Empty;
		int val13;
		if (abilityMod_MartyrSpendCrystals)
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
			val13 = abilityMod_MartyrSpendCrystals.m_allyHealPerEnemyHitMod.GetModifiedValue(this.m_allyHealPerEnemyHit);
		}
		else
		{
			val13 = this.m_allyHealPerEnemyHit;
		}
		base.AddTokenInt(tokens, name13, empty13, val13, false);
		StandardEffectInfo effectInfo3;
		if (abilityMod_MartyrSpendCrystals)
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
			effectInfo3 = abilityMod_MartyrSpendCrystals.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			effectInfo3 = this.m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "AllyHitEffect", this.m_allyHitEffect, true);
		string name14 = "SelfEnergyGainOnCast";
		string empty14 = string.Empty;
		int val14;
		if (abilityMod_MartyrSpendCrystals)
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
			val14 = abilityMod_MartyrSpendCrystals.m_selfEnergyGainOnCastMod.GetModifiedValue(this.m_selfEnergyGainOnCast);
		}
		else
		{
			val14 = this.m_selfEnergyGainOnCast;
		}
		base.AddTokenInt(tokens, name14, empty14, val14, false);
		string name15 = "CdrOnProtectAllyAbility";
		string empty15 = string.Empty;
		int val15;
		if (abilityMod_MartyrSpendCrystals)
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
			val15 = abilityMod_MartyrSpendCrystals.m_cdrOnProtectAllyAbilityMod.GetModifiedValue(this.m_cdrOnProtectAllyAbility);
		}
		else
		{
			val15 = this.m_cdrOnProtectAllyAbility;
		}
		base.AddTokenInt(tokens, name15, empty15, val15, false);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = base.CalculateNameplateTargetingNumbers();
		this.GetSpentCrystalsEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, 1);
		this.GetAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, 1);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		ActorData actorData = base.ActorData;
		int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
		if (targetActor == actorData)
		{
			int currentAbsorbOnSelf = this.GetCurrentAbsorbOnSelf(actorData);
			int currentHealingOnSelf = this.GetCurrentHealingOnSelf(actorData, visibleActorsCountByTooltipSubject);
			Ability.AddNameplateValueForSingleHit(ref dictionary, base.Targeter, actorData, currentAbsorbOnSelf, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self);
			Ability.AddNameplateValueForSingleHit(ref dictionary, base.Targeter, actorData, currentHealingOnSelf, AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self);
		}
		else
		{
			List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
			if (tooltipSubjectTypes != null)
			{
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
				{
					int currentDamage = this.GetCurrentDamage(actorData);
					dictionary[AbilityTooltipSymbol.Damage] = currentDamage;
				}
				else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
					}
					int currentHealingOnAlly = this.GetCurrentHealingOnAlly(actorData, visibleActorsCountByTooltipSubject);
					dictionary[AbilityTooltipSymbol.Healing] = currentHealingOnAlly;
				}
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.GetSelfEnergyGainOnCast() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			return this.GetSelfEnergyGainOnCast();
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_syncComponent != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrSpendCrystals.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			return this.m_syncComponent.DamageCrystals > 0;
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrSpendCrystals))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_MartyrSpendCrystals);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public enum TargetingMode
	{
		OnSelf,
		Aoe
	}
}
