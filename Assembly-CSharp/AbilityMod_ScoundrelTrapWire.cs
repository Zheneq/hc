using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ScoundrelTrapWire : AbilityMod
{
	[Header("-- Trap Wire barrier scale")]
	public AbilityModPropertyFloat m_barrierScaleMod;

	[Header("-- On Enemy Move Through")]
	public bool m_useEnemyMovedThroughOverride;

	public GameplayResponseForActor m_enemyMovedThroughOverride;

	[Header("-- On Ally Move Through")]
	public bool m_useAllyMovedThroughOverride;

	public GameplayResponseForActor m_allyMovedThroughOverride;

	[Header("-- Barrier Data Override")]
	public AbilityModPropertyBarrierDataV2 m_barrierDataMod;

	[Header("-- On Expire Without Hitting Anyone")]
	public AbilityModCooldownReduction m_cooldownReductionsWhenNoHits;

	public List<GameObject> m_barrierSequence;

	public bool m_barrierPlaySequenceOnAllBarriers;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScoundrelTrapWire);
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelTrapWire scoundrelTrapWire = GetTargetAbilityOnAbilityData(abilityData) as ScoundrelTrapWire;
		bool flag = scoundrelTrapWire != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat barrierScaleMod = m_barrierScaleMod;
		float baseVal;
		if (flag)
		{
			baseVal = scoundrelTrapWire.m_barrierData.m_width;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(barrierScaleMod, "[Barrier Scale]", flag, baseVal);
		if (m_barrierSequence != null)
		{
			if (m_barrierSequence.Count > 0)
			{
				empty += "Has Sequence Prefab overrides for Barrier\n";
			}
		}
		if (m_useEnemyMovedThroughOverride)
		{
			empty += m_enemyMovedThroughOverride.GetInEditorDescription("{ Enemy Moved Through Override }", "    ", flag, (!flag) ? null : scoundrelTrapWire.m_barrierData.m_onEnemyMovedThrough);
		}
		if (m_useAllyMovedThroughOverride)
		{
			string str2 = empty;
			GameplayResponseForActor allyMovedThroughOverride = m_allyMovedThroughOverride;
			object other;
			if (flag)
			{
				other = scoundrelTrapWire.m_barrierData.m_onAllyMovedThrough;
			}
			else
			{
				other = null;
			}
			empty = str2 + allyMovedThroughOverride.GetInEditorDescription("{ Ally Moved Through Override }", "    ", flag, (GameplayResponseForActor)other);
		}
		string str3 = empty;
		AbilityModPropertyBarrierDataV2 barrierDataMod = m_barrierDataMod;
		object baseVal2;
		if (flag)
		{
			baseVal2 = scoundrelTrapWire.m_barrierData;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(barrierDataMod, "{ Barrier Data Mod }", (StandardBarrierData)baseVal2);
		if (m_cooldownReductionsWhenNoHits.HasCooldownReduction())
		{
			empty += m_cooldownReductionsWhenNoHits.GetDescription(abilityData);
		}
		return empty + "\n";
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScoundrelTrapWire scoundrelTrapWire = targetAbility as ScoundrelTrapWire;
		bool flag = scoundrelTrapWire != null;
		AbilityModPropertyBarrierDataV2 barrierDataMod = m_barrierDataMod;
		object baseVal;
		if (flag)
		{
			baseVal = scoundrelTrapWire.m_barrierData;
		}
		else
		{
			baseVal = null;
		}
		AbilityMod.AddToken_BarrierMod(tokens, barrierDataMod, "Wall", (StandardBarrierData)baseVal);
		m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "CooldownReductionOnMiss");
	}
}
