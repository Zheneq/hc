using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class IceborgConsumeNova : GenericAbility_Container
{
	[Separator("Shield Gain", true)]
	public int m_shieldGainBase = 10;

	public int m_shieldGainPerNova = 10;

	[Header("-- Shield effect data, shield amount will be set by ability")]
	public StandardActorEffectData m_shieldEffectData;

	private Iceborg_SyncComponent m_syncComp;

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (!(targetActor == caster))
		{
			return;
		}
		while (true)
		{
			int numNovaEffects = 0;
			if (m_syncComp != null)
			{
				numNovaEffects = m_syncComp.m_numNovaEffectsOnTurnStart;
			}
			int num = results.m_absorb = CalcTotalShields(numNovaEffects);
			return;
		}
	}

	private int CalcTotalShields(int numNovaEffects)
	{
		int num = 0;
		if (m_shieldGainBase > 0)
		{
			num += m_shieldGainBase;
		}
		if (m_shieldGainPerNova > 0)
		{
			num += numNovaEffects * m_shieldGainPerNova;
		}
		return num;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
	}

	protected override void GenModImpl_ClearModRef()
	{
	}
}
