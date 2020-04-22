using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RampartAoeBuffDebuff : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyShape m_shapeMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- Self Heal Per Hit")]
	public AbilityModPropertyInt m_baseSelfHealMod;

	public AbilityModPropertyInt m_selfHealAmountPerHitMod;

	public AbilityModPropertyBool m_selfHealCountEnemyHitMod;

	public AbilityModPropertyBool m_selfHealCountAllyHitMod;

	[Header("-- Normal Hit Effects")]
	public AbilityModPropertyBool m_includeCasterMod;

	public AbilityModPropertyEffectInfo m_selfHitEffectMod;

	public AbilityModPropertyEffectInfo m_allyHitEffectMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	public AbilityModPropertyInt m_damageToEnemiesMod;

	public AbilityModPropertyInt m_healingToAlliesMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(RampartAoeBuffDebuff);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RampartAoeBuffDebuff rampartAoeBuffDebuff = targetAbility as RampartAoeBuffDebuff;
		if (!(rampartAoeBuffDebuff != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_baseSelfHealMod, "BaseSelfHeal", string.Empty, rampartAoeBuffDebuff.m_baseSelfHeal);
			AbilityMod.AddToken(tokens, m_selfHealAmountPerHitMod, "SelfHealAmountPerHit", string.Empty, rampartAoeBuffDebuff.m_selfHealAmountPerHit);
			AbilityMod.AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", rampartAoeBuffDebuff.m_selfHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", rampartAoeBuffDebuff.m_allyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", rampartAoeBuffDebuff.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_damageToEnemiesMod, "DamageToEnemies", string.Empty, rampartAoeBuffDebuff.m_damageToEnemies);
			AbilityMod.AddToken(tokens, m_healingToAlliesMod, "HealingToAllies", string.Empty, rampartAoeBuffDebuff.m_healingToAllies);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartAoeBuffDebuff rampartAoeBuffDebuff = GetTargetAbilityOnAbilityData(abilityData) as RampartAoeBuffDebuff;
		bool flag = rampartAoeBuffDebuff != null;
		string empty = string.Empty;
		empty += PropDesc(m_shapeMod, "[Shape]", flag, flag ? rampartAoeBuffDebuff.m_shape : AbilityAreaShape.SingleSquare);
		string str = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (1)
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
			baseVal = (rampartAoeBuffDebuff.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(penetrateLosMod, "[PenetrateLos]", flag, (byte)baseVal != 0);
		empty += PropDesc(m_baseSelfHealMod, "[BaseSelfHeal]", flag, flag ? rampartAoeBuffDebuff.m_baseSelfHeal : 0);
		string str2 = empty;
		AbilityModPropertyInt selfHealAmountPerHitMod = m_selfHealAmountPerHitMod;
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
			baseVal2 = rampartAoeBuffDebuff.m_selfHealAmountPerHit;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(selfHealAmountPerHitMod, "[SelfHealAmountPerHit]", flag, baseVal2);
		empty += PropDesc(m_selfHealCountEnemyHitMod, "[SelfHealCountEnemyHit]", flag, flag && rampartAoeBuffDebuff.m_selfHealCountEnemyHit);
		string str3 = empty;
		AbilityModPropertyBool selfHealCountAllyHitMod = m_selfHealCountAllyHitMod;
		int baseVal3;
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
			baseVal3 = (rampartAoeBuffDebuff.m_selfHealCountAllyHit ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(selfHealCountAllyHitMod, "[SelfHealCountAllyHit]", flag, (byte)baseVal3 != 0);
		empty += PropDesc(m_includeCasterMod, "[IncludeCaster]", flag, flag && rampartAoeBuffDebuff.m_includeCaster);
		string str4 = empty;
		AbilityModPropertyEffectInfo selfHitEffectMod = m_selfHitEffectMod;
		object baseVal4;
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
			baseVal4 = rampartAoeBuffDebuff.m_selfHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + PropDesc(selfHitEffectMod, "[SelfHitEffect]", flag, (StandardEffectInfo)baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo allyHitEffectMod = m_allyHitEffectMod;
		object baseVal5;
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
			baseVal5 = rampartAoeBuffDebuff.m_allyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(allyHitEffectMod, "[AllyHitEffect]", flag, (StandardEffectInfo)baseVal5);
		empty += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", flag, (!flag) ? null : rampartAoeBuffDebuff.m_enemyHitEffect);
		empty += PropDesc(m_damageToEnemiesMod, "[DamageToEnemies]", flag, flag ? rampartAoeBuffDebuff.m_damageToEnemies : 0);
		return empty + PropDesc(m_healingToAlliesMod, "[HealingToAllies]", flag, flag ? rampartAoeBuffDebuff.m_healingToAllies : 0);
	}
}
