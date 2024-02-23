using System;
using System.Collections.Generic;
using System.Text;
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
				AddToken(tokens, m_mineDamageMod, "Damage_OnMoveOver", string.Empty, component.m_damageAmount);
				if (m_directHitDamagePerShot != null)
				{
					for (int i = 0; i < m_directHitDamagePerShot.Count; i++)
					{
						AddToken(tokens, m_directHitDamagePerShot[i], new StringBuilder().Append("Damage_DirectHit_").Append(i).ToString(), string.Empty, component.m_directHitDamageAmount);
					}
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsMultiTargeterBasicAttack gremlinsMultiTargeterBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as GremlinsMultiTargeterBasicAttack;
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent = gremlinsMultiTargeterBasicAttack != null
			? gremlinsMultiTargeterBasicAttack.GetComponent<GremlinsLandMineInfoComponent>()
			: null;
		bool isAbilityPresent = gremlinsLandMineInfoComponent != null;
		string desc = string.Empty;
		for (int i = 0; i < m_directHitDamagePerShot.Count; i++)
		{
			desc += AbilityModHelper.GetModPropertyDesc(m_directHitDamagePerShot[i], new StringBuilder().Append("[Direct Hit Damage Per Shot ").Append(i).Append("]").ToString(), isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_directHitDamageAmount : 0);
		}
		desc += AbilityModHelper.GetModPropertyDesc(m_bombShapeMod, "[Bomb Shape]", isAbilityPresent, isAbilityPresent ? gremlinsMultiTargeterBasicAttack.m_bombShape : AbilityAreaShape.SingleSquare);
		desc += AbilityModHelper.GetModPropertyDesc(m_maxAngleWithFirst, "[Max Angle With First]", isAbilityPresent, isAbilityPresent ? gremlinsMultiTargeterBasicAttack.m_maxAngleWithFirst : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_minDistBetweenBombsMod, "[Min Dist Between Bombs]", isAbilityPresent, isAbilityPresent ? gremlinsMultiTargeterBasicAttack.m_minDistanceBetweenBombs : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_useShapeForDeadzoneMod, "[Use Shape for Deadzone]", isAbilityPresent, isAbilityPresent && gremlinsMultiTargeterBasicAttack.m_useShapeForDeadzone);
		if (m_useShapeForDeadzoneMod.GetModifiedValue(gremlinsMultiTargeterBasicAttack.m_useShapeForDeadzone))
		{
			desc += AbilityModHelper.GetModPropertyDesc(m_deadzoneShapeMod, "[Deadzone Shape]", isAbilityPresent, isAbilityPresent ? gremlinsMultiTargeterBasicAttack.m_deadZoneShape : AbilityAreaShape.SingleSquare);
		}
		desc += AbilityModHelper.GetModPropertyDesc(m_mineDamageMod, "[Mine Damage]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_mineDurationMod, "[Mine Duration]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_mineDuration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_enemyHitEffect : null);
		return new StringBuilder().Append(desc).Append(AbilityModHelper.GetModPropertyDesc(m_energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_energyGainOnExplosion : 0)).ToString();
	}
}
