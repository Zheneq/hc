using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_GremlinsBombingRun : AbilityMod
{
	public AbilityModPropertyInt m_damageMod;

	[Header("-- Range and turn angle mods")]
	public AbilityModPropertyInt m_minSquaresPerExplosionMod;

	public AbilityModPropertyInt m_maxSquaresPerExplosionMod;

	public AbilityModPropertyFloat m_angleWithFirstStepMod;

	[Space(10f)]
	public AbilityModPropertyShape m_explosionShapeMod;

	[Space(10f)]
	[Header("-- Global Mine Data Mods")]
	public AbilityModPropertyInt m_mineDamageMod;

	public AbilityModPropertyInt m_mineDurationMod;

	public AbilityModPropertyEffectInfo m_effectOnEnemyOverride;

	public AbilityModPropertyInt m_energyOnMineExplosionMod;

	public AbilityModPropertyBool m_shouldLeaveMinesAtTouchedSquares;

	public override Type GetTargetAbilityType()
	{
		return typeof(GremlinsBombingRun);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		GremlinsBombingRun gremlinsBombingRun = targetAbility as GremlinsBombingRun;
		if (gremlinsBombingRun != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_GremlinsBombingRun.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_minSquaresPerExplosionMod, "SquaresPerExplosion", string.Empty, gremlinsBombingRun.m_squaresPerExplosion, true, false);
			AbilityMod.AddToken(tokens, this.m_maxSquaresPerExplosionMod, "MaxSquaresPerStep", string.Empty, gremlinsBombingRun.m_maxSquaresPerStep, true, false);
			AbilityMod.AddToken(tokens, this.m_angleWithFirstStepMod, "MaxAngleWithFirstStep", string.Empty, gremlinsBombingRun.m_maxAngleWithFirstStep, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "ExplosionDamageAmount", string.Empty, gremlinsBombingRun.m_explosionDamageAmount, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsBombingRun gremlinsBombingRun = base.GetTargetAbilityOnAbilityData(abilityData) as GremlinsBombingRun;
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent;
		if (gremlinsBombingRun != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_GremlinsBombingRun.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			gremlinsLandMineInfoComponent = gremlinsBombingRun.GetComponent<GremlinsLandMineInfoComponent>();
		}
		else
		{
			gremlinsLandMineInfoComponent = null;
		}
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent2 = gremlinsLandMineInfoComponent;
		bool flag = gremlinsLandMineInfoComponent2 != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix = "[Damage]";
		bool showBaseVal = flag;
		int baseVal;
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
			baseVal = gremlinsBombingRun.m_explosionDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(damageMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_minSquaresPerExplosionMod, "[Min Squares Per Explosion]", flag, (!flag) ? 0 : gremlinsBombingRun.m_squaresPerExplosion);
		string str2 = text;
		AbilityModPropertyInt maxSquaresPerExplosionMod = this.m_maxSquaresPerExplosionMod;
		string prefix2 = "[Max Squares Per Explosion]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = gremlinsBombingRun.m_maxSquaresPerStep;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(maxSquaresPerExplosionMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat angleWithFirstStepMod = this.m_angleWithFirstStepMod;
		string prefix3 = "[Turn Angle (with first segment)]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = gremlinsBombingRun.m_maxAngleWithFirstStep;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(angleWithFirstStepMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyShape explosionShapeMod = this.m_explosionShapeMod;
		string prefix4 = "[Explosion Shape]";
		bool showBaseVal4 = flag;
		AbilityAreaShape baseVal4;
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
			baseVal4 = gremlinsBombingRun.m_explosionShape;
		}
		else
		{
			baseVal4 = AbilityAreaShape.SingleSquare;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(explosionShapeMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt mineDamageMod = this.m_mineDamageMod;
		string prefix5 = "[Mine Damage]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = gremlinsLandMineInfoComponent2.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(mineDamageMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt mineDurationMod = this.m_mineDurationMod;
		string prefix6 = "[Mine Duration]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = gremlinsLandMineInfoComponent2.m_mineDuration;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(mineDurationMod, prefix6, showBaseVal6, baseVal6);
		text += AbilityModHelper.GetModPropertyDesc(this.m_effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", flag, (!flag) ? null : gremlinsLandMineInfoComponent2.m_enemyHitEffect);
		string str7 = text;
		AbilityModPropertyInt energyOnMineExplosionMod = this.m_energyOnMineExplosionMod;
		string prefix7 = "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]";
		bool showBaseVal7 = flag;
		int baseVal7;
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
			baseVal7 = gremlinsLandMineInfoComponent2.m_energyGainOnExplosion;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + AbilityModHelper.GetModPropertyDesc(energyOnMineExplosionMod, prefix7, showBaseVal7, baseVal7);
		return text + base.PropDesc(this.m_shouldLeaveMinesAtTouchedSquares, "[Leave Mines At Each Touched Square?]", flag, false);
	}
}
