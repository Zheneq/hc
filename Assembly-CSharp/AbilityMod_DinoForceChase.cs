using System;
using System.Collections.Generic;

public class AbilityMod_DinoForceChase : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_Cone m_targetSelMod;

	[Separator("Cooldown reduction on knockback ability", true)]
	public AbilityModPropertyInt m_cdrOnKnockbackAbilityMod;

	[Separator("Energy Per Unstoppable Enemy", true)]
	public AbilityModPropertyInt m_energyPerUnstoppableEnemyHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(DinoForceChase);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		DinoForceChase dinoForceChase = targetAbility as DinoForceChase;
		if (!(dinoForceChase != null))
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
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, m_cdrOnKnockbackAbilityMod, "CdrOnKnockbackAbility", string.Empty, dinoForceChase.m_cdrOnKnockbackAbility);
			AbilityMod.AddToken(tokens, m_energyPerUnstoppableEnemyHitMod, "EnergyPerUnstoppableEnemyHit", string.Empty, dinoForceChase.m_energyPerUnstoppableEnemyHit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoForceChase dinoForceChase = GetTargetAbilityOnAbilityData(abilityData) as DinoForceChase;
		bool flag = dinoForceChase != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (dinoForceChase != null)
		{
			text += GetTargetSelectModDesc(m_targetSelMod, dinoForceChase.m_targetSelectComp);
			string str = text;
			AbilityModPropertyInt cdrOnKnockbackAbilityMod = m_cdrOnKnockbackAbilityMod;
			int baseVal;
			if (flag)
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
				baseVal = dinoForceChase.m_cdrOnKnockbackAbility;
			}
			else
			{
				baseVal = 0;
			}
			text = str + PropDesc(cdrOnKnockbackAbilityMod, "[CdrOnKnockbackAbility]", flag, baseVal);
			text += PropDesc(m_energyPerUnstoppableEnemyHitMod, "[EnergyPerUnstoppableEnemyHit]", flag, flag ? dinoForceChase.m_energyPerUnstoppableEnemyHit : 0);
		}
		return text;
	}
}
