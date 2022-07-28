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
		if (claymoreAoeBuffDebuff != null)
		{
			AddToken(tokens, m_baseSelfHealMod, "BaseSelfHeal", string.Empty, claymoreAoeBuffDebuff.m_baseSelfHeal);
			AddToken(tokens, m_selfHealAmountPerHitMod, "SelfHealAmountPerHit", string.Empty, claymoreAoeBuffDebuff.m_selfHealAmountPerHit);
			AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", claymoreAoeBuffDebuff.m_selfHitEffect);
			AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", claymoreAoeBuffDebuff.m_allyHitEffect);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", claymoreAoeBuffDebuff.m_enemyHitEffect);
			AddToken(tokens, m_allyEnergyGainMod, "AllyEnergyGain", string.Empty, claymoreAoeBuffDebuff.m_allyEnergyGain);
			AddToken(tokens, m_enemyEnergyLossMod, "EnemyEnergyLoss", string.Empty, claymoreAoeBuffDebuff.m_enemyEnergyLoss);
			if (m_cooldownReductionWhenOnlyHittingSelf != null)
			{
				m_cooldownReductionWhenOnlyHittingSelf.AddTooltipTokens(tokens, "OnOnlyHitSelf");
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreAoeBuffDebuff claymoreAoeBuffDebuff = GetTargetAbilityOnAbilityData(abilityData) as ClaymoreAoeBuffDebuff;
		bool isAbilityPresent = claymoreAoeBuffDebuff != null;
		string desc = string.Empty;
		desc += PropDesc(m_shapeMod, "[Shape]", isAbilityPresent, isAbilityPresent ? claymoreAoeBuffDebuff.m_shape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isAbilityPresent, isAbilityPresent && claymoreAoeBuffDebuff.m_penetrateLos);
		desc += PropDesc(m_baseSelfHealMod, "[BaseSelfHeal]", isAbilityPresent, isAbilityPresent ? claymoreAoeBuffDebuff.m_baseSelfHeal : 0);
		desc += PropDesc(m_selfHealAmountPerHitMod, "[SelfHealAmountPerHit]", isAbilityPresent, isAbilityPresent ? claymoreAoeBuffDebuff.m_selfHealAmountPerHit : 0);
		desc += PropDesc(m_selfHealCountEnemyHitMod, "[SelfHealCountEnemyHit]", isAbilityPresent, isAbilityPresent && claymoreAoeBuffDebuff.m_selfHealCountEnemyHit);
		desc += PropDesc(m_selfHealCountAllyHitMod, "[SelfHealCountAllyHit]", isAbilityPresent, isAbilityPresent && claymoreAoeBuffDebuff.m_selfHealCountAllyHit);
		desc += PropDesc(m_selfHitEffectMod, "[SelfHitEffect]", isAbilityPresent, isAbilityPresent ? claymoreAoeBuffDebuff.m_selfHitEffect : null);
		desc += PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", isAbilityPresent, isAbilityPresent ? claymoreAoeBuffDebuff.m_allyHitEffect : null);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isAbilityPresent, isAbilityPresent ? claymoreAoeBuffDebuff.m_enemyHitEffect : null);
		desc += PropDesc(m_allyEnergyGainMod, "[AllyEnergyGain]", isAbilityPresent, isAbilityPresent ? claymoreAoeBuffDebuff.m_allyEnergyGain : 0);
		desc += PropDesc(m_enemyEnergyLossMod, "[EnemyEnergyLoss]", isAbilityPresent, isAbilityPresent ? claymoreAoeBuffDebuff.m_enemyEnergyLoss : 0);
		desc += PropDesc(m_energyChangeOnlyIfHasAdjacentMod, "[EnergyChangeOnlyIfHasAdjacent]", isAbilityPresent, isAbilityPresent && claymoreAoeBuffDebuff.m_energyChangeOnlyIfHasAdjacent);
		if (m_cooldownReductionWhenOnlyHittingSelf.HasCooldownReduction())
		{
			desc += m_cooldownReductionWhenOnlyHittingSelf.GetDescription(abilityData);
		}
		return desc;
	}
}
