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
		bool isAbilityPresent = scoundrelTrapWire != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_barrierScaleMod, "[Barrier Scale]", isAbilityPresent, isAbilityPresent ? scoundrelTrapWire.m_barrierData.m_width : 0f);
		if (m_barrierSequence != null && m_barrierSequence.Count > 0)
		{
			desc += "Has Sequence Prefab overrides for Barrier\n";
		}
		if (m_useEnemyMovedThroughOverride)
		{
			desc += m_enemyMovedThroughOverride.GetInEditorDescription("{ Enemy Moved Through Override }", "    ", isAbilityPresent, isAbilityPresent ? scoundrelTrapWire.m_barrierData.m_onEnemyMovedThrough : null);
		}
		if (m_useAllyMovedThroughOverride)
		{
			desc += m_allyMovedThroughOverride.GetInEditorDescription("{ Ally Moved Through Override }", "    ", isAbilityPresent, isAbilityPresent ? scoundrelTrapWire.m_barrierData.m_onAllyMovedThrough : null);
		}
		desc += AbilityModHelper.GetModPropertyDesc(m_barrierDataMod, "{ Barrier Data Mod }", isAbilityPresent ? scoundrelTrapWire.m_barrierData : null);
		if (m_cooldownReductionsWhenNoHits.HasCooldownReduction())
		{
			desc += m_cooldownReductionsWhenNoHits.GetDescription(abilityData);
		}
		return desc + "\n";
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScoundrelTrapWire scoundrelTrapWire = targetAbility as ScoundrelTrapWire;
		AddToken_BarrierMod(tokens, m_barrierDataMod, "Wall", scoundrelTrapWire != null ? scoundrelTrapWire.m_barrierData : null);
		m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "CooldownReductionOnMiss");
	}
}
