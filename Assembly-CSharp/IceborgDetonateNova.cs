using AbilityContextNamespace;
using System.Collections.Generic;

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
		return base.GetUsageForEditor() + ContextVars.GetDebugString(s_cvarNumNovaCores.GetName(), "Number of nova cores detonating", false);
	}

	public override string GetOnHitDataDesc()
	{
		string text = base.GetOnHitDataDesc();
		if (m_empoweredDelayedAoeOnHitData != null)
		{
			while (true)
			{
				switch (1)
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
		OnHitAuthoredData empoweredDelayedAoeOnHitData = GetEmpoweredDelayedAoeOnHitData();
		return empoweredDelayedAoeOnHitData.GetFirstDamageValue();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		SetCachedFields();
	}

	private void SetCachedFields()
	{
		m_cachedEmpoweredDelayedAoeOnHitData = ((!(m_abilityMod != null)) ? m_empoweredDelayedAoeOnHitData : m_abilityMod.m_empoweredDelayedAoeOnHitDataMod._001D(m_empoweredDelayedAoeOnHitData));
	}

	public OnHitAuthoredData GetEmpoweredDelayedAoeOnHitData()
	{
		OnHitAuthoredData result;
		if (m_cachedEmpoweredDelayedAoeOnHitData != null)
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
			result = m_cachedEmpoweredDelayedAoeOnHitData;
		}
		else
		{
			result = m_empoweredDelayedAoeOnHitData;
		}
		return result;
	}

	public int GetNovaOnReactShieldPerDetonate()
	{
		int result;
		if (m_abilityMod != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_novaOnReactShieldPerDetonateMod.GetModifiedValue(m_novaOnReactShieldPerDetonate);
		}
		else
		{
			result = m_novaOnReactShieldPerDetonate;
		}
		return result;
	}

	public int GetShieldOnDetonateDuration()
	{
		int result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_shieldOnDetonateDurationMod.GetModifiedValue(m_shieldOnDetonateDuration);
		}
		else
		{
			result = m_shieldOnDetonateDuration;
		}
		return result;
	}

	public int GetCdrPerKill()
	{
		int result;
		if (m_abilityMod != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_cdrPerKillMod.GetModifiedValue(m_cdrPerKill);
		}
		else
		{
			result = m_cdrPerKill;
		}
		return result;
	}

	public int GetCdrIfAnyKill()
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
			result = m_abilityMod.m_cdrIfAnyKillMod.GetModifiedValue(m_cdrIfAnyKill);
		}
		else
		{
			result = m_cdrIfAnyKill;
		}
		return result;
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
			m_syncComp = GetComponent<Iceborg_SyncComponent>();
		}
		if (!(m_syncComp != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			m_syncComp.AddTooltipTokens(tokens);
			return;
		}
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_syncComp != null && m_syncComp.m_numNovaEffectsOnTurnStart <= 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		return base.CustomCanCastValidation(caster);
	}

	public override void PreProcessTargetingNumbers(ActorData targetActor, int currentTargetIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext)
	{
		if (!(m_syncComp != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			abilityContext.SetInt(s_cvarNumNovaCores.GetHash(), m_syncComp.m_numNovaEffectsOnTurnStart);
			return;
		}
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_IceborgDetonateNova);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
