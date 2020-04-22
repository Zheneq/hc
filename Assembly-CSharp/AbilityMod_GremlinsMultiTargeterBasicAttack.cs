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
		if (!(gremlinsMultiTargeterBasicAttack != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GremlinsLandMineInfoComponent component = gremlinsMultiTargeterBasicAttack.GetComponent<GremlinsLandMineInfoComponent>();
			if (!(component != null))
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
				AbilityMod.AddToken(tokens, m_mineDamageMod, "Damage_OnMoveOver", string.Empty, component.m_damageAmount);
				if (m_directHitDamagePerShot == null)
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					for (int i = 0; i < m_directHitDamagePerShot.Count; i++)
					{
						AbilityMod.AddToken(tokens, m_directHitDamagePerShot[i], "Damage_DirectHit_" + i, string.Empty, component.m_directHitDamageAmount);
					}
					while (true)
					{
						switch (7)
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
		GremlinsMultiTargeterBasicAttack gremlinsMultiTargeterBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as GremlinsMultiTargeterBasicAttack;
		object obj;
		if (gremlinsMultiTargeterBasicAttack != null)
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
			obj = gremlinsMultiTargeterBasicAttack.GetComponent<GremlinsLandMineInfoComponent>();
		}
		else
		{
			obj = null;
		}
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent = (GremlinsLandMineInfoComponent)obj;
		bool flag = gremlinsLandMineInfoComponent != null;
		string text = string.Empty;
		for (int i = 0; i < m_directHitDamagePerShot.Count; i++)
		{
			string str = text;
			AbilityModPropertyInt modProp = m_directHitDamagePerShot[i];
			string prefix = "[Direct Hit Damage Per Shot " + i + "]";
			int baseVal;
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
				baseVal = gremlinsLandMineInfoComponent.m_directHitDamageAmount;
			}
			else
			{
				baseVal = 0;
			}
			text = str + AbilityModHelper.GetModPropertyDesc(modProp, prefix, flag, baseVal);
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			string str2 = text;
			AbilityModPropertyShape bombShapeMod = m_bombShapeMod;
			int baseVal2;
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
				baseVal2 = (int)gremlinsMultiTargeterBasicAttack.m_bombShape;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + AbilityModHelper.GetModPropertyDesc(bombShapeMod, "[Bomb Shape]", flag, (AbilityAreaShape)baseVal2);
			string str3 = text;
			AbilityModPropertyFloat maxAngleWithFirst = m_maxAngleWithFirst;
			float baseVal3;
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
				baseVal3 = gremlinsMultiTargeterBasicAttack.m_maxAngleWithFirst;
			}
			else
			{
				baseVal3 = 0f;
			}
			text = str3 + AbilityModHelper.GetModPropertyDesc(maxAngleWithFirst, "[Max Angle With First]", flag, baseVal3);
			string str4 = text;
			AbilityModPropertyFloat minDistBetweenBombsMod = m_minDistBetweenBombsMod;
			float baseVal4;
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
				baseVal4 = gremlinsMultiTargeterBasicAttack.m_minDistanceBetweenBombs;
			}
			else
			{
				baseVal4 = 0f;
			}
			text = str4 + AbilityModHelper.GetModPropertyDesc(minDistBetweenBombsMod, "[Min Dist Between Bombs]", flag, baseVal4);
			string str5 = text;
			AbilityModPropertyBool useShapeForDeadzoneMod = m_useShapeForDeadzoneMod;
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
				baseVal5 = (gremlinsMultiTargeterBasicAttack.m_useShapeForDeadzone ? 1 : 0);
			}
			else
			{
				baseVal5 = 0;
			}
			text = str5 + AbilityModHelper.GetModPropertyDesc(useShapeForDeadzoneMod, "[Use Shape for Deadzone]", flag, (byte)baseVal5 != 0);
			if (m_useShapeForDeadzoneMod.GetModifiedValue(gremlinsMultiTargeterBasicAttack.m_useShapeForDeadzone))
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
				text += AbilityModHelper.GetModPropertyDesc(m_deadzoneShapeMod, "[Deadzone Shape]", flag, flag ? gremlinsMultiTargeterBasicAttack.m_deadZoneShape : AbilityAreaShape.SingleSquare);
			}
			text += AbilityModHelper.GetModPropertyDesc(m_mineDamageMod, "[Mine Damage]", flag, flag ? gremlinsLandMineInfoComponent.m_damageAmount : 0);
			text += AbilityModHelper.GetModPropertyDesc(m_mineDurationMod, "[Mine Duration]", flag, flag ? gremlinsLandMineInfoComponent.m_mineDuration : 0);
			string str6 = text;
			AbilityModPropertyEffectInfo effectOnEnemyOverride = m_effectOnEnemyOverride;
			object baseVal6;
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
				baseVal6 = gremlinsLandMineInfoComponent.m_enemyHitEffect;
			}
			else
			{
				baseVal6 = null;
			}
			text = str6 + AbilityModHelper.GetModPropertyDesc(effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", flag, (StandardEffectInfo)baseVal6);
			return text + AbilityModHelper.GetModPropertyDesc(m_energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", flag, flag ? gremlinsLandMineInfoComponent.m_energyGainOnExplosion : 0);
		}
	}
}
