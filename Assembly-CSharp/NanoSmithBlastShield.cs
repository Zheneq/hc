using System;
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithBlastShield : Ability
{
	[Header("-- Shield Effect")]
	public StandardActorEffectData m_shieldEffect;

	public int m_healOnEndIfHasRemainingAbsorb;

	public int m_energyGainOnShieldTarget;

	[Header("-- Extra Effect on Caster if targeting Ally")]
	public StandardEffectInfo m_extraEffectOnCasterIfTargetingAlly;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_shieldSequencePrefab;

	private bool m_allowOnEnemy;

	private AbilityMod_NanoSmithBlastShield m_abilityMod;

	private StandardEffectInfo m_cachedExtraEffectOnCasterIfTargetingAlly;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBlastShield.Start()).MethodHandle;
			}
			this.m_abilityName = "Blast Shield";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		AbilityUtil_Targeter.AffectsActor affectsActor;
		if (this.GetExtraEffectOnCasterIfTargetingAlly().m_applyEffect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBlastShield.Setup()).MethodHandle;
			}
			affectsActor = AbilityUtil_Targeter.AffectsActor.Always;
		}
		else
		{
			affectsActor = AbilityUtil_Targeter.AffectsActor.Possible;
		}
		AbilityUtil_Targeter.AffectsActor affectsCaster = affectsActor;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.m_allowOnEnemy, true, affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedExtraEffectOnCasterIfTargetingAlly;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBlastShield.SetCachedFields()).MethodHandle;
			}
			cachedExtraEffectOnCasterIfTargetingAlly = this.m_abilityMod.m_extraEffectOnCasterIfTargetingAllyMod.GetModifiedValue(this.m_extraEffectOnCasterIfTargetingAlly);
		}
		else
		{
			cachedExtraEffectOnCasterIfTargetingAlly = this.m_extraEffectOnCasterIfTargetingAlly;
		}
		this.m_cachedExtraEffectOnCasterIfTargetingAlly = cachedExtraEffectOnCasterIfTargetingAlly;
	}

	public StandardActorEffectData GetShieldEffectData()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_shieldEffectOverride.GetModifiedValue(this.m_shieldEffect) : this.m_shieldEffect;
	}

	public int GetHealOnEndIfHasRemainingAbsorb()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBlastShield.GetHealOnEndIfHasRemainingAbsorb()).MethodHandle;
			}
			result = this.m_healOnEndIfHasRemainingAbsorb;
		}
		else
		{
			result = this.m_abilityMod.m_healOnEndIfHasRemainingAbsorbMod.GetModifiedValue(this.m_healOnEndIfHasRemainingAbsorb);
		}
		return result;
	}

	public int GetEnergyGainOnShieldTarget()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBlastShield.GetEnergyGainOnShieldTarget()).MethodHandle;
			}
			result = this.m_energyGainOnShieldTarget;
		}
		else
		{
			result = this.m_abilityMod.m_energyGainOnShieldTargetMod.GetModifiedValue(this.m_energyGainOnShieldTarget);
		}
		return result;
	}

	public StandardEffectInfo GetExtraEffectOnCasterIfTargetingAlly()
	{
		StandardEffectInfo result;
		if (this.m_cachedExtraEffectOnCasterIfTargetingAlly != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBlastShield.GetExtraEffectOnCasterIfTargetingAlly()).MethodHandle;
			}
			result = this.m_cachedExtraEffectOnCasterIfTargetingAlly;
		}
		else
		{
			result = this.m_extraEffectOnCasterIfTargetingAlly;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetShieldEffectData().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportEnergy(ref result, AbilityTooltipSubject.Primary, this.GetEnergyGainOnShieldTarget());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (this.GetExtraEffectOnCasterIfTargetingAlly().m_applyEffect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBlastShield.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			ActorData actorData = base.ActorData;
			if (actorData != null)
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
				if (actorData == targetActor)
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
					int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
					if (visibleActorsCountByTooltipSubject > 0)
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
						dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						dictionary[AbilityTooltipSymbol.Absorb] = this.GetExtraEffectOnCasterIfTargetingAlly().m_effectData.m_absorbAmount;
					}
				}
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int energyGainOnShieldTarget = this.GetEnergyGainOnShieldTarget();
		if (energyGainOnShieldTarget > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBlastShield.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			BoardSquare boardSquare = Board.\u000E().\u000E(base.Targeter.LastUpdatingGridPos);
			if (boardSquare != null)
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
				if (boardSquare.OccupantActor == caster)
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
					return energyGainOnShieldTarget;
				}
			}
		}
		return 0;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, this.m_allowOnEnemy, true, true, Ability.ValidateCheckPath.Ignore, true, true, false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithBlastShield abilityMod_NanoSmithBlastShield = modAsBase as AbilityMod_NanoSmithBlastShield;
		StandardActorEffectData standardActorEffectData;
		if (abilityMod_NanoSmithBlastShield)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBlastShield.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			standardActorEffectData = abilityMod_NanoSmithBlastShield.m_shieldEffectOverride.GetModifiedValue(this.m_shieldEffect);
		}
		else
		{
			standardActorEffectData = this.m_shieldEffect;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "ShieldEffect", abilityMod_NanoSmithBlastShield != null, this.m_shieldEffect);
		string name = "HealOnEndIfHasRemainingAbsorb";
		string empty = string.Empty;
		int val;
		if (abilityMod_NanoSmithBlastShield)
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
			val = abilityMod_NanoSmithBlastShield.m_healOnEndIfHasRemainingAbsorbMod.GetModifiedValue(this.m_healOnEndIfHasRemainingAbsorb);
		}
		else
		{
			val = this.m_healOnEndIfHasRemainingAbsorb;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "EnergyGainOnShieldTarget", string.Empty, (!abilityMod_NanoSmithBlastShield) ? this.m_energyGainOnShieldTarget : abilityMod_NanoSmithBlastShield.m_energyGainOnShieldTargetMod.GetModifiedValue(this.m_energyGainOnShieldTarget), false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_NanoSmithBlastShield) ? this.m_extraEffectOnCasterIfTargetingAlly : abilityMod_NanoSmithBlastShield.m_extraEffectOnCasterIfTargetingAllyMod.GetModifiedValue(this.m_extraEffectOnCasterIfTargetingAlly), "ExtraEffectOnCasterIfTargetingAlly", this.m_extraEffectOnCasterIfTargetingAlly, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithBlastShield))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBlastShield.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_NanoSmithBlastShield);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
