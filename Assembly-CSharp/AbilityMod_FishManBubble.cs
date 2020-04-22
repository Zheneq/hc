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
		if (!(fishManBubble != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnAlliesMod, "EffectOnAllies", fishManBubble.m_effectOnAllies);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnEnemiesMod, "EffectOnEnemies", fishManBubble.m_effectOnEnemies);
			AbilityMod.AddToken(tokens, m_initialHitHealingToAlliesMod, "InitialHitHealingToAllies", string.Empty, fishManBubble.m_initialHitHealingToAllies);
			AbilityMod.AddToken(tokens, m_initialHitDamageToEnemiesMod, "InitialHitDamageToEnemies", string.Empty, fishManBubble.m_initialHitDamageToEnemies);
			AbilityMod.AddToken(tokens, m_numTurnsBeforeFirstExplosionMod, "NumTurnsBeforeFirstExplosionMod", string.Empty, fishManBubble.m_numTurnsBeforeFirstExplosion);
			AbilityMod.AddToken(tokens, m_numExplosionsBeforeEndingMod, "NumExplosionsBeforeEndingMod", string.Empty, fishManBubble.m_numExplosionsBeforeEnding);
			AbilityMod.AddToken(tokens, m_explosionHealingToAlliesMod, "ExplosionHealingToAllies", string.Empty, fishManBubble.m_explosionHealingToAllies);
			AbilityMod.AddToken(tokens, m_explosionDamageToEnemiesMod, "ExplosionDamageToEnemies", string.Empty, fishManBubble.m_explosionDamageToEnemies);
			AbilityMod.AddToken_EffectMod(tokens, m_explosionEffectToAlliesMod, "ExplosionEffectToAllies", fishManBubble.m_explosionEffectToAllies);
			AbilityMod.AddToken_EffectMod(tokens, m_explosionEffectToEnemiesMod, "ExplosionEffectToEnemies", fishManBubble.m_explosionEffectToEnemies);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManBubble fishManBubble = GetTargetAbilityOnAbilityData(abilityData) as FishManBubble;
		bool flag = fishManBubble != null;
		string empty = string.Empty;
		empty += PropDesc(m_targetShapeMod, "[TargetShape]", flag, flag ? fishManBubble.m_targetShape : AbilityAreaShape.SingleSquare);
		string str = empty;
		AbilityModPropertyBool canTargetEnemiesMod = m_canTargetEnemiesMod;
		int baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = (fishManBubble.m_canTargetEnemies ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(canTargetEnemiesMod, "[CanTargetEnemies]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyBool canTargetAlliesMod = m_canTargetAlliesMod;
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
			baseVal2 = (fishManBubble.m_canTargetAllies ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(canTargetAlliesMod, "[CanTargetAllies]", flag, (byte)baseVal2 != 0);
		empty += PropDesc(m_canTargetSelfMod, "[CanTargetSelf]", flag, flag && fishManBubble.m_canTargetSelf);
		empty += PropDesc(m_effectOnAlliesMod, "[EffectOnAllies]", flag, (!flag) ? null : fishManBubble.m_effectOnAllies);
		empty += PropDesc(m_effectOnEnemiesMod, "[EffectOnEnemies]", flag, (!flag) ? null : fishManBubble.m_effectOnEnemies);
		string str3 = empty;
		AbilityModPropertyInt initialHitHealingToAlliesMod = m_initialHitHealingToAlliesMod;
		int baseVal3;
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
			baseVal3 = fishManBubble.m_initialHitHealingToAllies;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(initialHitHealingToAlliesMod, "[InitialHitHealingToAllies]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt initialHitDamageToEnemiesMod = m_initialHitDamageToEnemiesMod;
		int baseVal4;
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
			baseVal4 = fishManBubble.m_initialHitDamageToEnemies;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(initialHitDamageToEnemiesMod, "[InitialHitDamageToEnemies]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt numTurnsBeforeFirstExplosionMod = m_numTurnsBeforeFirstExplosionMod;
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
			baseVal5 = fishManBubble.m_numTurnsBeforeFirstExplosion;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(numTurnsBeforeFirstExplosionMod, "[NumTurnsBeforeFirstExplosionMod]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt numExplosionsBeforeEndingMod = m_numExplosionsBeforeEndingMod;
		int baseVal6;
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
			baseVal6 = fishManBubble.m_numExplosionsBeforeEnding;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(numExplosionsBeforeEndingMod, "[NumExplosionsBeforeEndingMod]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyShape explosionShapeMod = m_explosionShapeMod;
		int baseVal7;
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
			baseVal7 = (int)fishManBubble.m_explosionShape;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(explosionShapeMod, "[ExplosionShape]", flag, (AbilityAreaShape)baseVal7);
		string str8 = empty;
		AbilityModPropertyBool explosionIgnoresLineOfSightMod = m_explosionIgnoresLineOfSightMod;
		int baseVal8;
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
			baseVal8 = (fishManBubble.m_explosionIgnoresLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(explosionIgnoresLineOfSightMod, "[ExplosionIgnoresLineOfSight]", flag, (byte)baseVal8 != 0);
		string str9 = empty;
		AbilityModPropertyBool explosionCanAffectEffectHolderMod = m_explosionCanAffectEffectHolderMod;
		int baseVal9;
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
			baseVal9 = (fishManBubble.m_explosionCanAffectEffectHolder ? 1 : 0);
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(explosionCanAffectEffectHolderMod, "[ExplosionCanAffectEffectHolder]", flag, (byte)baseVal9 != 0);
		empty += PropDesc(m_explosionHealingToAlliesMod, "[ExplosionHealingToAllies]", flag, flag ? fishManBubble.m_explosionHealingToAllies : 0);
		string str10 = empty;
		AbilityModPropertyInt explosionDamageToEnemiesMod = m_explosionDamageToEnemiesMod;
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
			baseVal10 = fishManBubble.m_explosionDamageToEnemies;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(explosionDamageToEnemiesMod, "[ExplosionDamageToEnemies]", flag, baseVal10);
		empty += PropDesc(m_explosionEffectToAlliesMod, "[ExplosionEffectToAllies]", flag, (!flag) ? null : fishManBubble.m_explosionEffectToAllies);
		string str11 = empty;
		AbilityModPropertyEffectInfo explosionEffectToEnemiesMod = m_explosionEffectToEnemiesMod;
		object baseVal11;
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
			baseVal11 = fishManBubble.m_explosionEffectToEnemies;
		}
		else
		{
			baseVal11 = null;
		}
		return str11 + PropDesc(explosionEffectToEnemiesMod, "[ExplosionEffectToEnemies]", flag, (StandardEffectInfo)baseVal11);
	}
}
