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
		if (!(soldierGrenade != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, soldierGrenade.m_damageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", soldierGrenade.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_allyHealAmountMod, "AllyHealAmount", string.Empty, soldierGrenade.m_allyHealAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", soldierGrenade.m_allyHitEffect);
			if (!m_useAdditionalShapeOverride || m_additionalShapeToDamageOverride == null)
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
				for (int i = 0; i < m_additionalShapeToDamageOverride.Count; i++)
				{
					SoldierGrenade.ShapeToDamage shapeToDamage = m_additionalShapeToDamageOverride[i];
					AbilityMod.AddToken_IntDiff(tokens, shapeToDamage.m_shape.ToString() + "_Damage", string.Empty, shapeToDamage.m_damage, true, soldierGrenade.m_damageAmount);
				}
				return;
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierGrenade soldierGrenade = GetTargetAbilityOnAbilityData(abilityData) as SoldierGrenade;
		bool flag = soldierGrenade != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyShape shapeMod = m_shapeMod;
		int baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = (int)soldierGrenade.m_shape;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(shapeMod, "[Shape]", flag, (AbilityAreaShape)baseVal);
		empty += PropDesc(m_penetrateLosMod, "[PenetrateLos]", flag, flag && soldierGrenade.m_penetrateLos);
		if (m_useAdditionalShapeOverride)
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
			if (m_additionalShapeToDamageOverride != null)
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
				empty += "Using Layered Shape Override, entries:\n";
				for (int i = 0; i < m_additionalShapeToDamageOverride.Count; i++)
				{
					string text = empty;
					object[] obj = new object[6]
					{
						text,
						"Shape: ",
						m_additionalShapeToDamageOverride[i].m_shape,
						" Damage: ",
						null,
						null
					};
					int damage = m_additionalShapeToDamageOverride[i].m_damage;
					int baseVal2;
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
						baseVal2 = soldierGrenade.m_damageAmount;
					}
					else
					{
						baseVal2 = 0;
					}
					obj[4] = AbilityModHelper.GetDiffString(damage, baseVal2, AbilityModPropertyInt.ModOp.Override);
					obj[5] = "\n";
					empty = string.Concat(obj);
				}
				while (true)
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
		string str2 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal3;
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
			baseVal3 = soldierGrenade.m_damageAmount;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str2 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal3);
		string str3 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
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
			baseVal4 = soldierGrenade.m_enemyHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str3 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal4);
		string str4 = empty;
		AbilityModPropertyInt allyHealAmountMod = m_allyHealAmountMod;
		int baseVal5;
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
			baseVal5 = soldierGrenade.m_allyHealAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str4 + PropDesc(allyHealAmountMod, "[AllyHealAmount]", flag, baseVal5);
		string str5 = empty;
		AbilityModPropertyEffectInfo allyHitEffectMod = m_allyHitEffectMod;
		object baseVal6;
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
			baseVal6 = soldierGrenade.m_allyHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		return str5 + PropDesc(allyHitEffectMod, "[AllyHitEffect]", flag, (StandardEffectInfo)baseVal6);
	}
}
