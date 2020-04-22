using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NinjaVanish : AbilityMod
{
	[Separator("Evade/Move settings", true)]
	public AbilityModPropertyBool m_canQueueMoveAfterEvadeMod;

	[Header("-- Whether to skip dash (if want to be a combat ability, etc) --")]
	public AbilityModPropertyBool m_skipEvadeMod;

	[Separator("Self Hit - Effects / Heal on Turn Start", true)]
	public AbilityModPropertyEffectInfo m_effectOnSelfMod;

	public AbilityModPropertyEffectInfo m_selfEffectOnNextTurnMod;

	[Header("-- Heal on Self on next turn start if inside field --")]
	public AbilityModPropertyInt m_selfHealOnTurnStartIfInFieldMod;

	[Separator("Initial Cast Hit On Enemy", true)]
	public AbilityModPropertyEffectInfo m_effectOnEnemyMod;

	[Separator("Duration for barrier and ground effect", true)]
	public AbilityModPropertyInt m_smokeFieldDurationMod;

	[Separator("Vision Blocking Barrier", true)]
	public AbilityModPropertyFloat m_barrierWidthMod;

	public AbilityModPropertyBarrierDataV2 m_visionBlockBarrierDataMod;

	[Separator("Ground Effect", true)]
	public AbilityModPropertyGroundEffectField m_groundEffectDataMod;

	[Separator("Cooldown Reduction if only ability used in turn", true)]
	public AbilityModPropertyInt m_cdrIfOnlyAbilityUsedMod;

	public AbilityModPropertyBool m_cdrConsiderCatalystMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NinjaVanish);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NinjaVanish ninjaVanish = targetAbility as NinjaVanish;
		if (!(ninjaVanish != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", ninjaVanish.m_effectOnSelf);
			AbilityMod.AddToken_EffectMod(tokens, m_selfEffectOnNextTurnMod, "SelfEffectOnNextTurn", ninjaVanish.m_selfEffectOnNextTurn);
			AbilityMod.AddToken(tokens, m_selfHealOnTurnStartIfInFieldMod, "SelfHealOnTurnStartIfInField", string.Empty, ninjaVanish.m_selfHealOnTurnStartIfInField);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnEnemyMod, "EffectOnEnemy", ninjaVanish.m_effectOnEnemy);
			AbilityMod.AddToken(tokens, m_smokeFieldDurationMod, "SmokeFieldDuration", string.Empty, ninjaVanish.m_smokeFieldDuration);
			AbilityMod.AddToken(tokens, m_barrierWidthMod, "BarrierWidth", string.Empty, ninjaVanish.m_barrierWidth);
			AbilityMod.AddToken_BarrierMod(tokens, m_visionBlockBarrierDataMod, "VisionBlockBarrierData", ninjaVanish.m_visionBlockBarrierData);
			AbilityMod.AddToken_GroundFieldMod(tokens, m_groundEffectDataMod, "GroundEffectData", ninjaVanish.m_groundEffectData);
			AbilityMod.AddToken(tokens, m_cdrIfOnlyAbilityUsedMod, "CdrIfOnlyAbilityUsed", string.Empty, ninjaVanish.m_cdrIfOnlyAbilityUsed);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NinjaVanish ninjaVanish = GetTargetAbilityOnAbilityData(abilityData) as NinjaVanish;
		bool flag = ninjaVanish != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool canQueueMoveAfterEvadeMod = m_canQueueMoveAfterEvadeMod;
		int baseVal;
		if (flag)
		{
			baseVal = (ninjaVanish.m_canQueueMoveAfterEvade ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(canQueueMoveAfterEvadeMod, "[CanQueueMoveAfterEvade]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyBool skipEvadeMod = m_skipEvadeMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = (ninjaVanish.m_skipEvade ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(skipEvadeMod, "[SkipEvade]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyEffectInfo effectOnSelfMod = m_effectOnSelfMod;
		object baseVal3;
		if (flag)
		{
			baseVal3 = ninjaVanish.m_effectOnSelf;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + PropDesc(effectOnSelfMod, "[EffectOnSelf]", flag, (StandardEffectInfo)baseVal3);
		string str4 = empty;
		AbilityModPropertyEffectInfo selfEffectOnNextTurnMod = m_selfEffectOnNextTurnMod;
		object baseVal4;
		if (flag)
		{
			baseVal4 = ninjaVanish.m_selfEffectOnNextTurn;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + PropDesc(selfEffectOnNextTurnMod, "[SelfEffectOnNextTurn]", flag, (StandardEffectInfo)baseVal4);
		string str5 = empty;
		AbilityModPropertyInt selfHealOnTurnStartIfInFieldMod = m_selfHealOnTurnStartIfInFieldMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = ninjaVanish.m_selfHealOnTurnStartIfInField;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(selfHealOnTurnStartIfInFieldMod, "[SelfHealOnTurnStartIfInField]", flag, baseVal5);
		empty += PropDesc(m_effectOnEnemyMod, "[EffectOnEnemy]", flag, (!flag) ? null : ninjaVanish.m_effectOnEnemy);
		string str6 = empty;
		AbilityModPropertyInt smokeFieldDurationMod = m_smokeFieldDurationMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = ninjaVanish.m_smokeFieldDuration;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(smokeFieldDurationMod, "[SmokeFieldDuration]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyFloat barrierWidthMod = m_barrierWidthMod;
		float baseVal7;
		if (flag)
		{
			baseVal7 = ninjaVanish.m_barrierWidth;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str7 + PropDesc(barrierWidthMod, "[BarrierWidth]", flag, baseVal7);
		empty += PropDescBarrierMod(m_visionBlockBarrierDataMod, "{ VisionBlockBarrierData }", ninjaVanish.m_visionBlockBarrierData);
		empty += PropDescGroundFieldMod(m_groundEffectDataMod, "{ GroundEffectData }", ninjaVanish.m_groundEffectData);
		string str8 = empty;
		AbilityModPropertyInt cdrIfOnlyAbilityUsedMod = m_cdrIfOnlyAbilityUsedMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = ninjaVanish.m_cdrIfOnlyAbilityUsed;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(cdrIfOnlyAbilityUsedMod, "[CdrIfOnlyAbilityUsed]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyBool cdrConsiderCatalystMod = m_cdrConsiderCatalystMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = (ninjaVanish.m_cdrConsiderCatalyst ? 1 : 0);
		}
		else
		{
			baseVal9 = 0;
		}
		return str9 + PropDesc(cdrConsiderCatalystMod, "[CdrConsiderCatalyst]", flag, (byte)baseVal9 != 0);
	}
}
