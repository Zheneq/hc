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
		if (ninjaVanish != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NinjaVanish.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnSelfMod, "EffectOnSelf", ninjaVanish.m_effectOnSelf, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfEffectOnNextTurnMod, "SelfEffectOnNextTurn", ninjaVanish.m_selfEffectOnNextTurn, true);
			AbilityMod.AddToken(tokens, this.m_selfHealOnTurnStartIfInFieldMod, "SelfHealOnTurnStartIfInField", string.Empty, ninjaVanish.m_selfHealOnTurnStartIfInField, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnEnemyMod, "EffectOnEnemy", ninjaVanish.m_effectOnEnemy, true);
			AbilityMod.AddToken(tokens, this.m_smokeFieldDurationMod, "SmokeFieldDuration", string.Empty, ninjaVanish.m_smokeFieldDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_barrierWidthMod, "BarrierWidth", string.Empty, ninjaVanish.m_barrierWidth, true, false, false);
			AbilityMod.AddToken_BarrierMod(tokens, this.m_visionBlockBarrierDataMod, "VisionBlockBarrierData", ninjaVanish.m_visionBlockBarrierData);
			AbilityMod.AddToken_GroundFieldMod(tokens, this.m_groundEffectDataMod, "GroundEffectData", ninjaVanish.m_groundEffectData);
			AbilityMod.AddToken(tokens, this.m_cdrIfOnlyAbilityUsedMod, "CdrIfOnlyAbilityUsed", string.Empty, ninjaVanish.m_cdrIfOnlyAbilityUsed, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NinjaVanish ninjaVanish = base.GetTargetAbilityOnAbilityData(abilityData) as NinjaVanish;
		bool flag = ninjaVanish != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool canQueueMoveAfterEvadeMod = this.m_canQueueMoveAfterEvadeMod;
		string prefix = "[CanQueueMoveAfterEvade]";
		bool showBaseVal = flag;
		bool baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NinjaVanish.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = ninjaVanish.m_canQueueMoveAfterEvade;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(canQueueMoveAfterEvadeMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool skipEvadeMod = this.m_skipEvadeMod;
		string prefix2 = "[SkipEvade]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = ninjaVanish.m_skipEvade;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(skipEvadeMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyEffectInfo effectOnSelfMod = this.m_effectOnSelfMod;
		string prefix3 = "[EffectOnSelf]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
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
			baseVal3 = ninjaVanish.m_effectOnSelf;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + base.PropDesc(effectOnSelfMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyEffectInfo selfEffectOnNextTurnMod = this.m_selfEffectOnNextTurnMod;
		string prefix4 = "[SelfEffectOnNextTurn]";
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
			baseVal4 = ninjaVanish.m_selfEffectOnNextTurn;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + base.PropDesc(selfEffectOnNextTurnMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt selfHealOnTurnStartIfInFieldMod = this.m_selfHealOnTurnStartIfInFieldMod;
		string prefix5 = "[SelfHealOnTurnStartIfInField]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = ninjaVanish.m_selfHealOnTurnStartIfInField;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(selfHealOnTurnStartIfInFieldMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_effectOnEnemyMod, "[EffectOnEnemy]", flag, (!flag) ? null : ninjaVanish.m_effectOnEnemy);
		string str6 = text;
		AbilityModPropertyInt smokeFieldDurationMod = this.m_smokeFieldDurationMod;
		string prefix6 = "[SmokeFieldDuration]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = ninjaVanish.m_smokeFieldDuration;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(smokeFieldDurationMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyFloat barrierWidthMod = this.m_barrierWidthMod;
		string prefix7 = "[BarrierWidth]";
		bool showBaseVal7 = flag;
		float baseVal7;
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
			baseVal7 = ninjaVanish.m_barrierWidth;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + base.PropDesc(barrierWidthMod, prefix7, showBaseVal7, baseVal7);
		text += base.PropDescBarrierMod(this.m_visionBlockBarrierDataMod, "{ VisionBlockBarrierData }", ninjaVanish.m_visionBlockBarrierData);
		text += base.PropDescGroundFieldMod(this.m_groundEffectDataMod, "{ GroundEffectData }", ninjaVanish.m_groundEffectData);
		string str8 = text;
		AbilityModPropertyInt cdrIfOnlyAbilityUsedMod = this.m_cdrIfOnlyAbilityUsedMod;
		string prefix8 = "[CdrIfOnlyAbilityUsed]";
		bool showBaseVal8 = flag;
		int baseVal8;
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
			baseVal8 = ninjaVanish.m_cdrIfOnlyAbilityUsed;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(cdrIfOnlyAbilityUsedMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyBool cdrConsiderCatalystMod = this.m_cdrConsiderCatalystMod;
		string prefix9 = "[CdrConsiderCatalyst]";
		bool showBaseVal9 = flag;
		bool baseVal9;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal9 = ninjaVanish.m_cdrConsiderCatalyst;
		}
		else
		{
			baseVal9 = false;
		}
		return str9 + base.PropDesc(cdrConsiderCatalystMod, prefix9, showBaseVal9, baseVal9);
	}
}
