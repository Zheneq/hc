using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RageBeastBasicAttack : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_coneAngleMod;

	public AbilityModPropertyFloat m_coneInnerRadiusMod;

	public AbilityModPropertyFloat m_coneOuterRadiusMod;

	[Header("-- Damage")]
	public AbilityModPropertyInt m_innerDamageMod;

	public AbilityModPropertyInt m_outerDamageMod;

	public int m_extraDamagePerAdjacentEnemy;

	[Header("-- Effect")]
	public AbilityModPropertyEffectInfo m_effectInnerMod;

	public AbilityModPropertyEffectInfo m_effectOuterMod;

	[Header("-- Tech Point change on Hit")]
	public AbilityModPropertyInt m_innerTpGain;

	public AbilityModPropertyInt m_outerTpGain;

	public int m_extraTechPointsPerAdjacentEnemy;

	public override Type GetTargetAbilityType()
	{
		return typeof(RageBeastBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RageBeastBasicAttack rageBeastBasicAttack = targetAbility as RageBeastBasicAttack;
		if (rageBeastBasicAttack != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RageBeastBasicAttack.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_coneAngleMod, "ConeWidthAngle", string.Empty, rageBeastBasicAttack.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneInnerRadiusMod, "ConeLengthInner", string.Empty, rageBeastBasicAttack.m_coneLengthInner, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneOuterRadiusMod, "ConeLengthOuter", string.Empty, rageBeastBasicAttack.m_coneLengthOuter, true, false, false);
			AbilityMod.AddToken(tokens, this.m_innerDamageMod, "DamageAmountInner", string.Empty, rageBeastBasicAttack.m_damageAmountInner, true, false);
			AbilityMod.AddToken(tokens, this.m_outerDamageMod, "DamageAmountOuter", string.Empty, rageBeastBasicAttack.m_damageAmountOuter, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectInnerMod, "EffectInner", rageBeastBasicAttack.m_effectInner, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOuterMod, "EffectOuter", rageBeastBasicAttack.m_effectOuter, true);
			AbilityMod.AddToken(tokens, this.m_innerTpGain, "TpGainInner", string.Empty, rageBeastBasicAttack.m_tpGainInner, true, false);
			AbilityMod.AddToken(tokens, this.m_outerTpGain, "TpGainOuter", string.Empty, rageBeastBasicAttack.m_tpGainOuter, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastBasicAttack rageBeastBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as RageBeastBasicAttack;
		bool flag = rageBeastBasicAttack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat coneAngleMod = this.m_coneAngleMod;
		string prefix = "[Cone Angle]";
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RageBeastBasicAttack.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = rageBeastBasicAttack.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(coneAngleMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat coneInnerRadiusMod = this.m_coneInnerRadiusMod;
		string prefix2 = "[Inner Radius]";
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
			baseVal2 = rageBeastBasicAttack.m_coneLengthInner;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(coneInnerRadiusMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat coneOuterRadiusMod = this.m_coneOuterRadiusMod;
		string prefix3 = "[Outer Radius]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
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
			baseVal3 = rageBeastBasicAttack.m_coneLengthOuter;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(coneOuterRadiusMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt innerDamageMod = this.m_innerDamageMod;
		string prefix4 = "[Inner Damage]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
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
			baseVal4 = rageBeastBasicAttack.m_damageAmountInner;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(innerDamageMod, prefix4, showBaseVal4, baseVal4);
		text += AbilityModHelper.GetModPropertyDesc(this.m_outerDamageMod, "[Outer Damage]", flag, (!flag) ? 0 : rageBeastBasicAttack.m_damageAmountOuter);
		text += base.PropDesc(this.m_effectInnerMod, "[EffectInner]", flag, (!flag) ? null : rageBeastBasicAttack.m_effectInner);
		string str5 = text;
		AbilityModPropertyEffectInfo effectOuterMod = this.m_effectOuterMod;
		string prefix5 = "[EffectOuter]";
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
			baseVal5 = rageBeastBasicAttack.m_effectOuter;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(effectOuterMod, prefix5, showBaseVal5, baseVal5);
		text += AbilityModHelper.GetModPropertyDesc(this.m_innerTpGain, "[Inner TechPoint Gain]", flag, (!flag) ? 0 : rageBeastBasicAttack.m_tpGainInner);
		string str6 = text;
		AbilityModPropertyInt outerTpGain = this.m_outerTpGain;
		string prefix6 = "[Outer TechPoint Gain]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
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
			baseVal6 = rageBeastBasicAttack.m_tpGainOuter;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(outerTpGain, prefix6, showBaseVal6, baseVal6);
		if (this.m_extraDamagePerAdjacentEnemy != 0)
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
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Extra Damage Per Adjacent Enemy] = ",
				this.m_extraDamagePerAdjacentEnemy,
				"\n"
			});
		}
		if (this.m_extraTechPointsPerAdjacentEnemy != 0)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Extra Tech Points Per Adjacent Enemy] = ",
				this.m_extraTechPointsPerAdjacentEnemy,
				"\n"
			});
		}
		return text;
	}
}
