using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TricksterCreateDamageFields : AbilityMod
{
	[Header("-- Targeting --")]
	public AbilityModPropertyBool m_addFieldAroundSelfMod;

	public AbilityModPropertyBool m_useInitialShapeOverrideMod;

	public AbilityModPropertyShape m_initialShapeOverrideMod;

	[Header("-- Ground Field Info --")]
	public AbilityModPropertyGroundEffectField m_groundFieldInfoMod;

	[Header("-- Self Effect for Multi Hit")]
	public AbilityModPropertyEffectInfo m_selfEffectForMultiHitMod;

	[Header("-- Extra Enemy Hit Effect On Cast")]
	public AbilityModPropertyEffectInfo m_extraEnemyEffectOnCastMod;

	[Header("-- Spoil spawn info")]
	public AbilityModPropertyBool m_spawnSpoilForEnemyHitMod;

	public AbilityModPropertyBool m_spawnSpoilForAllyHitMod;

	public AbilityModPropertyBool m_onlySpawnSpoilOnMultiHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(TricksterCreateDamageFields);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TricksterCreateDamageFields tricksterCreateDamageFields = targetAbility as TricksterCreateDamageFields;
		if (tricksterCreateDamageFields != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_TricksterCreateDamageFields.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_GroundFieldMod(tokens, this.m_groundFieldInfoMod, "GroundFieldInfo", tricksterCreateDamageFields.m_groundFieldInfo);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfEffectForMultiHitMod, "SelfEffectForMultiHit", tricksterCreateDamageFields.m_selfEffectForMultiHit, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_extraEnemyEffectOnCastMod, "ExtraEnemyEffectOnCast", tricksterCreateDamageFields.m_extraEnemyEffectOnCast, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterCreateDamageFields tricksterCreateDamageFields = base.GetTargetAbilityOnAbilityData(abilityData) as TricksterCreateDamageFields;
		bool flag = tricksterCreateDamageFields != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_addFieldAroundSelfMod, "[AddFieldAroundSelf]", flag, flag && tricksterCreateDamageFields.m_addFieldAroundSelf);
		bool flag2;
		if (this.m_useInitialShapeOverrideMod != null)
		{
			AbilityModPropertyBool useInitialShapeOverrideMod = this.m_useInitialShapeOverrideMod;
			bool input;
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_TricksterCreateDamageFields.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
				}
				input = tricksterCreateDamageFields.m_useInitialShapeOverride;
			}
			else
			{
				input = false;
			}
			flag2 = useInitialShapeOverrideMod.GetModifiedValue(input);
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		text += base.PropDesc(this.m_useInitialShapeOverrideMod, "[UseInitialShapeOverride]", flag, flag && tricksterCreateDamageFields.m_useInitialShapeOverride);
		if (flag3)
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
			string str = text;
			AbilityModPropertyShape initialShapeOverrideMod = this.m_initialShapeOverrideMod;
			string prefix = "[InitialShapeOverride]";
			bool showBaseVal = flag;
			AbilityAreaShape baseVal;
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
				baseVal = tricksterCreateDamageFields.m_initialShapeOverride;
			}
			else
			{
				baseVal = AbilityAreaShape.SingleSquare;
			}
			text = str + base.PropDesc(initialShapeOverrideMod, prefix, showBaseVal, baseVal);
		}
		text += base.PropDescGroundFieldMod(this.m_groundFieldInfoMod, "{ GroundFieldInfo }", tricksterCreateDamageFields.m_groundFieldInfo);
		string str2 = text;
		AbilityModPropertyEffectInfo selfEffectForMultiHitMod = this.m_selfEffectForMultiHitMod;
		string prefix2 = "[SelfEffectForMultiHit]";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal2;
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
			baseVal2 = tricksterCreateDamageFields.m_selfEffectForMultiHit;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(selfEffectForMultiHitMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyEffectInfo extraEnemyEffectOnCastMod = this.m_extraEnemyEffectOnCastMod;
		string prefix3 = "[ExtraEnemyEffectOnCast]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
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
			baseVal3 = tricksterCreateDamageFields.m_extraEnemyEffectOnCast;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + base.PropDesc(extraEnemyEffectOnCastMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool spawnSpoilForEnemyHitMod = this.m_spawnSpoilForEnemyHitMod;
		string prefix4 = "[SpawnSpoilForEnemyHit]";
		bool showBaseVal4 = flag;
		bool baseVal4;
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
			baseVal4 = tricksterCreateDamageFields.m_spawnSpoilForEnemyHit;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(spawnSpoilForEnemyHitMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool spawnSpoilForAllyHitMod = this.m_spawnSpoilForAllyHitMod;
		string prefix5 = "[SpawnSpoilForAllyHit]";
		bool showBaseVal5 = flag;
		bool baseVal5;
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
			baseVal5 = tricksterCreateDamageFields.m_spawnSpoilForAllyHit;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(spawnSpoilForAllyHitMod, prefix5, showBaseVal5, baseVal5);
		return text + base.PropDesc(this.m_onlySpawnSpoilOnMultiHitMod, "[OnlySpawnSpoilOnMultiHit]", flag, flag && tricksterCreateDamageFields.m_onlySpawnSpoilOnMultiHit);
	}
}
