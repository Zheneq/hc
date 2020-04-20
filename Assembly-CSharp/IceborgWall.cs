using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class IceborgWall : GenericAbility_Container
{
	[Separator("For Wall Effect", true)]
	public int m_wallEffectDuration = 2;

	[Separator("On Hit Data for Wall Hits", "yellow")]
	public OnHitAuthoredData m_wallEffectOnHitData;

	[Separator("Apply Nova effect?", true)]
	public bool m_applyDelayedAoeEffect;

	[Separator("Sequences for Wall Effect", true)]
	public GameObject m_persistentSeqPrefab;

	public GameObject m_onTriggerSeqPrefab;

	private Iceborg_SyncComponent m_syncComp;

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Clamp(this.GetTargetData().Length, 1, 2);
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	public override string GetOnHitDataDesc()
	{
		string text = base.GetOnHitDataDesc();
		if (this.m_wallEffectOnHitData != null)
		{
			text += "-- On Hit Data for lasers from walls --\n";
			text += this.m_wallEffectOnHitData.GetInEditorDesc();
		}
		return text;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		this.m_wallEffectOnHitData.AddTooltipTokens(tokens);
	}

	public unsafe override void GetHitContextForTargetingNumbers(int currentTargeterIndex, out Dictionary<ActorData, ActorHitContext> actorHitContext, out ContextVars abilityContext)
	{
		if (base.Targeters.Count > 0 && currentTargeterIndex > 0)
		{
			if (currentTargeterIndex < base.Targeters.Count)
			{
				AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[currentTargeterIndex];
				actorHitContext = abilityUtil_Targeter.GetActorContextVars();
				abilityContext = abilityUtil_Targeter.GetNonActorSpecificContext();
				return;
			}
		}
		base.GetHitContextForTargetingNumbers(currentTargeterIndex, out actorHitContext, out abilityContext);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (actorHitContext.ContainsKey(targetActor))
		{
			if (targetActor.GetTeam() != caster.GetTeam())
			{
				ActorHitContext actorContext = actorHitContext[targetActor];
				GenericAbility_Container.CalcIntFieldValues(targetActor, caster, actorContext, abilityContext, this.m_wallEffectOnHitData.m_enemyHitIntFields, this.m_calculatedValuesForTargeter);
				results.m_damage = this.m_calculatedValuesForTargeter.m_damage;
			}
		}
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
	}

	protected override void GenModImpl_ClearModRef()
	{
	}
}
