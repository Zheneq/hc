using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_NekoFlipDash : AbilityMod
{
	[Separator("Targeting - Dash Range (please use larger value in TargetData")]
	public AbilityModPropertyFloat m_dashTargetRangeMod;
	[Separator("Targeting - (if actor/disc targeted) landing position")]
	public AbilityModPropertyBool m_canTargetDiscsMod;
	public AbilityModPropertyBool m_canTargetEnemiesMod;
	public AbilityModPropertyFloat m_maxDistanceFromTargetMod;
	public AbilityModPropertyFloat m_minDistanceFromTargetMod;
	public AbilityModPropertyFloat m_maxAngleChangeMod;
	[Separator("Targeting - Thrown Disc targeting")]
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
	[Separator("On Enemy Hit")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_discDirectDamageMod;
	public AbilityModPropertyInt m_discReturnTripDamageMod;
	public AbilityModPropertyInt m_discReturnTripSubsequentHitDamageMod;
	public AbilityModPropertyBool m_returnTripIgnoreCoverMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	public AbilityModPropertyInt m_explodingTargetDiscDamageMod;
	[Header("-- Other Abilities --")]
	public AbilityModPropertyInt m_discsReturningThisTurnExtraDamageMod;
	[Separator("Cooldown Reduction")]
	public AbilityModPropertyInt m_cdrIfHasReturnDiscHitMod;
	public AbilityModPropertyInt m_cdrOnEnlargeDiscIfCastSameTurnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NekoFlipDash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NekoFlipDash nekoFlipDash = targetAbility as NekoFlipDash;
		if (nekoFlipDash != null)
		{
			AddToken(tokens, m_dashTargetRangeMod, "DashTargetRange", string.Empty, nekoFlipDash.m_dashTargetRange);
			AddToken(tokens, m_maxDistanceFromTargetMod, "MaxDistanceFromTarget", string.Empty, nekoFlipDash.m_maxDistanceFromTarget);
			AddToken(tokens, m_minDistanceFromTargetMod, "MinDistanceFromTarget", string.Empty, nekoFlipDash.m_minDistanceFromTarget);
			AddToken(tokens, m_maxAngleChangeMod, "MaxAngleChange", string.Empty, nekoFlipDash.m_maxAngleChange);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, nekoFlipDash.m_laserWidth);
			AddToken(tokens, m_laserLengthMod, "LaserLength", string.Empty, nekoFlipDash.m_laserLength);
			AddToken(tokens, m_aoeRadiusAtLaserEndMod, "AoeRadiusAtLaserEnd", string.Empty, nekoFlipDash.m_aoeRadiusAtLaserEnd);
			AddToken(tokens, m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoFlipDash.m_discReturnEndRadius);
			AddToken(tokens, m_chargeRadiusMod, "ChargeRadius", string.Empty, nekoFlipDash.m_chargeRadius);
			AddToken(tokens, m_chargeRadiusAtStartMod, "ChargeRadiusAtStart", string.Empty, nekoFlipDash.m_chargeRadiusAtStart);
			AddToken(tokens, m_chargeRadiusAtEndMod, "ChargeRadiusAtEnd", string.Empty, nekoFlipDash.m_chargeRadiusAtEnd);
			AddToken(tokens, m_explosionRadiusAtTargetedDiscMod, "ExplosionRadiusAtTargetedDisc", string.Empty, nekoFlipDash.m_explosionRadiusAtTargetedDisc);
			AddToken(tokens, m_discMaxTargetsMod, "DiscMaxTargets", string.Empty, nekoFlipDash.m_discMaxTargets);
			AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", nekoFlipDash.m_effectOnSelf);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, nekoFlipDash.m_damage);
			AddToken(tokens, m_discDirectDamageMod, "DiscDirectDamage", string.Empty, nekoFlipDash.m_discDirectDamage);
			AddToken(tokens, m_discReturnTripDamageMod, "DiscReturnTripDamage", string.Empty, nekoFlipDash.m_discReturnTripDamage);
			AddToken(tokens, m_discReturnTripSubsequentHitDamageMod, "DiscReturnTripSubsequentHitDamage", string.Empty, nekoFlipDash.m_discReturnTripSubsequentHitDamage);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", nekoFlipDash.m_enemyHitEffect);
			AddToken(tokens, m_explodingTargetDiscDamageMod, "ExplodingTargetDiscDamage", string.Empty, nekoFlipDash.m_explodingTargetDiscDamage);
			AddToken(tokens, m_discsReturningThisTurnExtraDamageMod, "DiscsReturningThisTurnExtraDamage", string.Empty, nekoFlipDash.m_discsReturningThisTurnExtraDamage);
			AddToken(tokens, m_cdrIfHasReturnDiscHitMod, "CdrIfHasReturnDiscHit", string.Empty, nekoFlipDash.m_cdrIfHasReturnDiscHit);
			AddToken(tokens, m_cdrOnEnlargeDiscIfCastSameTurnMod, "CdrOnEnlargeDiscIfCastSameTurn", string.Empty, nekoFlipDash.m_cdrOnEnlargeDiscIfCastSameTurn);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoFlipDash nekoFlipDash = GetTargetAbilityOnAbilityData(abilityData) as NekoFlipDash;
		bool isValid = nekoFlipDash != null;
		string desc = string.Empty;
		desc += PropDesc(m_dashTargetRangeMod, "[DashTargetRange]", isValid, isValid ? nekoFlipDash.m_dashTargetRange : 0f);
		desc += PropDesc(m_canTargetDiscsMod, "[CanTargetDiscs]", isValid, isValid && nekoFlipDash.m_canTargetDiscs);
		desc += PropDesc(m_canTargetEnemiesMod, "[CanTargetEnemies]", isValid, isValid && nekoFlipDash.m_canTargetEnemies);
		desc += PropDesc(m_maxDistanceFromTargetMod, "[MaxDistanceFromTarget]", isValid, isValid ? nekoFlipDash.m_maxDistanceFromTarget : 0f);
		desc += PropDesc(m_minDistanceFromTargetMod, "[MinDistanceFromTarget]", isValid, isValid ? nekoFlipDash.m_minDistanceFromTarget : 0f);
		desc += PropDesc(m_maxAngleChangeMod, "[MaxAngleChange]", isValid, isValid ? nekoFlipDash.m_maxAngleChange : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? nekoFlipDash.m_laserWidth : 0f);
		desc += PropDesc(m_laserLengthMod, "[LaserLength]", isValid, isValid ? nekoFlipDash.m_laserLength : 0f);
		desc += PropDesc(m_aoeRadiusAtLaserEndMod, "[AoeRadiusAtLaserEnd]", isValid, isValid ? nekoFlipDash.m_aoeRadiusAtLaserEnd : 0f);
		desc += PropDesc(m_discReturnEndRadiusMod, "[DiscReturnEndRadius]", isValid, isValid ? nekoFlipDash.m_discReturnEndRadius : 0f);
		desc += PropDesc(m_chargeRadiusMod, "[ChargeRadius]", isValid, isValid ? nekoFlipDash.m_chargeRadius : 0f);
		desc += PropDesc(m_chargeRadiusAtStartMod, "[ChargeRadiusAtStart]", isValid, isValid ? nekoFlipDash.m_chargeRadiusAtStart : 0f);
		desc += PropDesc(m_chargeRadiusAtEndMod, "[ChargeRadiusAtEnd]", isValid, isValid ? nekoFlipDash.m_chargeRadiusAtEnd : 0f);
		desc += PropDesc(m_explosionRadiusAtTargetedDiscMod, "[ExplosionRadiusAtTargetedDisc]", isValid, isValid ? nekoFlipDash.m_explosionRadiusAtTargetedDisc : 0f);
		desc += PropDesc(m_continueToEndIfTargetEvadesMod, "[ContinueToEndIfTargetEvades]", isValid, isValid && nekoFlipDash.m_continueToEndIfTargetEvades);
		desc += PropDesc(m_leaveDiscAtStartSquareMod, "[LeaveDiscAtStartSquare]", isValid, isValid && nekoFlipDash.m_leaveDiscAtStartSquare);
		desc += PropDesc(m_throwDiscFromStartMod, "[ThrowDiscFromStart]", isValid, isValid && nekoFlipDash.m_throwDiscFromStart);
		desc += PropDesc(m_canMoveAfterEvadeMod, "[CanMoveAfterEvade]", isValid, isValid && nekoFlipDash.m_canMoveAfterEvade);
		desc += PropDesc(m_explodeTargetedDiscMod, "[ExplodeTargetedDisc]", isValid, isValid && nekoFlipDash.m_explodeTargetedDisc);
		desc += PropDesc(m_discMaxTargetsMod, "[DiscMaxTargets]", isValid, isValid ? nekoFlipDash.m_discMaxTargets : 0);
		desc += PropDesc(m_effectOnSelfMod, "[EffectOnSelf]", isValid, isValid ? nekoFlipDash.m_effectOnSelf : null);
		desc += PropDesc(m_damageMod, "[Damage]", isValid, isValid ? nekoFlipDash.m_damage : 0);
		desc += PropDesc(m_discDirectDamageMod, "[DiscDirectDamage]", isValid, isValid ? nekoFlipDash.m_discDirectDamage : 0);
		desc += PropDesc(m_discReturnTripDamageMod, "[DiscReturnTripDamage]", isValid, isValid ? nekoFlipDash.m_discReturnTripDamage : 0);
		desc += PropDesc(m_discReturnTripSubsequentHitDamageMod, "[DiscReturnTripSubsequentHitDamage]", isValid, isValid ? nekoFlipDash.m_discReturnTripSubsequentHitDamage : 0);
		desc += PropDesc(m_returnTripIgnoreCoverMod, "[ReturnTripIgnoreCover]", isValid, isValid && nekoFlipDash.m_returnTripIgnoreCover);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? nekoFlipDash.m_enemyHitEffect : null);
		desc += PropDesc(m_explodingTargetDiscDamageMod, "[ExplodingTargetDiscDamage]", isValid, isValid ? nekoFlipDash.m_explodingTargetDiscDamage : 0);
		desc += PropDesc(m_discsReturningThisTurnExtraDamageMod, "[DiscsReturningThisTurnExtraDamage]", isValid, isValid ? nekoFlipDash.m_discsReturningThisTurnExtraDamage : 0);
		desc += PropDesc(m_cdrIfHasReturnDiscHitMod, "[CdrIfHasReturnDiscHit]", isValid, isValid ? nekoFlipDash.m_cdrIfHasReturnDiscHit : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_cdrOnEnlargeDiscIfCastSameTurnMod, "[CdrOnEnlargeDiscIfCastSameTurn]", isValid, isValid ? nekoFlipDash.m_cdrOnEnlargeDiscIfCastSameTurn : 0)).ToString();
	}
}
