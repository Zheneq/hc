using System.Collections.Generic;

public class FireborgSuperheat : GenericAbility_Container
{
	[Separator("Superheat", true)]
	public int m_superheatDuration = 2;

	public int m_igniteExtraDamageIfSuperheated;

	private Fireborg_SyncComponent m_syncComp;

	private AbilityMod_FireborgSuperheat m_abilityMod;

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Fireborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "SuperheatDuration", string.Empty, m_superheatDuration);
		AddTokenInt(tokens, "IgniteExtraDamageIfSuperheated", string.Empty, m_igniteExtraDamageIfSuperheated);
	}

	public int GetSuperheatDuration()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_superheatDurationMod.GetModifiedValue(m_superheatDuration);
		}
		else
		{
			result = m_superheatDuration;
		}
		return result;
	}

	public int GetIgniteExtraDamageIfSuperheated()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_igniteExtraDamageIfSuperheatedMod.GetModifiedValue(m_igniteExtraDamageIfSuperheated);
		}
		else
		{
			result = m_igniteExtraDamageIfSuperheated;
		}
		return result;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_FireborgSuperheat);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
