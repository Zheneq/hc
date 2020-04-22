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
		if (!(tricksterCreateDamageFields != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_GroundFieldMod(tokens, m_groundFieldInfoMod, "GroundFieldInfo", tricksterCreateDamageFields.m_groundFieldInfo);
			AbilityMod.AddToken_EffectMod(tokens, m_selfEffectForMultiHitMod, "SelfEffectForMultiHit", tricksterCreateDamageFields.m_selfEffectForMultiHit);
			AbilityMod.AddToken_EffectMod(tokens, m_extraEnemyEffectOnCastMod, "ExtraEnemyEffectOnCast", tricksterCreateDamageFields.m_extraEnemyEffectOnCast);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterCreateDamageFields tricksterCreateDamageFields = GetTargetAbilityOnAbilityData(abilityData) as TricksterCreateDamageFields;
		bool flag = tricksterCreateDamageFields != null;
		string empty = string.Empty;
		empty += PropDesc(m_addFieldAroundSelfMod, "[AddFieldAroundSelf]", flag, flag && tricksterCreateDamageFields.m_addFieldAroundSelf);
		int num;
		if (m_useInitialShapeOverrideMod != null)
		{
			AbilityModPropertyBool useInitialShapeOverrideMod = m_useInitialShapeOverrideMod;
			int input;
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				input = (tricksterCreateDamageFields.m_useInitialShapeOverride ? 1 : 0);
			}
			else
			{
				input = 0;
			}
			num = (useInitialShapeOverrideMod.GetModifiedValue((byte)input != 0) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag2 = (byte)num != 0;
		empty += PropDesc(m_useInitialShapeOverrideMod, "[UseInitialShapeOverride]", flag, flag && tricksterCreateDamageFields.m_useInitialShapeOverride);
		if (flag2)
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
			string str = empty;
			AbilityModPropertyShape initialShapeOverrideMod = m_initialShapeOverrideMod;
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
				baseVal = (int)tricksterCreateDamageFields.m_initialShapeOverride;
			}
			else
			{
				baseVal = 0;
			}
			empty = str + PropDesc(initialShapeOverrideMod, "[InitialShapeOverride]", flag, (AbilityAreaShape)baseVal);
		}
		empty += PropDescGroundFieldMod(m_groundFieldInfoMod, "{ GroundFieldInfo }", tricksterCreateDamageFields.m_groundFieldInfo);
		string str2 = empty;
		AbilityModPropertyEffectInfo selfEffectForMultiHitMod = m_selfEffectForMultiHitMod;
		object baseVal2;
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
			baseVal2 = tricksterCreateDamageFields.m_selfEffectForMultiHit;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + PropDesc(selfEffectForMultiHitMod, "[SelfEffectForMultiHit]", flag, (StandardEffectInfo)baseVal2);
		string str3 = empty;
		AbilityModPropertyEffectInfo extraEnemyEffectOnCastMod = m_extraEnemyEffectOnCastMod;
		object baseVal3;
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
			baseVal3 = tricksterCreateDamageFields.m_extraEnemyEffectOnCast;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + PropDesc(extraEnemyEffectOnCastMod, "[ExtraEnemyEffectOnCast]", flag, (StandardEffectInfo)baseVal3);
		string str4 = empty;
		AbilityModPropertyBool spawnSpoilForEnemyHitMod = m_spawnSpoilForEnemyHitMod;
		int baseVal4;
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
			baseVal4 = (tricksterCreateDamageFields.m_spawnSpoilForEnemyHit ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(spawnSpoilForEnemyHitMod, "[SpawnSpoilForEnemyHit]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyBool spawnSpoilForAllyHitMod = m_spawnSpoilForAllyHitMod;
		int baseVal5;
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
			baseVal5 = (tricksterCreateDamageFields.m_spawnSpoilForAllyHit ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(spawnSpoilForAllyHitMod, "[SpawnSpoilForAllyHit]", flag, (byte)baseVal5 != 0);
		return empty + PropDesc(m_onlySpawnSpoilOnMultiHitMod, "[OnlySpawnSpoilOnMultiHit]", flag, flag && tricksterCreateDamageFields.m_onlySpawnSpoilOnMultiHit);
	}
}
