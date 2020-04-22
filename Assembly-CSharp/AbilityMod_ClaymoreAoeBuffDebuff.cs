using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClaymoreAoeBuffDebuff : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyShape m_shapeMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- Self Heal")]
	public AbilityModPropertyInt m_baseSelfHealMod;

	public AbilityModPropertyInt m_selfHealAmountPerHitMod;

	public AbilityModPropertyBool m_selfHealCountEnemyHitMod;

	public AbilityModPropertyBool m_selfHealCountAllyHitMod;

	[Header("-- Hit Effects")]
	public AbilityModPropertyEffectInfo m_selfHitEffectMod;

	public AbilityModPropertyEffectInfo m_allyHitEffectMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	[Header("-- Energy Gain/Loss")]
	public AbilityModPropertyInt m_allyEnergyGainMod;

	public AbilityModPropertyInt m_enemyEnergyLossMod;

	public AbilityModPropertyBool m_energyChangeOnlyIfHasAdjacentMod;

	[Header("-- Cooldown Reduction When Only Hitting Self")]
	public AbilityModCooldownReduction m_cooldownReductionWhenOnlyHittingSelf;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClaymoreAoeBuffDebuff);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClaymoreAoeBuffDebuff claymoreAoeBuffDebuff = targetAbility as ClaymoreAoeBuffDebuff;
		if (!(claymoreAoeBuffDebuff != null))
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
			AbilityMod.AddToken(tokens, m_baseSelfHealMod, "BaseSelfHeal", string.Empty, claymoreAoeBuffDebuff.m_baseSelfHeal);
			AbilityMod.AddToken(tokens, m_selfHealAmountPerHitMod, "SelfHealAmountPerHit", string.Empty, claymoreAoeBuffDebuff.m_selfHealAmountPerHit);
			AbilityMod.AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", claymoreAoeBuffDebuff.m_selfHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", claymoreAoeBuffDebuff.m_allyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", claymoreAoeBuffDebuff.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_allyEnergyGainMod, "AllyEnergyGain", string.Empty, claymoreAoeBuffDebuff.m_allyEnergyGain);
			AbilityMod.AddToken(tokens, m_enemyEnergyLossMod, "EnemyEnergyLoss", string.Empty, claymoreAoeBuffDebuff.m_enemyEnergyLoss);
			if (m_cooldownReductionWhenOnlyHittingSelf != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					m_cooldownReductionWhenOnlyHittingSelf.AddTooltipTokens(tokens, "OnOnlyHitSelf");
					return;
				}
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreAoeBuffDebuff claymoreAoeBuffDebuff = GetTargetAbilityOnAbilityData(abilityData) as ClaymoreAoeBuffDebuff;
		bool flag = claymoreAoeBuffDebuff != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyShape shapeMod = m_shapeMod;
		int baseVal;
		if (flag)
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
			baseVal = (int)claymoreAoeBuffDebuff.m_shape;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(shapeMod, "[Shape]", flag, (AbilityAreaShape)baseVal);
		empty += PropDesc(m_penetrateLosMod, "[PenetrateLos]", flag, flag && claymoreAoeBuffDebuff.m_penetrateLos);
		string str2 = empty;
		AbilityModPropertyInt baseSelfHealMod = m_baseSelfHealMod;
		int baseVal2;
		if (flag)
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
			baseVal2 = claymoreAoeBuffDebuff.m_baseSelfHeal;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(baseSelfHealMod, "[BaseSelfHeal]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt selfHealAmountPerHitMod = m_selfHealAmountPerHitMod;
		int baseVal3;
		if (flag)
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
			baseVal3 = claymoreAoeBuffDebuff.m_selfHealAmountPerHit;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(selfHealAmountPerHitMod, "[SelfHealAmountPerHit]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyBool selfHealCountEnemyHitMod = m_selfHealCountEnemyHitMod;
		int baseVal4;
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
			baseVal4 = (claymoreAoeBuffDebuff.m_selfHealCountEnemyHit ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(selfHealCountEnemyHitMod, "[SelfHealCountEnemyHit]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyBool selfHealCountAllyHitMod = m_selfHealCountAllyHitMod;
		int baseVal5;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = (claymoreAoeBuffDebuff.m_selfHealCountAllyHit ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(selfHealCountAllyHitMod, "[SelfHealCountAllyHit]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyEffectInfo selfHitEffectMod = m_selfHitEffectMod;
		object baseVal6;
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
			baseVal6 = claymoreAoeBuffDebuff.m_selfHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(selfHitEffectMod, "[SelfHitEffect]", flag, (StandardEffectInfo)baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo allyHitEffectMod = m_allyHitEffectMod;
		object baseVal7;
		if (flag)
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
			baseVal7 = claymoreAoeBuffDebuff.m_allyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(allyHitEffectMod, "[AllyHitEffect]", flag, (StandardEffectInfo)baseVal7);
		string str8 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal8;
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
			baseVal8 = claymoreAoeBuffDebuff.m_enemyHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str8 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal8);
		empty += PropDesc(m_allyEnergyGainMod, "[AllyEnergyGain]", flag, flag ? claymoreAoeBuffDebuff.m_allyEnergyGain : 0);
		string str9 = empty;
		AbilityModPropertyInt enemyEnergyLossMod = m_enemyEnergyLossMod;
		int baseVal9;
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
			baseVal9 = claymoreAoeBuffDebuff.m_enemyEnergyLoss;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(enemyEnergyLossMod, "[EnemyEnergyLoss]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyBool energyChangeOnlyIfHasAdjacentMod = m_energyChangeOnlyIfHasAdjacentMod;
		int baseVal10;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal10 = (claymoreAoeBuffDebuff.m_energyChangeOnlyIfHasAdjacent ? 1 : 0);
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(energyChangeOnlyIfHasAdjacentMod, "[EnergyChangeOnlyIfHasAdjacent]", flag, (byte)baseVal10 != 0);
		if (m_cooldownReductionWhenOnlyHittingSelf.HasCooldownReduction())
		{
			empty += m_cooldownReductionWhenOnlyHittingSelf.GetDescription(abilityData);
		}
		return empty;
	}
}
