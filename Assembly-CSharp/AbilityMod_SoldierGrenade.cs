using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SoldierGrenade : AbilityMod
{
	[Header("-- Targeting --")]
	public AbilityModPropertyShape m_shapeMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- Layered Shape Override")]
	public bool m_useAdditionalShapeOverride;

	public List<SoldierGrenade.ShapeToDamage> m_additionalShapeToDamageOverride = new List<SoldierGrenade.ShapeToDamage>();

	[Header("-- On Hit Stuff --")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	[Space(10f)]
	public AbilityModPropertyInt m_allyHealAmountMod;

	public AbilityModPropertyEffectInfo m_allyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SoldierGrenade);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SoldierGrenade soldierGrenade = targetAbility as SoldierGrenade;
		if (soldierGrenade != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SoldierGrenade.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, soldierGrenade.m_damageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", soldierGrenade.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_allyHealAmountMod, "AllyHealAmount", string.Empty, soldierGrenade.m_allyHealAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectMod, "AllyHitEffect", soldierGrenade.m_allyHitEffect, true);
			if (this.m_useAdditionalShapeOverride && this.m_additionalShapeToDamageOverride != null)
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
				for (int i = 0; i < this.m_additionalShapeToDamageOverride.Count; i++)
				{
					SoldierGrenade.ShapeToDamage shapeToDamage = this.m_additionalShapeToDamageOverride[i];
					AbilityMod.AddToken_IntDiff(tokens, shapeToDamage.m_shape.ToString() + "_Damage", string.Empty, shapeToDamage.m_damage, true, soldierGrenade.m_damageAmount);
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierGrenade soldierGrenade = base.GetTargetAbilityOnAbilityData(abilityData) as SoldierGrenade;
		bool flag = soldierGrenade != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyShape shapeMod = this.m_shapeMod;
		string prefix = "[Shape]";
		bool showBaseVal = flag;
		AbilityAreaShape baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SoldierGrenade.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = soldierGrenade.m_shape;
		}
		else
		{
			baseVal = AbilityAreaShape.SingleSquare;
		}
		text = str + base.PropDesc(shapeMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_penetrateLosMod, "[PenetrateLos]", flag, flag && soldierGrenade.m_penetrateLos);
		if (this.m_useAdditionalShapeOverride)
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
			if (this.m_additionalShapeToDamageOverride != null)
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
				text += "Using Layered Shape Override, entries:\n";
				for (int i = 0; i < this.m_additionalShapeToDamageOverride.Count; i++)
				{
					string text2 = text;
					object[] array = new object[6];
					array[0] = text2;
					array[1] = "Shape: ";
					array[2] = this.m_additionalShapeToDamageOverride[i].m_shape;
					array[3] = " Damage: ";
					int num = 4;
					int damage = this.m_additionalShapeToDamageOverride[i].m_damage;
					int baseVal2;
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
						baseVal2 = soldierGrenade.m_damageAmount;
					}
					else
					{
						baseVal2 = 0;
					}
					array[num] = AbilityModHelper.GetDiffString(damage, baseVal2, AbilityModPropertyInt.ModOp.Override);
					array[5] = "\n";
					text = string.Concat(array);
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		string str2 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix2 = "[DamageAmount]";
		bool showBaseVal2 = flag;
		int baseVal3;
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = soldierGrenade.m_damageAmount;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str2 + base.PropDesc(damageAmountMod, prefix2, showBaseVal2, baseVal3);
		string str3 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix3 = "[EnemyHitEffect]";
		bool showBaseVal3 = flag;
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
			baseVal4 = soldierGrenade.m_enemyHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		text = str3 + base.PropDesc(enemyHitEffectMod, prefix3, showBaseVal3, baseVal4);
		string str4 = text;
		AbilityModPropertyInt allyHealAmountMod = this.m_allyHealAmountMod;
		string prefix4 = "[AllyHealAmount]";
		bool showBaseVal4 = flag;
		int baseVal5;
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
			baseVal5 = soldierGrenade.m_allyHealAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str4 + base.PropDesc(allyHealAmountMod, prefix4, showBaseVal4, baseVal5);
		string str5 = text;
		AbilityModPropertyEffectInfo allyHitEffectMod = this.m_allyHitEffectMod;
		string prefix5 = "[AllyHitEffect]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal6;
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
			baseVal6 = soldierGrenade.m_allyHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		return str5 + base.PropDesc(allyHitEffectMod, prefix5, showBaseVal5, baseVal6);
	}
}
