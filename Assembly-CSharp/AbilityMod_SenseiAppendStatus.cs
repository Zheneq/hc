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
		if (senseiAppendStatus != null)
		{
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", senseiAppendStatus.m_laserInfo, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyCastHitEffectDataMod, "EnemyCastHitEffectData", senseiAppendStatus.m_enemyCastHitEffectData, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyCastHitEffectDataMod, "AllyCastHitEffectData", senseiAppendStatus.m_allyCastHitEffectData, true);
			AbilityMod.AddToken(tokens, this.m_energyToAllyTargetOnCastMod, "EnergyToAllyTargetOnCast", string.Empty, senseiAppendStatus.m_energyToAllyTargetOnCast, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectAddedOnEnemyAttackMod, "EffectAddedOnEnemyAttack", senseiAppendStatus.m_effectAddedOnEnemyAttack, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectAddedOnAllyAttackMod, "EffectAddedOnAllyAttack", senseiAppendStatus.m_effectAddedOnAllyAttack, true);
			AbilityMod.AddToken(tokens, this.m_energyGainOnAllyAppendHitMod, "EnergyGainOnAllyAppendHit", string.Empty, senseiAppendStatus.m_energyGainOnAllyAppendHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SenseiAppendStatus senseiAppendStatus = base.GetTargetAbilityOnAbilityData(abilityData) as SenseiAppendStatus;
		bool flag = senseiAppendStatus != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_canTargetAllyMod, "[CanTargetAlly]", flag, flag && senseiAppendStatus.m_canTargetAlly);
		string str = text;
		AbilityModPropertyBool canTargetEnemyMod = this.m_canTargetEnemyMod;
		string prefix = "[CanTargetEnemy]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			baseVal = senseiAppendStatus.m_canTargetEnemy;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(canTargetEnemyMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_canTagetSelfMod, "[CanTagetSelf]", flag, flag && senseiAppendStatus.m_canTagetSelf);
		text += base.PropDesc(this.m_targetingIgnoreLosMod, "[TargetingIgnoreLos]", flag, flag && senseiAppendStatus.m_targetingIgnoreLos);
		string str2 = text;
		AbilityModPropertyLaserInfo laserInfoMod = this.m_laserInfoMod;
		string prefix2 = "[LaserInfo]";
		bool showBaseVal2 = flag;
		LaserTargetingInfo baseLaserInfo;
		if (flag)
		{
			baseLaserInfo = senseiAppendStatus.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str2 + base.PropDesc(laserInfoMod, prefix2, showBaseVal2, baseLaserInfo);
		text += base.PropDesc(this.m_enemyCastHitEffectDataMod, "[EnemyCastHitEffectData]", flag, (!flag) ? null : senseiAppendStatus.m_enemyCastHitEffectData);
		string str3 = text;
		AbilityModPropertyEffectData allyCastHitEffectDataMod = this.m_allyCastHitEffectDataMod;
		string prefix3 = "[AllyCastHitEffectData]";
		bool showBaseVal3 = flag;
		StandardActorEffectData baseVal2;
		if (flag)
		{
			baseVal2 = senseiAppendStatus.m_allyCastHitEffectData;
		}
		else
		{
			baseVal2 = null;
		}
		text = str3 + base.PropDesc(allyCastHitEffectDataMod, prefix3, showBaseVal3, baseVal2);
		text += base.PropDesc(this.m_energyToAllyTargetOnCastMod, "[EnergyToAllyTargetOnCast]", flag, (!flag) ? 0 : senseiAppendStatus.m_energyToAllyTargetOnCast);
		string str4 = text;
		AbilityModPropertyBool endEffectIfAppendedStatusMod = this.m_endEffectIfAppendedStatusMod;
		string prefix4 = "[EndEffectIfAppendedStatus]";
		bool showBaseVal4 = flag;
		bool baseVal3;
		if (flag)
		{
			baseVal3 = senseiAppendStatus.m_endEffectIfAppendedStatus;
		}
		else
		{
			baseVal3 = false;
		}
		text = str4 + base.PropDesc(endEffectIfAppendedStatusMod, prefix4, showBaseVal4, baseVal3);
		string str5 = text;
		AbilityModPropertyEffectInfo effectAddedOnEnemyAttackMod = this.m_effectAddedOnEnemyAttackMod;
		string prefix5 = "[EffectAddedOnEnemyAttack]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal4;
		if (flag)
		{
			baseVal4 = senseiAppendStatus.m_effectAddedOnEnemyAttack;
		}
		else
		{
			baseVal4 = null;
		}
		text = str5 + base.PropDesc(effectAddedOnEnemyAttackMod, prefix5, showBaseVal5, baseVal4);
		string str6 = text;
		AbilityModPropertyEffectInfo effectAddedOnAllyAttackMod = this.m_effectAddedOnAllyAttackMod;
		string prefix6 = "[EffectAddedOnAllyAttack]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal5;
		if (flag)
		{
			baseVal5 = senseiAppendStatus.m_effectAddedOnAllyAttack;
		}
		else
		{
			baseVal5 = null;
		}
		text = str6 + base.PropDesc(effectAddedOnAllyAttackMod, prefix6, showBaseVal6, baseVal5);
		string str7 = text;
		AbilityModPropertyInt energyGainOnAllyAppendHitMod = this.m_energyGainOnAllyAppendHitMod;
		string prefix7 = "[EnergyGainOnAllyAppendHit]";
		bool showBaseVal7 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = senseiAppendStatus.m_energyGainOnAllyAppendHit;
		}
		else
		{
			baseVal6 = 0;
		}
		return str7 + base.PropDesc(energyGainOnAllyAppendHitMod, prefix7, showBaseVal7, baseVal6);
	}
}
