using System.Collections.Generic;
using System.Text;
using AbilityContextNamespace;

public class IceborgDetonateNova : GenericAbility_Container
{
	[Separator("Empowered Nova Core On Hit Data", "yellow")]
	public OnHitAuthoredData m_empoweredDelayedAoeOnHitData;
	[Separator("Shield Per Detonate on NovaOnReact ability (Arctic Armor)")]
	public int m_novaOnReactShieldPerDetonate;
	public int m_shieldOnDetonateDuration = 1;
	[Separator("Cdr Per Target Killed")]
	public int m_cdrPerKill;
	public int m_cdrIfAnyKill;
	[Separator("Animation Index for detonate portion of the ability")]
	public int m_detonateAnimIndex;
	public static ContextNameKeyPair s_cvarNumNovaCores = new ContextNameKeyPair("NumNovaCores");

	private Iceborg_SyncComponent m_syncComp;
	private AbilityMod_IceborgDetonateNova m_abilityMod;
	private OnHitAuthoredData m_cachedEmpoweredDelayedAoeOnHitData;

	public override string GetUsageForEditor()
	{
		return new StringBuilder().Append(base.GetUsageForEditor()).Append(ContextVars.GetContextUsageStr(s_cvarNumNovaCores.GetName(), "Number of nova cores detonating", false)).ToString();
	}

	public override string GetOnHitDataDesc()
	{
		string text = base.GetOnHitDataDesc();
		if (m_empoweredDelayedAoeOnHitData != null)
		{
			text += m_empoweredDelayedAoeOnHitData.GetInEditorDesc();
		}
		return text;
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(s_cvarNumNovaCores.GetName());
		return contextNamesForEditor;
	}

	public int GetDelayedDenotateDamage()
	{
		return GetEmpoweredDelayedAoeOnHitData().GetFirstDamageValue();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		SetCachedFields();
	}

	private void SetCachedFields()
	{
		m_cachedEmpoweredDelayedAoeOnHitData = m_abilityMod != null
			? m_abilityMod.m_empoweredDelayedAoeOnHitDataMod.GetModdedOnHitData(m_empoweredDelayedAoeOnHitData)
			: m_empoweredDelayedAoeOnHitData;
	}

	public OnHitAuthoredData GetEmpoweredDelayedAoeOnHitData()
	{
		return m_cachedEmpoweredDelayedAoeOnHitData ?? m_empoweredDelayedAoeOnHitData;
	}

	public int GetNovaOnReactShieldPerDetonate()
	{
		return m_abilityMod != null
			? m_abilityMod.m_novaOnReactShieldPerDetonateMod.GetModifiedValue(m_novaOnReactShieldPerDetonate)
			: m_novaOnReactShieldPerDetonate;
	}

	public int GetShieldOnDetonateDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldOnDetonateDurationMod.GetModifiedValue(m_shieldOnDetonateDuration)
			: m_shieldOnDetonateDuration;
	}

	public int GetCdrPerKill()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrPerKillMod.GetModifiedValue(m_cdrPerKill)
			: m_cdrPerKill;
	}

	public int GetCdrIfAnyKill()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfAnyKillMod.GetModifiedValue(m_cdrIfAnyKill)
			: m_cdrIfAnyKill;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_empoweredDelayedAoeOnHitData.AddTooltipTokens(tokens);
		AddTokenInt(tokens, "NovaOnReactShieldPerDetonate", string.Empty, m_novaOnReactShieldPerDetonate);
		AddTokenInt(tokens, "ShieldOnDetonateDuration", string.Empty, m_shieldOnDetonateDuration);
		AddTokenInt(tokens, "CdrPerKill", string.Empty, m_cdrPerKill);
		AddTokenInt(tokens, "CdrIfAnyKill", string.Empty, m_cdrIfAnyKill);
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Iceborg_SyncComponent>();
		}
		if (m_syncComp != null)
		{
			m_syncComp.AddTooltipTokens(tokens);
		}
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return (m_syncComp == null || m_syncComp.m_numNovaEffectsOnTurnStart > 0)
		       && base.CustomCanCastValidation(caster);
	}

	public override void PreProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargetIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext)
	{
		if (m_syncComp != null)
		{
			abilityContext.SetValue(s_cvarNumNovaCores.GetKey(), m_syncComp.m_numNovaEffectsOnTurnStart);
		}
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_IceborgDetonateNova;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
