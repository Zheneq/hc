using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken(tokens, m_baseSelfHealMod, "BaseSelfHeal", string.Empty, rampartAoeBuffDebuff.m_baseSelfHeal);
			AddToken(tokens, m_selfHealAmountPerHitMod, "SelfHealAmountPerHit", string.Empty, rampartAoeBuffDebuff.m_selfHealAmountPerHit);
			AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", rampartAoeBuffDebuff.m_selfHitEffect);
			AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", rampartAoeBuffDebuff.m_allyHitEffect);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", rampartAoeBuffDebuff.m_enemyHitEffect);
			AddToken(tokens, m_damageToEnemiesMod, "DamageToEnemies", string.Empty, rampartAoeBuffDebuff.m_damageToEnemies);
			AddToken(tokens, m_healingToAlliesMod, "HealingToAllies", string.Empty, rampartAoeBuffDebuff.m_healingToAllies);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartAoeBuffDebuff rampartAoeBuffDebuff = GetTargetAbilityOnAbilityData(abilityData) as RampartAoeBuffDebuff;
		bool isAbilityPresent = rampartAoeBuffDebuff != null;
		string desc = string.Empty;
		desc += PropDesc(m_shapeMod, "[Shape]", isAbilityPresent, isAbilityPresent ? rampartAoeBuffDebuff.m_shape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isAbilityPresent, isAbilityPresent && rampartAoeBuffDebuff.m_penetrateLos);
		desc += PropDesc(m_baseSelfHealMod, "[BaseSelfHeal]", isAbilityPresent, isAbilityPresent ? rampartAoeBuffDebuff.m_baseSelfHeal : 0);
		desc += PropDesc(m_selfHealAmountPerHitMod, "[SelfHealAmountPerHit]", isAbilityPresent, isAbilityPresent ? rampartAoeBuffDebuff.m_selfHealAmountPerHit : 0);
		desc += PropDesc(m_selfHealCountEnemyHitMod, "[SelfHealCountEnemyHit]", isAbilityPresent, isAbilityPresent && rampartAoeBuffDebuff.m_selfHealCountEnemyHit);
		desc += PropDesc(m_selfHealCountAllyHitMod, "[SelfHealCountAllyHit]", isAbilityPresent, isAbilityPresent && rampartAoeBuffDebuff.m_selfHealCountAllyHit);
		desc += PropDesc(m_includeCasterMod, "[IncludeCaster]", isAbilityPresent, isAbilityPresent && rampartAoeBuffDebuff.m_includeCaster);
		desc += PropDesc(m_selfHitEffectMod, "[SelfHitEffect]", isAbilityPresent, isAbilityPresent ? rampartAoeBuffDebuff.m_selfHitEffect : null);
		desc += PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", isAbilityPresent, isAbilityPresent ? rampartAoeBuffDebuff.m_allyHitEffect : null);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isAbilityPresent, isAbilityPresent ? rampartAoeBuffDebuff.m_enemyHitEffect : null);
		desc += PropDesc(m_damageToEnemiesMod, "[DamageToEnemies]", isAbilityPresent, isAbilityPresent ? rampartAoeBuffDebuff.m_damageToEnemies : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_healingToAlliesMod, "[HealingToAllies]", isAbilityPresent, isAbilityPresent ? rampartAoeBuffDebuff.m_healingToAllies : 0)).ToString();
	}
}
