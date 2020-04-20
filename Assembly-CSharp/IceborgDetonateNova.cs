using System;
using System.Collections.Generic;
using AbilityContextNamespace;

public class IceborgDetonateNova : GenericAbility_Container
{
	[Separator("Empowered Nova Core On Hit Data", "yellow")]
	public OnHitAuthoredData m_empoweredDelayedAoeOnHitData;

	[Separator("Shield Per Detonate on NovaOnReact ability (Arctic Armor)", true)]
	public int m_novaOnReactShieldPerDetonate;

	public int m_shieldOnDetonateDuration = 1;

	[Separator("Cdr Per Target Killed", true)]
	public int m_cdrPerKill;

	public int m_cdrIfAnyKill;

	[Separator("Animation Index for detonate portion of the ability", true)]
	public int m_detonateAnimIndex;

	public static ContextNameKeyPair s_cvarNumNovaCores = new ContextNameKeyPair("NumNovaCores");

	private Iceborg_SyncComponent m_syncComp;

	private AbilityMod_IceborgDetonateNova m_abilityMod;

	private OnHitAuthoredData m_cachedEmpoweredDelayedAoeOnHitData;

	public override string GetUsageForEditor()
	{
		return base.GetUsageForEditor() + ContextVars.GetDebugString(IceborgDetonateNova.s_cvarNumNovaCores.GetName(), "Number of nova cores detonating", false);
	}

	public override string GetOnHitDataDesc()
	{
		string text = base.GetOnHitDataDesc();
		if (this.m_empoweredDelayedAoeOnHitData != null)
		{
			text += this.m_empoweredDelayedAoeOnHitData.GetInEditorDesc();
		}
		return text;
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(IceborgDetonateNova.s_cvarNumNovaCores.GetName());
		return contextNamesForEditor;
	}

	public int GetDelayedDenotateDamage()
	{
		OnHitAuthoredData empoweredDelayedAoeOnHitData = this.GetEmpoweredDelayedAoeOnHitData();
		return empoweredDelayedAoeOnHitData.GetFirstDamageValue();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		this.SetCachedFields();
	}

	private void SetCachedFields()
	{
		this.m_cachedEmpoweredDelayedAoeOnHitData = ((!(this.m_abilityMod != null)) ? this.m_empoweredDelayedAoeOnHitData : this.m_abilityMod.m_empoweredDelayedAoeOnHitDataMod.symbol_001D(this.m_empoweredDelayedAoeOnHitData));
	}

	public OnHitAuthoredData GetEmpoweredDelayedAoeOnHitData()
	{
		OnHitAuthoredData result;
		if (this.m_cachedEmpoweredDelayedAoeOnHitData != null)
		{
			result = this.m_cachedEmpoweredDelayedAoeOnHitData;
		}
		else
		{
			result = this.m_empoweredDelayedAoeOnHitData;
		}
		return result;
	}

	public int GetNovaOnReactShieldPerDetonate()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_novaOnReactShieldPerDetonateMod.GetModifiedValue(this.m_novaOnReactShieldPerDetonate);
		}
		else
		{
			result = this.m_novaOnReactShieldPerDetonate;
		}
		return result;
	}

	public int GetShieldOnDetonateDuration()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_shieldOnDetonateDurationMod.GetModifiedValue(this.m_shieldOnDetonateDuration);
		}
		else
		{
			result = this.m_shieldOnDetonateDuration;
		}
		return result;
	}

	public int GetCdrPerKill()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_cdrPerKillMod.GetModifiedValue(this.m_cdrPerKill);
		}
		else
		{
			result = this.m_cdrPerKill;
		}
		return result;
	}

	public int GetCdrIfAnyKill()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_cdrIfAnyKillMod.GetModifiedValue(this.m_cdrIfAnyKill);
		}
		else
		{
			result = this.m_cdrIfAnyKill;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		this.m_empoweredDelayedAoeOnHitData.AddTooltipTokens(tokens);
		base.AddTokenInt(tokens, "NovaOnReactShieldPerDetonate", string.Empty, this.m_novaOnReactShieldPerDetonate, false);
		base.AddTokenInt(tokens, "ShieldOnDetonateDuration", string.Empty, this.m_shieldOnDetonateDuration, false);
		base.AddTokenInt(tokens, "CdrPerKill", string.Empty, this.m_cdrPerKill, false);
		base.AddTokenInt(tokens, "CdrIfAnyKill", string.Empty, this.m_cdrIfAnyKill, false);
		if (this.m_syncComp == null)
		{
			this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		}
		if (this.m_syncComp != null)
		{
			this.m_syncComp.AddTooltipTokens(tokens);
		}
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_syncComp != null && this.m_syncComp.m_numNovaEffectsOnTurnStart <= 0)
		{
			return false;
		}
		return base.CustomCanCastValidation(caster);
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		if (this.m_syncComp != null)
		{
			abilityContext.SetInt(IceborgDetonateNova.s_cvarNumNovaCores.GetHash(), (int)this.m_syncComp.m_numNovaEffectsOnTurnStart);
		}
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_IceborgDetonateNova);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
