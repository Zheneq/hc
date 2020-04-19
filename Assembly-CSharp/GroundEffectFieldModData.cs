using System;
using UnityEngine;

[Serializable]
public class GroundEffectFieldModData
{
	public AbilityModPropertyInt m_durationMod;

	public AbilityModPropertyInt m_hitDelayTurnsMod;

	public AbilityModPropertyShape m_shapeMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	public AbilityModPropertyBool m_canIncludeCasterMod;

	[Header("-- Whether to ignore movement hits")]
	public AbilityModPropertyBool m_ignoreMovementHitsMod;

	public AbilityModPropertyBool m_endIfHasDoneHitsMod;

	public AbilityModPropertyBool m_ignoreNonCasterAlliesMod;

	[Header("-- On Hit Stuff --")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyInt m_subsequentDamageAmountMod;

	[Space(5f)]
	public AbilityModPropertyInt m_healAmountMod;

	public AbilityModPropertyInt m_subsequentHealAmountMod;

	[Space(5f)]
	public AbilityModPropertyInt m_energyGainMod;

	public AbilityModPropertyInt m_subsequentEnergyGainMod;

	[Space(5f)]
	public AbilityModPropertyEffectInfo m_effectOnEnemiesMod;

	public AbilityModPropertyEffectInfo m_effectOnAlliesMod;

	[Header("-- Sequences --")]
	public AbilityModPropertySequenceOverride m_persistentSequencePrefabMod;

	public AbilityModPropertySequenceOverride m_hitPulseSequencePrefabMod;

	public AbilityModPropertySequenceOverride m_allyHitSequencePrefabMod;

	public AbilityModPropertySequenceOverride m_enemyHitSequencePrefabMod;

	public GroundEffectField GetModifiedCopy(GroundEffectField input)
	{
		GroundEffectField shallowCopy = input.GetShallowCopy();
		shallowCopy.duration = this.m_durationMod.GetModifiedValue(input.duration);
		shallowCopy.hitDelayTurns = this.m_hitDelayTurnsMod.GetModifiedValue(input.hitDelayTurns);
		shallowCopy.shape = this.m_shapeMod.GetModifiedValue(input.shape);
		shallowCopy.canIncludeCaster = this.m_canIncludeCasterMod.GetModifiedValue(input.canIncludeCaster);
		shallowCopy.ignoreMovementHits = this.m_ignoreMovementHitsMod.GetModifiedValue(input.ignoreMovementHits);
		shallowCopy.endIfHasDoneHits = this.m_endIfHasDoneHitsMod.GetModifiedValue(input.endIfHasDoneHits);
		shallowCopy.ignoreNonCasterAllies = this.m_ignoreNonCasterAlliesMod.GetModifiedValue(input.ignoreNonCasterAllies);
		shallowCopy.damageAmount = this.m_damageAmountMod.GetModifiedValue(input.damageAmount);
		shallowCopy.subsequentDamageAmount = this.m_subsequentDamageAmountMod.GetModifiedValue(input.subsequentDamageAmount);
		shallowCopy.healAmount = this.m_healAmountMod.GetModifiedValue(input.healAmount);
		shallowCopy.subsequentHealAmount = this.m_subsequentHealAmountMod.GetModifiedValue(input.subsequentHealAmount);
		shallowCopy.energyGain = this.m_energyGainMod.GetModifiedValue(input.energyGain);
		shallowCopy.subsequentEnergyGain = this.m_subsequentEnergyGainMod.GetModifiedValue(input.subsequentEnergyGain);
		shallowCopy.effectOnEnemies = this.m_effectOnEnemiesMod.GetModifiedValue(input.effectOnEnemies);
		shallowCopy.effectOnAllies = this.m_effectOnAlliesMod.GetModifiedValue(input.effectOnAllies);
		shallowCopy.persistentSequencePrefab = this.m_persistentSequencePrefabMod.GetModifiedValue(input.persistentSequencePrefab);
		shallowCopy.hitPulseSequencePrefab = this.m_hitPulseSequencePrefabMod.GetModifiedValue(input.hitPulseSequencePrefab);
		shallowCopy.allyHitSequencePrefab = this.m_allyHitSequencePrefabMod.GetModifiedValue(input.allyHitSequencePrefab);
		shallowCopy.enemyHitSequencePrefab = this.m_enemyHitSequencePrefabMod.GetModifiedValue(input.enemyHitSequencePrefab);
		return shallowCopy;
	}
}
