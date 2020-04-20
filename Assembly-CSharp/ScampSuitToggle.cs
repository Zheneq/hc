using System;
using System.Collections.Generic;
using UnityEngine;

public class ScampSuitToggle : Ability
{
	[Separator("Whether shield down mode is free action", true)]
	public bool m_shieldDownModeFreeAction;

	[Separator("Cooldowns", true)]
	public int m_cooldownCreateSuit = 2;

	public int m_cooldownRefillShield = 2;

	[Header("-- Cooldown override for when suit is destroyed")]
	public int m_cooldownOverrideOnSuitDestroy = 2;

	[Separator("Energy to Shield (shield = energy x multiplier)", true)]
	public float m_energyToShieldMult = 1f;

	[Separator("Clear Energy Orbs on cast", true)]
	public bool m_clearEnergyOrbsOnCast = true;

	[Separator("Extra Orbs to spawn on suit lost", true)]
	public int m_extraOrbsToSpawnOnSuitLost;

	[Separator("Passive Energy Regen", true)]
	public int m_passiveEnergyRegen;

	[Separator("Effect to apply when suit is gained or lost (applied on start of turn)", true)]
	public bool m_considerRespawnForSuitGainEffect;

	public StandardEffectInfo m_effectForSuitGained;

	public StandardEffectInfo m_effectForSuitLost;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	[Header("-- for setting anim param, only spawned when adding new suit")]
	public GameObject m_addSuitAnimSeqPrefab;

	private AbilityMod_ScampSuitToggle m_abilityMod;

	private Scamp_SyncComponent m_syncComp;

	private Passive_Scamp m_passive;

	private StandardEffectInfo m_cachedEffectForSuitGained;

