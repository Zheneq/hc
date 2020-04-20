using System;
using System.Collections.Generic;

public class DinoForceChase : GenericAbility_Container
{
	[Separator("Cooldown reduction on knockback ability", true)]
	public int m_cdrOnKnockbackAbility;

	[Separator("Energy Per Unstoppable Enemy (if ability is combat phase or later)", true)]
	public int m_energyPerUnstoppableEnemyHit;

	private AbilityMod_DinoForceChase m_abilityMod;

	private AbilityData.ActionType m_knockbackActionType = AbilityData.ActionType.INVALID_ACTION;

	protected override void SetupTargetersAndCachedVars()
	{
		AbilityData component = base.GetComponent<AbilityData>();
		DinoTargetedKnockback abilityOfType = base.GetAbilityOfType<DinoTargetedKnockback>();
		if (component != null)
		{
			if (abilityOfType != null)
			{
				this.m_knockbackActionType = component.GetActionTypeOfAbility(abilityOfType);
			}
		}
		base.SetupTargetersAndCachedVars();
	}

	public int GetCdrOnKnockbackAbility()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_cdrOnKnockbackAbilityMod.GetModifiedValue(this.m_cdrOnKnockbackAbility);
		}
		else
		{
			result = this.m_cdrOnKnockbackAbility;
		}
		return result;
	}

	public int GetEnergyPerUnstoppableEnemyHit()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_energyPerUnstoppableEnemyHitMod.GetModifiedValue(this.m_energyPerUnstoppableEnemyHit);
		}
		else
		{
			result = this.m_energyPerUnstoppableEnemyHit;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "CdrOnKnockbackAbility", string.Empty, this.m_cdrOnKnockbackAbility, false);
		base.AddTokenInt(tokens, "EnergyPerUnstoppableEnemyHit", string.Empty, this.m_energyPerUnstoppableEnemyHit, false);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_DinoForceChase);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
