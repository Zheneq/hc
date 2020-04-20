using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_FishManBubble : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyShape m_targetShapeMod;

	public AbilityModPropertyBool m_canTargetEnemiesMod;

	public AbilityModPropertyBool m_canTargetAlliesMod;

	public AbilityModPropertyBool m_canTargetSelfMod;

	[Header("-- Initial Hit")]
	public AbilityModPropertyEffectInfo m_effectOnAlliesMod;

	public AbilityModPropertyEffectInfo m_effectOnEnemiesMod;

	public AbilityModPropertyInt m_initialHitHealingToAlliesMod;

	public AbilityModPropertyInt m_initialHitDamageToEnemiesMod;

	[Header("-- Explosion Data")]
	public AbilityModPropertyInt m_numTurnsBeforeFirstExplosionMod;

	public AbilityModPropertyInt m_numExplosionsBeforeEndingMod;

	public AbilityModPropertyShape m_explosionShapeMod;

	public AbilityModPropertyBool m_explosionIgnoresLineOfSightMod;

	public AbilityModPropertyBool m_explosionCanAffectEffectHolderMod;

	[Header("-- Explosion Hit")]
	public AbilityModPropertyInt m_explosionHealingToAlliesMod;

	public AbilityModPropertyInt m_explosionDamageToEnemiesMod;

	public AbilityModPropertyEffectInfo m_explosionEffectToAlliesMod;

	public AbilityModPropertyEffectInfo m_explosionEffectToEnemiesMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FishManBubble);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FishManBubble fishManBubble = targetAbility as FishManBubble;
		if (fishManBubble != null)
		{
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnAlliesMod, "EffectOnAllies", fishManBubble.m_effectOnAllies, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnEnemiesMod, "EffectOnEnemies", fishManBubble.m_effectOnEnemies, true);
			AbilityMod.AddToken(tokens, this.m_initialHitHealingToAlliesMod, "InitialHitHealingToAllies", string.Empty, fishManBubble.m_initialHitHealingToAllies, true, false);
			AbilityMod.AddToken(tokens, this.m_initialHitDamageToEnemiesMod, "InitialHitDamageToEnemies", string.Empty, fishManBubble.m_initialHitDamageToEnemies, true, false);
			AbilityMod.AddToken(tokens, this.m_numTurnsBeforeFirstExplosionMod, "NumTurnsBeforeFirstExplosionMod", string.Empty, fishManBubble.m_numTurnsBeforeFirstExplosion, true, false);
			AbilityMod.AddToken(tokens, this.m_numExplosionsBeforeEndingMod, "NumExplosionsBeforeEndingMod", string.Empty, fishManBubble.m_numExplosionsBeforeEnding, true, false);
			AbilityMod.AddToken(tokens, this.m_explosionHealingToAlliesMod, "ExplosionHealingToAllies", string.Empty, fishManBubble.m_explosionHealingToAllies, true, false);
			AbilityMod.AddToken(tokens, this.m_explosionDamageToEnemiesMod, "ExplosionDamageToEnemies", string.Empty, fishManBubble.m_explosionDamageToEnemies, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_explosionEffectToAlliesMod, "ExplosionEffectToAllies", fishManBubble.m_explosionEffectToAllies, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_explosionEffectToEnemiesMod, "ExplosionEffectToEnemies", fishManBubble.m_explosionEffectToEnemies, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManBubble fishManBubble = base.GetTargetAbilityOnAbilityData(abilityData) as FishManBubble;
		bool flag = fishManBubble != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_targetShapeMod, "[TargetShape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : fishManBubble.m_targetShape);
		string str = text;
		AbilityModPropertyBool canTargetEnemiesMod = this.m_canTargetEnemiesMod;
		string prefix = "[CanTargetEnemies]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			baseVal = fishManBubble.m_canTargetEnemies;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(canTargetEnemiesMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool canTargetAlliesMod = this.m_canTargetAlliesMod;
		string prefix2 = "[CanTargetAllies]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			baseVal2 = fishManBubble.m_canTargetAllies;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(canTargetAlliesMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_canTargetSelfMod, "[CanTargetSelf]", flag, flag && fishManBubble.m_canTargetSelf);
		text += base.PropDesc(this.m_effectOnAlliesMod, "[EffectOnAllies]", flag, (!flag) ? null : fishManBubble.m_effectOnAllies);
		text += base.PropDesc(this.m_effectOnEnemiesMod, "[EffectOnEnemies]", flag, (!flag) ? null : fishManBubble.m_effectOnEnemies);
		string str3 = text;
		AbilityModPropertyInt initialHitHealingToAlliesMod = this.m_initialHitHealingToAlliesMod;
		string prefix3 = "[InitialHitHealingToAllies]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = fishManBubble.m_initialHitHealingToAllies;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(initialHitHealingToAlliesMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt initialHitDamageToEnemiesMod = this.m_initialHitDamageToEnemiesMod;
		string prefix4 = "[InitialHitDamageToEnemies]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = fishManBubble.m_initialHitDamageToEnemies;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(initialHitDamageToEnemiesMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt numTurnsBeforeFirstExplosionMod = this.m_numTurnsBeforeFirstExplosionMod;
		string prefix5 = "[NumTurnsBeforeFirstExplosionMod]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = fishManBubble.m_numTurnsBeforeFirstExplosion;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(numTurnsBeforeFirstExplosionMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt numExplosionsBeforeEndingMod = this.m_numExplosionsBeforeEndingMod;
		string prefix6 = "[NumExplosionsBeforeEndingMod]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = fishManBubble.m_numExplosionsBeforeEnding;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(numExplosionsBeforeEndingMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyShape explosionShapeMod = this.m_explosionShapeMod;
		string prefix7 = "[ExplosionShape]";
		bool showBaseVal7 = flag;
		AbilityAreaShape baseVal7;
		if (flag)
		{
			baseVal7 = fishManBubble.m_explosionShape;
		}
		else
		{
			baseVal7 = AbilityAreaShape.SingleSquare;
		}
		text = str7 + base.PropDesc(explosionShapeMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyBool explosionIgnoresLineOfSightMod = this.m_explosionIgnoresLineOfSightMod;
		string prefix8 = "[ExplosionIgnoresLineOfSight]";
		bool showBaseVal8 = flag;
		bool baseVal8;
		if (flag)
		{
			baseVal8 = fishManBubble.m_explosionIgnoresLineOfSight;
		}
		else
		{
			baseVal8 = false;
		}
		text = str8 + base.PropDesc(explosionIgnoresLineOfSightMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyBool explosionCanAffectEffectHolderMod = this.m_explosionCanAffectEffectHolderMod;
		string prefix9 = "[ExplosionCanAffectEffectHolder]";
		bool showBaseVal9 = flag;
		bool baseVal9;
		if (flag)
		{
			baseVal9 = fishManBubble.m_explosionCanAffectEffectHolder;
		}
		else
		{
			baseVal9 = false;
		}
		text = str9 + base.PropDesc(explosionCanAffectEffectHolderMod, prefix9, showBaseVal9, baseVal9);
		text += base.PropDesc(this.m_explosionHealingToAlliesMod, "[ExplosionHealingToAllies]", flag, (!flag) ? 0 : fishManBubble.m_explosionHealingToAllies);
		string str10 = text;
		AbilityModPropertyInt explosionDamageToEnemiesMod = this.m_explosionDamageToEnemiesMod;
		string prefix10 = "[ExplosionDamageToEnemies]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = fishManBubble.m_explosionDamageToEnemies;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(explosionDamageToEnemiesMod, prefix10, showBaseVal10, baseVal10);
		text += base.PropDesc(this.m_explosionEffectToAlliesMod, "[ExplosionEffectToAllies]", flag, (!flag) ? null : fishManBubble.m_explosionEffectToAllies);
		string str11 = text;
		AbilityModPropertyEffectInfo explosionEffectToEnemiesMod = this.m_explosionEffectToEnemiesMod;
		string prefix11 = "[ExplosionEffectToEnemies]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal11;
		if (flag)
		{
			baseVal11 = fishManBubble.m_explosionEffectToEnemies;
		}
		else
		{
			baseVal11 = null;
		}
		return str11 + base.PropDesc(explosionEffectToEnemiesMod, prefix11, showBaseVal11, baseVal11);
	}
}
