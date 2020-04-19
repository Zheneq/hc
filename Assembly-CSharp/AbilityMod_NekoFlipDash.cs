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
		if (nekoFlipDash != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NekoFlipDash.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_dashTargetRangeMod, "DashTargetRange", string.Empty, nekoFlipDash.m_dashTargetRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxDistanceFromTargetMod, "MaxDistanceFromTarget", string.Empty, nekoFlipDash.m_maxDistanceFromTarget, true, false, false);
			AbilityMod.AddToken(tokens, this.m_minDistanceFromTargetMod, "MinDistanceFromTarget", string.Empty, nekoFlipDash.m_minDistanceFromTarget, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxAngleChangeMod, "MaxAngleChange", string.Empty, nekoFlipDash.m_maxAngleChange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, nekoFlipDash.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserLengthMod, "LaserLength", string.Empty, nekoFlipDash.m_laserLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_aoeRadiusAtLaserEndMod, "AoeRadiusAtLaserEnd", string.Empty, nekoFlipDash.m_aoeRadiusAtLaserEnd, true, false, false);
			AbilityMod.AddToken(tokens, this.m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoFlipDash.m_discReturnEndRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_chargeRadiusMod, "ChargeRadius", string.Empty, nekoFlipDash.m_chargeRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_chargeRadiusAtStartMod, "ChargeRadiusAtStart", string.Empty, nekoFlipDash.m_chargeRadiusAtStart, true, false, false);
			AbilityMod.AddToken(tokens, this.m_chargeRadiusAtEndMod, "ChargeRadiusAtEnd", string.Empty, nekoFlipDash.m_chargeRadiusAtEnd, true, false, false);
			AbilityMod.AddToken(tokens, this.m_explosionRadiusAtTargetedDiscMod, "ExplosionRadiusAtTargetedDisc", string.Empty, nekoFlipDash.m_explosionRadiusAtTargetedDisc, true, false, false);
			AbilityMod.AddToken(tokens, this.m_discMaxTargetsMod, "DiscMaxTargets", string.Empty, nekoFlipDash.m_discMaxTargets, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnSelfMod, "EffectOnSelf", nekoFlipDash.m_effectOnSelf, true);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, nekoFlipDash.m_damage, true, false);
			AbilityMod.AddToken(tokens, this.m_discDirectDamageMod, "DiscDirectDamage", string.Empty, nekoFlipDash.m_discDirectDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_discReturnTripDamageMod, "DiscReturnTripDamage", string.Empty, nekoFlipDash.m_discReturnTripDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_discReturnTripSubsequentHitDamageMod, "DiscReturnTripSubsequentHitDamage", string.Empty, nekoFlipDash.m_discReturnTripSubsequentHitDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", nekoFlipDash.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_explodingTargetDiscDamageMod, "ExplodingTargetDiscDamage", string.Empty, nekoFlipDash.m_explodingTargetDiscDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_discsReturningThisTurnExtraDamageMod, "DiscsReturningThisTurnExtraDamage", string.Empty, nekoFlipDash.m_discsReturningThisTurnExtraDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrIfHasReturnDiscHitMod, "CdrIfHasReturnDiscHit", string.Empty, nekoFlipDash.m_cdrIfHasReturnDiscHit, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrOnEnlargeDiscIfCastSameTurnMod, "CdrOnEnlargeDiscIfCastSameTurn", string.Empty, nekoFlipDash.m_cdrOnEnlargeDiscIfCastSameTurn, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoFlipDash nekoFlipDash = base.GetTargetAbilityOnAbilityData(abilityData) as NekoFlipDash;
		bool flag = nekoFlipDash != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat dashTargetRangeMod = this.m_dashTargetRangeMod;
		string prefix = "[DashTargetRange]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NekoFlipDash.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = nekoFlipDash.m_dashTargetRange;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(dashTargetRangeMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool canTargetDiscsMod = this.m_canTargetDiscsMod;
		string prefix2 = "[CanTargetDiscs]";
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
			baseVal2 = nekoFlipDash.m_canTargetDiscs;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(canTargetDiscsMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool canTargetEnemiesMod = this.m_canTargetEnemiesMod;
		string prefix3 = "[CanTargetEnemies]";
		bool showBaseVal3 = flag;
		bool baseVal3;
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
			baseVal3 = nekoFlipDash.m_canTargetEnemies;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(canTargetEnemiesMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_maxDistanceFromTargetMod, "[MaxDistanceFromTarget]", flag, (!flag) ? 0f : nekoFlipDash.m_maxDistanceFromTarget);
		string str4 = text;
		AbilityModPropertyFloat minDistanceFromTargetMod = this.m_minDistanceFromTargetMod;
		string prefix4 = "[MinDistanceFromTarget]";
		bool showBaseVal4 = flag;
		float baseVal4;
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
			baseVal4 = nekoFlipDash.m_minDistanceFromTarget;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(minDistanceFromTargetMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyFloat maxAngleChangeMod = this.m_maxAngleChangeMod;
		string prefix5 = "[MaxAngleChange]";
		bool showBaseVal5 = flag;
		float baseVal5;
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
			baseVal5 = nekoFlipDash.m_maxAngleChange;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(maxAngleChangeMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : nekoFlipDash.m_laserWidth);
		string str6 = text;
		AbilityModPropertyFloat laserLengthMod = this.m_laserLengthMod;
		string prefix6 = "[LaserLength]";
		bool showBaseVal6 = flag;
		float baseVal6;
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
			baseVal6 = nekoFlipDash.m_laserLength;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(laserLengthMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyFloat aoeRadiusAtLaserEndMod = this.m_aoeRadiusAtLaserEndMod;
		string prefix7 = "[AoeRadiusAtLaserEnd]";
		bool showBaseVal7 = flag;
		float baseVal7;
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
			baseVal7 = nekoFlipDash.m_aoeRadiusAtLaserEnd;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + base.PropDesc(aoeRadiusAtLaserEndMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyFloat discReturnEndRadiusMod = this.m_discReturnEndRadiusMod;
		string prefix8 = "[DiscReturnEndRadius]";
		bool showBaseVal8 = flag;
		float baseVal8;
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
			baseVal8 = nekoFlipDash.m_discReturnEndRadius;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str8 + base.PropDesc(discReturnEndRadiusMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyFloat chargeRadiusMod = this.m_chargeRadiusMod;
		string prefix9 = "[ChargeRadius]";
		bool showBaseVal9 = flag;
		float baseVal9;
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
			baseVal9 = nekoFlipDash.m_chargeRadius;
		}
		else
		{
			baseVal9 = 0f;
		}
		text = str9 + base.PropDesc(chargeRadiusMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyFloat chargeRadiusAtStartMod = this.m_chargeRadiusAtStartMod;
		string prefix10 = "[ChargeRadiusAtStart]";
		bool showBaseVal10 = flag;
		float baseVal10;
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
			baseVal10 = nekoFlipDash.m_chargeRadiusAtStart;
		}
		else
		{
			baseVal10 = 0f;
		}
		text = str10 + base.PropDesc(chargeRadiusAtStartMod, prefix10, showBaseVal10, baseVal10);
		text += base.PropDesc(this.m_chargeRadiusAtEndMod, "[ChargeRadiusAtEnd]", flag, (!flag) ? 0f : nekoFlipDash.m_chargeRadiusAtEnd);
		string str11 = text;
		AbilityModPropertyFloat explosionRadiusAtTargetedDiscMod = this.m_explosionRadiusAtTargetedDiscMod;
		string prefix11 = "[ExplosionRadiusAtTargetedDisc]";
		bool showBaseVal11 = flag;
		float baseVal11;
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
			baseVal11 = nekoFlipDash.m_explosionRadiusAtTargetedDisc;
		}
		else
		{
			baseVal11 = 0f;
		}
		text = str11 + base.PropDesc(explosionRadiusAtTargetedDiscMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyBool continueToEndIfTargetEvadesMod = this.m_continueToEndIfTargetEvadesMod;
		string prefix12 = "[ContinueToEndIfTargetEvades]";
		bool showBaseVal12 = flag;
		bool baseVal12;
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
			baseVal12 = nekoFlipDash.m_continueToEndIfTargetEvades;
		}
		else
		{
			baseVal12 = false;
		}
		text = str12 + base.PropDesc(continueToEndIfTargetEvadesMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyBool leaveDiscAtStartSquareMod = this.m_leaveDiscAtStartSquareMod;
		string prefix13 = "[LeaveDiscAtStartSquare]";
		bool showBaseVal13 = flag;
		bool baseVal13;
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
			baseVal13 = nekoFlipDash.m_leaveDiscAtStartSquare;
		}
		else
		{
			baseVal13 = false;
		}
		text = str13 + base.PropDesc(leaveDiscAtStartSquareMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyBool throwDiscFromStartMod = this.m_throwDiscFromStartMod;
		string prefix14 = "[ThrowDiscFromStart]";
		bool showBaseVal14 = flag;
		bool baseVal14;
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
			baseVal14 = nekoFlipDash.m_throwDiscFromStart;
		}
		else
		{
			baseVal14 = false;
		}
		text = str14 + base.PropDesc(throwDiscFromStartMod, prefix14, showBaseVal14, baseVal14);
		string str15 = text;
		AbilityModPropertyBool canMoveAfterEvadeMod = this.m_canMoveAfterEvadeMod;
		string prefix15 = "[CanMoveAfterEvade]";
		bool showBaseVal15 = flag;
		bool baseVal15;
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
			baseVal15 = nekoFlipDash.m_canMoveAfterEvade;
		}
		else
		{
			baseVal15 = false;
		}
		text = str15 + base.PropDesc(canMoveAfterEvadeMod, prefix15, showBaseVal15, baseVal15);
		string str16 = text;
		AbilityModPropertyBool explodeTargetedDiscMod = this.m_explodeTargetedDiscMod;
		string prefix16 = "[ExplodeTargetedDisc]";
		bool showBaseVal16 = flag;
		bool baseVal16;
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
			baseVal16 = nekoFlipDash.m_explodeTargetedDisc;
		}
		else
		{
			baseVal16 = false;
		}
		text = str16 + base.PropDesc(explodeTargetedDiscMod, prefix16, showBaseVal16, baseVal16);
		string str17 = text;
		AbilityModPropertyInt discMaxTargetsMod = this.m_discMaxTargetsMod;
		string prefix17 = "[DiscMaxTargets]";
		bool showBaseVal17 = flag;
		int baseVal17;
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
			baseVal17 = nekoFlipDash.m_discMaxTargets;
		}
		else
		{
			baseVal17 = 0;
		}
		text = str17 + base.PropDesc(discMaxTargetsMod, prefix17, showBaseVal17, baseVal17);
		string str18 = text;
		AbilityModPropertyEffectInfo effectOnSelfMod = this.m_effectOnSelfMod;
		string prefix18 = "[EffectOnSelf]";
		bool showBaseVal18 = flag;
		StandardEffectInfo baseVal18;
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
			baseVal18 = nekoFlipDash.m_effectOnSelf;
		}
		else
		{
			baseVal18 = null;
		}
		text = str18 + base.PropDesc(effectOnSelfMod, prefix18, showBaseVal18, baseVal18);
		string str19 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix19 = "[Damage]";
		bool showBaseVal19 = flag;
		int baseVal19;
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
			baseVal19 = nekoFlipDash.m_damage;
		}
		else
		{
			baseVal19 = 0;
		}
		text = str19 + base.PropDesc(damageMod, prefix19, showBaseVal19, baseVal19);
		string str20 = text;
		AbilityModPropertyInt discDirectDamageMod = this.m_discDirectDamageMod;
		string prefix20 = "[DiscDirectDamage]";
		bool showBaseVal20 = flag;
		int baseVal20;
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
			baseVal20 = nekoFlipDash.m_discDirectDamage;
		}
		else
		{
			baseVal20 = 0;
		}
		text = str20 + base.PropDesc(discDirectDamageMod, prefix20, showBaseVal20, baseVal20);
		text += base.PropDesc(this.m_discReturnTripDamageMod, "[DiscReturnTripDamage]", flag, (!flag) ? 0 : nekoFlipDash.m_discReturnTripDamage);
		string str21 = text;
		AbilityModPropertyInt discReturnTripSubsequentHitDamageMod = this.m_discReturnTripSubsequentHitDamageMod;
		string prefix21 = "[DiscReturnTripSubsequentHitDamage]";
		bool showBaseVal21 = flag;
		int baseVal21;
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
			baseVal21 = nekoFlipDash.m_discReturnTripSubsequentHitDamage;
		}
		else
		{
			baseVal21 = 0;
		}
		text = str21 + base.PropDesc(discReturnTripSubsequentHitDamageMod, prefix21, showBaseVal21, baseVal21);
		string str22 = text;
		AbilityModPropertyBool returnTripIgnoreCoverMod = this.m_returnTripIgnoreCoverMod;
		string prefix22 = "[ReturnTripIgnoreCover]";
		bool showBaseVal22 = flag;
		bool baseVal22;
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
			baseVal22 = nekoFlipDash.m_returnTripIgnoreCover;
		}
		else
		{
			baseVal22 = false;
		}
		text = str22 + base.PropDesc(returnTripIgnoreCoverMod, prefix22, showBaseVal22, baseVal22);
		string str23 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix23 = "[EnemyHitEffect]";
		bool showBaseVal23 = flag;
		StandardEffectInfo baseVal23;
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
			baseVal23 = nekoFlipDash.m_enemyHitEffect;
		}
		else
		{
			baseVal23 = null;
		}
		text = str23 + base.PropDesc(enemyHitEffectMod, prefix23, showBaseVal23, baseVal23);
		string str24 = text;
		AbilityModPropertyInt explodingTargetDiscDamageMod = this.m_explodingTargetDiscDamageMod;
		string prefix24 = "[ExplodingTargetDiscDamage]";
		bool showBaseVal24 = flag;
		int baseVal24;
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
			baseVal24 = nekoFlipDash.m_explodingTargetDiscDamage;
		}
		else
		{
			baseVal24 = 0;
		}
		text = str24 + base.PropDesc(explodingTargetDiscDamageMod, prefix24, showBaseVal24, baseVal24);
		text += base.PropDesc(this.m_discsReturningThisTurnExtraDamageMod, "[DiscsReturningThisTurnExtraDamage]", flag, (!flag) ? 0 : nekoFlipDash.m_discsReturningThisTurnExtraDamage);
		text += base.PropDesc(this.m_cdrIfHasReturnDiscHitMod, "[CdrIfHasReturnDiscHit]", flag, (!flag) ? 0 : nekoFlipDash.m_cdrIfHasReturnDiscHit);
		string str25 = text;
		AbilityModPropertyInt cdrOnEnlargeDiscIfCastSameTurnMod = this.m_cdrOnEnlargeDiscIfCastSameTurnMod;
		string prefix25 = "[CdrOnEnlargeDiscIfCastSameTurn]";
		bool showBaseVal25 = flag;
		int baseVal25;
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
			baseVal25 = nekoFlipDash.m_cdrOnEnlargeDiscIfCastSameTurn;
		}
		else
		{
			baseVal25 = 0;
		}
		return str25 + base.PropDesc(cdrOnEnlargeDiscIfCastSameTurnMod, prefix25, showBaseVal25, baseVal25);
	}
}
