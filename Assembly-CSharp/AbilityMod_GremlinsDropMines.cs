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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_GremlinsDropMines.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			GremlinsLandMineInfoComponent component = gremlinsDropMines.GetComponent<GremlinsLandMineInfoComponent>();
			if (component != null)
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
				AbilityMod.AddToken(tokens, this.m_mineDamageMod, "MineDamage", string.Empty, component.m_damageAmount, true, false);
				AbilityMod.AddToken(tokens, this.m_mineDurationMod, "MineDuration", string.Empty, component.m_mineDuration, true, false);
				AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnEnemyOverride, "EnemyHitEffect", component.m_enemyHitEffect, true);
				AbilityMod.AddToken(tokens, this.m_energyOnMineExplosionMod, "EnergyOnExplosion", string.Empty, component.m_energyGainOnExplosion, true, false);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsDropMines gremlinsDropMines = base.GetTargetAbilityOnAbilityData(abilityData) as GremlinsDropMines;
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent = (!(gremlinsDropMines != null)) ? null : gremlinsDropMines.GetComponent<GremlinsLandMineInfoComponent>();
		bool flag = gremlinsLandMineInfoComponent != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt mineDamageMod = this.m_mineDamageMod;
		string prefix = "[Mine Damage]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_GremlinsDropMines.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = gremlinsLandMineInfoComponent.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(mineDamageMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt mineDurationMod = this.m_mineDurationMod;
		string prefix2 = "[Mine Duration]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
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
			baseVal2 = gremlinsLandMineInfoComponent.m_mineDuration;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(mineDurationMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModPropertyDesc(this.m_effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", flag, (!flag) ? null : gremlinsLandMineInfoComponent.m_enemyHitEffect);
		return text + AbilityModHelper.GetModPropertyDesc(this.m_energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", flag, (!flag) ? 0 : gremlinsLandMineInfoComponent.m_energyGainOnExplosion);
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		if (abilityAsBase != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_GremlinsDropMines.AppendModSpecificTooltipCheckNumbers(Ability, List<int>)).MethodHandle;
			}
			if (abilityAsBase.GetType() == this.GetTargetAbilityType())
			{
				GremlinsDropMines gremlinsDropMines = abilityAsBase as GremlinsDropMines;
				GremlinsLandMineInfoComponent component = gremlinsDropMines.gameObject.GetComponent<GremlinsLandMineInfoComponent>();
				if (component != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					numbers.Add(this.m_mineDamageMod.GetModifiedValue(component.m_damageAmount));
					StandardEffectInfo modifiedValue = this.m_effectOnEnemyOverride.GetModifiedValue(component.m_enemyHitEffect);
					AbilityModHelper.AddTooltipNumbersFromEffect(modifiedValue, numbers);
				}
			}
		}
	}
}