	private StandardEffectInfo m_cachedEffectForSuitLost;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "ScampSuitToggle";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_passive = base.GetPassiveOfType<Passive_Scamp>();
		this.SetCachedFields();
		this.m_syncComp = base.GetComponent<Scamp_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, false, false, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "CooldownCreateSuit", string.Empty, this.m_cooldownCreateSuit, false);
		base.AddTokenInt(tokens, "CooldownRefillShield", string.Empty, this.m_cooldownRefillShield, false);
		base.AddTokenInt(tokens, "CooldownOverrideOnSuitDestroy", string.Empty, this.m_cooldownOverrideOnSuitDestroy, false);
		base.AddTokenInt(tokens, "ExtraOrbsToSpawnOnSuitLost", string.Empty, this.m_extraOrbsToSpawnOnSuitLost, false);
		base.AddTokenInt(tokens, "PassiveEnergyRegen", string.Empty, this.m_passiveEnergyRegen, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectForSuitGained, "EffectForSuitGained", this.m_effectForSuitGained, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectForSuitLost, "EffectForSuitLost", this.m_effectForSuitLost, true);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectForSuitGained;
		if (this.m_abilityMod != null)
		{
			cachedEffectForSuitGained = this.m_abilityMod.m_effectForSuitGainedMod.GetModifiedValue(this.m_effectForSuitGained);
		}
		else
		{
			cachedEffectForSuitGained = this.m_effectForSuitGained;
		}
		this.m_cachedEffectForSuitGained = cachedEffectForSuitGained;
		StandardEffectInfo cachedEffectForSuitLost;
		if (this.m_abilityMod != null)
		{
			cachedEffectForSuitLost = this.m_abilityMod.m_effectForSuitLostMod.GetModifiedValue(this.m_effectForSuitLost);
		}
		else
		{
			cachedEffectForSuitLost = this.m_effectForSuitLost;
		}
		this.m_cachedEffectForSuitLost = cachedEffectForSuitLost;
	}

	public bool ShieldDownModeFreeAction()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_shieldDownModeFreeActionMod.GetModifiedValue(this.m_shieldDownModeFreeAction);
		}
		else
		{
			result = this.m_shieldDownModeFreeAction;
		}
		return result;
	}

	public int GetCooldownCreateSuit()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_cooldownCreateSuitMod.GetModifiedValue(this.m_cooldownCreateSuit);
		}
		else
		{
			result = this.m_cooldownCreateSuit;
		}
		return result;
	}

	public int GetCooldownRefillShield()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_cooldownRefillShieldMod.GetModifiedValue(this.m_cooldownRefillShield);
		}
		else
		{
			result = this.m_cooldownRefillShield;
		}
		return result;
	}

	public int GetCooldownOverrideOnSuitDestroy()
	{
		return (!(this.m_abilityMod != null)) ? this.m_cooldownOverrideOnSuitDestroy : this.m_abilityMod.m_cooldownOverrideOnSuitDestroyMod.GetModifiedValue(this.m_cooldownOverrideOnSuitDestroy);
	}

	public float GetEnergyToShieldMult()
	{
		float result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_energyToShieldMultMod.GetModifiedValue(this.m_energyToShieldMult);
		}
		else
		{
			result = this.m_energyToShieldMult;
		}
		return result;
	}

	public bool ClearEnergyOrbsOnCast()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_clearEnergyOrbsOnCastMod.GetModifiedValue(this.m_clearEnergyOrbsOnCast);
		}
		else
		{
			result = this.m_clearEnergyOrbsOnCast;
		}
		return result;
	}

	public int GetExtraOrbsToSpawnOnSuitLost()
	{
		return (!(this.m_abilityMod != null)) ? this.m_extraOrbsToSpawnOnSuitLost : this.m_abilityMod.m_extraOrbsToSpawnOnSuitLostMod.GetModifiedValue(this.m_extraOrbsToSpawnOnSuitLost);
	}

	public int GetPassiveEnergyRegen()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_passiveEnergyRegenMod.GetModifiedValue(this.m_passiveEnergyRegen);
		}
		else
		{
			result = this.m_passiveEnergyRegen;
		}
		return result;
	}

	public bool ConsiderRespawnForSuitGainEffect()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_considerRespawnForSuitGainEffectMod.GetModifiedValue(this.m_considerRespawnForSuitGainEffect);
		}
		else
		{
			result = this.m_considerRespawnForSuitGainEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEffectForSuitGained()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectForSuitGained != null)
		{
			result = this.m_cachedEffectForSuitGained;
		}
		else
		{
			result = this.m_effectForSuitGained;
		}
		return result;
	}

	public StandardEffectInfo GetEffectForSuitLost()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectForSuitLost != null)
		{
			result = this.m_cachedEffectForSuitLost;
		}
		else
		{
			result = this.m_effectForSuitLost;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Self, 1);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		ActorData actorData = base.ActorData;
		int num = actorData.TechPoints + actorData.ReservedTechPoints;
		int value = Mathf.RoundToInt((float)num * this.GetEnergyToShieldMult());
		results.m_absorb = Mathf.Clamp(value, 1, this.m_passive.GetMaxSuitShield());
		return true;
	}

	public override bool IsFreeAction()
	{
		if (this.m_syncComp != null)
		{
			if (this.m_syncComp.m_suitWasActiveOnTurnStart)
			{
				return base.IsFreeAction();
			}
		}
		return this.ShieldDownModeFreeAction();
	}

	public override int GetModdedCost()
	{
		int b = 0;
		if (base.ActorData != null)
		{
			b = base.ActorData.TechPoints + base.ActorData.ReservedTechPoints;
		}
		return Mathf.Max(1, b);
	}

	public override int GetTechPointRegenContribution()
	{
		int passiveEnergyRegen = this.GetPassiveEnergyRegen();
		int result;
		if (passiveEnergyRegen > 0)
		{
			result = passiveEnergyRegen;
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_syncComp != null)
		{
			return caster.TechPoints + caster.ReservedTechPoints > 0;
		}
		return base.CustomCanCastValidation(caster);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScampSuitToggle))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ScampSuitToggle);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
