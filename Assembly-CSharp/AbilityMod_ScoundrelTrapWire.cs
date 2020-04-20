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
		ScoundrelTrapWire scoundrelTrapWire = base.GetTargetAbilityOnAbilityData(abilityData) as ScoundrelTrapWire;
		bool flag = scoundrelTrapWire != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat barrierScaleMod = this.m_barrierScaleMod;
		string prefix = "[Barrier Scale]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = scoundrelTrapWire.m_barrierData.m_width;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(barrierScaleMod, prefix, showBaseVal, baseVal);
		if (this.m_barrierSequence != null)
		{
			if (this.m_barrierSequence.Count > 0)
			{
				text += "Has Sequence Prefab overrides for Barrier\n";
			}
		}
		if (this.m_useEnemyMovedThroughOverride)
		{
			text += this.m_enemyMovedThroughOverride.GetInEditorDescription("{ Enemy Moved Through Override }", "    ", flag, (!flag) ? null : scoundrelTrapWire.m_barrierData.m_onEnemyMovedThrough);
		}
		if (this.m_useAllyMovedThroughOverride)
		{
			string str2 = text;
			GameplayResponseForActor allyMovedThroughOverride = this.m_allyMovedThroughOverride;
			string header = "{ Ally Moved Through Override }";
			string indent = "    ";
			bool showDiff = flag;
			GameplayResponseForActor other;
			if (flag)
			{
				other = scoundrelTrapWire.m_barrierData.m_onAllyMovedThrough;
			}
			else
			{
				other = null;
			}
			text = str2 + allyMovedThroughOverride.GetInEditorDescription(header, indent, showDiff, other);
		}
		string str3 = text;
		AbilityModPropertyBarrierDataV2 barrierDataMod = this.m_barrierDataMod;
		string prefix2 = "{ Barrier Data Mod }";
		StandardBarrierData baseVal2;
		if (flag)
		{
			baseVal2 = scoundrelTrapWire.m_barrierData;
		}
		else
		{
			baseVal2 = null;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(barrierDataMod, prefix2, baseVal2);
		if (this.m_cooldownReductionsWhenNoHits.HasCooldownReduction())
		{
			text += this.m_cooldownReductionsWhenNoHits.GetDescription(abilityData);
		}
		return text + "\n";
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScoundrelTrapWire scoundrelTrapWire = targetAbility as ScoundrelTrapWire;
		bool flag = scoundrelTrapWire != null;
		AbilityModPropertyBarrierDataV2 barrierDataMod = this.m_barrierDataMod;
		string tokenName = "Wall";
		StandardBarrierData baseVal;
		if (flag)
		{
			baseVal = scoundrelTrapWire.m_barrierData;
		}
		else
		{
			baseVal = null;
		}
		AbilityMod.AddToken_BarrierMod(tokens, barrierDataMod, tokenName, baseVal);
		this.m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "CooldownReductionOnMiss");
	}
}
