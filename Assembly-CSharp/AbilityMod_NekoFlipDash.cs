using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NekoFlipDash : AbilityMod
{
	[Separator("Targeting - Dash Range (please use larger value in TargetData", true)]
	public AbilityModPropertyFloat m_dashTargetRangeMod;

	[Separator("Targeting - (if actor/disc targeted) landing position", true)]
	public AbilityModPropertyBool m_canTargetDiscsMod;

	public AbilityModPropertyBool m_canTargetEnemiesMod;

	public AbilityModPropertyFloat m_maxDistanceFromTargetMod;

	public AbilityModPropertyFloat m_minDistanceFromTargetMod;

	public AbilityModPropertyFloat m_maxAngleChangeMod;

	[Separator("Targeting - Thrown Disc targeting", true)]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserLengthMod;

	public AbilityModPropertyFloat m_aoeRadiusAtLaserEndMod;

	[Header("-- Disc return end radius")]
	public AbilityModPropertyFloat m_discReturnEndRadiusMod;

	[Header("-- Dash options --")]
	public AbilityModPropertyFloat m_chargeRadiusMod;

	public AbilityModPropertyFloat m_chargeRadiusAtStartMod;

	public AbilityModPropertyFloat m_chargeRadiusAtEndMod;

	public AbilityModPropertyFloat m_explosionRadiusAtTargetedDiscMod;

	public AbilityModPropertyBool m_continueToEndIfTargetEvadesMod;

	public AbilityModPropertyBool m_leaveDiscAtStartSquareMod;

	public AbilityModPropertyBool m_throwDiscFromStartMod;

	public AbilityModPropertyBool m_canMoveAfterEvadeMod;

	public AbilityModPropertyBool m_explodeTargetedDiscMod;

	public AbilityModPropertyInt m_discMaxTargetsMod;

	public AbilityModPropertyEffectInfo m_effectOnSelfMod;

	[Separator("On Enemy Hit", true)]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_discDirectDamageMod;

	public AbilityModPropertyInt m_discReturnTripDamageMod;

	public AbilityModPropertyInt m_discReturnTripSubsequentHitDamageMod;

	public AbilityModPropertyBool m_returnTripIgnoreCoverMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	public AbilityModPropertyInt m_explodingTargetDiscDamageMod;

	[Header("-- Other Abilities --")]
	public AbilityModPropertyInt m_discsReturningThisTurnExtraDamageMod;

	[Separator("Cooldown Reduction", true)]
	public AbilityModPropertyInt m_cdrIfHasReturnDiscHitMod;

	public AbilityModPropertyInt m_cdrOnEnlargeDiscIfCastSameTurnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NekoFlipDash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NekoFlipDash nekoFlipDash = targetAbility as NekoFlipDash;
		if (!(nekoFlipDash != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_dashTargetRangeMod, "DashTargetRange", string.Empty, nekoFlipDash.m_dashTargetRange);
			AbilityMod.AddToken(tokens, m_maxDistanceFromTargetMod, "MaxDistanceFromTarget", string.Empty, nekoFlipDash.m_maxDistanceFromTarget);
			AbilityMod.AddToken(tokens, m_minDistanceFromTargetMod, "MinDistanceFromTarget", string.Empty, nekoFlipDash.m_minDistanceFromTarget);
			AbilityMod.AddToken(tokens, m_maxAngleChangeMod, "MaxAngleChange", string.Empty, nekoFlipDash.m_maxAngleChange);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, nekoFlipDash.m_laserWidth);
			AbilityMod.AddToken(tokens, m_laserLengthMod, "LaserLength", string.Empty, nekoFlipDash.m_laserLength);
			AbilityMod.AddToken(tokens, m_aoeRadiusAtLaserEndMod, "AoeRadiusAtLaserEnd", string.Empty, nekoFlipDash.m_aoeRadiusAtLaserEnd);
			AbilityMod.AddToken(tokens, m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoFlipDash.m_discReturnEndRadius);
			AbilityMod.AddToken(tokens, m_chargeRadiusMod, "ChargeRadius", string.Empty, nekoFlipDash.m_chargeRadius);
			AbilityMod.AddToken(tokens, m_chargeRadiusAtStartMod, "ChargeRadiusAtStart", string.Empty, nekoFlipDash.m_chargeRadiusAtStart);
			AbilityMod.AddToken(tokens, m_chargeRadiusAtEndMod, "ChargeRadiusAtEnd", string.Empty, nekoFlipDash.m_chargeRadiusAtEnd);
			AbilityMod.AddToken(tokens, m_explosionRadiusAtTargetedDiscMod, "ExplosionRadiusAtTargetedDisc", string.Empty, nekoFlipDash.m_explosionRadiusAtTargetedDisc);
			AbilityMod.AddToken(tokens, m_discMaxTargetsMod, "DiscMaxTargets", string.Empty, nekoFlipDash.m_discMaxTargets);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", nekoFlipDash.m_effectOnSelf);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, nekoFlipDash.m_damage);
			AbilityMod.AddToken(tokens, m_discDirectDamageMod, "DiscDirectDamage", string.Empty, nekoFlipDash.m_discDirectDamage);
			AbilityMod.AddToken(tokens, m_discReturnTripDamageMod, "DiscReturnTripDamage", string.Empty, nekoFlipDash.m_discReturnTripDamage);
			AbilityMod.AddToken(tokens, m_discReturnTripSubsequentHitDamageMod, "DiscReturnTripSubsequentHitDamage", string.Empty, nekoFlipDash.m_discReturnTripSubsequentHitDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", nekoFlipDash.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_explodingTargetDiscDamageMod, "ExplodingTargetDiscDamage", string.Empty, nekoFlipDash.m_explodingTargetDiscDamage);
			AbilityMod.AddToken(tokens, m_discsReturningThisTurnExtraDamageMod, "DiscsReturningThisTurnExtraDamage", string.Empty, nekoFlipDash.m_discsReturningThisTurnExtraDamage);
			AbilityMod.AddToken(tokens, m_cdrIfHasReturnDiscHitMod, "CdrIfHasReturnDiscHit", string.Empty, nekoFlipDash.m_cdrIfHasReturnDiscHit);
			AbilityMod.AddToken(tokens, m_cdrOnEnlargeDiscIfCastSameTurnMod, "CdrOnEnlargeDiscIfCastSameTurn", string.Empty, nekoFlipDash.m_cdrOnEnlargeDiscIfCastSameTurn);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoFlipDash nekoFlipDash = GetTargetAbilityOnAbilityData(abilityData) as NekoFlipDash;
		bool flag = nekoFlipDash != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat dashTargetRangeMod = m_dashTargetRangeMod;
		float baseVal;
		if (flag)
		{
			baseVal = nekoFlipDash.m_dashTargetRange;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(dashTargetRangeMod, "[DashTargetRange]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyBool canTargetDiscsMod = m_canTargetDiscsMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = (nekoFlipDash.m_canTargetDiscs ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(canTargetDiscsMod, "[CanTargetDiscs]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyBool canTargetEnemiesMod = m_canTargetEnemiesMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (nekoFlipDash.m_canTargetEnemies ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(canTargetEnemiesMod, "[CanTargetEnemies]", flag, (byte)baseVal3 != 0);
		empty += PropDesc(m_maxDistanceFromTargetMod, "[MaxDistanceFromTarget]", flag, (!flag) ? 0f : nekoFlipDash.m_maxDistanceFromTarget);
		string str4 = empty;
		AbilityModPropertyFloat minDistanceFromTargetMod = m_minDistanceFromTargetMod;
		float baseVal4;
		if (flag)
		{
			baseVal4 = nekoFlipDash.m_minDistanceFromTarget;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(minDistanceFromTargetMod, "[MinDistanceFromTarget]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyFloat maxAngleChangeMod = m_maxAngleChangeMod;
		float baseVal5;
		if (flag)
		{
			baseVal5 = nekoFlipDash.m_maxAngleChange;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(maxAngleChangeMod, "[MaxAngleChange]", flag, baseVal5);
		empty += PropDesc(m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : nekoFlipDash.m_laserWidth);
		string str6 = empty;
		AbilityModPropertyFloat laserLengthMod = m_laserLengthMod;
		float baseVal6;
		if (flag)
		{
			baseVal6 = nekoFlipDash.m_laserLength;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(laserLengthMod, "[LaserLength]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyFloat aoeRadiusAtLaserEndMod = m_aoeRadiusAtLaserEndMod;
		float baseVal7;
		if (flag)
		{
			baseVal7 = nekoFlipDash.m_aoeRadiusAtLaserEnd;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str7 + PropDesc(aoeRadiusAtLaserEndMod, "[AoeRadiusAtLaserEnd]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyFloat discReturnEndRadiusMod = m_discReturnEndRadiusMod;
		float baseVal8;
		if (flag)
		{
			baseVal8 = nekoFlipDash.m_discReturnEndRadius;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + PropDesc(discReturnEndRadiusMod, "[DiscReturnEndRadius]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyFloat chargeRadiusMod = m_chargeRadiusMod;
		float baseVal9;
		if (flag)
		{
			baseVal9 = nekoFlipDash.m_chargeRadius;
		}
		else
		{
			baseVal9 = 0f;
		}
		empty = str9 + PropDesc(chargeRadiusMod, "[ChargeRadius]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyFloat chargeRadiusAtStartMod = m_chargeRadiusAtStartMod;
		float baseVal10;
		if (flag)
		{
			baseVal10 = nekoFlipDash.m_chargeRadiusAtStart;
		}
		else
		{
			baseVal10 = 0f;
		}
		empty = str10 + PropDesc(chargeRadiusAtStartMod, "[ChargeRadiusAtStart]", flag, baseVal10);
		empty += PropDesc(m_chargeRadiusAtEndMod, "[ChargeRadiusAtEnd]", flag, (!flag) ? 0f : nekoFlipDash.m_chargeRadiusAtEnd);
		string str11 = empty;
		AbilityModPropertyFloat explosionRadiusAtTargetedDiscMod = m_explosionRadiusAtTargetedDiscMod;
		float baseVal11;
		if (flag)
		{
			baseVal11 = nekoFlipDash.m_explosionRadiusAtTargetedDisc;
		}
		else
		{
			baseVal11 = 0f;
		}
		empty = str11 + PropDesc(explosionRadiusAtTargetedDiscMod, "[ExplosionRadiusAtTargetedDisc]", flag, baseVal11);
		string str12 = empty;
		AbilityModPropertyBool continueToEndIfTargetEvadesMod = m_continueToEndIfTargetEvadesMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = (nekoFlipDash.m_continueToEndIfTargetEvades ? 1 : 0);
		}
		else
		{
			baseVal12 = 0;
		}
		empty = str12 + PropDesc(continueToEndIfTargetEvadesMod, "[ContinueToEndIfTargetEvades]", flag, (byte)baseVal12 != 0);
		string str13 = empty;
		AbilityModPropertyBool leaveDiscAtStartSquareMod = m_leaveDiscAtStartSquareMod;
		int baseVal13;
		if (flag)
		{
			baseVal13 = (nekoFlipDash.m_leaveDiscAtStartSquare ? 1 : 0);
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str13 + PropDesc(leaveDiscAtStartSquareMod, "[LeaveDiscAtStartSquare]", flag, (byte)baseVal13 != 0);
		string str14 = empty;
		AbilityModPropertyBool throwDiscFromStartMod = m_throwDiscFromStartMod;
		int baseVal14;
		if (flag)
		{
			baseVal14 = (nekoFlipDash.m_throwDiscFromStart ? 1 : 0);
		}
		else
		{
			baseVal14 = 0;
		}
		empty = str14 + PropDesc(throwDiscFromStartMod, "[ThrowDiscFromStart]", flag, (byte)baseVal14 != 0);
		string str15 = empty;
		AbilityModPropertyBool canMoveAfterEvadeMod = m_canMoveAfterEvadeMod;
		int baseVal15;
		if (flag)
		{
			baseVal15 = (nekoFlipDash.m_canMoveAfterEvade ? 1 : 0);
		}
		else
		{
			baseVal15 = 0;
		}
		empty = str15 + PropDesc(canMoveAfterEvadeMod, "[CanMoveAfterEvade]", flag, (byte)baseVal15 != 0);
		string str16 = empty;
		AbilityModPropertyBool explodeTargetedDiscMod = m_explodeTargetedDiscMod;
		int baseVal16;
		if (flag)
		{
			baseVal16 = (nekoFlipDash.m_explodeTargetedDisc ? 1 : 0);
		}
		else
		{
			baseVal16 = 0;
		}
		empty = str16 + PropDesc(explodeTargetedDiscMod, "[ExplodeTargetedDisc]", flag, (byte)baseVal16 != 0);
		string str17 = empty;
		AbilityModPropertyInt discMaxTargetsMod = m_discMaxTargetsMod;
		int baseVal17;
		if (flag)
		{
			baseVal17 = nekoFlipDash.m_discMaxTargets;
		}
		else
		{
			baseVal17 = 0;
		}
		empty = str17 + PropDesc(discMaxTargetsMod, "[DiscMaxTargets]", flag, baseVal17);
		string str18 = empty;
		AbilityModPropertyEffectInfo effectOnSelfMod = m_effectOnSelfMod;
		object baseVal18;
		if (flag)
		{
			baseVal18 = nekoFlipDash.m_effectOnSelf;
		}
		else
		{
			baseVal18 = null;
		}
		empty = str18 + PropDesc(effectOnSelfMod, "[EffectOnSelf]", flag, (StandardEffectInfo)baseVal18);
		string str19 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal19;
		if (flag)
		{
			baseVal19 = nekoFlipDash.m_damage;
		}
		else
		{
			baseVal19 = 0;
		}
		empty = str19 + PropDesc(damageMod, "[Damage]", flag, baseVal19);
		string str20 = empty;
		AbilityModPropertyInt discDirectDamageMod = m_discDirectDamageMod;
		int baseVal20;
		if (flag)
		{
			baseVal20 = nekoFlipDash.m_discDirectDamage;
		}
		else
		{
			baseVal20 = 0;
		}
		empty = str20 + PropDesc(discDirectDamageMod, "[DiscDirectDamage]", flag, baseVal20);
		empty += PropDesc(m_discReturnTripDamageMod, "[DiscReturnTripDamage]", flag, flag ? nekoFlipDash.m_discReturnTripDamage : 0);
		string str21 = empty;
		AbilityModPropertyInt discReturnTripSubsequentHitDamageMod = m_discReturnTripSubsequentHitDamageMod;
		int baseVal21;
		if (flag)
		{
			baseVal21 = nekoFlipDash.m_discReturnTripSubsequentHitDamage;
		}
		else
		{
			baseVal21 = 0;
		}
		empty = str21 + PropDesc(discReturnTripSubsequentHitDamageMod, "[DiscReturnTripSubsequentHitDamage]", flag, baseVal21);
		string str22 = empty;
		AbilityModPropertyBool returnTripIgnoreCoverMod = m_returnTripIgnoreCoverMod;
		int baseVal22;
		if (flag)
		{
			baseVal22 = (nekoFlipDash.m_returnTripIgnoreCover ? 1 : 0);
		}
		else
		{
			baseVal22 = 0;
		}
		empty = str22 + PropDesc(returnTripIgnoreCoverMod, "[ReturnTripIgnoreCover]", flag, (byte)baseVal22 != 0);
		string str23 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal23;
		if (flag)
		{
			baseVal23 = nekoFlipDash.m_enemyHitEffect;
		}
		else
		{
			baseVal23 = null;
		}
		empty = str23 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal23);
		string str24 = empty;
		AbilityModPropertyInt explodingTargetDiscDamageMod = m_explodingTargetDiscDamageMod;
		int baseVal24;
		if (flag)
		{
			baseVal24 = nekoFlipDash.m_explodingTargetDiscDamage;
		}
		else
		{
			baseVal24 = 0;
		}
		empty = str24 + PropDesc(explodingTargetDiscDamageMod, "[ExplodingTargetDiscDamage]", flag, baseVal24);
		empty += PropDesc(m_discsReturningThisTurnExtraDamageMod, "[DiscsReturningThisTurnExtraDamage]", flag, flag ? nekoFlipDash.m_discsReturningThisTurnExtraDamage : 0);
		empty += PropDesc(m_cdrIfHasReturnDiscHitMod, "[CdrIfHasReturnDiscHit]", flag, flag ? nekoFlipDash.m_cdrIfHasReturnDiscHit : 0);
		string str25 = empty;
		AbilityModPropertyInt cdrOnEnlargeDiscIfCastSameTurnMod = m_cdrOnEnlargeDiscIfCastSameTurnMod;
		int baseVal25;
		if (flag)
		{
			baseVal25 = nekoFlipDash.m_cdrOnEnlargeDiscIfCastSameTurn;
		}
		else
		{
			baseVal25 = 0;
		}
		return str25 + PropDesc(cdrOnEnlargeDiscIfCastSameTurnMod, "[CdrOnEnlargeDiscIfCastSameTurn]", flag, baseVal25);
	}
}
