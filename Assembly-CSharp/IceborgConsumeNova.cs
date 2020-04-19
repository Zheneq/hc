using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class IceborgConsumeNova : GenericAbility_Container
{
	[Separator("Shield Gain", true)]
	public int m_shieldGainBase = 0xA;

	public int m_shieldGainPerNova = 0xA;

	[Header("-- Shield effect data, shield amount will be set by ability")]
	public StandardActorEffectData m_shieldEffectData;

	private Iceborg_SyncComponent m_syncComp;

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (targetActor == caster)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConsumeNova.PostProcessTargetingNumbers(ActorData, int, Dictionary<ActorData, ActorHitContext>, ContextVars, ActorData, TargetingNumberUpdateScratch)).MethodHandle;
			}
			int numNovaEffects = 0;
			if (this.m_syncComp != null)
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
				numNovaEffects = (int)this.m_syncComp.m_numNovaEffectsOnTurnStart;
			}
			int absorb = this.CalcTotalShields(numNovaEffects);
			results.m_absorb = absorb;
		}
	}

	private int CalcTotalShields(int numNovaEffects)
	{
		int num = 0;
		if (this.m_shieldGainBase > 0)
		{
			num += this.m_shieldGainBase;
		}
		if (this.m_shieldGainPerNova > 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgConsumeNova.CalcTotalShields(int)).MethodHandle;
			}
			num += numNovaEffects * this.m_shieldGainPerNova;
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
