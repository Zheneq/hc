using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BattleMonkHamstring : AbilityMod
{
	[Header("-- Laser Damage and Targeting Mod")]
	public AbilityModPropertyInt m_laserDamageMod;

	public AbilityModPropertyInt m_damageAfterFirstHitMod;

	[Space(10f)]
	public AbilityModPropertyFloat m_widthMod;

	public AbilityModPropertyFloat m_rangeMod;

	public AbilityModPropertyInt m_maxTargetMod;

	public bool m_useLaserHitEffectOverride;

	public StandardEffectInfo m_laserHitEffectOverride;

	[Header("-- Laser Explosion, Damage and Effects")]
	public AbilityModPropertyInt m_explosionDamageMod;

	public AbilityModPropertyBool m_explodeOnActorHitMod;

	public AbilityModPropertyShape m_explodeShapeMod;

	public bool m_useExplosionHitEffectOverride;

	public StandardEffectInfo m_explosionHitEffectOverride;

	[Header("-- Laser Bounce")]
	public AbilityModPropertyInt m_maxBounces;

	public AbilityModPropertyFloat m_distancePerBounce;

	public AbilityModPropertySequenceOverride m_projectileSequencePrefab;

	public override Type GetTargetAbilityType()
	{
		return typeof(BattleMonkHamstring);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BattleMonkHamstring battleMonkHamstring = targetAbility as BattleMonkHamstring;
		if (!(battleMonkHamstring != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_laserDamageMod, "LaserDamageAmount", string.Empty, battleMonkHamstring.m_laserDamageAmount);
			AbilityMod.AddToken(tokens, m_damageAfterFirstHitMod, "DamageAfterFirstHit", string.Empty, battleMonkHamstring.m_damageAfterFirstHit);
			AbilityMod.AddToken(tokens, m_widthMod, "LaserInfo_Width", string.Empty, battleMonkHamstring.m_laserInfo.width);
			AbilityMod.AddToken(tokens, m_rangeMod, "LaserInfo_Range", string.Empty, battleMonkHamstring.m_laserInfo.range);
			AbilityMod.AddToken(tokens, m_maxTargetMod, "LaserInfo_MaxTargets", string.Empty, battleMonkHamstring.m_laserInfo.maxTargets);
			AbilityMod.AddToken(tokens, m_explosionDamageMod, "ExplosionDamageAmount", string.Empty, battleMonkHamstring.m_explosionDamageAmount);
			if (m_useLaserHitEffectOverride)
			{
				AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffectOverride, "LaserHitEffect", battleMonkHamstring.m_laserHitEffect);
			}
			if (m_useExplosionHitEffectOverride)
			{
				AbilityMod.AddToken_EffectInfo(tokens, m_explosionHitEffectOverride, "ExplosionHitEffect", battleMonkHamstring.m_explosionHitEffect);
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BattleMonkHamstring battleMonkHamstring = GetTargetAbilityOnAbilityData(abilityData) as BattleMonkHamstring;
		bool flag = battleMonkHamstring != null;
		string empty = string.Empty;
		empty += AbilityModHelper.GetModPropertyDesc(m_laserDamageMod, "[Laser Damage]", flag, flag ? battleMonkHamstring.m_laserDamageAmount : 0);
		string str = empty;
		AbilityModPropertyInt damageAfterFirstHitMod = m_damageAfterFirstHitMod;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = battleMonkHamstring.m_damageAfterFirstHit;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(damageAfterFirstHitMod, "[DamageAfterFirstHit]", flag, baseVal);
		if (m_useLaserHitEffectOverride)
		{
			empty += "Overrideing Laser Hit Effect\n";
			empty += AbilityModHelper.GetModEffectInfoDesc(m_laserHitEffectOverride, "{ Effect Override On Laser Hit }", string.Empty, flag, (!flag) ? null : battleMonkHamstring.m_laserHitEffect);
		}
		string str2 = empty;
		AbilityModPropertyFloat widthMod = m_widthMod;
		float baseVal2;
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
			baseVal2 = battleMonkHamstring.m_laserInfo.width;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(widthMod, "[Laser Width]", flag, baseVal2);
		empty += AbilityModHelper.GetModPropertyDesc(m_rangeMod, "[Laser Range]", flag, (!flag) ? 0f : battleMonkHamstring.m_laserInfo.range);
		string str3 = empty;
		AbilityModPropertyInt maxTargetMod = m_maxTargetMod;
		int baseVal3;
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
			baseVal3 = battleMonkHamstring.m_laserInfo.maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(maxTargetMod, "[Max Targets]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyBool explodeOnActorHitMod = m_explodeOnActorHitMod;
		int baseVal4;
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
			baseVal4 = (battleMonkHamstring.m_explodeOnActorHit ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(explodeOnActorHitMod, "[Should Explode On Actor Hit?]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyInt explosionDamageMod = m_explosionDamageMod;
		int baseVal5;
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
			baseVal5 = battleMonkHamstring.m_explosionDamageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(explosionDamageMod, "[Explosion Damage]", flag, baseVal5);
		if (m_useExplosionHitEffectOverride)
		{
			empty += "Overriding Explosion Hit Effect\n";
			string str6 = empty;
			StandardEffectInfo explosionHitEffectOverride = m_explosionHitEffectOverride;
			string empty2 = string.Empty;
			object baseVal6;
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
				baseVal6 = battleMonkHamstring.m_explosionHitEffect;
			}
			else
			{
				baseVal6 = null;
			}
			empty = str6 + AbilityModHelper.GetModEffectInfoDesc(explosionHitEffectOverride, "{ Effect Override on Explosion Hit }", empty2, flag, (StandardEffectInfo)baseVal6);
		}
		string str7 = empty;
		AbilityModPropertyShape explodeShapeMod = m_explodeShapeMod;
		int baseVal7;
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
			baseVal7 = (int)battleMonkHamstring.m_explodeShape;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + AbilityModHelper.GetModPropertyDesc(explodeShapeMod, "[Explode Shape]", flag, (AbilityAreaShape)baseVal7);
		empty += PropDesc(m_maxBounces, "[Max Laser Bounces]", flag);
		empty += PropDesc(m_distancePerBounce, "[Distance Per Bounce]", flag);
		if (m_projectileSequencePrefab != null)
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
			if (m_projectileSequencePrefab.operation == AbilityModPropertySequenceOverride.ModOp.Override)
			{
				string text = string.Empty;
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
					if (battleMonkHamstring.m_projectileSequencePrefab != null)
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
						text = " (base was " + battleMonkHamstring.m_projectileSequencePrefab.name + ")";
					}
				}
				string text2 = empty;
				empty = text2 + "[Projectile Sequence Override] = " + m_projectileSequencePrefab.value.name + text + "\n";
			}
		}
		return empty;
	}
}
