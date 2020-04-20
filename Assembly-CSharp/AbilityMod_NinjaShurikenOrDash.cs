using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NinjaShurikenOrDash : AbilityMod
{
	[Separator("Dash - Type, Targeting Info", true)]
	public AbilityModPropertyBool m_isTeleportMod;

	public AbilityModPropertyFloat m_dashRangeDefaultMod;

	public AbilityModPropertyFloat m_dashRangeMarkedMod;

	[Header("-- Who can be dash targets --")]
	public AbilityModPropertyBool m_dashRequireDeathmarkMod;

	public AbilityModPropertyFloat m_dashToUnmarkedRangeMod;

	[Space(5f)]
	public AbilityModPropertyBool m_canDashToAllyMod;

	public AbilityModPropertyBool m_canDashToEnemyMod;

	public AbilityModPropertyBool m_dashIgnoreLosMod;

	public AbilityModPropertyShape m_dashDestShapeMod;

	[Separator("Dash - On Hit Stuff", true)]
	public AbilityModPropertyInt m_dashDamageMod;

	public AbilityModPropertyInt m_extraDamageOnMarkedMod;

	public AbilityModPropertyInt m_extraDamageIfNotMarkedMod;

	public AbilityModPropertyEffectInfo m_dashEnemyHitEffectMod;

	public AbilityModPropertyEffectInfo m_extraEnemyEffectOnMarkedMod;

	[Header("-- For All Hit --")]
	public AbilityModPropertyInt m_dashHealingMod;

	public AbilityModPropertyEffectInfo m_dashAllyHitEffectMod;

	[Separator("Dash - [Deathmark]", "magenta")]
	public AbilityModPropertyBool m_dashApplyDeathmarkMod;

	public AbilityModPropertyBool m_canTriggerDeathmarkMod;

	[Separator("Dash - Allow move after evade?", true)]
	public AbilityModPropertyBool m_canQueueMoveAfterEvadeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NinjaShurikenOrDash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NinjaShurikenOrDash ninjaShurikenOrDash = targetAbility as NinjaShurikenOrDash;
		if (ninjaShurikenOrDash != null)
		{
			AbilityMod.AddToken(tokens, this.m_dashRangeDefaultMod, "DashRangeDefault", string.Empty, ninjaShurikenOrDash.m_dashRangeDefault, true, false, false);
			AbilityMod.AddToken(tokens, this.m_dashRangeMarkedMod, "DashRangeMarked", string.Empty, ninjaShurikenOrDash.m_dashRangeMarked, true, false, false);
			AbilityMod.AddToken(tokens, this.m_dashToUnmarkedRangeMod, "DashToUnmarkedRange", string.Empty, ninjaShurikenOrDash.m_dashToUnmarkedRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_dashDamageMod, "DashDamage", string.Empty, ninjaShurikenOrDash.m_dashDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageOnMarkedMod, "ExtraDamageOnMarked", string.Empty, ninjaShurikenOrDash.m_extraDamageOnMarked, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageIfNotMarkedMod, "ExtraDamageIfNotMarked", string.Empty, ninjaShurikenOrDash.m_extraDamageIfNotMarked, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_dashEnemyHitEffectMod, "DashEnemyHitEffect", ninjaShurikenOrDash.m_dashEnemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_extraEnemyEffectOnMarkedMod, "ExtraEnemyEffectOnMarked", ninjaShurikenOrDash.m_extraEnemyEffectOnMarked, true);
			AbilityMod.AddToken(tokens, this.m_dashHealingMod, "DashHealing", string.Empty, ninjaShurikenOrDash.m_dashHealing, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_dashAllyHitEffectMod, "DashAllyHitEffect", ninjaShurikenOrDash.m_dashAllyHitEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NinjaShurikenOrDash ninjaShurikenOrDash = base.GetTargetAbilityOnAbilityData(abilityData) as NinjaShurikenOrDash;
		bool flag = ninjaShurikenOrDash != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_isTeleportMod, "[IsTeleport]", flag, flag && ninjaShurikenOrDash.m_isTeleport);
		text += base.PropDesc(this.m_dashRangeDefaultMod, "[DashRangeDefault]", flag, (!flag) ? 0f : ninjaShurikenOrDash.m_dashRangeDefault);
		text += base.PropDesc(this.m_dashRangeMarkedMod, "[DashRangeMarked]", flag, (!flag) ? 0f : ninjaShurikenOrDash.m_dashRangeMarked);
		text += base.PropDesc(this.m_dashRequireDeathmarkMod, "[DashRequireDeathmark]", flag, flag && ninjaShurikenOrDash.m_dashRequireDeathmark);
		string str = text;
		AbilityModPropertyFloat dashToUnmarkedRangeMod = this.m_dashToUnmarkedRangeMod;
		string prefix = "[DashToUnmarkedRange]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = ninjaShurikenOrDash.m_dashToUnmarkedRange;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(dashToUnmarkedRangeMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool canDashToAllyMod = this.m_canDashToAllyMod;
		string prefix2 = "[CanDashToAlly]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			baseVal2 = ninjaShurikenOrDash.m_canDashToAlly;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(canDashToAllyMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool canDashToEnemyMod = this.m_canDashToEnemyMod;
		string prefix3 = "[CanDashToEnemy]";
		bool showBaseVal3 = flag;
		bool baseVal3;
		if (flag)
		{
			baseVal3 = ninjaShurikenOrDash.m_canDashToEnemy;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(canDashToEnemyMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_dashIgnoreLosMod, "[DashIgnoreLos]", flag, flag && ninjaShurikenOrDash.m_dashIgnoreLos);
		string str4 = text;
		AbilityModPropertyShape dashDestShapeMod = this.m_dashDestShapeMod;
		string prefix4 = "[DashDestShape]";
		bool showBaseVal4 = flag;
		AbilityAreaShape baseVal4;
		if (flag)
		{
			baseVal4 = ninjaShurikenOrDash.m_dashDestShape;
		}
		else
		{
			baseVal4 = AbilityAreaShape.SingleSquare;
		}
		text = str4 + base.PropDesc(dashDestShapeMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_dashDamageMod, "[DashDamage]", flag, (!flag) ? 0 : ninjaShurikenOrDash.m_dashDamage);
		string str5 = text;
		AbilityModPropertyInt extraDamageOnMarkedMod = this.m_extraDamageOnMarkedMod;
		string prefix5 = "[ExtraDamageOnMarked]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = ninjaShurikenOrDash.m_extraDamageOnMarked;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(extraDamageOnMarkedMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt extraDamageIfNotMarkedMod = this.m_extraDamageIfNotMarkedMod;
		string prefix6 = "[ExtraDamageIfNotMarked]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = ninjaShurikenOrDash.m_extraDamageIfNotMarked;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(extraDamageIfNotMarkedMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_dashEnemyHitEffectMod, "[DashEnemyHitEffect]", flag, (!flag) ? null : ninjaShurikenOrDash.m_dashEnemyHitEffect);
		text += base.PropDesc(this.m_extraEnemyEffectOnMarkedMod, "[ExtraEnemyEffectOnMarked]", flag, (!flag) ? null : ninjaShurikenOrDash.m_extraEnemyEffectOnMarked);
		string str7 = text;
		AbilityModPropertyInt dashHealingMod = this.m_dashHealingMod;
		string prefix7 = "[DashHealing]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = ninjaShurikenOrDash.m_dashHealing;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(dashHealingMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyEffectInfo dashAllyHitEffectMod = this.m_dashAllyHitEffectMod;
		string prefix8 = "[DashAllyHitEffect]";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
		if (flag)
		{
			baseVal8 = ninjaShurikenOrDash.m_dashAllyHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + base.PropDesc(dashAllyHitEffectMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyBool dashApplyDeathmarkMod = this.m_dashApplyDeathmarkMod;
		string prefix9 = "[DashApplyDeathmark]";
		bool showBaseVal9 = flag;
		bool baseVal9;
		if (flag)
		{
			baseVal9 = ninjaShurikenOrDash.m_dashApplyDeathmark;
		}
		else
		{
			baseVal9 = false;
		}
		text = str9 + base.PropDesc(dashApplyDeathmarkMod, prefix9, showBaseVal9, baseVal9);
		text += base.PropDesc(this.m_canTriggerDeathmarkMod, "[CanTriggerDeathmark]", flag, flag && ninjaShurikenOrDash.m_canTriggerDeathmark);
		string str10 = text;
		AbilityModPropertyBool canQueueMoveAfterEvadeMod = this.m_canQueueMoveAfterEvadeMod;
		string prefix10 = "[CanQueueMoveAfterEvade]";
		bool showBaseVal10 = flag;
		bool baseVal10;
		if (flag)
		{
			baseVal10 = ninjaShurikenOrDash.m_canQueueMoveAfterEvade;
		}
		else
		{
			baseVal10 = false;
		}
		return str10 + base.PropDesc(canQueueMoveAfterEvadeMod, prefix10, showBaseVal10, baseVal10);
	}
}
