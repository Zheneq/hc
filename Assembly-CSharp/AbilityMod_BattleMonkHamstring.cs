using System;
using System.Collections.Generic;
using System.Text;
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
		if (battleMonkHamstring != null)
		{
			AddToken(tokens, m_laserDamageMod, "LaserDamageAmount", string.Empty, battleMonkHamstring.m_laserDamageAmount);
			AddToken(tokens, m_damageAfterFirstHitMod, "DamageAfterFirstHit", string.Empty, battleMonkHamstring.m_damageAfterFirstHit);
			AddToken(tokens, m_widthMod, "LaserInfo_Width", string.Empty, battleMonkHamstring.m_laserInfo.width);
			AddToken(tokens, m_rangeMod, "LaserInfo_Range", string.Empty, battleMonkHamstring.m_laserInfo.range);
			AddToken(tokens, m_maxTargetMod, "LaserInfo_MaxTargets", string.Empty, battleMonkHamstring.m_laserInfo.maxTargets);
			AddToken(tokens, m_explosionDamageMod, "ExplosionDamageAmount", string.Empty, battleMonkHamstring.m_explosionDamageAmount);
			if (m_useLaserHitEffectOverride)
			{
				AddToken_EffectInfo(tokens, m_laserHitEffectOverride, "LaserHitEffect", battleMonkHamstring.m_laserHitEffect);
			}
			if (m_useExplosionHitEffectOverride)
			{
				AddToken_EffectInfo(tokens, m_explosionHitEffectOverride, "ExplosionHitEffect", battleMonkHamstring.m_explosionHitEffect);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BattleMonkHamstring battleMonkHamstring = GetTargetAbilityOnAbilityData(abilityData) as BattleMonkHamstring;
		bool isAbilityPresent = battleMonkHamstring != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_laserDamageMod, "[Laser Damage]", isAbilityPresent, isAbilityPresent ? battleMonkHamstring.m_laserDamageAmount : 0);
		desc += PropDesc(m_damageAfterFirstHitMod, "[DamageAfterFirstHit]", isAbilityPresent, isAbilityPresent ? battleMonkHamstring.m_damageAfterFirstHit : 0);
		if (m_useLaserHitEffectOverride)
		{
			desc += "Overrideing Laser Hit Effect\n";
			desc += AbilityModHelper.GetModEffectInfoDesc(m_laserHitEffectOverride, "{ Effect Override On Laser Hit }", string.Empty, isAbilityPresent, isAbilityPresent ? battleMonkHamstring.m_laserHitEffect : null);
		}

		desc += AbilityModHelper.GetModPropertyDesc(m_widthMod, "[Laser Width]", isAbilityPresent, isAbilityPresent ? battleMonkHamstring.m_laserInfo.width : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_rangeMod, "[Laser Range]", isAbilityPresent, isAbilityPresent ? battleMonkHamstring.m_laserInfo.range : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_maxTargetMod, "[Max Targets]", isAbilityPresent, isAbilityPresent ? battleMonkHamstring.m_laserInfo.maxTargets : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_explodeOnActorHitMod, "[Should Explode On Actor Hit?]", isAbilityPresent, isAbilityPresent && battleMonkHamstring.m_explodeOnActorHit);
		desc += AbilityModHelper.GetModPropertyDesc(m_explosionDamageMod, "[Explosion Damage]", isAbilityPresent, isAbilityPresent ? battleMonkHamstring.m_explosionDamageAmount : 0);
		if (m_useExplosionHitEffectOverride)
		{
			desc += "Overriding Explosion Hit Effect\n";
			desc += AbilityModHelper.GetModEffectInfoDesc(m_explosionHitEffectOverride, "{ Effect Override on Explosion Hit }", string.Empty, isAbilityPresent, isAbilityPresent ? battleMonkHamstring.m_explosionHitEffect : null);
		}

		desc += AbilityModHelper.GetModPropertyDesc(m_explodeShapeMod, "[Explode Shape]", isAbilityPresent, isAbilityPresent ? battleMonkHamstring.m_explodeShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_maxBounces, "[Max Laser Bounces]", isAbilityPresent);
		desc += PropDesc(m_distancePerBounce, "[Distance Per Bounce]", isAbilityPresent);
		if (m_projectileSequencePrefab != null && m_projectileSequencePrefab.operation == AbilityModPropertySequenceOverride.ModOp.Override)
		{
			string text = string.Empty;
			if (isAbilityPresent && battleMonkHamstring.m_projectileSequencePrefab != null)
			{
				text = new StringBuilder().Append(" (base was ").Append(battleMonkHamstring.m_projectileSequencePrefab.name).Append(")").ToString();
			}

			desc += new StringBuilder().Append("[Projectile Sequence Override] = ").Append(m_projectileSequencePrefab.value.name).Append(text).Append("\n").ToString();
		}
		return desc;
	}
}
