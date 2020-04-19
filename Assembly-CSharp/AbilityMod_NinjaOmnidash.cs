using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NinjaOmnidash : AbilityMod
{
	[Separator("Damage/Heal mod on Deathmark", true)]
	public AbilityModPropertyInt m_deathmarkDamageMod;

	public AbilityModPropertyInt m_deathmarkCasterHealMod;

	[Separator("On Hit Stuff", true)]
	public AbilityModPropertyInt m_baseDamageMod;

	public AbilityModPropertyInt m_damageChangePerEnemyAfterFirstMod;

	public AbilityModPropertyInt m_minDamageMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	[Header("-- Effect for single hit --")]
	public AbilityModPropertyEffectInfo m_singleHitEnemyEffectMod;

	public AbilityModPropertyEffectInfo m_extraSingleHitEnemyEffectMod;

	[Separator("Energy gain on Marked hit", true)]
	public AbilityModPropertyInt m_energyGainPerMarkedHitMod;

	[Separator("For Dash", true)]
	public AbilityModPropertyBool m_skipEvadeMod;

	[Space(10f)]
	public AbilityModPropertyBool m_isTeleportMod;

	public AbilityModPropertyFloat m_dashRadiusAtStartMod;

	public AbilityModPropertyFloat m_dashRadiusMiddleMod;

	public AbilityModPropertyFloat m_dashRadiusAtEndMod;

	public AbilityModPropertyBool m_dashPenetrateLineOfSightMod;

	[Header("-- Whether can queue movement evade")]
	public AbilityModPropertyBool m_canQueueMoveAfterEvadeMod;

	[Separator("[Deathmark] Effect", "magenta")]
	public AbilityModPropertyBool m_applyDeathmarkEffectMod;

	[Separator("Cooldown Reset on other ability", true)]
	public AbilityModPropertyInt m_cdrOnAbilityMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NinjaOmnidash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NinjaOmnidash ninjaOmnidash = targetAbility as NinjaOmnidash;
		if (ninjaOmnidash != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NinjaOmnidash.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			Ninja_SyncComponent component = ninjaOmnidash.GetComponent<Ninja_SyncComponent>();
			if (component != null)
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
				AbilityMod.AddToken(tokens, this.m_deathmarkDamageMod, "Deathmark_TriggerDamage", string.Empty, component.m_deathmarkOnTriggerDamage, true, false);
				AbilityMod.AddToken(tokens, this.m_deathmarkCasterHealMod, "Deathmark_CasterHeal", string.Empty, component.m_deathmarkOnTriggerCasterHeal, true, false);
			}
			AbilityMod.AddToken(tokens, this.m_baseDamageMod, "BaseDamage", string.Empty, ninjaOmnidash.m_baseDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_damageChangePerEnemyAfterFirstMod, "DamageChangePerEnemyAfterFirst", string.Empty, ninjaOmnidash.m_damageChangePerEnemyAfterFirst, true, false);
			AbilityMod.AddToken(tokens, this.m_minDamageMod, "MinDamage", string.Empty, ninjaOmnidash.m_minDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", ninjaOmnidash.m_enemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_singleHitEnemyEffectMod, "SingleHitEnemyEffect", ninjaOmnidash.m_singleHitEnemyEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_extraSingleHitEnemyEffectMod, "ExtraSingleHitEnemyEffect", ninjaOmnidash.m_extraSingleHitEnemyEffect, true);
			AbilityMod.AddToken(tokens, this.m_energyGainPerMarkedHitMod, "EnergyGainPerMarkedHit", string.Empty, ninjaOmnidash.m_energyGainPerMarkedHit, true, false);
			AbilityMod.AddToken(tokens, this.m_dashRadiusAtStartMod, "DashRadiusAtStart", string.Empty, ninjaOmnidash.m_dashRadiusAtStart, true, false, false);
			AbilityMod.AddToken(tokens, this.m_dashRadiusMiddleMod, "DashRadiusMiddle", string.Empty, ninjaOmnidash.m_dashRadiusMiddle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_dashRadiusAtEndMod, "DashRadiusAtEnd", string.Empty, ninjaOmnidash.m_dashRadiusAtEnd, true, false, false);
			AbilityMod.AddToken(tokens, this.m_cdrOnAbilityMod, "CdrOnAbility", string.Empty, ninjaOmnidash.m_cdrOnAbility, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NinjaOmnidash ninjaOmnidash = base.GetTargetAbilityOnAbilityData(abilityData) as NinjaOmnidash;
		bool flag = ninjaOmnidash != null;
		Ninja_SyncComponent ninja_SyncComponent = null;
		if (ninjaOmnidash != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NinjaOmnidash.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			ninja_SyncComponent = ninjaOmnidash.GetComponent<Ninja_SyncComponent>();
		}
		string text = string.Empty;
		text += base.PropDesc(this.m_deathmarkDamageMod, "[Deathmark_Damage]", ninja_SyncComponent != null, ninja_SyncComponent.m_deathmarkOnTriggerDamage);
		text += base.PropDesc(this.m_deathmarkCasterHealMod, "[Deathmark_CasterHeal]", ninja_SyncComponent != null, ninja_SyncComponent.m_deathmarkOnTriggerCasterHeal);
		string str = text;
		AbilityModPropertyInt baseDamageMod = this.m_baseDamageMod;
		string prefix = "[BaseDamage]";
		bool showBaseVal = flag;
		int baseVal;
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
			baseVal = ninjaOmnidash.m_baseDamage;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(baseDamageMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt damageChangePerEnemyAfterFirstMod = this.m_damageChangePerEnemyAfterFirstMod;
		string prefix2 = "[DamageChangePerEnemyAfterFirst]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = ninjaOmnidash.m_damageChangePerEnemyAfterFirst;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(damageChangePerEnemyAfterFirstMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt minDamageMod = this.m_minDamageMod;
		string prefix3 = "[MinDamage]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = ninjaOmnidash.m_minDamage;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(minDamageMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_enemyHitEffectMod, "[EnemyHitEffect]", flag, (!flag) ? null : ninjaOmnidash.m_enemyHitEffect);
		string str4 = text;
		AbilityModPropertyEffectInfo singleHitEnemyEffectMod = this.m_singleHitEnemyEffectMod;
		string prefix4 = "[SingleHitEnemyEffect]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
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
			baseVal4 = ninjaOmnidash.m_singleHitEnemyEffect;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + base.PropDesc(singleHitEnemyEffectMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo extraSingleHitEnemyEffectMod = this.m_extraSingleHitEnemyEffectMod;
		string prefix5 = "[ExtraSingleHitEnemyEffect]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
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
			baseVal5 = ninjaOmnidash.m_extraSingleHitEnemyEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(extraSingleHitEnemyEffectMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt energyGainPerMarkedHitMod = this.m_energyGainPerMarkedHitMod;
		string prefix6 = "[EnergyGainPerMarkedHit]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = ninjaOmnidash.m_energyGainPerMarkedHit;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(energyGainPerMarkedHitMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyBool skipEvadeMod = this.m_skipEvadeMod;
		string prefix7 = "[SkipEvade]";
		bool showBaseVal7 = flag;
		bool baseVal7;
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
			baseVal7 = ninjaOmnidash.m_skipEvade;
		}
		else
		{
			baseVal7 = false;
		}
		text = str7 + base.PropDesc(skipEvadeMod, prefix7, showBaseVal7, baseVal7);
		text += base.PropDesc(this.m_isTeleportMod, "[IsTeleport]", flag, flag && ninjaOmnidash.m_isTeleport);
		string str8 = text;
		AbilityModPropertyFloat dashRadiusAtStartMod = this.m_dashRadiusAtStartMod;
		string prefix8 = "[DashRadiusAtStart]";
		bool showBaseVal8 = flag;
		float baseVal8;
		if (flag)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal8 = ninjaOmnidash.m_dashRadiusAtStart;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str8 + base.PropDesc(dashRadiusAtStartMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyFloat dashRadiusMiddleMod = this.m_dashRadiusMiddleMod;
		string prefix9 = "[DashRadiusMiddle]";
		bool showBaseVal9 = flag;
		float baseVal9;
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
			baseVal9 = ninjaOmnidash.m_dashRadiusMiddle;
		}
		else
		{
			baseVal9 = 0f;
		}
		text = str9 + base.PropDesc(dashRadiusMiddleMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyFloat dashRadiusAtEndMod = this.m_dashRadiusAtEndMod;
		string prefix10 = "[DashRadiusAtEnd]";
		bool showBaseVal10 = flag;
		float baseVal10;
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
			baseVal10 = ninjaOmnidash.m_dashRadiusAtEnd;
		}
		else
		{
			baseVal10 = 0f;
		}
		text = str10 + base.PropDesc(dashRadiusAtEndMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyBool dashPenetrateLineOfSightMod = this.m_dashPenetrateLineOfSightMod;
		string prefix11 = "[DashPenetrateLineOfSight]";
		bool showBaseVal11 = flag;
		bool baseVal11;
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
			baseVal11 = ninjaOmnidash.m_dashPenetrateLineOfSight;
		}
		else
		{
			baseVal11 = false;
		}
		text = str11 + base.PropDesc(dashPenetrateLineOfSightMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyBool canQueueMoveAfterEvadeMod = this.m_canQueueMoveAfterEvadeMod;
		string prefix12 = "[CanQueueMoveAfterEvade]";
		bool showBaseVal12 = flag;
		bool baseVal12;
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
			baseVal12 = ninjaOmnidash.m_canQueueMoveAfterEvade;
		}
		else
		{
			baseVal12 = false;
		}
		text = str12 + base.PropDesc(canQueueMoveAfterEvadeMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyBool applyDeathmarkEffectMod = this.m_applyDeathmarkEffectMod;
		string prefix13 = "[ApplyDeathmarkEffect]";
		bool showBaseVal13 = flag;
		bool baseVal13;
		if (flag)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal13 = ninjaOmnidash.m_applyDeathmarkEffect;
		}
		else
		{
			baseVal13 = false;
		}
		text = str13 + base.PropDesc(applyDeathmarkEffectMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyInt cdrOnAbilityMod = this.m_cdrOnAbilityMod;
		string prefix14 = "[CdrOnAbility]";
		bool showBaseVal14 = flag;
		int baseVal14;
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
			baseVal14 = ninjaOmnidash.m_cdrOnAbility;
		}
		else
		{
			baseVal14 = 0;
		}
		return str14 + base.PropDesc(cdrOnAbilityMod, prefix14, showBaseVal14, baseVal14);
	}
}
