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
		targetSelect.SetTargetSelectMod(this.m_targetSelMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		DinoForceChase dinoForceChase = targetAbility as DinoForceChase;
		if (dinoForceChase != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_DinoForceChase.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_cdrOnKnockbackAbilityMod, "CdrOnKnockbackAbility", string.Empty, dinoForceChase.m_cdrOnKnockbackAbility, true, false);
			AbilityMod.AddToken(tokens, this.m_energyPerUnstoppableEnemyHitMod, "EnergyPerUnstoppableEnemyHit", string.Empty, dinoForceChase.m_energyPerUnstoppableEnemyHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoForceChase dinoForceChase = base.GetTargetAbilityOnAbilityData(abilityData) as DinoForceChase;
		bool flag = dinoForceChase != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (dinoForceChase != null)
		{
			text += base.GetTargetSelectModDesc(this.m_targetSelMod, dinoForceChase.m_targetSelectComp, "-- Target Select Mod --");
			string str = text;
			AbilityModPropertyInt cdrOnKnockbackAbilityMod = this.m_cdrOnKnockbackAbilityMod;
			string prefix = "[CdrOnKnockbackAbility]";
			bool showBaseVal = flag;
			int baseVal;
			if (flag)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_DinoForceChase.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
				}
				baseVal = dinoForceChase.m_cdrOnKnockbackAbility;
			}
			else
			{
				baseVal = 0;
			}
			text = str + base.PropDesc(cdrOnKnockbackAbilityMod, prefix, showBaseVal, baseVal);
			text += base.PropDesc(this.m_energyPerUnstoppableEnemyHitMod, "[EnergyPerUnstoppableEnemyHit]", flag, (!flag) ? 0 : dinoForceChase.m_energyPerUnstoppableEnemyHit);
		}
		return text;
	}
}
