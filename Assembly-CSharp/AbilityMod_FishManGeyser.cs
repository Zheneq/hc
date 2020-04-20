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
		if (fishManGeyser != null)
		{
			AbilityMod.AddToken(tokens, this.m_damageToEnemiesOnCastMod, "DamageToEnemiesOnCast", string.Empty, fishManGeyser.m_damageToEnemiesOnCast, true, false);
			AbilityMod.AddToken(tokens, this.m_healingToAlliesOnCastMod, "HealingToAlliesOnCast", string.Empty, fishManGeyser.m_healingToAlliesOnCast, true, false);
			AbilityMod.AddToken(tokens, this.m_healOnCasterPerEnemyHitMod, "HealOnCasterPerEnemyHit", string.Empty, fishManGeyser.m_healOnCasterPerEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistOnCastMod, "KnockbackDistOnCast", string.Empty, fishManGeyser.m_knockbackDistOnCast, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectToEnemiesOnCastMod, "EffectToEnemiesOnCast", fishManGeyser.m_effectToEnemiesOnCast, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectToAlliesOnCastMod, "EffectToAlliesOnCast", fishManGeyser.m_effectToAlliesOnCast, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyEffectOnNextTurnMod, "EnemyEffectOnNextTurn", fishManGeyser.m_enemyEffectOnNextTurn, true);
			AbilityMod.AddToken(tokens, this.m_eelDamageMod, "EelDamage", string.Empty, fishManGeyser.m_eelDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_eelEffectOnEnemiesMod, "EelEffectOnEnemies", fishManGeyser.m_eelEffectOnEnemies, true);
			AbilityMod.AddToken(tokens, this.m_eelRadiusMod, "EelRadius", string.Empty, fishManGeyser.m_eelRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_turnsTillFirstExplosionMod, "TurnsTillFirstExplosion", string.Empty, fishManGeyser.m_turnsTillFirstExplosion, true, false);
			AbilityMod.AddToken(tokens, this.m_numExplosionsBeforeEndingMod, "NumExplosionsBeforeEnding", string.Empty, fishManGeyser.m_numExplosionsBeforeEnding, true, false);
			AbilityMod.AddToken(tokens, this.m_damageToEnemiesOnExplodeMod, "DamageToEnemiesOnExplode", string.Empty, fishManGeyser.m_damageToEnemiesOnExplode, true, false);
			AbilityMod.AddToken(tokens, this.m_healingToAlliesOnExplodeMod, "HealingToAlliesOnExplode", string.Empty, fishManGeyser.m_healingToAlliesOnExplode, true, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistOnExplodeMod, "KnockbackDistOnExplode", string.Empty, fishManGeyser.m_knockbackDistOnExplode, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectToEnemiesOnExplodeMod, "EffectToEnemiesOnExplode", fishManGeyser.m_effectToEnemiesOnExplode, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectToAlliesOnExplodeMod, "EffectToAlliesOnExplode", fishManGeyser.m_effectToAlliesOnExplode, true);
			if (this.m_useAdditionalShapeOverride)
			{
				if (this.m_additionalShapeToDamageOverride != null)
				{
					for (int i = 0; i < this.m_additionalShapeToDamageOverride.Count; i++)
					{
						AbilityMod.AddToken_IntDiff(tokens, "Damage_AdditionalLayer" + i, string.Empty, this.m_additionalShapeToDamageOverride[i].m_damage, true, fishManGeyser.m_damageToEnemiesOnCast);
					}
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManGeyser fishManGeyser = base.GetTargetAbilityOnAbilityData(abilityData) as FishManGeyser;
		bool flag = fishManGeyser != null;
		string text = string.Empty;
		if (this.m_useAdditionalShapeOverride)
		{
			if (this.m_additionalShapeToDamageOverride != null)
			{
				text += "Using Layered Shape Override, entries:\n";
				for (int i = 0; i < this.m_additionalShapeToDamageOverride.Count; i++)
				{
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"Shape: ",
						this.m_additionalShapeToDamageOverride[i].m_shape,
						" Damage: ",
						this.m_additionalShapeToDamageOverride[i].m_damage,
						"\n"
					});
				}
			}
		}
		text += base.PropDesc(this.m_castShapeMod, "[CastShape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : fishManGeyser.m_castShape);
		string str = text;
		AbilityModPropertyBool castPenetratesLoSMod = this.m_castPenetratesLoSMod;
		string prefix = "[CastPenetratesLoS]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			baseVal = fishManGeyser.m_castPenetratesLoS;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(castPenetratesLoSMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt damageToEnemiesOnCastMod = this.m_damageToEnemiesOnCastMod;
		string prefix2 = "[DamageToEnemiesOnCast]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = fishManGeyser.m_damageToEnemiesOnCast;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(damageToEnemiesOnCastMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_healingToAlliesOnCastMod, "[HealingToAlliesOnCast]", flag, (!flag) ? 0 : fishManGeyser.m_healingToAlliesOnCast);
		string str3 = text;
		AbilityModPropertyInt healOnCasterPerEnemyHitMod = this.m_healOnCasterPerEnemyHitMod;
		string prefix3 = "[HealOnCasterPerEnemyHit]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = fishManGeyser.m_healOnCasterPerEnemyHit;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(healOnCasterPerEnemyHitMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_applyKnockbackOnCastMod, "[ApplyKnockbackOnCast]", flag, flag && fishManGeyser.m_applyKnockbackOnCast);
		string str4 = text;
		AbilityModPropertyFloat knockbackDistOnCastMod = this.m_knockbackDistOnCastMod;
		string prefix4 = "[KnockbackDistOnCast]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = fishManGeyser.m_knockbackDistOnCast;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(knockbackDistOnCastMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyKnockbackType knockbackTypeOnCastMod = this.m_knockbackTypeOnCastMod;
		string prefix5 = "[KnockbackTypeOnCast]";
		bool showBaseVal5 = flag;
		KnockbackType baseVal5;
		if (flag)
		{
			baseVal5 = fishManGeyser.m_knockbackTypeOnCast;
		}
		else
		{
			baseVal5 = KnockbackType.AwayFromSource;
		}
		text = str5 + base.PropDesc(knockbackTypeOnCastMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo enemyEffectOnNextTurnMod = this.m_enemyEffectOnNextTurnMod;
		string prefix6 = "[EnemyEffectOnNextTurn]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = fishManGeyser.m_enemyEffectOnNextTurn;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(enemyEffectOnNextTurnMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyBool applyEelEffectOnEnemiesMod = this.m_applyEelEffectOnEnemiesMod;
		string prefix7 = "[ApplyEelEffectOnEnemies]";
		bool showBaseVal7 = flag;
		bool baseVal7;
		if (flag)
		{
			baseVal7 = fishManGeyser.m_applyEelEffectOnEnemies;
		}
		else
		{
			baseVal7 = false;
		}
		text = str7 + base.PropDesc(applyEelEffectOnEnemiesMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt eelDamageMod = this.m_eelDamageMod;
		string prefix8 = "[EelDamage]";
		bool showBaseVal8 = flag;
		int baseVal8;
		if (flag)
		{
			baseVal8 = fishManGeyser.m_eelDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(eelDamageMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyEffectInfo eelEffectOnEnemiesMod = this.m_eelEffectOnEnemiesMod;
		string prefix9 = "[EelEffectOnEnemies]";
		bool showBaseVal9 = flag;
		StandardEffectInfo baseVal9;
		if (flag)
		{
			baseVal9 = fishManGeyser.m_eelEffectOnEnemies;
		}
		else
		{
			baseVal9 = null;
		}
		text = str9 + base.PropDesc(eelEffectOnEnemiesMod, prefix9, showBaseVal9, baseVal9);
		text += base.PropDesc(this.m_eelRadiusMod, "[EelRadius]", flag, (!flag) ? 0f : fishManGeyser.m_eelRadius);
		string str10 = text;
		AbilityModPropertyEffectInfo effectToEnemiesOnCastMod = this.m_effectToEnemiesOnCastMod;
		string prefix10 = "[EffectToEnemiesOnCast]";
		bool showBaseVal10 = flag;
		StandardEffectInfo baseVal10;
		if (flag)
		{
			baseVal10 = fishManGeyser.m_effectToEnemiesOnCast;
		}
		else
		{
			baseVal10 = null;
		}
		text = str10 + base.PropDesc(effectToEnemiesOnCastMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyEffectInfo effectToAlliesOnCastMod = this.m_effectToAlliesOnCastMod;
		string prefix11 = "[EffectToAlliesOnCast]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal11;
		if (flag)
		{
			baseVal11 = fishManGeyser.m_effectToAlliesOnCast;
		}
		else
		{
			baseVal11 = null;
		}
		text = str11 + base.PropDesc(effectToAlliesOnCastMod, prefix11, showBaseVal11, baseVal11);
		text += base.PropDesc(this.m_turnsTillFirstExplosionMod, "[TurnsTillFirstExplosion]", flag, (!flag) ? 0 : fishManGeyser.m_turnsTillFirstExplosion);
		string str12 = text;
		AbilityModPropertyInt numExplosionsBeforeEndingMod = this.m_numExplosionsBeforeEndingMod;
		string prefix12 = "[NumExplosionsBeforeEnding]";
		bool showBaseVal12 = flag;
		int baseVal12;
		if (flag)
		{
			baseVal12 = fishManGeyser.m_numExplosionsBeforeEnding;
		}
		else
		{
			baseVal12 = 0;
		}
		text = str12 + base.PropDesc(numExplosionsBeforeEndingMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyShape explodeShapeMod = this.m_explodeShapeMod;
		string prefix13 = "[ExplodeShape]";
		bool showBaseVal13 = flag;
		AbilityAreaShape baseVal13;
		if (flag)
		{
			baseVal13 = fishManGeyser.m_explodeShape;
		}
		else
		{
			baseVal13 = AbilityAreaShape.SingleSquare;
		}
		text = str13 + base.PropDesc(explodeShapeMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyBool explodePenetratesLoSMod = this.m_explodePenetratesLoSMod;
		string prefix14 = "[ExplodePenetratesLoS]";
		bool showBaseVal14 = flag;
		bool baseVal14;
		if (flag)
		{
			baseVal14 = fishManGeyser.m_explodePenetratesLoS;
		}
		else
		{
			baseVal14 = false;
		}
		text = str14 + base.PropDesc(explodePenetratesLoSMod, prefix14, showBaseVal14, baseVal14);
		string str15 = text;
		AbilityModPropertyInt damageToEnemiesOnExplodeMod = this.m_damageToEnemiesOnExplodeMod;
		string prefix15 = "[DamageToEnemiesOnExplode]";
		bool showBaseVal15 = flag;
		int baseVal15;
		if (flag)
		{
			baseVal15 = fishManGeyser.m_damageToEnemiesOnExplode;
		}
		else
		{
			baseVal15 = 0;
		}
		text = str15 + base.PropDesc(damageToEnemiesOnExplodeMod, prefix15, showBaseVal15, baseVal15);
		text += base.PropDesc(this.m_healingToAlliesOnExplodeMod, "[HealingToAlliesOnExplode]", flag, (!flag) ? 0 : fishManGeyser.m_healingToAlliesOnExplode);
		string str16 = text;
		AbilityModPropertyBool applyKnockbackOnExplodeMod = this.m_applyKnockbackOnExplodeMod;
		string prefix16 = "[ApplyKnockbackOnExplode]";
		bool showBaseVal16 = flag;
		bool baseVal16;
		if (flag)
		{
			baseVal16 = fishManGeyser.m_applyKnockbackOnExplode;
		}
		else
		{
			baseVal16 = false;
		}
		text = str16 + base.PropDesc(applyKnockbackOnExplodeMod, prefix16, showBaseVal16, baseVal16);
		string str17 = text;
		AbilityModPropertyFloat knockbackDistOnExplodeMod = this.m_knockbackDistOnExplodeMod;
		string prefix17 = "[KnockbackDistOnExplode]";
		bool showBaseVal17 = flag;
		float baseVal17;
		if (flag)
		{
			baseVal17 = fishManGeyser.m_knockbackDistOnExplode;
		}
		else
		{
			baseVal17 = 0f;
		}
		text = str17 + base.PropDesc(knockbackDistOnExplodeMod, prefix17, showBaseVal17, baseVal17);
		string str18 = text;
		AbilityModPropertyEffectInfo effectToEnemiesOnExplodeMod = this.m_effectToEnemiesOnExplodeMod;
		string prefix18 = "[EffectToEnemiesOnExplode]";
		bool showBaseVal18 = flag;
		StandardEffectInfo baseVal18;
		if (flag)
		{
			baseVal18 = fishManGeyser.m_effectToEnemiesOnExplode;
		}
		else
		{
			baseVal18 = null;
		}
		text = str18 + base.PropDesc(effectToEnemiesOnExplodeMod, prefix18, showBaseVal18, baseVal18);
		string str19 = text;
		AbilityModPropertyEffectInfo effectToAlliesOnExplodeMod = this.m_effectToAlliesOnExplodeMod;
		string prefix19 = "[EffectToAlliesOnExplode]";
		bool showBaseVal19 = flag;
		StandardEffectInfo baseVal19;
		if (flag)
		{
			baseVal19 = fishManGeyser.m_effectToAlliesOnExplode;
		}
		else
		{
			baseVal19 = null;
		}
		return str19 + base.PropDesc(effectToAlliesOnExplodeMod, prefix19, showBaseVal19, baseVal19);
	}
}
