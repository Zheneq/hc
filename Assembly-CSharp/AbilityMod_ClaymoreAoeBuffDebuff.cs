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
			AbilityMod.AddToken(tokens, this.m_baseSelfHealMod, "BaseSelfHeal", string.Empty, claymoreAoeBuffDebuff.m_baseSelfHeal, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealAmountPerHitMod, "SelfHealAmountPerHit", string.Empty, claymoreAoeBuffDebuff.m_selfHealAmountPerHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfHitEffectMod, "SelfHitEffect", claymoreAoeBuffDebuff.m_selfHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectMod, "AllyHitEffect", claymoreAoeBuffDebuff.m_allyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", claymoreAoeBuffDebuff.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_allyEnergyGainMod, "AllyEnergyGain", string.Empty, claymoreAoeBuffDebuff.m_allyEnergyGain, true, false);
			AbilityMod.AddToken(tokens, this.m_enemyEnergyLossMod, "EnemyEnergyLoss", string.Empty, claymoreAoeBuffDebuff.m_enemyEnergyLoss, true, false);
			if (this.m_cooldownReductionWhenOnlyHittingSelf != null)
			{
				this.m_cooldownReductionWhenOnlyHittingSelf.AddTooltipTokens(tokens, "OnOnlyHitSelf");
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreAoeBuffDebuff claymoreAoeBuffDebuff = base.GetTargetAbilityOnAbilityData(abilityData) as ClaymoreAoeBuffDebuff;
		bool flag = claymoreAoeBuffDebuff != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyShape shapeMod = this.m_shapeMod;
		string prefix = "[Shape]";
		bool showBaseVal = flag;
		AbilityAreaShape baseVal;
		if (flag)
		{
			baseVal = claymoreAoeBuffDebuff.m_shape;
		}
		else
		{
			baseVal = AbilityAreaShape.SingleSquare;
		}
		text = str + base.PropDesc(shapeMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_penetrateLosMod, "[PenetrateLos]", flag, flag && claymoreAoeBuffDebuff.m_penetrateLos);
		string str2 = text;
		AbilityModPropertyInt baseSelfHealMod = this.m_baseSelfHealMod;
		string prefix2 = "[BaseSelfHeal]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = claymoreAoeBuffDebuff.m_baseSelfHeal;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(baseSelfHealMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt selfHealAmountPerHitMod = this.m_selfHealAmountPerHitMod;
		string prefix3 = "[SelfHealAmountPerHit]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = claymoreAoeBuffDebuff.m_selfHealAmountPerHit;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(selfHealAmountPerHitMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool selfHealCountEnemyHitMod = this.m_selfHealCountEnemyHitMod;
		string prefix4 = "[SelfHealCountEnemyHit]";
		bool showBaseVal4 = flag;
		bool baseVal4;
		if (flag)
		{
			baseVal4 = claymoreAoeBuffDebuff.m_selfHealCountEnemyHit;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(selfHealCountEnemyHitMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool selfHealCountAllyHitMod = this.m_selfHealCountAllyHitMod;
		string prefix5 = "[SelfHealCountAllyHit]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = claymoreAoeBuffDebuff.m_selfHealCountAllyHit;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(selfHealCountAllyHitMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo selfHitEffectMod = this.m_selfHitEffectMod;
		string prefix6 = "[SelfHitEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = claymoreAoeBuffDebuff.m_selfHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(selfHitEffectMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo allyHitEffectMod = this.m_allyHitEffectMod;
		string prefix7 = "[AllyHitEffect]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = claymoreAoeBuffDebuff.m_allyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(allyHitEffectMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix8 = "[EnemyHitEffect]";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
		if (flag)
		{
			baseVal8 = claymoreAoeBuffDebuff.m_enemyHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + base.PropDesc(enemyHitEffectMod, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_allyEnergyGainMod, "[AllyEnergyGain]", flag, (!flag) ? 0 : claymoreAoeBuffDebuff.m_allyEnergyGain);
		string str9 = text;
		AbilityModPropertyInt enemyEnergyLossMod = this.m_enemyEnergyLossMod;
		string prefix9 = "[EnemyEnergyLoss]";
		bool showBaseVal9 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = claymoreAoeBuffDebuff.m_enemyEnergyLoss;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(enemyEnergyLossMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyBool energyChangeOnlyIfHasAdjacentMod = this.m_energyChangeOnlyIfHasAdjacentMod;
		string prefix10 = "[EnergyChangeOnlyIfHasAdjacent]";
		bool showBaseVal10 = flag;
		bool baseVal10;
		if (flag)
		{
			baseVal10 = claymoreAoeBuffDebuff.m_energyChangeOnlyIfHasAdjacent;
		}
		else
		{
			baseVal10 = false;
		}
		text = str10 + base.PropDesc(energyChangeOnlyIfHasAdjacentMod, prefix10, showBaseVal10, baseVal10);
		if (this.m_cooldownReductionWhenOnlyHittingSelf.HasCooldownReduction())
		{
			text += this.m_cooldownReductionWhenOnlyHittingSelf.GetDescription(abilityData);
		}
		return text;
	}
}
