using System;
using System.Collections.Generic;

public class AbilityMod_ClericHammerThrow : AbilityMod
{
	[Separator("Targeting", true)]
	public AbilityModPropertyFloat m_maxDistToRingCenterMod;

	public AbilityModPropertyFloat m_outerRadiusMod;

	public AbilityModPropertyFloat m_innerRadiusMod;

	public AbilityModPropertyBool m_ignoreLosMod;

	public AbilityModPropertyBool m_clampRingToCursorPosMod;

	[Separator("On Hit", true)]
	public AbilityModPropertyInt m_outerHitDamageMod;

	public AbilityModPropertyEffectInfo m_outerEnemyHitEffectMod;

	public AbilityModPropertyInt m_innerHitDamageMod;

	public AbilityModPropertyEffectInfo m_innerEnemyHitEffectMod;

	public AbilityModPropertyEffectInfo m_outerEnemyHitEffectWithNoInnerHits;

	public AbilityModPropertyInt m_extraInnerDamagePerOuterHit;

	public AbilityModPropertyInt m_extraTechPointGainInAreaBuff;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClericHammerThrow);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClericHammerThrow clericHammerThrow = targetAbility as ClericHammerThrow;
		if (!(clericHammerThrow != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_maxDistToRingCenterMod, "MaxDistToRingCenter", string.Empty, clericHammerThrow.m_maxDistToRingCenter);
			AbilityMod.AddToken(tokens, m_outerRadiusMod, "OuterRadius", string.Empty, clericHammerThrow.m_outerRadius);
			AbilityMod.AddToken(tokens, m_innerRadiusMod, "InnerRadius", string.Empty, clericHammerThrow.m_innerRadius);
			AbilityMod.AddToken(tokens, m_outerHitDamageMod, "OuterHitDamage", string.Empty, clericHammerThrow.m_outerHitDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_outerEnemyHitEffectMod, "OuterEnemyHitEffect", clericHammerThrow.m_outerEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_innerHitDamageMod, "InnerHitDamage", string.Empty, clericHammerThrow.m_innerHitDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_innerEnemyHitEffectMod, "InnerEnemyHitEffect", clericHammerThrow.m_innerEnemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_outerEnemyHitEffectWithNoInnerHits, "OuterEnemyHitEffectWithNoInnerHits");
			AbilityMod.AddToken(tokens, m_extraInnerDamagePerOuterHit, "ExtraInnerDamagePerOuterHit", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_extraTechPointGainInAreaBuff, "ExtraEnergyGainInAreaBuff", string.Empty, 0);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericHammerThrow clericHammerThrow = GetTargetAbilityOnAbilityData(abilityData) as ClericHammerThrow;
		bool flag = clericHammerThrow != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat maxDistToRingCenterMod = m_maxDistToRingCenterMod;
		float baseVal;
		if (flag)
		{
			baseVal = clericHammerThrow.m_maxDistToRingCenter;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(maxDistToRingCenterMod, "[MaxDistToRingCenter]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat outerRadiusMod = m_outerRadiusMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = clericHammerThrow.m_outerRadius;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(outerRadiusMod, "[OuterRadius]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat innerRadiusMod = m_innerRadiusMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = clericHammerThrow.m_innerRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(innerRadiusMod, "[InnerRadius]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyBool ignoreLosMod = m_ignoreLosMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = (clericHammerThrow.m_ignoreLos ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(ignoreLosMod, "[IgnoreLos]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyBool clampRingToCursorPosMod = m_clampRingToCursorPosMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (clericHammerThrow.m_clampRingToCursorPos ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(clampRingToCursorPosMod, "[ClampRingToCursorPos]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyInt outerHitDamageMod = m_outerHitDamageMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = clericHammerThrow.m_outerHitDamage;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(outerHitDamageMod, "[OuterHitDamage]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo outerEnemyHitEffectMod = m_outerEnemyHitEffectMod;
		object baseVal7;
		if (flag)
		{
			baseVal7 = clericHammerThrow.m_outerEnemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(outerEnemyHitEffectMod, "[OuterEnemyHitEffect]", flag, (StandardEffectInfo)baseVal7);
		string str8 = empty;
		AbilityModPropertyInt innerHitDamageMod = m_innerHitDamageMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = clericHammerThrow.m_innerHitDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(innerHitDamageMod, "[InnerHitDamage]", flag, baseVal8);
		empty += PropDesc(m_innerEnemyHitEffectMod, "[InnerEnemyHitEffect]", flag, (!flag) ? null : clericHammerThrow.m_innerEnemyHitEffect);
		empty += PropDesc(m_outerEnemyHitEffectWithNoInnerHits, "[OuterEnemyHitEffectWithNoInnerHits]", flag);
		empty += PropDesc(m_extraInnerDamagePerOuterHit, "[ExtraInnerDamagePerOuterHit]", flag);
		return empty + PropDesc(m_extraTechPointGainInAreaBuff, "[ExtraEnergyGainInAreaBuff]", flag);
	}
}
