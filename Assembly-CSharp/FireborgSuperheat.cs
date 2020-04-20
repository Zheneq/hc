using System;
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
		this.m_syncComp = base.GetComponent<Fireborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "SuperheatDuration", string.Empty, this.m_superheatDuration, false);
		base.AddTokenInt(tokens, "IgniteExtraDamageIfSuperheated", string.Empty, this.m_igniteExtraDamageIfSuperheated, false);
	}

	public int GetSuperheatDuration()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_superheatDurationMod.GetModifiedValue(this.m_superheatDuration);
		}
		else
		{
			result = this.m_superheatDuration;
		}
		return result;
	}

	public int GetIgniteExtraDamageIfSuperheated()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_igniteExtraDamageIfSuperheatedMod.GetModifiedValue(this.m_igniteExtraDamageIfSuperheated);
		}
		else
		{
			result = this.m_igniteExtraDamageIfSuperheated;
		}
		return result;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_FireborgSuperheat);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
