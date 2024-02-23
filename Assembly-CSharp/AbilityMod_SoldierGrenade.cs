using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, soldierGrenade.m_damageAmount);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", soldierGrenade.m_enemyHitEffect);
			AddToken(tokens, m_allyHealAmountMod, "AllyHealAmount", string.Empty, soldierGrenade.m_allyHealAmount);
			AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", soldierGrenade.m_allyHitEffect);
			if (m_useAdditionalShapeOverride && m_additionalShapeToDamageOverride != null)
			{
				foreach (SoldierGrenade.ShapeToDamage shapeToDamage in m_additionalShapeToDamageOverride)
				{
					AddToken_IntDiff(
						tokens,
						new StringBuilder().Append(shapeToDamage.m_shape).Append("_Damage").ToString(),
						string.Empty,
						shapeToDamage.m_damage,
						true,
						soldierGrenade.m_damageAmount);
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierGrenade soldierGrenade = GetTargetAbilityOnAbilityData(abilityData) as SoldierGrenade;
		bool isValid = soldierGrenade != null;
		string desc = string.Empty;
		desc += PropDesc(m_shapeMod, "[Shape]", isValid, isValid ? soldierGrenade.m_shape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isValid, isValid && soldierGrenade.m_penetrateLos);
		if (m_useAdditionalShapeOverride && m_additionalShapeToDamageOverride != null)
		{
			desc += "Using Layered Shape Override, entries:\n";
			foreach (SoldierGrenade.ShapeToDamage shapeToDamage in m_additionalShapeToDamageOverride)
			{
				desc += new StringBuilder().Append("Shape: ")
					.Append(shapeToDamage.m_shape)
					.Append(" Damage: ")
					.Append(AbilityModHelper.GetDiffString(shapeToDamage.m_damage, isValid ? soldierGrenade.m_damageAmount : 0,
						AbilityModPropertyInt.ModOp.Override))
					.Append("\n")
					.ToString();
			}
		}
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? soldierGrenade.m_damageAmount : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? soldierGrenade.m_enemyHitEffect : null);
		desc += PropDesc(m_allyHealAmountMod, "[AllyHealAmount]", isValid, isValid ? soldierGrenade.m_allyHealAmount : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", isValid, isValid ? soldierGrenade.m_allyHitEffect : null)).ToString();
	}
}
