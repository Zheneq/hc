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
		shallowCopy.duration = m_durationMod.GetModifiedValue(input.duration);
		shallowCopy.hitDelayTurns = m_hitDelayTurnsMod.GetModifiedValue(input.hitDelayTurns);
		shallowCopy.shape = m_shapeMod.GetModifiedValue(input.shape);
		shallowCopy.canIncludeCaster = m_canIncludeCasterMod.GetModifiedValue(input.canIncludeCaster);
		shallowCopy.ignoreMovementHits = m_ignoreMovementHitsMod.GetModifiedValue(input.ignoreMovementHits);
		shallowCopy.endIfHasDoneHits = m_endIfHasDoneHitsMod.GetModifiedValue(input.endIfHasDoneHits);
		shallowCopy.ignoreNonCasterAllies = m_ignoreNonCasterAlliesMod.GetModifiedValue(input.ignoreNonCasterAllies);
		shallowCopy.damageAmount = m_damageAmountMod.GetModifiedValue(input.damageAmount);
		shallowCopy.subsequentDamageAmount = m_subsequentDamageAmountMod.GetModifiedValue(input.subsequentDamageAmount);
		shallowCopy.healAmount = m_healAmountMod.GetModifiedValue(input.healAmount);
		shallowCopy.subsequentHealAmount = m_subsequentHealAmountMod.GetModifiedValue(input.subsequentHealAmount);
		shallowCopy.energyGain = m_energyGainMod.GetModifiedValue(input.energyGain);
		shallowCopy.subsequentEnergyGain = m_subsequentEnergyGainMod.GetModifiedValue(input.subsequentEnergyGain);
		shallowCopy.effectOnEnemies = m_effectOnEnemiesMod.GetModifiedValue(input.effectOnEnemies);
		shallowCopy.effectOnAllies = m_effectOnAlliesMod.GetModifiedValue(input.effectOnAllies);
		shallowCopy.persistentSequencePrefab = m_persistentSequencePrefabMod.GetModifiedValue(input.persistentSequencePrefab);
		shallowCopy.hitPulseSequencePrefab = m_hitPulseSequencePrefabMod.GetModifiedValue(input.hitPulseSequencePrefab);
		shallowCopy.allyHitSequencePrefab = m_allyHitSequencePrefabMod.GetModifiedValue(input.allyHitSequencePrefab);
		shallowCopy.enemyHitSequencePrefab = m_enemyHitSequencePrefabMod.GetModifiedValue(input.enemyHitSequencePrefab);
		return shallowCopy;
	}
}
