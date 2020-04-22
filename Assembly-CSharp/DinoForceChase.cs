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
		AbilityData component = GetComponent<AbilityData>();
		DinoTargetedKnockback abilityOfType = GetAbilityOfType<DinoTargetedKnockback>();
		if (component != null)
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
			if (abilityOfType != null)
			{
				m_knockbackActionType = component.GetActionTypeOfAbility(abilityOfType);
			}
		}
		base.SetupTargetersAndCachedVars();
	}

	public int GetCdrOnKnockbackAbility()
	{
		int result;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (4)
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
			result = m_abilityMod.m_cdrOnKnockbackAbilityMod.GetModifiedValue(m_cdrOnKnockbackAbility);
		}
		else
		{
			result = m_cdrOnKnockbackAbility;
		}
		return result;
	}

	public int GetEnergyPerUnstoppableEnemyHit()
	{
		int result;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (4)
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
			result = m_abilityMod.m_energyPerUnstoppableEnemyHitMod.GetModifiedValue(m_energyPerUnstoppableEnemyHit);
		}
		else
		{
			result = m_energyPerUnstoppableEnemyHit;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "CdrOnKnockbackAbility", string.Empty, m_cdrOnKnockbackAbility);
		AddTokenInt(tokens, "EnergyPerUnstoppableEnemyHit", string.Empty, m_energyPerUnstoppableEnemyHit);
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_DinoForceChase);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
