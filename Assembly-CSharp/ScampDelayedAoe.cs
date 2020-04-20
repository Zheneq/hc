using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class ScampDelayedAoe : GenericAbility_Container
{
	[Separator("Effect Data for Delayed Aoe (for caster)", true)]
	public StandardActorEffectData m_delayedEffectBase;

	[Separator("Extra Damage if in Shield Down form", true)]
	public int m_extraDamageIfShieldDownForm;

	[Header("-- If >= 0, will multiply on delayed AoE damage after first damage turn")]
	public float m_subseqTurnDamageMultiplier = -1f;

	public bool m_subseqTurnNoEnergyGain;

	[Separator("Disable in Shield Down mode?", true)]
	public bool m_disableIfShieldsDown = true;

	[Separator("Anim Index for delayed Aoe Triggering", true)]
	public int m_animIndexOnTrigger;

	[Separator("Sequences - For Delayed Effect", true)]
	public GameObject m_onTriggerSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

	private Passive_Scamp m_passive;

	private AbilityMod_ScampDelayedAoe m_abilityMod;

	private const string c_missingShields = "MissingShields";

	public static ContextNameKeyPair s_cvarMissingShields = new ContextNameKeyPair("MissingShields");

	private StandardActorEffectData m_cachedDelayedEffectBase;

	public override string GetUsageForEditor()
	{
		return base.GetUsageForEditor() + ContextVars.GetDebugString(ScampDelayedAoe.s_cvarMissingShields.GetName(), "amount of missing shield on Scamp", false);
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add("MissingShields");
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Scamp_SyncComponent>();
		this.m_passive = base.GetPassiveOfType<Passive_Scamp>();
		this.SetCachedFields();
		base.SetupTargetersAndCachedVars();
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedDelayedEffectBase;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDelayedAoe.SetCachedFields()).MethodHandle;
			}
			cachedDelayedEffectBase = this.m_abilityMod.m_delayedEffectBaseMod.GetModifiedValue(this.m_delayedEffectBase);
		}
		else
		{
			cachedDelayedEffectBase = this.m_delayedEffectBase;
		}
		this.m_cachedDelayedEffectBase = cachedDelayedEffectBase;
	}

	public StandardActorEffectData GetDelayedEffectBase()
	{
		return (this.m_cachedDelayedEffectBase == null) ? this.m_delayedEffectBase : this.m_cachedDelayedEffectBase;
	}

	public int GetExtraDamageIfShieldDownForm()
	{
		int result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDelayedAoe.GetExtraDamageIfShieldDownForm()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageIfShieldDownFormMod.GetModifiedValue(this.m_extraDamageIfShieldDownForm);
		}
		else
		{
			result = this.m_extraDamageIfShieldDownForm;
		}
		return result;
	}

	public float GetSubseqTurnDamageMultiplier()
	{
		float result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDelayedAoe.GetSubseqTurnDamageMultiplier()).MethodHandle;
			}
			result = this.m_abilityMod.m_subseqTurnDamageMultiplierMod.GetModifiedValue(this.m_subseqTurnDamageMultiplier);
		}
		else
		{
			result = this.m_subseqTurnDamageMultiplier;
		}
		return result;
	}

	public bool SubseqTurnNoEnergyGain()
	{
		bool result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDelayedAoe.SubseqTurnNoEnergyGain()).MethodHandle;
			}
			result = this.m_abilityMod.m_subseqTurnNoEnergyGainMod.GetModifiedValue(this.m_subseqTurnNoEnergyGain);
		}
		else
		{
			result = this.m_subseqTurnNoEnergyGain;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		this.m_delayedEffectBase.AddTooltipTokens(tokens, "DelayedEffectBase", false, null);
		base.AddTokenInt(tokens, "ExtraDamageIfShieldDownForm", string.Empty, this.m_extraDamageIfShieldDownForm, false);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScampDelayedAoe.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			if (this.m_syncComp.m_suitWasActiveOnTurnStart)
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
				return true;
			}
		}
		return !this.m_disableIfShieldsDown;
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> hitContext, ContextVars abilityContext)
	{
		int num = 0;
		if (this.m_passive != null && this.m_syncComp != null)
		{
			num = this.m_passive.GetMaxSuitShield() - (int)this.m_syncComp.m_suitShieldingOnTurnStart;
			num = Mathf.Max(0, num);
		}
		abilityContext.SetInt(ScampDelayedAoe.s_cvarMissingShields.GetHash(), num);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_ScampDelayedAoe);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
