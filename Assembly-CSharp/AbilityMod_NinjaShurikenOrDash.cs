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
		if (!(ninjaShurikenOrDash != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_dashRangeDefaultMod, "DashRangeDefault", string.Empty, ninjaShurikenOrDash.m_dashRangeDefault);
			AbilityMod.AddToken(tokens, m_dashRangeMarkedMod, "DashRangeMarked", string.Empty, ninjaShurikenOrDash.m_dashRangeMarked);
			AbilityMod.AddToken(tokens, m_dashToUnmarkedRangeMod, "DashToUnmarkedRange", string.Empty, ninjaShurikenOrDash.m_dashToUnmarkedRange);
			AbilityMod.AddToken(tokens, m_dashDamageMod, "DashDamage", string.Empty, ninjaShurikenOrDash.m_dashDamage);
			AbilityMod.AddToken(tokens, m_extraDamageOnMarkedMod, "ExtraDamageOnMarked", string.Empty, ninjaShurikenOrDash.m_extraDamageOnMarked);
			AbilityMod.AddToken(tokens, m_extraDamageIfNotMarkedMod, "ExtraDamageIfNotMarked", string.Empty, ninjaShurikenOrDash.m_extraDamageIfNotMarked);
			AbilityMod.AddToken_EffectMod(tokens, m_dashEnemyHitEffectMod, "DashEnemyHitEffect", ninjaShurikenOrDash.m_dashEnemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_extraEnemyEffectOnMarkedMod, "ExtraEnemyEffectOnMarked", ninjaShurikenOrDash.m_extraEnemyEffectOnMarked);
			AbilityMod.AddToken(tokens, m_dashHealingMod, "DashHealing", string.Empty, ninjaShurikenOrDash.m_dashHealing);
			AbilityMod.AddToken_EffectMod(tokens, m_dashAllyHitEffectMod, "DashAllyHitEffect", ninjaShurikenOrDash.m_dashAllyHitEffect);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NinjaShurikenOrDash ninjaShurikenOrDash = GetTargetAbilityOnAbilityData(abilityData) as NinjaShurikenOrDash;
		bool flag = ninjaShurikenOrDash != null;
		string empty = string.Empty;
		empty += PropDesc(m_isTeleportMod, "[IsTeleport]", flag, flag && ninjaShurikenOrDash.m_isTeleport);
		empty += PropDesc(m_dashRangeDefaultMod, "[DashRangeDefault]", flag, (!flag) ? 0f : ninjaShurikenOrDash.m_dashRangeDefault);
		empty += PropDesc(m_dashRangeMarkedMod, "[DashRangeMarked]", flag, (!flag) ? 0f : ninjaShurikenOrDash.m_dashRangeMarked);
		empty += PropDesc(m_dashRequireDeathmarkMod, "[DashRequireDeathmark]", flag, flag && ninjaShurikenOrDash.m_dashRequireDeathmark);
		string str = empty;
		AbilityModPropertyFloat dashToUnmarkedRangeMod = m_dashToUnmarkedRangeMod;
		float baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = ninjaShurikenOrDash.m_dashToUnmarkedRange;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(dashToUnmarkedRangeMod, "[DashToUnmarkedRange]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyBool canDashToAllyMod = m_canDashToAllyMod;
		int baseVal2;
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
			baseVal2 = (ninjaShurikenOrDash.m_canDashToAlly ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(canDashToAllyMod, "[CanDashToAlly]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyBool canDashToEnemyMod = m_canDashToEnemyMod;
		int baseVal3;
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
			baseVal3 = (ninjaShurikenOrDash.m_canDashToEnemy ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(canDashToEnemyMod, "[CanDashToEnemy]", flag, (byte)baseVal3 != 0);
		empty += PropDesc(m_dashIgnoreLosMod, "[DashIgnoreLos]", flag, flag && ninjaShurikenOrDash.m_dashIgnoreLos);
		string str4 = empty;
		AbilityModPropertyShape dashDestShapeMod = m_dashDestShapeMod;
		int baseVal4;
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
			baseVal4 = (int)ninjaShurikenOrDash.m_dashDestShape;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(dashDestShapeMod, "[DashDestShape]", flag, (AbilityAreaShape)baseVal4);
		empty += PropDesc(m_dashDamageMod, "[DashDamage]", flag, flag ? ninjaShurikenOrDash.m_dashDamage : 0);
		string str5 = empty;
		AbilityModPropertyInt extraDamageOnMarkedMod = m_extraDamageOnMarkedMod;
		int baseVal5;
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
			baseVal5 = ninjaShurikenOrDash.m_extraDamageOnMarked;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(extraDamageOnMarkedMod, "[ExtraDamageOnMarked]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt extraDamageIfNotMarkedMod = m_extraDamageIfNotMarkedMod;
		int baseVal6;
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
			baseVal6 = ninjaShurikenOrDash.m_extraDamageIfNotMarked;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(extraDamageIfNotMarkedMod, "[ExtraDamageIfNotMarked]", flag, baseVal6);
		empty += PropDesc(m_dashEnemyHitEffectMod, "[DashEnemyHitEffect]", flag, (!flag) ? null : ninjaShurikenOrDash.m_dashEnemyHitEffect);
		empty += PropDesc(m_extraEnemyEffectOnMarkedMod, "[ExtraEnemyEffectOnMarked]", flag, (!flag) ? null : ninjaShurikenOrDash.m_extraEnemyEffectOnMarked);
		string str7 = empty;
		AbilityModPropertyInt dashHealingMod = m_dashHealingMod;
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
			baseVal7 = ninjaShurikenOrDash.m_dashHealing;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(dashHealingMod, "[DashHealing]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyEffectInfo dashAllyHitEffectMod = m_dashAllyHitEffectMod;
		object baseVal8;
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
			baseVal8 = ninjaShurikenOrDash.m_dashAllyHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str8 + PropDesc(dashAllyHitEffectMod, "[DashAllyHitEffect]", flag, (StandardEffectInfo)baseVal8);
		string str9 = empty;
		AbilityModPropertyBool dashApplyDeathmarkMod = m_dashApplyDeathmarkMod;
		int baseVal9;
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
			baseVal9 = (ninjaShurikenOrDash.m_dashApplyDeathmark ? 1 : 0);
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(dashApplyDeathmarkMod, "[DashApplyDeathmark]", flag, (byte)baseVal9 != 0);
		empty += PropDesc(m_canTriggerDeathmarkMod, "[CanTriggerDeathmark]", flag, flag && ninjaShurikenOrDash.m_canTriggerDeathmark);
		string str10 = empty;
		AbilityModPropertyBool canQueueMoveAfterEvadeMod = m_canQueueMoveAfterEvadeMod;
		int baseVal10;
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
			baseVal10 = (ninjaShurikenOrDash.m_canQueueMoveAfterEvade ? 1 : 0);
		}
		else
		{
			baseVal10 = 0;
		}
		return str10 + PropDesc(canQueueMoveAfterEvadeMod, "[CanQueueMoveAfterEvade]", flag, (byte)baseVal10 != 0);
	}
}
