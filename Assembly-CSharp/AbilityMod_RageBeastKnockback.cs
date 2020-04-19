using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RageBeastKnockback : AbilityMod
{
	[Header("-- Targeting Mod")]
	public AbilityModPropertyInt m_maxTargetMod;

	public AbilityModPropertyFloat m_targeterWidthMod;

	public AbilityModPropertyFloat m_targeterLengthMod;

	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_onHitDamageMod;

	[Header("-- UNUSED: Damage Mods from Knockback Move")]
	public AbilityModPropertyInt m_collisionDamageToSelfMod;

	public AbilityModPropertyInt m_collisionDamageToOtherMod;

	public AbilityModPropertyInt m_collisionDamageFromGeoMod;

	[Header("-- UNUSED Effect on Geo Collision")]
	public StandardEffectInfo m_effectOnGeoCollision;

	[Header("-- Knockback Mod")]
	public AbilityModPropertyFloat m_knockbackDistanceMinMod;

	public AbilityModPropertyFloat m_knockbackDistanceMaxMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(RageBeastKnockback);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RageBeastKnockback rageBeastKnockback = targetAbility as RageBeastKnockback;
		if (rageBeastKnockback != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RageBeastKnockback.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_targeterWidthMod, "LaserWidth", string.Empty, rageBeastKnockback.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_targeterLengthMod, "LaserDistance", string.Empty, rageBeastKnockback.m_laserDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetMod, "MaxTargets", string.Empty, rageBeastKnockback.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceMinMod, "KnockbackMinDistance", string.Empty, rageBeastKnockback.m_knockbackDistanceMin, true, false, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceMaxMod, "KnockbackMaxDistance", string.Empty, rageBeastKnockback.m_knockbackDistanceMax, true, false, false);
			AbilityMod.AddToken(tokens, this.m_onHitDamageMod, "DamageAmount", string.Empty, rageBeastKnockback.m_damageAmount, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastKnockback rageBeastKnockback = base.GetTargetAbilityOnAbilityData(abilityData) as RageBeastKnockback;
		bool flag = rageBeastKnockback != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt maxTargetMod = this.m_maxTargetMod;
		string prefix = "[Max Targets]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RageBeastKnockback.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = rageBeastKnockback.m_maxTargets;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(maxTargetMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat targeterWidthMod = this.m_targeterWidthMod;
		string prefix2 = "[Targeter Width]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = rageBeastKnockback.m_laserWidth;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(targeterWidthMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat targeterLengthMod = this.m_targeterLengthMod;
		string prefix3 = "[Targeter Length]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = rageBeastKnockback.m_laserDistance;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(targeterLengthMod, prefix3, showBaseVal3, baseVal3);
		text += AbilityModHelper.GetModPropertyDesc(this.m_onHitDamageMod, "[On Hit Damage]", flag, (!flag) ? 0 : rageBeastKnockback.m_damageAmount);
		text += AbilityModHelper.GetModPropertyDesc(this.m_collisionDamageToSelfMod, "[ !!!UNUSED: Collision Damage to Mover]", flag, (!flag) ? 0 : rageBeastKnockback.m_damageToMoverOnCollision);
		string str4 = text;
		AbilityModPropertyInt collisionDamageToOtherMod = this.m_collisionDamageToOtherMod;
		string prefix4 = "[ !!!UNUSED: Collision Damage to Other]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = rageBeastKnockback.m_damageToOtherOnCollision;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(collisionDamageToOtherMod, prefix4, showBaseVal4, baseVal4);
		text += AbilityModHelper.GetModPropertyDesc(this.m_collisionDamageFromGeoMod, "[ !!!UNUSED: Collision Damage from Geo]", flag, (!flag) ? 0 : rageBeastKnockback.m_damageCollisionWithGeo);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnGeoCollision, "[Effect on Geo Collision]", string.Empty, flag, null);
		string str5 = text;
		AbilityModPropertyFloat knockbackDistanceMinMod = this.m_knockbackDistanceMinMod;
		string prefix5 = "[Knockback Min Distance]";
		bool showBaseVal5 = flag;
		float baseVal5;
		if (flag)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = rageBeastKnockback.m_knockbackDistanceMin;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(knockbackDistanceMinMod, prefix5, showBaseVal5, baseVal5);
		return text + AbilityModHelper.GetModPropertyDesc(this.m_knockbackDistanceMaxMod, "[Knockback Max Distance]", flag, (!flag) ? 0f : rageBeastKnockback.m_knockbackDistanceMax);
	}
}
