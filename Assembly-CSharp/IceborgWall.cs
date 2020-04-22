using AbilityContextNamespace;
using System.Collections.Generic;
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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

	public override void GetHitContextForTargetingNumbers(int currentTargeterIndex, out Dictionary<ActorData, ActorHitContext> actorHitContext, out ContextVars abilityContext)
	{
		if (base.Targeters.Count > 0 && currentTargeterIndex > 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (currentTargeterIndex < base.Targeters.Count)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[currentTargeterIndex];
						actorHitContext = abilityUtil_Targeter.GetActorContextVars();
						abilityContext = abilityUtil_Targeter.GetNonActorSpecificContext();
						return;
					}
					}
				}
			}
		}
		base.GetHitContextForTargetingNumbers(currentTargeterIndex, out actorHitContext, out abilityContext);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (!actorHitContext.ContainsKey(targetActor))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (targetActor.GetTeam() != caster.GetTeam())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					ActorHitContext actorContext = actorHitContext[targetActor];
					GenericAbility_Container.CalcIntFieldValues(targetActor, caster, actorContext, abilityContext, m_wallEffectOnHitData.m_enemyHitIntFields, m_calculatedValuesForTargeter);
					results.m_damage = m_calculatedValuesForTargeter.m_damage;
					return;
				}
			}
			return;
		}
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
	}

	protected override void GenModImpl_ClearModRef()
	{
	}
}
