using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_FishManGeyser : AbilityMod
{
	[Header("-- Layered Shape Override")]
	public bool m_useAdditionalShapeOverride;

	public List<FishManGeyser.ShapeToDamage> m_additionalShapeToDamageOverride = new List<FishManGeyser.ShapeToDamage>();

	[Header("-- Initial Cast")]
	public AbilityModPropertyShape m_castShapeMod;

	public AbilityModPropertyBool m_castPenetratesLoSMod;

	public AbilityModPropertyInt m_damageToEnemiesOnCastMod;

	public AbilityModPropertyInt m_healingToAlliesOnCastMod;

	public AbilityModPropertyInt m_healOnCasterPerEnemyHitMod;

	[Space(10f)]
	public AbilityModPropertyEffectInfo m_effectToEnemiesOnCastMod;

	public AbilityModPropertyEffectInfo m_effectToAlliesOnCastMod;

	[Header("-- Knockback on Cast")]
	public AbilityModPropertyBool m_applyKnockbackOnCastMod;

	public AbilityModPropertyFloat m_knockbackDistOnCastMod;

	public AbilityModPropertyKnockbackType m_knockbackTypeOnCastMod;

	[Header("-- Effect on Enemies on start of Next Turn")]
	public AbilityModPropertyEffectInfo m_enemyEffectOnNextTurnMod;

	[Header("-- Eel effect on enemies hit")]
	public AbilityModPropertyBool m_applyEelEffectOnEnemiesMod;

	public AbilityModPropertyInt m_eelDamageMod;

	public AbilityModPropertyEffectInfo m_eelEffectOnEnemiesMod;

	public AbilityModPropertyFloat m_eelRadiusMod;

	[Header("-- Explosion Timing (may be depricated if not needed)")]
	public AbilityModPropertyInt m_turnsTillFirstExplosionMod;

	public AbilityModPropertyInt m_numExplosionsBeforeEndingMod;

	[Header("-- Effect Explode (may be depricated if not needed)")]
	public AbilityModPropertyShape m_explodeShapeMod;

	public AbilityModPropertyBool m_explodePenetratesLoSMod;

	public AbilityModPropertyInt m_damageToEnemiesOnExplodeMod;

	public AbilityModPropertyInt m_healingToAlliesOnExplodeMod;

	public AbilityModPropertyBool m_applyKnockbackOnExplodeMod;

	public AbilityModPropertyFloat m_knockbackDistOnExplodeMod;

	public AbilityModPropertyEffectInfo m_effectToEnemiesOnExplodeMod;

	public AbilityModPropertyEffectInfo m_effectToAlliesOnExplodeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FishManGeyser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FishManGeyser fishManGeyser = targetAbility as FishManGeyser;
		if (!(fishManGeyser != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_damageToEnemiesOnCastMod, "DamageToEnemiesOnCast", string.Empty, fishManGeyser.m_damageToEnemiesOnCast);
			AbilityMod.AddToken(tokens, m_healingToAlliesOnCastMod, "HealingToAlliesOnCast", string.Empty, fishManGeyser.m_healingToAlliesOnCast);
			AbilityMod.AddToken(tokens, m_healOnCasterPerEnemyHitMod, "HealOnCasterPerEnemyHit", string.Empty, fishManGeyser.m_healOnCasterPerEnemyHit);
			AbilityMod.AddToken(tokens, m_knockbackDistOnCastMod, "KnockbackDistOnCast", string.Empty, fishManGeyser.m_knockbackDistOnCast);
			AbilityMod.AddToken_EffectMod(tokens, m_effectToEnemiesOnCastMod, "EffectToEnemiesOnCast", fishManGeyser.m_effectToEnemiesOnCast);
			AbilityMod.AddToken_EffectMod(tokens, m_effectToAlliesOnCastMod, "EffectToAlliesOnCast", fishManGeyser.m_effectToAlliesOnCast);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyEffectOnNextTurnMod, "EnemyEffectOnNextTurn", fishManGeyser.m_enemyEffectOnNextTurn);
			AbilityMod.AddToken(tokens, m_eelDamageMod, "EelDamage", string.Empty, fishManGeyser.m_eelDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_eelEffectOnEnemiesMod, "EelEffectOnEnemies", fishManGeyser.m_eelEffectOnEnemies);
			AbilityMod.AddToken(tokens, m_eelRadiusMod, "EelRadius", string.Empty, fishManGeyser.m_eelRadius);
			AbilityMod.AddToken(tokens, m_turnsTillFirstExplosionMod, "TurnsTillFirstExplosion", string.Empty, fishManGeyser.m_turnsTillFirstExplosion);
			AbilityMod.AddToken(tokens, m_numExplosionsBeforeEndingMod, "NumExplosionsBeforeEnding", string.Empty, fishManGeyser.m_numExplosionsBeforeEnding);
			AbilityMod.AddToken(tokens, m_damageToEnemiesOnExplodeMod, "DamageToEnemiesOnExplode", string.Empty, fishManGeyser.m_damageToEnemiesOnExplode);
			AbilityMod.AddToken(tokens, m_healingToAlliesOnExplodeMod, "HealingToAlliesOnExplode", string.Empty, fishManGeyser.m_healingToAlliesOnExplode);
			AbilityMod.AddToken(tokens, m_knockbackDistOnExplodeMod, "KnockbackDistOnExplode", string.Empty, fishManGeyser.m_knockbackDistOnExplode);
			AbilityMod.AddToken_EffectMod(tokens, m_effectToEnemiesOnExplodeMod, "EffectToEnemiesOnExplode", fishManGeyser.m_effectToEnemiesOnExplode);
			AbilityMod.AddToken_EffectMod(tokens, m_effectToAlliesOnExplodeMod, "EffectToAlliesOnExplode", fishManGeyser.m_effectToAlliesOnExplode);
			if (!m_useAdditionalShapeOverride)
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
				if (m_additionalShapeToDamageOverride == null)
				{
					return;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					for (int i = 0; i < m_additionalShapeToDamageOverride.Count; i++)
					{
						AbilityMod.AddToken_IntDiff(tokens, "Damage_AdditionalLayer" + i, string.Empty, m_additionalShapeToDamageOverride[i].m_damage, true, fishManGeyser.m_damageToEnemiesOnCast);
					}
					while (true)
					{
						switch (1)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManGeyser fishManGeyser = GetTargetAbilityOnAbilityData(abilityData) as FishManGeyser;
		bool flag = fishManGeyser != null;
		string text = string.Empty;
		if (m_useAdditionalShapeOverride)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_additionalShapeToDamageOverride != null)
			{
				text += "Using Layered Shape Override, entries:\n";
				for (int i = 0; i < m_additionalShapeToDamageOverride.Count; i++)
				{
					string text2 = text;
					text = string.Concat(text2, "Shape: ", m_additionalShapeToDamageOverride[i].m_shape, " Damage: ", m_additionalShapeToDamageOverride[i].m_damage, "\n");
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		text += PropDesc(m_castShapeMod, "[CastShape]", flag, flag ? fishManGeyser.m_castShape : AbilityAreaShape.SingleSquare);
		string str = text;
		AbilityModPropertyBool castPenetratesLoSMod = m_castPenetratesLoSMod;
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
			baseVal = (fishManGeyser.m_castPenetratesLoS ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		text = str + PropDesc(castPenetratesLoSMod, "[CastPenetratesLoS]", flag, (byte)baseVal != 0);
		string str2 = text;
		AbilityModPropertyInt damageToEnemiesOnCastMod = m_damageToEnemiesOnCastMod;
		int baseVal2;
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
			baseVal2 = fishManGeyser.m_damageToEnemiesOnCast;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + PropDesc(damageToEnemiesOnCastMod, "[DamageToEnemiesOnCast]", flag, baseVal2);
		text += PropDesc(m_healingToAlliesOnCastMod, "[HealingToAlliesOnCast]", flag, flag ? fishManGeyser.m_healingToAlliesOnCast : 0);
		string str3 = text;
		AbilityModPropertyInt healOnCasterPerEnemyHitMod = m_healOnCasterPerEnemyHitMod;
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
			baseVal3 = fishManGeyser.m_healOnCasterPerEnemyHit;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + PropDesc(healOnCasterPerEnemyHitMod, "[HealOnCasterPerEnemyHit]", flag, baseVal3);
		text += PropDesc(m_applyKnockbackOnCastMod, "[ApplyKnockbackOnCast]", flag, flag && fishManGeyser.m_applyKnockbackOnCast);
		string str4 = text;
		AbilityModPropertyFloat knockbackDistOnCastMod = m_knockbackDistOnCastMod;
		float baseVal4;
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
			baseVal4 = fishManGeyser.m_knockbackDistOnCast;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + PropDesc(knockbackDistOnCastMod, "[KnockbackDistOnCast]", flag, baseVal4);
		string str5 = text;
		AbilityModPropertyKnockbackType knockbackTypeOnCastMod = m_knockbackTypeOnCastMod;
		int baseVal5;
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
			baseVal5 = (int)fishManGeyser.m_knockbackTypeOnCast;
		}
		else
		{
			baseVal5 = 4;
		}
		text = str5 + PropDesc(knockbackTypeOnCastMod, "[KnockbackTypeOnCast]", flag, (KnockbackType)baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo enemyEffectOnNextTurnMod = m_enemyEffectOnNextTurnMod;
		object baseVal6;
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
			baseVal6 = fishManGeyser.m_enemyEffectOnNextTurn;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + PropDesc(enemyEffectOnNextTurnMod, "[EnemyEffectOnNextTurn]", flag, (StandardEffectInfo)baseVal6);
		string str7 = text;
		AbilityModPropertyBool applyEelEffectOnEnemiesMod = m_applyEelEffectOnEnemiesMod;
		int baseVal7;
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
			baseVal7 = (fishManGeyser.m_applyEelEffectOnEnemies ? 1 : 0);
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + PropDesc(applyEelEffectOnEnemiesMod, "[ApplyEelEffectOnEnemies]", flag, (byte)baseVal7 != 0);
		string str8 = text;
		AbilityModPropertyInt eelDamageMod = m_eelDamageMod;
		int baseVal8;
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
			baseVal8 = fishManGeyser.m_eelDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + PropDesc(eelDamageMod, "[EelDamage]", flag, baseVal8);
		string str9 = text;
		AbilityModPropertyEffectInfo eelEffectOnEnemiesMod = m_eelEffectOnEnemiesMod;
		object baseVal9;
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
			baseVal9 = fishManGeyser.m_eelEffectOnEnemies;
		}
		else
		{
			baseVal9 = null;
		}
		text = str9 + PropDesc(eelEffectOnEnemiesMod, "[EelEffectOnEnemies]", flag, (StandardEffectInfo)baseVal9);
		text += PropDesc(m_eelRadiusMod, "[EelRadius]", flag, (!flag) ? 0f : fishManGeyser.m_eelRadius);
		string str10 = text;
		AbilityModPropertyEffectInfo effectToEnemiesOnCastMod = m_effectToEnemiesOnCastMod;
		object baseVal10;
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
			baseVal10 = fishManGeyser.m_effectToEnemiesOnCast;
		}
		else
		{
			baseVal10 = null;
		}
		text = str10 + PropDesc(effectToEnemiesOnCastMod, "[EffectToEnemiesOnCast]", flag, (StandardEffectInfo)baseVal10);
		string str11 = text;
		AbilityModPropertyEffectInfo effectToAlliesOnCastMod = m_effectToAlliesOnCastMod;
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
			baseVal11 = fishManGeyser.m_effectToAlliesOnCast;
		}
		else
		{
			baseVal11 = null;
		}
		text = str11 + PropDesc(effectToAlliesOnCastMod, "[EffectToAlliesOnCast]", flag, (StandardEffectInfo)baseVal11);
		text += PropDesc(m_turnsTillFirstExplosionMod, "[TurnsTillFirstExplosion]", flag, flag ? fishManGeyser.m_turnsTillFirstExplosion : 0);
		string str12 = text;
		AbilityModPropertyInt numExplosionsBeforeEndingMod = m_numExplosionsBeforeEndingMod;
		int baseVal12;
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
			baseVal12 = fishManGeyser.m_numExplosionsBeforeEnding;
		}
		else
		{
			baseVal12 = 0;
		}
		text = str12 + PropDesc(numExplosionsBeforeEndingMod, "[NumExplosionsBeforeEnding]", flag, baseVal12);
		string str13 = text;
		AbilityModPropertyShape explodeShapeMod = m_explodeShapeMod;
		int baseVal13;
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
			baseVal13 = (int)fishManGeyser.m_explodeShape;
		}
		else
		{
			baseVal13 = 0;
		}
		text = str13 + PropDesc(explodeShapeMod, "[ExplodeShape]", flag, (AbilityAreaShape)baseVal13);
		string str14 = text;
		AbilityModPropertyBool explodePenetratesLoSMod = m_explodePenetratesLoSMod;
		int baseVal14;
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
			baseVal14 = (fishManGeyser.m_explodePenetratesLoS ? 1 : 0);
		}
		else
		{
			baseVal14 = 0;
		}
		text = str14 + PropDesc(explodePenetratesLoSMod, "[ExplodePenetratesLoS]", flag, (byte)baseVal14 != 0);
		string str15 = text;
		AbilityModPropertyInt damageToEnemiesOnExplodeMod = m_damageToEnemiesOnExplodeMod;
		int baseVal15;
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
			baseVal15 = fishManGeyser.m_damageToEnemiesOnExplode;
		}
		else
		{
			baseVal15 = 0;
		}
		text = str15 + PropDesc(damageToEnemiesOnExplodeMod, "[DamageToEnemiesOnExplode]", flag, baseVal15);
		text += PropDesc(m_healingToAlliesOnExplodeMod, "[HealingToAlliesOnExplode]", flag, flag ? fishManGeyser.m_healingToAlliesOnExplode : 0);
		string str16 = text;
		AbilityModPropertyBool applyKnockbackOnExplodeMod = m_applyKnockbackOnExplodeMod;
		int baseVal16;
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
			baseVal16 = (fishManGeyser.m_applyKnockbackOnExplode ? 1 : 0);
		}
		else
		{
			baseVal16 = 0;
		}
		text = str16 + PropDesc(applyKnockbackOnExplodeMod, "[ApplyKnockbackOnExplode]", flag, (byte)baseVal16 != 0);
		string str17 = text;
		AbilityModPropertyFloat knockbackDistOnExplodeMod = m_knockbackDistOnExplodeMod;
		float baseVal17;
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
			baseVal17 = fishManGeyser.m_knockbackDistOnExplode;
		}
		else
		{
			baseVal17 = 0f;
		}
		text = str17 + PropDesc(knockbackDistOnExplodeMod, "[KnockbackDistOnExplode]", flag, baseVal17);
		string str18 = text;
		AbilityModPropertyEffectInfo effectToEnemiesOnExplodeMod = m_effectToEnemiesOnExplodeMod;
		object baseVal18;
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
			baseVal18 = fishManGeyser.m_effectToEnemiesOnExplode;
		}
		else
		{
			baseVal18 = null;
		}
		text = str18 + PropDesc(effectToEnemiesOnExplodeMod, "[EffectToEnemiesOnExplode]", flag, (StandardEffectInfo)baseVal18);
		string str19 = text;
		AbilityModPropertyEffectInfo effectToAlliesOnExplodeMod = m_effectToAlliesOnExplodeMod;
		object baseVal19;
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
			baseVal19 = fishManGeyser.m_effectToAlliesOnExplode;
		}
		else
		{
			baseVal19 = null;
		}
		return str19 + PropDesc(effectToAlliesOnExplodeMod, "[EffectToAlliesOnExplode]", flag, (StandardEffectInfo)baseVal19);
	}
}
