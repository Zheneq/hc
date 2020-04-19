using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ArcherShieldGeneratorArrow : AbilityMod
{
	[Header("-- Ground effect")]
	public AbilityModPropertyBool m_penetrateLoSMod;

	public AbilityModPropertyBool m_affectsEnemiesMod;

	public AbilityModPropertyBool m_affectsAlliesMod;

	public AbilityModPropertyBool m_affectsCasterMod;

	public AbilityModPropertyInt m_lessAbsorbPerTurnMod;

	public AbilityModPropertyEffectInfo m_directHitEnemyEffectMod;

	public AbilityModPropertyEffectInfo m_directHitAllyEffectMod;

	[Header("-- Extra effect for shielding that last different number of turns from main effect, etc")]
	public AbilityModPropertyEffectInfo m_extraAllyHitEffectMod;

	[Header("-- Num hits mods")]
	public AbilityModPropertyInt m_extraAbsorbPerEnemyHit;

	public AbilityModPropertyInt m_extraAbsorbIfEnemyHit;

	public AbilityModPropertyInt m_extraAbsorbIfOnlyOneAllyHit;

	[Header("-- Misc ability interactions")]
	public AbilityModPropertyInt m_cooldownReductionOnDash;

	public override Type GetTargetAbilityType()
	{
		return typeof(ArcherShieldGeneratorArrow);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ArcherShieldGeneratorArrow archerShieldGeneratorArrow = targetAbility as ArcherShieldGeneratorArrow;
		if (archerShieldGeneratorArrow != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ArcherShieldGeneratorArrow.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_lessAbsorbPerTurnMod, "LessAbsorbPerTurn", string.Empty, archerShieldGeneratorArrow.m_lessAbsorbPerTurn, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_directHitEnemyEffectMod, "DirectHitEnemyEffect", archerShieldGeneratorArrow.m_directHitEnemyEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_directHitAllyEffectMod, "DirectHitAllyEffect", archerShieldGeneratorArrow.m_directHitAllyEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_extraAllyHitEffectMod, "ExtraAllyHitEffect", archerShieldGeneratorArrow.m_extraAllyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_cooldownReductionOnDash, "CooldownReductionOnDashAbility", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_extraAbsorbPerEnemyHit, "ExtraAbsorbPerEnemyHit", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_extraAbsorbIfEnemyHit, "ExtraAbsorbIfAnyEnemyHit", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_extraAbsorbIfOnlyOneAllyHit, "ExtraAbsorbIfOnlyOneAllyHit", string.Empty, 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherShieldGeneratorArrow archerShieldGeneratorArrow = base.GetTargetAbilityOnAbilityData(abilityData) as ArcherShieldGeneratorArrow;
		bool flag = archerShieldGeneratorArrow != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_penetrateLoSMod, "[PenetrateLoS]", flag, flag && archerShieldGeneratorArrow.m_penetrateLoS);
		string str = text;
		AbilityModPropertyBool affectsEnemiesMod = this.m_affectsEnemiesMod;
		string prefix = "[AffectsEnemies]";
		bool showBaseVal = flag;
		bool baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ArcherShieldGeneratorArrow.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = archerShieldGeneratorArrow.m_affectsEnemies;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(affectsEnemiesMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool affectsAlliesMod = this.m_affectsAlliesMod;
		string prefix2 = "[AffectsAllies]";
		bool showBaseVal2 = flag;
		bool baseVal2;
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
			baseVal2 = archerShieldGeneratorArrow.m_affectsAllies;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(affectsAlliesMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool affectsCasterMod = this.m_affectsCasterMod;
		string prefix3 = "[AffectsCaster]";
		bool showBaseVal3 = flag;
		bool baseVal3;
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
			baseVal3 = archerShieldGeneratorArrow.m_affectsCaster;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(affectsCasterMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt lessAbsorbPerTurnMod = this.m_lessAbsorbPerTurnMod;
		string prefix4 = "[LessAbsorbPerTurn]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = archerShieldGeneratorArrow.m_lessAbsorbPerTurn;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(lessAbsorbPerTurnMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo directHitEnemyEffectMod = this.m_directHitEnemyEffectMod;
		string prefix5 = "[DirectHitEnemyEffect]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
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
			baseVal5 = archerShieldGeneratorArrow.m_directHitEnemyEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(directHitEnemyEffectMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo directHitAllyEffectMod = this.m_directHitAllyEffectMod;
		string prefix6 = "[DirectHitAllyEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
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
			baseVal6 = archerShieldGeneratorArrow.m_directHitAllyEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(directHitAllyEffectMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_extraAllyHitEffectMod, "[ExtraAllyHitEffect]", flag, (!flag) ? null : archerShieldGeneratorArrow.m_extraAllyHitEffect);
		text += base.PropDesc(this.m_cooldownReductionOnDash, "[CooldownReductionOnDashAbility]", flag, 0);
		text += base.PropDesc(this.m_extraAbsorbPerEnemyHit, "[ExtraAbsorbPerEnemyHit]", flag, 0);
		text += base.PropDesc(this.m_extraAbsorbIfEnemyHit, "[ExtraAbsorbIfAnyEnemyHit]", flag, 0);
		return text + base.PropDesc(this.m_extraAbsorbIfOnlyOneAllyHit, "[ExtraAbsorbIfOnlyOneAllyHit]", flag, 0);
	}
}
