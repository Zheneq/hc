using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_GremlinsMultiTargeterBasicAttack : AbilityMod
{
	[Header("-- Damage and Targeted Area")]
	public List<AbilityModPropertyInt> m_directHitDamagePerShot = new List<AbilityModPropertyInt>();

	public AbilityModPropertyShape m_bombShapeMod;

	[Header("-- For Bomb Placement")]
	public AbilityModPropertyFloat m_maxAngleWithFirst;

	public AbilityModPropertyFloat m_minDistBetweenBombsMod;

	public AbilityModPropertyBool m_useShapeForDeadzoneMod;

	public AbilityModPropertyShape m_deadzoneShapeMod;

	[Header("-- Global Mine Data Mods")]
	[Space(10f)]
	public AbilityModPropertyInt m_mineDamageMod;

	public AbilityModPropertyInt m_mineDurationMod;

	public AbilityModPropertyEffectInfo m_effectOnEnemyOverride;

	public AbilityModPropertyInt m_energyOnMineExplosionMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(GremlinsMultiTargeterBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		GremlinsMultiTargeterBasicAttack gremlinsMultiTargeterBasicAttack = targetAbility as GremlinsMultiTargeterBasicAttack;
		if (gremlinsMultiTargeterBasicAttack != null)
		{
			GremlinsLandMineInfoComponent component = gremlinsMultiTargeterBasicAttack.GetComponent<GremlinsLandMineInfoComponent>();
			if (component != null)
			{
				AbilityMod.AddToken(tokens, this.m_mineDamageMod, "Damage_OnMoveOver", string.Empty, component.m_damageAmount, true, false);
				if (this.m_directHitDamagePerShot != null)
				{
					for (int i = 0; i < this.m_directHitDamagePerShot.Count; i++)
					{
						AbilityMod.AddToken(tokens, this.m_directHitDamagePerShot[i], "Damage_DirectHit_" + i, string.Empty, component.m_directHitDamageAmount, true, false);
					}
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsMultiTargeterBasicAttack gremlinsMultiTargeterBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as GremlinsMultiTargeterBasicAttack;
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent;
		if (gremlinsMultiTargeterBasicAttack != null)
		{
			gremlinsLandMineInfoComponent = gremlinsMultiTargeterBasicAttack.GetComponent<GremlinsLandMineInfoComponent>();
		}
		else
		{
			gremlinsLandMineInfoComponent = null;
		}
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent2 = gremlinsLandMineInfoComponent;
		bool flag = gremlinsLandMineInfoComponent2 != null;
		string text = string.Empty;
		for (int i = 0; i < this.m_directHitDamagePerShot.Count; i++)
		{
			string str = text;
			AbilityModPropertyInt modProp = this.m_directHitDamagePerShot[i];
			string prefix = "[Direct Hit Damage Per Shot " + i + "]";
			bool showBaseVal = flag;
			int baseVal;
			if (flag)
			{
				baseVal = gremlinsLandMineInfoComponent2.m_directHitDamageAmount;
			}
			else
			{
				baseVal = 0;
			}
			text = str + AbilityModHelper.GetModPropertyDesc(modProp, prefix, showBaseVal, baseVal);
		}
		string str2 = text;
		AbilityModPropertyShape bombShapeMod = this.m_bombShapeMod;
		string prefix2 = "[Bomb Shape]";
		bool showBaseVal2 = flag;
		AbilityAreaShape baseVal2;
		if (flag)
		{
			baseVal2 = gremlinsMultiTargeterBasicAttack.m_bombShape;
		}
		else
		{
			baseVal2 = AbilityAreaShape.SingleSquare;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(bombShapeMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat maxAngleWithFirst = this.m_maxAngleWithFirst;
		string prefix3 = "[Max Angle With First]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = gremlinsMultiTargeterBasicAttack.m_maxAngleWithFirst;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(maxAngleWithFirst, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat minDistBetweenBombsMod = this.m_minDistBetweenBombsMod;
		string prefix4 = "[Min Dist Between Bombs]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = gremlinsMultiTargeterBasicAttack.m_minDistanceBetweenBombs;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(minDistBetweenBombsMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool useShapeForDeadzoneMod = this.m_useShapeForDeadzoneMod;
		string prefix5 = "[Use Shape for Deadzone]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = gremlinsMultiTargeterBasicAttack.m_useShapeForDeadzone;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(useShapeForDeadzoneMod, prefix5, showBaseVal5, baseVal5);
		if (this.m_useShapeForDeadzoneMod.GetModifiedValue(gremlinsMultiTargeterBasicAttack.m_useShapeForDeadzone))
		{
			text += AbilityModHelper.GetModPropertyDesc(this.m_deadzoneShapeMod, "[Deadzone Shape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : gremlinsMultiTargeterBasicAttack.m_deadZoneShape);
		}
		text += AbilityModHelper.GetModPropertyDesc(this.m_mineDamageMod, "[Mine Damage]", flag, (!flag) ? 0 : gremlinsLandMineInfoComponent2.m_damageAmount);
		text += AbilityModHelper.GetModPropertyDesc(this.m_mineDurationMod, "[Mine Duration]", flag, (!flag) ? 0 : gremlinsLandMineInfoComponent2.m_mineDuration);
		string str6 = text;
		AbilityModPropertyEffectInfo effectOnEnemyOverride = this.m_effectOnEnemyOverride;
		string prefix6 = "{ Effect on Enemy Hit Override }";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = gremlinsLandMineInfoComponent2.m_enemyHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(effectOnEnemyOverride, prefix6, showBaseVal6, baseVal6);
		return text + AbilityModHelper.GetModPropertyDesc(this.m_energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", flag, (!flag) ? 0 : gremlinsLandMineInfoComponent2.m_energyGainOnExplosion);
	}
}
