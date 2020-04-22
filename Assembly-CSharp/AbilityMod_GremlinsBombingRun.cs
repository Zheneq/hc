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
		if (!(gremlinsBombingRun != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_minSquaresPerExplosionMod, "SquaresPerExplosion", string.Empty, gremlinsBombingRun.m_squaresPerExplosion);
			AbilityMod.AddToken(tokens, m_maxSquaresPerExplosionMod, "MaxSquaresPerStep", string.Empty, gremlinsBombingRun.m_maxSquaresPerStep);
			AbilityMod.AddToken(tokens, m_angleWithFirstStepMod, "MaxAngleWithFirstStep", string.Empty, gremlinsBombingRun.m_maxAngleWithFirstStep);
			AbilityMod.AddToken(tokens, m_damageMod, "ExplosionDamageAmount", string.Empty, gremlinsBombingRun.m_explosionDamageAmount);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsBombingRun gremlinsBombingRun = GetTargetAbilityOnAbilityData(abilityData) as GremlinsBombingRun;
		object obj;
		if (gremlinsBombingRun != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			obj = gremlinsBombingRun.GetComponent<GremlinsLandMineInfoComponent>();
		}
		else
		{
			obj = null;
		}
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent = (GremlinsLandMineInfoComponent)obj;
		bool flag = gremlinsLandMineInfoComponent != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal;
		if (flag)
		{
			while (true)
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
		empty = str + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_minSquaresPerExplosionMod, "[Min Squares Per Explosion]", flag, flag ? gremlinsBombingRun.m_squaresPerExplosion : 0);
		string str2 = empty;
		AbilityModPropertyInt maxSquaresPerExplosionMod = m_maxSquaresPerExplosionMod;
		int baseVal2;
		if (flag)
		{
			while (true)
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
		empty = str2 + AbilityModHelper.GetModPropertyDesc(maxSquaresPerExplosionMod, "[Max Squares Per Explosion]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat angleWithFirstStepMod = m_angleWithFirstStepMod;
		float baseVal3;
		if (flag)
		{
			while (true)
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
		empty = str3 + AbilityModHelper.GetModPropertyDesc(angleWithFirstStepMod, "[Turn Angle (with first segment)]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyShape explosionShapeMod = m_explosionShapeMod;
		int baseVal4;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = (int)gremlinsBombingRun.m_explosionShape;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(explosionShapeMod, "[Explosion Shape]", flag, (AbilityAreaShape)baseVal4);
		string str5 = empty;
		AbilityModPropertyInt mineDamageMod = m_mineDamageMod;
		int baseVal5;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = gremlinsLandMineInfoComponent.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(mineDamageMod, "[Mine Damage]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt mineDurationMod = m_mineDurationMod;
		int baseVal6;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal6 = gremlinsLandMineInfoComponent.m_mineDuration;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + AbilityModHelper.GetModPropertyDesc(mineDurationMod, "[Mine Duration]", flag, baseVal6);
		empty += AbilityModHelper.GetModPropertyDesc(m_effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", flag, (!flag) ? null : gremlinsLandMineInfoComponent.m_enemyHitEffect);
		string str7 = empty;
		AbilityModPropertyInt energyOnMineExplosionMod = m_energyOnMineExplosionMod;
		int baseVal7;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal7 = gremlinsLandMineInfoComponent.m_energyGainOnExplosion;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + AbilityModHelper.GetModPropertyDesc(energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", flag, baseVal7);
		return empty + PropDesc(m_shouldLeaveMinesAtTouchedSquares, "[Leave Mines At Each Touched Square?]", flag);
	}
}
