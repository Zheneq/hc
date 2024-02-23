using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class BarrierResponseOnShot
{
	[Header("-- Sequences for On Shot Visual --")]
	public GameObject m_onShotSequencePrefab;
	public bool m_useShooterPosAsReactionSequenceTargetPos;
	[Header("-- On Owner: For Enemy Shot --")]
	public int m_healOnOwnerFromEnemyShot;
	public int m_energyGainOnOwnerFromEnemyShot;
	public StandardEffectInfo m_effectOnOwnerFromEnemyShot;
	[Header("-- On Shooter: For Enemy Shot --")]
	public int m_damageOnEnemyOnShot;
	public int m_energyLossOnEnemyOnShot;
	public StandardEffectInfo m_effectOnEnemyOnShot;

	public bool HasResponses()
	{
		return m_healOnOwnerFromEnemyShot > 0
			|| m_energyGainOnOwnerFromEnemyShot > 0
			|| m_effectOnOwnerFromEnemyShot.m_applyEffect
			|| m_damageOnEnemyOnShot > 0
			|| m_energyLossOnEnemyOnShot > 0
			|| m_effectOnEnemyOnShot.m_applyEffect;
	}

	public BarrierResponseOnShot GetShallowCopy()
	{
		return (BarrierResponseOnShot)MemberwiseClone();
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare, BarrierResponseOnShot other)
	{
		bool addDiff = addCompare && other != null;
		AbilityMod.AddToken_IntDiff(tokens, new StringBuilder().Append(name).Append("_OnShot_HealOnOwner").ToString(), "", m_healOnOwnerFromEnemyShot, addDiff, addDiff ? other.m_healOnOwnerFromEnemyShot : 0);
		AbilityMod.AddToken_IntDiff(tokens, new StringBuilder().Append(name).Append("_OnShot_EnergyOnOwner").ToString(), "", m_energyGainOnOwnerFromEnemyShot, addDiff, addDiff ? other.m_energyGainOnOwnerFromEnemyShot : 0);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnOwnerFromEnemyShot, new StringBuilder().Append(name).Append("_OnShot_EffectOnOwner").ToString(), addDiff ? other.m_effectOnOwnerFromEnemyShot : null, addDiff);
		AbilityMod.AddToken_IntDiff(tokens, new StringBuilder().Append(name).Append("_OnShot_DamageOnEnemy").ToString(), "", m_damageOnEnemyOnShot, addDiff, addDiff ? other.m_damageOnEnemyOnShot : 0);
		AbilityMod.AddToken_IntDiff(tokens, new StringBuilder().Append(name).Append("_OnShot_EnergyLossOnEnemy").ToString(), "", m_energyLossOnEnemyOnShot, addDiff, addDiff ? other.m_energyLossOnEnemyOnShot : 0);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnEnemyOnShot, new StringBuilder().Append(name).Append("_OnShot_EffectOnEnemy").ToString(), addDiff ? other.m_effectOnEnemyOnShot : null, addDiff);
	}

	public string GetInEditorDescription(string header = "- Response -", string indent = "", bool showDiff = false, BarrierResponseOnShot other = null)
	{
		bool addDiff = showDiff && other != null;
		string otherSep = "\t        \t | in base  =";
		string desc = new StringBuilder().Append("\n").Append(InEditorDescHelper.BoldedStirng(header)).Append("\n").ToString();
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Heal on Owner on Enemy shot ] = ", indent, otherSep, m_healOnOwnerFromEnemyShot, addDiff, addDiff ? other.m_healOnOwnerFromEnemyShot : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ EnergyGain on Owner on Enemy shot ] = ", indent, otherSep, m_energyGainOnOwnerFromEnemyShot, addDiff, addDiff ? other.m_energyGainOnOwnerFromEnemyShot : 0);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnOwnerFromEnemyShot, "{ Effect on Owner on Enemy shot }", indent, addDiff, addDiff ? other.m_effectOnOwnerFromEnemyShot : null);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Damage on Enemy on shot ] = ", indent, otherSep, m_damageOnEnemyOnShot, addDiff, addDiff ? other.m_damageOnEnemyOnShot : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ EnergyLoss on Enemy on shot ] = ", indent, otherSep, m_energyLossOnEnemyOnShot, addDiff, addDiff ? other.m_energyLossOnEnemyOnShot : 0);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnEnemyOnShot, "{ Effect on Enemy on shot }", indent, addDiff, addDiff ? other.m_effectOnEnemyOnShot : null);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ On Shot Sequence Prefab ]", indent, otherSep, m_onShotSequencePrefab, addDiff, addDiff ? other.m_onShotSequencePrefab : null);
		return new StringBuilder().Append(desc).Append(InEditorDescHelper.AssembleFieldWithDiff("[ Use Shooter Pos As Target Pos For Sequence ] = ", indent, otherSep, m_useShooterPosAsReactionSequenceTargetPos, addDiff, addDiff && other.m_useShooterPosAsReactionSequenceTargetPos)).ToString();
	}
}
