using AbilityContextNamespace;
using System.Collections.Generic;

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
		contextNamesForEditor.Add(Iceborg_SyncComponent.s_cvarHasNova.GetName());
		return contextNamesForEditor;
	}

	public override string GetUsageForEditor()
	{
		return base.GetUsageForEditor() + ContextVars.GetDebugString(Iceborg_SyncComponent.s_cvarHasNova.GetName(), "set to 1 if target has nova core on start of turn, 0 otherwise");
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

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		if (!(m_syncComp != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_syncComp.SetHasCoreContext_Client(actorHitContext, targetActor, base.ActorData);
			return;
		}
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		int energyOnCasterIfTargetHasNovaCore = GetEnergyOnCasterIfTargetHasNovaCore();
		if (energyOnCasterIfTargetHasNovaCore > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Dictionary<ActorData, ActorHitContext> actorContextVars = base.Targeter.GetActorContextVars();
					using (Dictionary<ActorData, ActorHitContext>.Enumerator enumerator = actorContextVars.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<ActorData, ActorHitContext> current = enumerator.Current;
							ActorData key = current.Key;
							if (key.GetTeam() != caster.GetTeam())
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (current.Value._0012)
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
									if (m_syncComp.HasNovaCore(key))
									{
										while (true)
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
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return num;
							}
						}
					}
				}
				}
			}
		}
		return num;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		object result;
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (6)
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
			result = m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
		}
		else
		{
			result = null;
		}
		return (string)result;
	}

	public int GetEnergyOnCasterIfTargetHasNovaCore()
	{
		int result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_energyOnCasterIfTargetHasNovaCoreMod.GetModifiedValue(m_energyOnCasterIfTargetHasNovaCore);
		}
		else
		{
			result = m_energyOnCasterIfTargetHasNovaCore;
		}
		return result;
	}

	public int GetCdrIfHasHit()
	{
		int result;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (6)
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
			result = m_abilityMod.m_cdrIfHasHitMod.GetModifiedValue(m_cdrIfHasHit);
		}
		else
		{
			result = m_cdrIfHasHit;
		}
		return result;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_IceborgIcicle);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
