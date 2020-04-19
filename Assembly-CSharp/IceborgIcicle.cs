using System;
using System.Collections.Generic;
using AbilityContextNamespace;

public class IceborgIcicle : GenericAbility_Container
{
	[Separator("Apply Nova effect?", true)]
	public bool m_applyDelayedAoeEffect = true;

	[Separator("Energy on caster if target has nova core on start of turn", true)]
	public int m_energyOnCasterIfTargetHasNovaCore;

	[Separator("Cdr if has hit (applied to Cdr Target Ability)", true)]
	public int m_cdrIfHasHit;

	public AbilityData.ActionType m_cdrTargetAbility = AbilityData.ActionType.ABILITY_2;

	private Iceborg_SyncComponent m_syncComp;

	private AbilityMod_IceborgIcicle m_abilityMod;

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(Iceborg_SyncComponent.s_cvarHasNova.\u0012());
		return contextNamesForEditor;
	}

	public override string GetUsageForEditor()
	{
		return base.GetUsageForEditor() + ContextVars.\u0015(Iceborg_SyncComponent.s_cvarHasNova.\u0012(), "set to 1 if target has nova core on start of turn, 0 otherwise", true);
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "CdrIfHasHit", string.Empty, this.m_cdrIfHasHit, false);
		base.AddTokenInt(tokens, "EnergyOnCasterIfTargetHasNovaCore", string.Empty, this.m_energyOnCasterIfTargetHasNovaCore, false);
		if (this.m_syncComp == null)
		{
			this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		}
		if (this.m_syncComp != null)
		{
			this.m_syncComp.AddTooltipTokens(tokens);
		}
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgIcicle.PreProcessTargetingNumbers(ActorData, int, Dictionary<ActorData, ActorHitContext>, ContextVars)).MethodHandle;
			}
			this.m_syncComp.SetHasCoreContext_Client(actorHitContext, targetActor, base.ActorData);
		}
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		int energyOnCasterIfTargetHasNovaCore = this.GetEnergyOnCasterIfTargetHasNovaCore();
		if (energyOnCasterIfTargetHasNovaCore > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgIcicle.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			Dictionary<ActorData, ActorHitContext> actorContextVars = base.Targeter.GetActorContextVars();
			using (Dictionary<ActorData, ActorHitContext>.Enumerator enumerator = actorContextVars.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ActorData, ActorHitContext> keyValuePair = enumerator.Current;
					ActorData key = keyValuePair.Key;
					if (key.\u000E() != caster.\u000E())
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
						if (keyValuePair.Value.\u0012)
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
							if (this.m_syncComp.HasNovaCore(key))
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
								num += energyOnCasterIfTargetHasNovaCore;
							}
						}
					}
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return num;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		string result;
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgIcicle.GetAccessoryTargeterNumberString(ActorData, AbilityTooltipSymbol, int)).MethodHandle;
			}
			result = this.m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
		}
		else
		{
			result = null;
		}
		return result;
	}

	public int GetEnergyOnCasterIfTargetHasNovaCore()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgIcicle.GetEnergyOnCasterIfTargetHasNovaCore()).MethodHandle;
			}
			result = this.m_abilityMod.m_energyOnCasterIfTargetHasNovaCoreMod.GetModifiedValue(this.m_energyOnCasterIfTargetHasNovaCore);
		}
		else
		{
			result = this.m_energyOnCasterIfTargetHasNovaCore;
		}
		return result;
	}

	public int GetCdrIfHasHit()
	{
		int result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgIcicle.GetCdrIfHasHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrIfHasHitMod.GetModifiedValue(this.m_cdrIfHasHit);
		}
		else
		{
			result = this.m_cdrIfHasHit;
		}
		return result;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_IceborgIcicle);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
