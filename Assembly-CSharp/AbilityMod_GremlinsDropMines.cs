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
		if (!(gremlinsDropMines != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GremlinsLandMineInfoComponent component = gremlinsDropMines.GetComponent<GremlinsLandMineInfoComponent>();
			if (component != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					AbilityMod.AddToken(tokens, m_mineDamageMod, "MineDamage", string.Empty, component.m_damageAmount);
					AbilityMod.AddToken(tokens, m_mineDurationMod, "MineDuration", string.Empty, component.m_mineDuration);
					AbilityMod.AddToken_EffectMod(tokens, m_effectOnEnemyOverride, "EnemyHitEffect", component.m_enemyHitEffect);
					AbilityMod.AddToken(tokens, m_energyOnMineExplosionMod, "EnergyOnExplosion", string.Empty, component.m_energyGainOnExplosion);
					return;
				}
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsDropMines gremlinsDropMines = GetTargetAbilityOnAbilityData(abilityData) as GremlinsDropMines;
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent = (!(gremlinsDropMines != null)) ? null : gremlinsDropMines.GetComponent<GremlinsLandMineInfoComponent>();
		bool flag = gremlinsLandMineInfoComponent != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt mineDamageMod = m_mineDamageMod;
		int baseVal;
		if (flag)
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
			baseVal = gremlinsLandMineInfoComponent.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(mineDamageMod, "[Mine Damage]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt mineDurationMod = m_mineDurationMod;
		int baseVal2;
		if (flag)
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
			baseVal2 = gremlinsLandMineInfoComponent.m_mineDuration;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(mineDurationMod, "[Mine Duration]", flag, baseVal2);
		empty += AbilityModHelper.GetModPropertyDesc(m_effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", flag, (!flag) ? null : gremlinsLandMineInfoComponent.m_enemyHitEffect);
		return empty + AbilityModHelper.GetModPropertyDesc(m_energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", flag, flag ? gremlinsLandMineInfoComponent.m_energyGainOnExplosion : 0);
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		if (!(abilityAsBase != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (abilityAsBase.GetType() == GetTargetAbilityType())
			{
				GremlinsDropMines gremlinsDropMines = abilityAsBase as GremlinsDropMines;
				GremlinsLandMineInfoComponent component = gremlinsDropMines.gameObject.GetComponent<GremlinsLandMineInfoComponent>();
				if (component != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						numbers.Add(m_mineDamageMod.GetModifiedValue(component.m_damageAmount));
						StandardEffectInfo modifiedValue = m_effectOnEnemyOverride.GetModifiedValue(component.m_enemyHitEffect);
						AbilityModHelper.AddTooltipNumbersFromEffect(modifiedValue, numbers);
						return;
					}
				}
				return;
			}
			return;
		}
	}
}
