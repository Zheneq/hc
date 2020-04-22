using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TricksterMadeYouLook : AbilityMod
{
	[Header("-- Target Actors In-Between")]
	public AbilityModPropertyBool m_hitActorsInBetweenMod;

	public AbilityModPropertyFloat m_radiusFromLineMod;

	public AbilityModPropertyFloat m_radiusAroundEndsMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- Cooldown Reduction for Hitting Targets In Between --")]
	public AbilityModCooldownReduction m_cooldownReductionForTravelHit;

	[Header("-- Enemy Hit Damage and Effect")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyEffectInfo m_enemyOnHitEffectMod;

	[Header("-- Spoils to spawn on clone disappear --")]
	public AbilityModPropertySpoilsSpawnData m_spoilsSpawnDataOnDisappear;

	public override Type GetTargetAbilityType()
	{
		return typeof(TricksterMadeYouLook);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TricksterMadeYouLook tricksterMadeYouLook = targetAbility as TricksterMadeYouLook;
		if (tricksterMadeYouLook != null)
		{
			AbilityMod.AddToken(tokens, m_radiusFromLineMod, "RadiusFromLine", string.Empty, tricksterMadeYouLook.m_radiusFromLine);
			AbilityMod.AddToken(tokens, m_radiusAroundEndsMod, "RadiusAroundEnds", string.Empty, tricksterMadeYouLook.m_radiusAroundEnds);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, tricksterMadeYouLook.m_damageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyOnHitEffectMod, "EnemyOnHitEffect", tricksterMadeYouLook.m_enemyOnHitEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterMadeYouLook tricksterMadeYouLook = GetTargetAbilityOnAbilityData(abilityData) as TricksterMadeYouLook;
		bool flag = tricksterMadeYouLook != null;
		string text = string.Empty;
		if (m_spoilsSpawnDataOnDisappear != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			text += PropDesc(m_spoilsSpawnDataOnDisappear, "[SpoilSpawnDataOnDisappear]");
		}
		string str = text;
		AbilityModPropertyBool hitActorsInBetweenMod = m_hitActorsInBetweenMod;
		int baseVal;
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
			baseVal = (tricksterMadeYouLook.m_hitActorsInBetween ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		text = str + PropDesc(hitActorsInBetweenMod, "[HitActorsInBetween]", flag, (byte)baseVal != 0);
		text += PropDesc(m_radiusFromLineMod, "[RadiusFromLine]", flag, (!flag) ? 0f : tricksterMadeYouLook.m_radiusFromLine);
		text += PropDesc(m_radiusAroundEndsMod, "[RadiusAroundEnds]", flag, (!flag) ? 0f : tricksterMadeYouLook.m_radiusAroundEnds);
		string str2 = text;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal2;
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
			baseVal2 = (tricksterMadeYouLook.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + PropDesc(penetrateLosMod, "[PenetrateLos]", flag, (byte)baseVal2 != 0);
		if (m_cooldownReductionForTravelHit != null)
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
			if (m_cooldownReductionForTravelHit.HasCooldownReduction())
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
				text += "Cooldown Reductions For Enemy Hit In Travel:\n";
				text += m_cooldownReductionForTravelHit.GetDescription(abilityData);
			}
		}
		string str3 = text;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal3;
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
			baseVal3 = tricksterMadeYouLook.m_damageAmount;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal3);
		string str4 = text;
		AbilityModPropertyEffectInfo enemyOnHitEffectMod = m_enemyOnHitEffectMod;
		object baseVal4;
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
			baseVal4 = tricksterMadeYouLook.m_enemyOnHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		return str4 + PropDesc(enemyOnHitEffectMod, "[EnemyOnHitEffect]", flag, (StandardEffectInfo)baseVal4);
	}
}
