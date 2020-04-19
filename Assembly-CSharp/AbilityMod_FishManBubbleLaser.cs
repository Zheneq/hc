using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_FishManBubbleLaser : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

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

	[Header("-- Explosion Results")]
	public AbilityModPropertyInt m_explosionHealingToAlliesMod;

	public AbilityModPropertyInt m_explosionDamageToEnemiesMod;

	public AbilityModPropertyEffectInfo m_explosionEffectToAlliesMod;

	public AbilityModPropertyEffectInfo m_explosionEffectToEnemiesMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FishManBubbleLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FishManBubbleLaser fishManBubbleLaser = targetAbility as FishManBubbleLaser;
		if (fishManBubbleLaser != null)
		{
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", fishManBubbleLaser.m_laserInfo, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnAlliesMod, "EffectOnAllies", fishManBubbleLaser.m_effectOnAllies, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnEnemiesMod, "EffectOnEnemies", fishManBubbleLaser.m_effectOnEnemies, true);
			AbilityMod.AddToken(tokens, this.m_initialHitHealingToAlliesMod, "InitialHitHealingToAllies", string.Empty, fishManBubbleLaser.m_initialHitHealingToAllies, true, false);
			AbilityMod.AddToken(tokens, this.m_initialHitDamageToEnemiesMod, "InitialHitDamageToEnemies", string.Empty, fishManBubbleLaser.m_initialHitDamageToEnemies, true, false);
			AbilityMod.AddToken(tokens, this.m_numTurnsBeforeFirstExplosionMod, "NumTurnsBeforeFirstExplosionMod", string.Empty, fishManBubbleLaser.m_numTurnsBeforeFirstExplosion, true, false);
			AbilityMod.AddToken(tokens, this.m_numExplosionsBeforeEndingMod, "NumExplosionsBeforeEndingMod", string.Empty, fishManBubbleLaser.m_numExplosionsBeforeEnding, true, false);
			AbilityMod.AddToken(tokens, this.m_explosionHealingToAlliesMod, "ExplosionHealingToAllies", string.Empty, fishManBubbleLaser.m_explosionHealingToAllies, true, false);
			AbilityMod.AddToken(tokens, this.m_explosionDamageToEnemiesMod, "ExplosionDamageToEnemies", string.Empty, fishManBubbleLaser.m_explosionDamageToEnemies, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_explosionEffectToAlliesMod, "ExplosionEffectToAllies", fishManBubbleLaser.m_explosionEffectToAllies, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_explosionEffectToEnemiesMod, "ExplosionEffectToEnemies", fishManBubbleLaser.m_explosionEffectToEnemies, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManBubbleLaser fishManBubbleLaser = base.GetTargetAbilityOnAbilityData(abilityData) as FishManBubbleLaser;
		bool flag = fishManBubbleLaser != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_laserInfoMod, "[LaserInfo]", flag, (!flag) ? null : fishManBubbleLaser.m_laserInfo);
		string str = text;
		AbilityModPropertyEffectInfo effectOnAlliesMod = this.m_effectOnAlliesMod;
		string prefix = "[EffectOnAllies]";
		bool showBaseVal = flag;
		StandardEffectInfo baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_FishManBubbleLaser.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = fishManBubbleLaser.m_effectOnAllies;
		}
		else
		{
			baseVal = null;
		}
		text = str + base.PropDesc(effectOnAlliesMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyEffectInfo effectOnEnemiesMod = this.m_effectOnEnemiesMod;
		string prefix2 = "[EffectOnEnemies]";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal2;
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
			baseVal2 = fishManBubbleLaser.m_effectOnEnemies;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(effectOnEnemiesMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt initialHitHealingToAlliesMod = this.m_initialHitHealingToAlliesMod;
		string prefix3 = "[InitialHitHealingToAllies]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = fishManBubbleLaser.m_initialHitHealingToAllies;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(initialHitHealingToAlliesMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_initialHitDamageToEnemiesMod, "[InitialHitDamageToEnemies]", flag, (!flag) ? 0 : fishManBubbleLaser.m_initialHitDamageToEnemies);
		string str4 = text;
		AbilityModPropertyInt numTurnsBeforeFirstExplosionMod = this.m_numTurnsBeforeFirstExplosionMod;
		string prefix4 = "[NumTurnsBeforeFirstExplosionMod]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = fishManBubbleLaser.m_numTurnsBeforeFirstExplosion;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(numTurnsBeforeFirstExplosionMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_numExplosionsBeforeEndingMod, "[NumExplosionsBeforeEndingMod]", flag, (!flag) ? 0 : fishManBubbleLaser.m_numExplosionsBeforeEnding);
		text += base.PropDesc(this.m_explosionShapeMod, "[ExplosionShape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : fishManBubbleLaser.m_explosionShape);
		string str5 = text;
		AbilityModPropertyBool explosionIgnoresLineOfSightMod = this.m_explosionIgnoresLineOfSightMod;
		string prefix5 = "[ExplosionIgnoresLineOfSight]";
		bool showBaseVal5 = flag;
		bool baseVal5;
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
			baseVal5 = fishManBubbleLaser.m_explosionIgnoresLineOfSight;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(explosionIgnoresLineOfSightMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_explosionCanAffectEffectHolderMod, "[ExplosionCanAffectEffectHolder]", flag, flag && fishManBubbleLaser.m_explosionCanAffectEffectHolder);
		string str6 = text;
		AbilityModPropertyInt explosionHealingToAlliesMod = this.m_explosionHealingToAlliesMod;
		string prefix6 = "[ExplosionHealingToAllies]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = fishManBubbleLaser.m_explosionHealingToAllies;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(explosionHealingToAlliesMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt explosionDamageToEnemiesMod = this.m_explosionDamageToEnemiesMod;
		string prefix7 = "[ExplosionDamageToEnemies]";
		bool showBaseVal7 = flag;
		int baseVal7;
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
			baseVal7 = fishManBubbleLaser.m_explosionDamageToEnemies;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(explosionDamageToEnemiesMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyEffectInfo explosionEffectToAlliesMod = this.m_explosionEffectToAlliesMod;
		string prefix8 = "[ExplosionEffectToAllies]";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
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
			baseVal8 = fishManBubbleLaser.m_explosionEffectToAllies;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + base.PropDesc(explosionEffectToAlliesMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyEffectInfo explosionEffectToEnemiesMod = this.m_explosionEffectToEnemiesMod;
		string prefix9 = "[ExplosionEffectToEnemies]";
		bool showBaseVal9 = flag;
		StandardEffectInfo baseVal9;
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
			baseVal9 = fishManBubbleLaser.m_explosionEffectToEnemies;
		}
		else
		{
			baseVal9 = null;
		}
		return str9 + base.PropDesc(explosionEffectToEnemiesMod, prefix9, showBaseVal9, baseVal9);
	}
}
