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
		if (rampartAoeBuffDebuff != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RampartAoeBuffDebuff.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_baseSelfHealMod, "BaseSelfHeal", string.Empty, rampartAoeBuffDebuff.m_baseSelfHeal, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealAmountPerHitMod, "SelfHealAmountPerHit", string.Empty, rampartAoeBuffDebuff.m_selfHealAmountPerHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfHitEffectMod, "SelfHitEffect", rampartAoeBuffDebuff.m_selfHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectMod, "AllyHitEffect", rampartAoeBuffDebuff.m_allyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", rampartAoeBuffDebuff.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_damageToEnemiesMod, "DamageToEnemies", string.Empty, rampartAoeBuffDebuff.m_damageToEnemies, true, false);
			AbilityMod.AddToken(tokens, this.m_healingToAlliesMod, "HealingToAllies", string.Empty, rampartAoeBuffDebuff.m_healingToAllies, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartAoeBuffDebuff rampartAoeBuffDebuff = base.GetTargetAbilityOnAbilityData(abilityData) as RampartAoeBuffDebuff;
		bool flag = rampartAoeBuffDebuff != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_shapeMod, "[Shape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : rampartAoeBuffDebuff.m_shape);
		string str = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix = "[PenetrateLos]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RampartAoeBuffDebuff.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = rampartAoeBuffDebuff.m_penetrateLos;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(penetrateLosMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_baseSelfHealMod, "[BaseSelfHeal]", flag, (!flag) ? 0 : rampartAoeBuffDebuff.m_baseSelfHeal);
		string str2 = text;
		AbilityModPropertyInt selfHealAmountPerHitMod = this.m_selfHealAmountPerHitMod;
		string prefix2 = "[SelfHealAmountPerHit]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			for (;;)
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
		text = str2 + base.PropDesc(selfHealAmountPerHitMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_selfHealCountEnemyHitMod, "[SelfHealCountEnemyHit]", flag, flag && rampartAoeBuffDebuff.m_selfHealCountEnemyHit);
		string str3 = text;
		AbilityModPropertyBool selfHealCountAllyHitMod = this.m_selfHealCountAllyHitMod;
		string prefix3 = "[SelfHealCountAllyHit]";
		bool showBaseVal3 = flag;
		bool baseVal3;
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
			baseVal3 = rampartAoeBuffDebuff.m_selfHealCountAllyHit;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(selfHealCountAllyHitMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_includeCasterMod, "[IncludeCaster]", flag, flag && rampartAoeBuffDebuff.m_includeCaster);
		string str4 = text;
		AbilityModPropertyEffectInfo selfHitEffectMod = this.m_selfHitEffectMod;
		string prefix4 = "[SelfHitEffect]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
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
			baseVal4 = rampartAoeBuffDebuff.m_selfHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + base.PropDesc(selfHitEffectMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo allyHitEffectMod = this.m_allyHitEffectMod;
		string prefix5 = "[AllyHitEffect]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
		if (flag)
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
			baseVal5 = rampartAoeBuffDebuff.m_allyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(allyHitEffectMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_enemyHitEffectMod, "[EnemyHitEffect]", flag, (!flag) ? null : rampartAoeBuffDebuff.m_enemyHitEffect);
		text += base.PropDesc(this.m_damageToEnemiesMod, "[DamageToEnemies]", flag, (!flag) ? 0 : rampartAoeBuffDebuff.m_damageToEnemies);
		return text + base.PropDesc(this.m_healingToAlliesMod, "[HealingToAllies]", flag, (!flag) ? 0 : rampartAoeBuffDebuff.m_healingToAllies);
	}
}
