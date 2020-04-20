using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ValkyriePullToLaserCenter : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserRangeInSquaresMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	public AbilityModPropertyBool m_lengthIgnoreLosMod;

	[Header("-- Damage & effects")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_extraDamageIfKnockedInPlaceMod;

	public AbilityModPropertyEffectInfo m_effectToEnemiesMod;

	[Header("-- Extra Damage for Center")]
	public AbilityModPropertyInt m_extraDamageForCenterHitsMod;

	public AbilityModPropertyFloat m_centerHitWidthMod;

	[Header("-- Knockback on Cast")]
	public AbilityModPropertyFloat m_maxKnockbackDistMod;

	public AbilityModPropertyKnockbackType m_knockbackTypeMod;

	[Header("-- Misc ability interactions")]
	public AbilityModPropertyBool m_nextTurnStabSkipsDamageReduction;

	public override Type GetTargetAbilityType()
	{
		return typeof(ValkyriePullToLaserCenter);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ValkyriePullToLaserCenter valkyriePullToLaserCenter = targetAbility as ValkyriePullToLaserCenter;
		if (valkyriePullToLaserCenter != null)
		{
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, valkyriePullToLaserCenter.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserRangeInSquaresMod, "LaserRangeInSquares", string.Empty, valkyriePullToLaserCenter.m_laserRangeInSquares, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, valkyriePullToLaserCenter.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, valkyriePullToLaserCenter.m_damage, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageIfKnockedInPlaceMod, "ExtraDamageIfKnockedInPlace", string.Empty, 0, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectToEnemiesMod, "EffectToEnemies", valkyriePullToLaserCenter.m_effectToEnemies, true);
			AbilityMod.AddToken(tokens, this.m_extraDamageForCenterHitsMod, "ExtraDamageForCenterHits", string.Empty, valkyriePullToLaserCenter.m_extraDamageForCenterHits, true, false);
			AbilityMod.AddToken(tokens, this.m_centerHitWidthMod, "CenterHitWidth", string.Empty, valkyriePullToLaserCenter.m_centerHitWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxKnockbackDistMod, "MaxKnockbackDist", string.Empty, valkyriePullToLaserCenter.m_maxKnockbackDist, true, false, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyriePullToLaserCenter valkyriePullToLaserCenter = base.GetTargetAbilityOnAbilityData(abilityData) as ValkyriePullToLaserCenter;
		bool flag = valkyriePullToLaserCenter != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix = "[LaserWidth]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = valkyriePullToLaserCenter.m_laserWidth;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(laserWidthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat laserRangeInSquaresMod = this.m_laserRangeInSquaresMod;
		string prefix2 = "[LaserRangeInSquares]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = valkyriePullToLaserCenter.m_laserRangeInSquares;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(laserRangeInSquaresMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix3 = "[MaxTargets]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = valkyriePullToLaserCenter.m_maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(maxTargetsMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_lengthIgnoreLosMod, "[LengthIgnoreLos]", flag, flag && valkyriePullToLaserCenter.m_lengthIgnoreLos);
		string str4 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix4 = "[Damage]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = valkyriePullToLaserCenter.m_damage;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(damageMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_extraDamageIfKnockedInPlaceMod, "[ExtraDamageIfKnockedInPlace]", flag, 0);
		string str5 = text;
		AbilityModPropertyEffectInfo effectToEnemiesMod = this.m_effectToEnemiesMod;
		string prefix5 = "[EffectToEnemies]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
		if (flag)
		{
			baseVal5 = valkyriePullToLaserCenter.m_effectToEnemies;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(effectToEnemiesMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_extraDamageForCenterHitsMod, "[ExtraDamageForCenterHits]", flag, (!flag) ? 0 : valkyriePullToLaserCenter.m_extraDamageForCenterHits);
		string str6 = text;
		AbilityModPropertyFloat centerHitWidthMod = this.m_centerHitWidthMod;
		string prefix6 = "[CenterHitWidth]";
		bool showBaseVal6 = flag;
		float baseVal6;
		if (flag)
		{
			baseVal6 = valkyriePullToLaserCenter.m_centerHitWidth;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(centerHitWidthMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_maxKnockbackDistMod, "[MaxKnockbackDist]", flag, (!flag) ? 0f : valkyriePullToLaserCenter.m_maxKnockbackDist);
		string str7 = text;
		AbilityModPropertyKnockbackType knockbackTypeMod = this.m_knockbackTypeMod;
		string prefix7 = "[KnockbackType]";
		bool showBaseVal7 = flag;
		KnockbackType baseVal7;
		if (flag)
		{
			baseVal7 = valkyriePullToLaserCenter.m_knockbackType;
		}
		else
		{
			baseVal7 = KnockbackType.AwayFromSource;
		}
		text = str7 + base.PropDesc(knockbackTypeMod, prefix7, showBaseVal7, baseVal7);
		return text + base.PropDesc(this.m_nextTurnStabSkipsDamageReduction, "[NextTurnStabSkipsDamageReduction]", flag, false);
	}
}
