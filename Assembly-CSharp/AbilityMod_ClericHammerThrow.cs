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
		if (clericHammerThrow != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ClericHammerThrow.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_maxDistToRingCenterMod, "MaxDistToRingCenter", string.Empty, clericHammerThrow.m_maxDistToRingCenter, true, false, false);
			AbilityMod.AddToken(tokens, this.m_outerRadiusMod, "OuterRadius", string.Empty, clericHammerThrow.m_outerRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_innerRadiusMod, "InnerRadius", string.Empty, clericHammerThrow.m_innerRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_outerHitDamageMod, "OuterHitDamage", string.Empty, clericHammerThrow.m_outerHitDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_outerEnemyHitEffectMod, "OuterEnemyHitEffect", clericHammerThrow.m_outerEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_innerHitDamageMod, "InnerHitDamage", string.Empty, clericHammerThrow.m_innerHitDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_innerEnemyHitEffectMod, "InnerEnemyHitEffect", clericHammerThrow.m_innerEnemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_outerEnemyHitEffectWithNoInnerHits, "OuterEnemyHitEffectWithNoInnerHits", null, true);
			AbilityMod.AddToken(tokens, this.m_extraInnerDamagePerOuterHit, "ExtraInnerDamagePerOuterHit", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_extraTechPointGainInAreaBuff, "ExtraEnergyGainInAreaBuff", string.Empty, 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericHammerThrow clericHammerThrow = base.GetTargetAbilityOnAbilityData(abilityData) as ClericHammerThrow;
		bool flag = clericHammerThrow != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat maxDistToRingCenterMod = this.m_maxDistToRingCenterMod;
		string prefix = "[MaxDistToRingCenter]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ClericHammerThrow.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = clericHammerThrow.m_maxDistToRingCenter;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(maxDistToRingCenterMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat outerRadiusMod = this.m_outerRadiusMod;
		string prefix2 = "[OuterRadius]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = clericHammerThrow.m_outerRadius;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(outerRadiusMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat innerRadiusMod = this.m_innerRadiusMod;
		string prefix3 = "[InnerRadius]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = clericHammerThrow.m_innerRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(innerRadiusMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool ignoreLosMod = this.m_ignoreLosMod;
		string prefix4 = "[IgnoreLos]";
		bool showBaseVal4 = flag;
		bool baseVal4;
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
			baseVal4 = clericHammerThrow.m_ignoreLos;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(ignoreLosMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool clampRingToCursorPosMod = this.m_clampRingToCursorPosMod;
		string prefix5 = "[ClampRingToCursorPos]";
		bool showBaseVal5 = flag;
		bool baseVal5;
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
			baseVal5 = clericHammerThrow.m_clampRingToCursorPos;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(clampRingToCursorPosMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt outerHitDamageMod = this.m_outerHitDamageMod;
		string prefix6 = "[OuterHitDamage]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = clericHammerThrow.m_outerHitDamage;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(outerHitDamageMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo outerEnemyHitEffectMod = this.m_outerEnemyHitEffectMod;
		string prefix7 = "[OuterEnemyHitEffect]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
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
			baseVal7 = clericHammerThrow.m_outerEnemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(outerEnemyHitEffectMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt innerHitDamageMod = this.m_innerHitDamageMod;
		string prefix8 = "[InnerHitDamage]";
		bool showBaseVal8 = flag;
		int baseVal8;
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
			baseVal8 = clericHammerThrow.m_innerHitDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(innerHitDamageMod, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_innerEnemyHitEffectMod, "[InnerEnemyHitEffect]", flag, (!flag) ? null : clericHammerThrow.m_innerEnemyHitEffect);
		text += base.PropDesc(this.m_outerEnemyHitEffectWithNoInnerHits, "[OuterEnemyHitEffectWithNoInnerHits]", flag, null);
		text += base.PropDesc(this.m_extraInnerDamagePerOuterHit, "[ExtraInnerDamagePerOuterHit]", flag, 0);
		return text + base.PropDesc(this.m_extraTechPointGainInAreaBuff, "[ExtraEnergyGainInAreaBuff]", flag, 0);
	}
}
