using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GroundEffectField
{
	public int duration = 2;
	public int hitDelayTurns;
	public AbilityAreaShape shape = AbilityAreaShape.Three_x_Three;
	public bool canIncludeCaster = true;
	[Header("-- Whether to ignore movement hits")]
	public bool ignoreMovementHits;
	public bool endIfHasDoneHits;
	public bool ignoreNonCasterAllies;
	[Header("-- For Hits --")]
	public int damageAmount;
	public int subsequentDamageAmount;
	public int healAmount;
	public int subsequentHealAmount;
	public int energyGain;
	public int subsequentEnergyGain;
	public bool stopMovementInField;
	public bool stopMovementOutOfField;
	public StandardEffectInfo effectOnEnemies;
	public StandardEffectInfo effectOnAllies;
	[Header("-- Sequences --")]
	public GameObject persistentSequencePrefab;
	public GameObject hitPulseSequencePrefab;
	public GameObject allyHitSequencePrefab;
	public GameObject enemyHitSequencePrefab;
	public bool perSquareSequences;

	internal bool penetrateLos => true;

	public void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemySubject, AbilityTooltipSubject allySubject)
	{
		effectOnEnemies.ReportAbilityTooltipNumbers(ref numbers, enemySubject);
		effectOnAllies.ReportAbilityTooltipNumbers(ref numbers, allySubject);
		AbilityTooltipHelper.ReportDamage(ref numbers, enemySubject, damageAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, allySubject, healAmount);
		AbilityTooltipHelper.ReportEnergy(ref numbers, allySubject, energyGain);
	}

	public bool IncludeEnemies()
	{
		return damageAmount > 0 || effectOnEnemies.m_applyEffect;
	}

	public bool IncludeAllies()
	{
		return healAmount > 0 || effectOnAllies.m_applyEffect || energyGain > 0;
	}

	public List<Team> GetAffectedTeams(ActorData allyActor)
	{
		return TargeterUtils.GetRelevantTeams(allyActor, IncludeAllies(), IncludeEnemies());
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare = false, GroundEffectField other = null)
	{
		bool addDiff = addCompare && other != null;
		AbilityMod.AddToken_IntDiff(tokens, name + "_Duration", "", duration, addDiff, addDiff ? other.duration : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_HitDelayTurns", "", hitDelayTurns, addDiff, addDiff ? other.hitDelayTurns : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_Damage", "", damageAmount, addDiff, addDiff ? other.damageAmount : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_SubsequentDamage", "", subsequentDamageAmount, addDiff, addDiff ? other.subsequentDamageAmount : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_Healing", "", healAmount, addDiff, addDiff ? other.healAmount : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_SubsequentHealing", "", subsequentHealAmount, addDiff, addDiff ? other.subsequentHealAmount : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_AllyEnergyGain", "", energyGain, addDiff, addDiff ? other.energyGain : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_SubsequentEnergyGain", "", subsequentEnergyGain, addDiff, addDiff ? other.subsequentEnergyGain : 0);
		AbilityMod.AddToken_EffectInfo(tokens, effectOnEnemies, "EnemyHitEffect", addDiff ? other.effectOnEnemies : null, addDiff);
		AbilityMod.AddToken_EffectInfo(tokens, effectOnAllies, "AllyHitEffect", addDiff ? other.effectOnAllies : null, addDiff);
	}

	public GroundEffectField GetShallowCopy()
	{
		return (GroundEffectField)MemberwiseClone();
	}

	public string GetInEditorDescription(string header = "GroundEffectField", string indent = "    ", bool diff = false, GroundEffectField other = null)
	{
		bool addDiff = diff && other != null;
		string otherSep = "\t        \t | in base  =";
		string desc = InEditorDescHelper.BoldedStirng(header) + "\n";
		if (duration <= 0)
		{
			desc += indent + "WARNING: IS PERMANENT (duration <= 0). Woof Woof Woof Woof\n";
		}
		else
		{
			desc += InEditorDescHelper.AssembleFieldWithDiff("[ Max Duration ] = ", indent, otherSep, duration, addDiff, addDiff ? other.duration : 0);
		}
		if (hitDelayTurns > 0)
		{
			desc += InEditorDescHelper.AssembleFieldWithDiff("[ Hit Delay Turns ] = ", indent, otherSep, hitDelayTurns, addDiff, addDiff ? other.hitDelayTurns : 0);
		}
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Shape ] = ", indent, otherSep, shape, addDiff, addDiff ? other.shape : AbilityAreaShape.SingleSquare);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Ignore Movement Hits? ] = ", indent, otherSep, ignoreMovementHits, addDiff, addDiff && other.ignoreMovementHits);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ End If Has Done Hits? ] = ", indent, otherSep, endIfHasDoneHits, addDiff, addDiff && other.endIfHasDoneHits);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Ignore Non Caster Allies? ] = ", indent, otherSep, ignoreNonCasterAllies, addDiff, addDiff && other.ignoreNonCasterAllies);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Damage ] = ", indent, otherSep, damageAmount, addDiff, addDiff ? other.damageAmount : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Subsequent Damage ] = ", indent, otherSep, subsequentDamageAmount, addDiff, addDiff ? other.subsequentDamageAmount : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Healing ] = ", indent, otherSep, healAmount, addDiff, addDiff ? other.healAmount : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Subsequent Healing ] = ", indent, otherSep, subsequentHealAmount, addDiff, addDiff ? other.subsequentHealAmount : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ EnergyGain ] = ", indent, otherSep, energyGain, addDiff, addDiff ? other.energyGain : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Subsequent EnergyGain ] = ", indent, otherSep, subsequentEnergyGain, addDiff, addDiff ? other.subsequentEnergyGain : 0);
		if (effectOnEnemies.m_applyEffect)
		{
			desc += indent + "Effect on Enemies:\n";
			desc += effectOnEnemies.m_effectData.GetInEditorDescription(indent, false, addDiff, addDiff ? other.effectOnEnemies.m_effectData : null);
		}
		if (effectOnAllies.m_applyEffect)
		{
			desc += indent + "Effect on Allies:\n";
			desc += effectOnAllies.m_effectData.GetInEditorDescription(indent, false, addDiff, addDiff ? other.effectOnAllies.m_effectData : null);
		}
		desc += InEditorDescHelper.AssembleFieldWithDiff("Persistent Sequence Prefab", indent, otherSep, persistentSequencePrefab, addDiff, addDiff ? other.persistentSequencePrefab : null);
		desc += InEditorDescHelper.AssembleFieldWithDiff("Hit Pulse Sequence", indent, otherSep, hitPulseSequencePrefab, addDiff, addDiff ? other.hitPulseSequencePrefab : null);
		desc += InEditorDescHelper.AssembleFieldWithDiff("Ally Hit Sequence", indent, otherSep, allyHitSequencePrefab, addDiff, addDiff ? other.allyHitSequencePrefab : null);
		desc += InEditorDescHelper.AssembleFieldWithDiff("Enemy Hit Sequence", indent, otherSep, enemyHitSequencePrefab, addDiff, addDiff ? other.enemyHitSequencePrefab : null);
		return desc + "-  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -\n";
	}
}
