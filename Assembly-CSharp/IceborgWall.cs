using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class IceborgWall : GenericAbility_Container
{
	[Separator("For Wall Effect")]
	public int m_wallEffectDuration = 2;
	[Separator("On Hit Data for Wall Hits", "yellow")]
	public OnHitAuthoredData m_wallEffectOnHitData;
	[Separator("Apply Nova effect?")]
	public bool m_applyDelayedAoeEffect;
	[Separator("Sequences for Wall Effect")]
	public GameObject m_persistentSeqPrefab;
	public GameObject m_onTriggerSeqPrefab;

	private Iceborg_SyncComponent m_syncComp;

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Clamp(GetTargetData().Length, 1, 2);
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	public override string GetOnHitDataDesc()
	{
		string text = base.GetOnHitDataDesc();
		if (m_wallEffectOnHitData != null)
		{
			text += "-- On Hit Data for lasers from walls --\n";
			text += m_wallEffectOnHitData.GetInEditorDesc();
		}
		return text;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_wallEffectOnHitData.AddTooltipTokens(tokens);
	}

	public override void GetHitContextForTargetingNumbers(
		int currentTargeterIndex,
		out Dictionary<ActorData, ActorHitContext> actorHitContext,
		out ContextVars abilityContext)
	{
		if (Targeters.Count > 0
		    && currentTargeterIndex > 0
		    && currentTargeterIndex < Targeters.Count)
		{
			AbilityUtil_Targeter targeter = Targeters[currentTargeterIndex];
			actorHitContext = targeter.GetActorContextVars();
			abilityContext = targeter.GetNonActorSpecificContext();
		}
		else
		{
			base.GetHitContextForTargetingNumbers(currentTargeterIndex, out actorHitContext, out abilityContext);
		}
	}

	public override void PostProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargeterIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext,
		ActorData caster,
		TargetingNumberUpdateScratch results)
	{
		if (actorHitContext.ContainsKey(targetActor) && targetActor.GetTeam() != caster.GetTeam())
		{
			ActorHitContext actorContext = actorHitContext[targetActor];
			CalcIntFieldValues(targetActor, caster, actorContext, abilityContext, m_wallEffectOnHitData.m_enemyHitIntFields, m_calculatedValuesForTargeter);
			results.m_damage = m_calculatedValuesForTargeter.m_damage;
		}
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
	}

	protected override void GenModImpl_ClearModRef()
	{
	}
}
