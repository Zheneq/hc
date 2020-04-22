using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_MartyrAoeOnReactHit : AbilityMod
{
	[Header("-- Targeting --")]
	public AbilityModPropertyBool m_canTargetEnemyMod;

	public AbilityModPropertyBool m_canTargetAllyMod;

	public AbilityModPropertyBool m_canTargetSelfMod;

	[Space(10f)]
	public AbilityModPropertyBool m_targetingIgnoreLosMod;

	[Header("-- Base Effect Data")]
	public AbilityModPropertyEffectData m_enemyBaseEffectDataMod;

	public AbilityModPropertyEffectData m_allyBaseEffectDataMod;

	[Header("-- Extra Shielding for Allies")]
	public AbilityModPropertyInt m_extraAbsorbPerCrystalMod;

	[Header("-- For React Area --")]
	public AbilityModPropertyFloat m_reactBaseRadiusMod;

	public AbilityModPropertyFloat m_reactRadiusPerCrystalMod;

	public AbilityModPropertyBool m_reactOnlyOncePerTurnMod;

	public AbilityModPropertyBool m_reactPenetrateLosMod;

	public AbilityModPropertyBool m_reactIncludeEffectTargetMod;

	[Header("-- On React Hit --")]
	public AbilityModPropertyInt m_reactAoeDamageMod;

	public AbilityModPropertyInt m_reactDamagePerCrystalMod;

	public AbilityModPropertyEffectInfo m_reactEnemyHitEffectMod;

	public AbilityModPropertyInt m_reactHealOnTargetMod;

	public AbilityModPropertyInt m_reactEnergyOnCasterPerReactMod;

	[Header("-- Cooldown reduction if no reacts")]
	public AbilityModPropertyInt m_cdrIfNoReactionTriggeredMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MartyrAoeOnReactHit);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MartyrAoeOnReactHit martyrAoeOnReactHit = targetAbility as MartyrAoeOnReactHit;
		if (martyrAoeOnReactHit != null)
		{
			AbilityMod.AddToken_EffectMod(tokens, m_enemyBaseEffectDataMod, "EnemyBaseEffectData", martyrAoeOnReactHit.m_enemyBaseEffectData);
			AbilityMod.AddToken_EffectMod(tokens, m_allyBaseEffectDataMod, "AllyBaseEffectData", martyrAoeOnReactHit.m_allyBaseEffectData);
			AbilityMod.AddToken(tokens, m_extraAbsorbPerCrystalMod, "ExtraAbsorbPerCrystal", string.Empty, martyrAoeOnReactHit.m_extraAbsorbPerCrystal);
			AbilityMod.AddToken(tokens, m_reactBaseRadiusMod, "ReactBaseRadius", string.Empty, martyrAoeOnReactHit.m_reactBaseRadius);
			AbilityMod.AddToken(tokens, m_reactRadiusPerCrystalMod, "ReactRadiusPerCrystal", string.Empty, martyrAoeOnReactHit.m_reactRadiusPerCrystal);
			AbilityMod.AddToken(tokens, m_reactAoeDamageMod, "ReactAoeDamage", string.Empty, martyrAoeOnReactHit.m_reactAoeDamage);
			AbilityMod.AddToken(tokens, m_reactDamagePerCrystalMod, "ReactDamagePerCrystal", string.Empty, martyrAoeOnReactHit.m_reactDamagePerCrystal);
			AbilityMod.AddToken_EffectMod(tokens, m_reactEnemyHitEffectMod, "ReactEnemyHitEffect", martyrAoeOnReactHit.m_reactEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_reactHealOnTargetMod, "ReactHealOnTarget", string.Empty, martyrAoeOnReactHit.m_reactHealOnTarget);
			AbilityMod.AddToken(tokens, m_reactEnergyOnCasterPerReactMod, "ReactEnergyOnCasterPerReact", string.Empty, martyrAoeOnReactHit.m_reactEnergyOnCasterPerReact);
			AbilityMod.AddToken(tokens, m_cdrIfNoReactionTriggeredMod, "CdrIfNoReactionTriggered", string.Empty, martyrAoeOnReactHit.m_cdrIfNoReactionTriggered);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrAoeOnReactHit martyrAoeOnReactHit = GetTargetAbilityOnAbilityData(abilityData) as MartyrAoeOnReactHit;
		bool flag = martyrAoeOnReactHit != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool canTargetEnemyMod = m_canTargetEnemyMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (7)
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
			baseVal = (martyrAoeOnReactHit.m_canTargetEnemy ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(canTargetEnemyMod, "[CanTargetEnemy]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyBool canTargetAllyMod = m_canTargetAllyMod;
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
			baseVal2 = (martyrAoeOnReactHit.m_canTargetAlly ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(canTargetAllyMod, "[CanTargetAlly]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyBool canTargetSelfMod = m_canTargetSelfMod;
		int baseVal3;
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
			baseVal3 = (martyrAoeOnReactHit.m_canTargetSelf ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(canTargetSelfMod, "[CanTargetSelf]", flag, (byte)baseVal3 != 0);
		string str4 = empty;
		AbilityModPropertyBool targetingIgnoreLosMod = m_targetingIgnoreLosMod;
		int baseVal4;
		if (flag)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = (martyrAoeOnReactHit.m_targetingIgnoreLos ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(targetingIgnoreLosMod, "[TargetingIgnoreLos]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyEffectData enemyBaseEffectDataMod = m_enemyBaseEffectDataMod;
		object baseVal5;
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
			baseVal5 = martyrAoeOnReactHit.m_enemyBaseEffectData;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(enemyBaseEffectDataMod, "[EnemyBaseEffectData]", flag, (StandardActorEffectData)baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectData allyBaseEffectDataMod = m_allyBaseEffectDataMod;
		object baseVal6;
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
			baseVal6 = martyrAoeOnReactHit.m_allyBaseEffectData;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(allyBaseEffectDataMod, "[AllyBaseEffectData]", flag, (StandardActorEffectData)baseVal6);
		empty += PropDesc(m_extraAbsorbPerCrystalMod, "[ExtraAbsorbPerCrystal]", flag, flag ? martyrAoeOnReactHit.m_extraAbsorbPerCrystal : 0);
		string str7 = empty;
		AbilityModPropertyFloat reactBaseRadiusMod = m_reactBaseRadiusMod;
		float baseVal7;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal7 = martyrAoeOnReactHit.m_reactBaseRadius;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str7 + PropDesc(reactBaseRadiusMod, "[ReactBaseRadius]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyFloat reactRadiusPerCrystalMod = m_reactRadiusPerCrystalMod;
		float baseVal8;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal8 = martyrAoeOnReactHit.m_reactRadiusPerCrystal;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + PropDesc(reactRadiusPerCrystalMod, "[ReactRadiusPerCrystal]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyBool reactOnlyOncePerTurnMod = m_reactOnlyOncePerTurnMod;
		int baseVal9;
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
			baseVal9 = (martyrAoeOnReactHit.m_reactOnlyOncePerTurn ? 1 : 0);
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(reactOnlyOncePerTurnMod, "[ReactOnlyOncePerTurn]", flag, (byte)baseVal9 != 0);
		empty += PropDesc(m_reactPenetrateLosMod, "[ReactPenetrateLos]", flag, flag && martyrAoeOnReactHit.m_reactPenetrateLos);
		string str10 = empty;
		AbilityModPropertyBool reactIncludeEffectTargetMod = m_reactIncludeEffectTargetMod;
		int baseVal10;
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
			baseVal10 = (martyrAoeOnReactHit.m_reactIncludeEffectTarget ? 1 : 0);
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(reactIncludeEffectTargetMod, "[ReactIncludeEffectTarget]", flag, (byte)baseVal10 != 0);
		empty += PropDesc(m_reactAoeDamageMod, "[ReactAoeDamage]", flag, flag ? martyrAoeOnReactHit.m_reactAoeDamage : 0);
		empty += PropDesc(m_reactDamagePerCrystalMod, "[ReactDamagePerCrystal]", flag, flag ? martyrAoeOnReactHit.m_reactDamagePerCrystal : 0);
		string str11 = empty;
		AbilityModPropertyEffectInfo reactEnemyHitEffectMod = m_reactEnemyHitEffectMod;
		object baseVal11;
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
			baseVal11 = martyrAoeOnReactHit.m_reactEnemyHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		empty = str11 + PropDesc(reactEnemyHitEffectMod, "[ReactEnemyHitEffect]", flag, (StandardEffectInfo)baseVal11);
		string str12 = empty;
		AbilityModPropertyInt reactHealOnTargetMod = m_reactHealOnTargetMod;
		int baseVal12;
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
			baseVal12 = martyrAoeOnReactHit.m_reactHealOnTarget;
		}
		else
		{
			baseVal12 = 0;
		}
		empty = str12 + PropDesc(reactHealOnTargetMod, "[ReactHealOnTarget]", flag, baseVal12);
		string str13 = empty;
		AbilityModPropertyInt reactEnergyOnCasterPerReactMod = m_reactEnergyOnCasterPerReactMod;
		int baseVal13;
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
			baseVal13 = martyrAoeOnReactHit.m_reactEnergyOnCasterPerReact;
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str13 + PropDesc(reactEnergyOnCasterPerReactMod, "[ReactEnergyOnCasterPerReact]", flag, baseVal13);
		string str14 = empty;
		AbilityModPropertyInt cdrIfNoReactionTriggeredMod = m_cdrIfNoReactionTriggeredMod;
		int baseVal14;
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
			baseVal14 = martyrAoeOnReactHit.m_cdrIfNoReactionTriggered;
		}
		else
		{
			baseVal14 = 0;
		}
		return str14 + PropDesc(cdrIfNoReactionTriggeredMod, "[CdrIfNoReactionTriggered]", flag, baseVal14);
	}
}
