using AbilityContextNamespace;
using System.Collections.Generic;
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
		return base.GetUsageForEditor() + ContextVars.GetContextUsageStr(s_cvarMissingShields.GetName(), "amount of missing shield on Scamp", false);
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add("MissingShields");
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		m_passive = GetPassiveOfType<Passive_Scamp>();
		SetCachedFields();
		base.SetupTargetersAndCachedVars();
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedDelayedEffectBase;
		if (m_abilityMod != null)
		{
			cachedDelayedEffectBase = m_abilityMod.m_delayedEffectBaseMod.GetModifiedValue(m_delayedEffectBase);
		}
		else
		{
			cachedDelayedEffectBase = m_delayedEffectBase;
		}
		m_cachedDelayedEffectBase = cachedDelayedEffectBase;
	}

	public StandardActorEffectData GetDelayedEffectBase()
	{
		return (m_cachedDelayedEffectBase == null) ? m_delayedEffectBase : m_cachedDelayedEffectBase;
	}

	public int GetExtraDamageIfShieldDownForm()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraDamageIfShieldDownFormMod.GetModifiedValue(m_extraDamageIfShieldDownForm);
		}
		else
		{
			result = m_extraDamageIfShieldDownForm;
		}
		return result;
	}

	public float GetSubseqTurnDamageMultiplier()
	{
		float result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_subseqTurnDamageMultiplierMod.GetModifiedValue(m_subseqTurnDamageMultiplier);
		}
		else
		{
			result = m_subseqTurnDamageMultiplier;
		}
		return result;
	}

	public bool SubseqTurnNoEnergyGain()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_subseqTurnNoEnergyGainMod.GetModifiedValue(m_subseqTurnNoEnergyGain);
		}
		else
		{
			result = m_subseqTurnNoEnergyGain;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_delayedEffectBase.AddTooltipTokens(tokens, "DelayedEffectBase");
		AddTokenInt(tokens, "ExtraDamageIfShieldDownForm", string.Empty, m_extraDamageIfShieldDownForm);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_suitWasActiveOnTurnStart)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		return !m_disableIfShieldsDown;
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> hitContext, ContextVars abilityContext)
	{
		int value = 0;
		if (m_passive != null && m_syncComp != null)
		{
			value = m_passive.GetMaxSuitShield() - (int)m_syncComp.m_suitShieldingOnTurnStart;
			value = Mathf.Max(0, value);
		}
		abilityContext.SetValue(s_cvarMissingShields.GetKey(), value);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_ScampDelayedAoe);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
