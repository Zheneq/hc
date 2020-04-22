using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SenseiAppendStatus : AbilityMod
{
	[Header("    (( Targeting: If using ActorSquare mode ))")]
	public AbilityModPropertyBool m_canTargetAllyMod;

	public AbilityModPropertyBool m_canTargetEnemyMod;

	public AbilityModPropertyBool m_canTagetSelfMod;

	public AbilityModPropertyBool m_targetingIgnoreLosMod;

	[Header("    (( Targeting: If using Laser mode ))")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	[Separator("On Cast Hit Stuff", true)]
	public AbilityModPropertyEffectData m_enemyCastHitEffectDataMod;

	public AbilityModPropertyEffectData m_allyCastHitEffectDataMod;

	public AbilityModPropertyInt m_energyToAllyTargetOnCastMod;

	[Separator("For Append Effect", true)]
	public AbilityModPropertyBool m_endEffectIfAppendedStatusMod;

	[Header("-- Effect to append --")]
	public AbilityModPropertyEffectInfo m_effectAddedOnEnemyAttackMod;

	public AbilityModPropertyEffectInfo m_effectAddedOnAllyAttackMod;

	[Space(10f)]
	public AbilityModPropertyInt m_energyGainOnAllyAppendHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SenseiAppendStatus);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SenseiAppendStatus senseiAppendStatus = targetAbility as SenseiAppendStatus;
		if (!(senseiAppendStatus != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", senseiAppendStatus.m_laserInfo);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyCastHitEffectDataMod, "EnemyCastHitEffectData", senseiAppendStatus.m_enemyCastHitEffectData);
			AbilityMod.AddToken_EffectMod(tokens, m_allyCastHitEffectDataMod, "AllyCastHitEffectData", senseiAppendStatus.m_allyCastHitEffectData);
			AbilityMod.AddToken(tokens, m_energyToAllyTargetOnCastMod, "EnergyToAllyTargetOnCast", string.Empty, senseiAppendStatus.m_energyToAllyTargetOnCast);
			AbilityMod.AddToken_EffectMod(tokens, m_effectAddedOnEnemyAttackMod, "EffectAddedOnEnemyAttack", senseiAppendStatus.m_effectAddedOnEnemyAttack);
			AbilityMod.AddToken_EffectMod(tokens, m_effectAddedOnAllyAttackMod, "EffectAddedOnAllyAttack", senseiAppendStatus.m_effectAddedOnAllyAttack);
			AbilityMod.AddToken(tokens, m_energyGainOnAllyAppendHitMod, "EnergyGainOnAllyAppendHit", string.Empty, senseiAppendStatus.m_energyGainOnAllyAppendHit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SenseiAppendStatus senseiAppendStatus = GetTargetAbilityOnAbilityData(abilityData) as SenseiAppendStatus;
		bool flag = senseiAppendStatus != null;
		string empty = string.Empty;
		empty += PropDesc(m_canTargetAllyMod, "[CanTargetAlly]", flag, flag && senseiAppendStatus.m_canTargetAlly);
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
			baseVal = (senseiAppendStatus.m_canTargetEnemy ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(canTargetEnemyMod, "[CanTargetEnemy]", flag, (byte)baseVal != 0);
		empty += PropDesc(m_canTagetSelfMod, "[CanTagetSelf]", flag, flag && senseiAppendStatus.m_canTagetSelf);
		empty += PropDesc(m_targetingIgnoreLosMod, "[TargetingIgnoreLos]", flag, flag && senseiAppendStatus.m_targetingIgnoreLos);
		string str2 = empty;
		AbilityModPropertyLaserInfo laserInfoMod = m_laserInfoMod;
		object baseLaserInfo;
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
			baseLaserInfo = senseiAppendStatus.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		empty = str2 + PropDesc(laserInfoMod, "[LaserInfo]", flag, (LaserTargetingInfo)baseLaserInfo);
		empty += PropDesc(m_enemyCastHitEffectDataMod, "[EnemyCastHitEffectData]", flag, (!flag) ? null : senseiAppendStatus.m_enemyCastHitEffectData);
		string str3 = empty;
		AbilityModPropertyEffectData allyCastHitEffectDataMod = m_allyCastHitEffectDataMod;
		object baseVal2;
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
			baseVal2 = senseiAppendStatus.m_allyCastHitEffectData;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str3 + PropDesc(allyCastHitEffectDataMod, "[AllyCastHitEffectData]", flag, (StandardActorEffectData)baseVal2);
		empty += PropDesc(m_energyToAllyTargetOnCastMod, "[EnergyToAllyTargetOnCast]", flag, flag ? senseiAppendStatus.m_energyToAllyTargetOnCast : 0);
		string str4 = empty;
		AbilityModPropertyBool endEffectIfAppendedStatusMod = m_endEffectIfAppendedStatusMod;
		int baseVal3;
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
			baseVal3 = (senseiAppendStatus.m_endEffectIfAppendedStatus ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str4 + PropDesc(endEffectIfAppendedStatusMod, "[EndEffectIfAppendedStatus]", flag, (byte)baseVal3 != 0);
		string str5 = empty;
		AbilityModPropertyEffectInfo effectAddedOnEnemyAttackMod = m_effectAddedOnEnemyAttackMod;
		object baseVal4;
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
			baseVal4 = senseiAppendStatus.m_effectAddedOnEnemyAttack;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str5 + PropDesc(effectAddedOnEnemyAttackMod, "[EffectAddedOnEnemyAttack]", flag, (StandardEffectInfo)baseVal4);
		string str6 = empty;
		AbilityModPropertyEffectInfo effectAddedOnAllyAttackMod = m_effectAddedOnAllyAttackMod;
		object baseVal5;
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
			baseVal5 = senseiAppendStatus.m_effectAddedOnAllyAttack;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str6 + PropDesc(effectAddedOnAllyAttackMod, "[EffectAddedOnAllyAttack]", flag, (StandardEffectInfo)baseVal5);
		string str7 = empty;
		AbilityModPropertyInt energyGainOnAllyAppendHitMod = m_energyGainOnAllyAppendHitMod;
		int baseVal6;
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
			baseVal6 = senseiAppendStatus.m_energyGainOnAllyAppendHit;
		}
		else
		{
			baseVal6 = 0;
		}
		return str7 + PropDesc(energyGainOnAllyAppendHitMod, "[EnergyGainOnAllyAppendHit]", flag, baseVal6);
	}
}
