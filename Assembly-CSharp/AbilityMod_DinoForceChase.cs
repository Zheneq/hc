using System;
using System.Collections.Generic;

public class AbilityMod_DinoForceChase : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod")]
	public TargetSelectMod_Cone m_targetSelMod;
	[Separator("Cooldown reduction on knockback ability")]
	public AbilityModPropertyInt m_cdrOnKnockbackAbilityMod;
	[Separator("Energy Per Unstoppable Enemy")]
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
		if (dinoForceChase != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddToken(tokens, m_cdrOnKnockbackAbilityMod, "CdrOnKnockbackAbility", string.Empty, dinoForceChase.m_cdrOnKnockbackAbility);
			AddToken(tokens, m_energyPerUnstoppableEnemyHitMod, "EnergyPerUnstoppableEnemyHit", string.Empty, dinoForceChase.m_energyPerUnstoppableEnemyHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoForceChase dinoForceChase = GetTargetAbilityOnAbilityData(abilityData) as DinoForceChase;
		bool isValid = dinoForceChase != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		if (dinoForceChase != null)
		{
			desc += GetTargetSelectModDesc(m_targetSelMod, dinoForceChase.m_targetSelectComp);
			desc += PropDesc(m_cdrOnKnockbackAbilityMod, "[CdrOnKnockbackAbility]", isValid, isValid ? dinoForceChase.m_cdrOnKnockbackAbility : 0);
			desc += PropDesc(m_energyPerUnstoppableEnemyHitMod, "[EnergyPerUnstoppableEnemyHit]", isValid, isValid ? dinoForceChase.m_energyPerUnstoppableEnemyHit : 0);
		}
		return desc;
	}
}
