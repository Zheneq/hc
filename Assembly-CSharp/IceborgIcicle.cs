using System.Collections.Generic;
using System.Text;
using AbilityContextNamespace;

public class IceborgIcicle : GenericAbility_Container
{
	[Separator("Apply Nova effect?")]
	public bool m_applyDelayedAoeEffect = true;
	[Separator("Energy on caster if target has nova core on start of turn")]
	public int m_energyOnCasterIfTargetHasNovaCore;
	[Separator("Cdr if has hit (applied to Cdr Target Ability)")]
	public int m_cdrIfHasHit;
	public AbilityData.ActionType m_cdrTargetAbility = AbilityData.ActionType.ABILITY_2;

	private Iceborg_SyncComponent m_syncComp;
	private AbilityMod_IceborgIcicle m_abilityMod;

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(Iceborg_SyncComponent.s_cvarHasNova.GetName());
		return contextNamesForEditor;
	}

	public override string GetUsageForEditor()
	{
		return new StringBuilder().Append(base.GetUsageForEditor()).Append(ContextVars.GetContextUsageStr(Iceborg_SyncComponent.s_cvarHasNova.GetName(), "set to 1 if target has nova core on start of turn, 0 otherwise")).ToString();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "CdrIfHasHit", string.Empty, m_cdrIfHasHit);
		AddTokenInt(tokens, "EnergyOnCasterIfTargetHasNovaCore", string.Empty, m_energyOnCasterIfTargetHasNovaCore);
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Iceborg_SyncComponent>();
		}
		if (m_syncComp != null)
		{
			m_syncComp.AddTooltipTokens(tokens);
		}
	}

	public override void PreProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargetIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext)
	{
		if (m_syncComp != null)
		{
			m_syncComp.SetHasCoreContext_Client(actorHitContext, targetActor, ActorData);
		}
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int energyOnCasterIfTargetHasNovaCore = GetEnergyOnCasterIfTargetHasNovaCore();
		if (energyOnCasterIfTargetHasNovaCore <= 0)
		{
			return 0;
		}
		int num = 0;
		foreach (KeyValuePair<ActorData, ActorHitContext> actorContext in Targeter.GetActorContextVars())
		{
			if (actorContext.Key.GetTeam() != caster.GetTeam()
			    && actorContext.Value.m_inRangeForTargeter
			    && m_syncComp.HasNovaCore(actorContext.Key))
			{
				num += energyOnCasterIfTargetHasNovaCore;
			}
		}
		return num;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return m_syncComp != null
			? m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, ActorData)
			: null;
	}

	public int GetEnergyOnCasterIfTargetHasNovaCore()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyOnCasterIfTargetHasNovaCoreMod.GetModifiedValue(m_energyOnCasterIfTargetHasNovaCore)
			: m_energyOnCasterIfTargetHasNovaCore;
	}

	public int GetCdrIfHasHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfHasHitMod.GetModifiedValue(m_cdrIfHasHit)
			: m_cdrIfHasHit;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_IceborgIcicle;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
