using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_GremlinsDropMines : AbilityMod
{
	[Space(10f)]
	public AbilityModPropertyInt m_mineDamageMod;
	public AbilityModPropertyInt m_mineDurationMod;
	public AbilityModPropertyEffectInfo m_effectOnEnemyOverride;
	public AbilityModPropertyInt m_energyOnMineExplosionMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(GremlinsDropMines);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		GremlinsDropMines gremlinsDropMines = targetAbility as GremlinsDropMines;
		if (gremlinsDropMines != null)
		{
			GremlinsLandMineInfoComponent component = gremlinsDropMines.GetComponent<GremlinsLandMineInfoComponent>();
			if (component != null)
			{
				AddToken(tokens, m_mineDamageMod, "MineDamage", string.Empty, component.m_damageAmount);
				AddToken(tokens, m_mineDurationMod, "MineDuration", string.Empty, component.m_mineDuration);
				AddToken_EffectMod(tokens, m_effectOnEnemyOverride, "EnemyHitEffect", component.m_enemyHitEffect);
				AddToken(tokens, m_energyOnMineExplosionMod, "EnergyOnExplosion", string.Empty, component.m_energyGainOnExplosion);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsDropMines gremlinsDropMines = GetTargetAbilityOnAbilityData(abilityData) as GremlinsDropMines;
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent = gremlinsDropMines != null
			? gremlinsDropMines.GetComponent<GremlinsLandMineInfoComponent>()
			: null;
		bool isAbilityPresent = gremlinsLandMineInfoComponent != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_mineDamageMod, "[Mine Damage]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_mineDurationMod, "[Mine Duration]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_mineDuration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_enemyHitEffect : null);
		return desc + AbilityModHelper.GetModPropertyDesc(m_energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_energyGainOnExplosion : 0);
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		if (abilityAsBase != null && abilityAsBase.GetType() == GetTargetAbilityType())
		{
			GremlinsDropMines gremlinsDropMines = abilityAsBase as GremlinsDropMines;
			GremlinsLandMineInfoComponent component = gremlinsDropMines.gameObject.GetComponent<GremlinsLandMineInfoComponent>();
			if (component != null)
			{
				numbers.Add(m_mineDamageMod.GetModifiedValue(component.m_damageAmount));
				StandardEffectInfo modifiedValue = m_effectOnEnemyOverride.GetModifiedValue(component.m_enemyHitEffect);
				AbilityModHelper.AddTooltipNumbersFromEffect(modifiedValue, numbers);
			}
		}
	}
}
