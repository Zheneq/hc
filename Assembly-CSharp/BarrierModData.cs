using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BarrierModData
{
	public AbilityModPropertyInt m_durationMod;

	public AbilityModPropertyFloat m_widthMod;

	public AbilityModPropertyBool m_bidirectionalMod;

	public AbilityModPropertyInt m_maxHitsMod;

	[Header("-- Blocking Rules")]
	public AbilityModPropertyBlockingRules m_blocksVisionMod;

	public AbilityModPropertyBlockingRules m_blocksAbilitiesMod;

	public AbilityModPropertyBlockingRules m_blocksMovementMod;

	public AbilityModPropertyBlockingRules m_blocksMovementOnCrossoverMod;

	public AbilityModPropertyBlockingRules m_blocksPositionTargetingMod;

	[Header("-- As Cover")]
	public AbilityModPropertyBool m_considerAsCoverMod;

	[Header("-- Enemy Move-Through Hit")]
	public AbilityModPropertyInt m_enemyMoveThroughDamageMod;

	public AbilityModPropertyInt m_enemyMoveThroughEnergyMod;

	public AbilityModPropertyEffectInfo m_enemyMoveThroughEffectMod;

	[Header("-- Ally Move-Through Hit")]
	public AbilityModPropertyInt m_allyMoveThroughHealMod;

	public AbilityModPropertyInt m_allyMoveThroughEnergyMod;

	public AbilityModPropertyEffectInfo m_allyMoveThroughEffectMod;

	[Header("-- Removal Rules")]
	public AbilityModPropertyBool m_removeOnTurnEndIfEnemyCrossed;

	public AbilityModPropertyBool m_removeOnTurnEndIfAllyCrossed;

	[Space(5f)]
	public AbilityModPropertyBool m_removeOnPhaseEndIfEnemyCrossed;

	public AbilityModPropertyBool m_removeOnPhaseEndIfAllyCrossed;

	[Space(5f)]
	public AbilityModPropertyBool m_removeOnCasterDeath;

	[Header("-- On Shot Block Response")]
	public AbilityModPropertyInt m_healOnOwnerForShotBlock;

	public AbilityModPropertyInt m_energyGainOnOwnerForShotBlock;

	public AbilityModPropertyEffectInfo m_effectOnOwnerForShotBlock;

	[Space(10f)]
	public AbilityModPropertyInt m_damageOnEnemyForShotBlock;

	public AbilityModPropertyInt m_energyLossOnEnemyForShotBlock;

	public AbilityModPropertyEffectInfo m_effectOnEnemyForShotBlock;

	[Header("-- Sequences Overrides")]
	public AbilityModPropertySequenceOverride m_onEnemyCrossHitSequenceOverride;

	public AbilityModPropertySequenceOverride m_onAllyCrossHitSequenceOverride;

	public bool m_useBarrierSequenceOverride;

	public List<GameObject> m_barrierSequenceOverrides = new List<GameObject>();

	public AbilityModPropertySequenceOverride m_responseOnShotSequenceOverride;

	public AbilityModPropertyBool m_useShooterPosAsReactionSequenceTargetPosMod;

	public StandardBarrierData GetModifiedCopy(StandardBarrierData input)
	{
		StandardBarrierData shallowCopy = input.GetShallowCopy();
		shallowCopy.m_maxDuration = m_durationMod.GetModifiedValue(input.m_maxDuration);
		shallowCopy.m_width = m_widthMod.GetModifiedValue(input.m_width);
		shallowCopy.m_bidirectional = m_bidirectionalMod.GetModifiedValue(input.m_bidirectional);
		shallowCopy.m_maxHits = m_maxHitsMod.GetModifiedValue(input.m_maxHits);
		shallowCopy.m_blocksVision = m_blocksVisionMod.GetModifiedValue(input.m_blocksVision);
		shallowCopy.m_blocksAbilities = m_blocksAbilitiesMod.GetModifiedValue(input.m_blocksAbilities);
		shallowCopy.m_blocksMovement = m_blocksMovementMod.GetModifiedValue(input.m_blocksMovement);
		shallowCopy.m_blocksMovementOnCrossover = m_blocksMovementOnCrossoverMod.GetModifiedValue(input.m_blocksMovementOnCrossover);
		shallowCopy.m_blocksPositionTargeting = m_blocksPositionTargetingMod.GetModifiedValue(input.m_blocksPositionTargeting);
		shallowCopy.m_considerAsCover = m_considerAsCoverMod.GetModifiedValue(input.m_considerAsCover);
		shallowCopy.m_onEnemyMovedThrough.m_damage = m_enemyMoveThroughDamageMod.GetModifiedValue(input.m_onEnemyMovedThrough.m_damage);
		shallowCopy.m_onEnemyMovedThrough.m_techPoints = m_enemyMoveThroughEnergyMod.GetModifiedValue(input.m_onEnemyMovedThrough.m_techPoints);
		shallowCopy.m_onEnemyMovedThrough.m_effect = m_enemyMoveThroughEffectMod.GetModifiedValue(input.m_onEnemyMovedThrough.m_effect);
		shallowCopy.m_onAllyMovedThrough.m_healing = m_allyMoveThroughHealMod.GetModifiedValue(input.m_onAllyMovedThrough.m_healing);
		shallowCopy.m_onAllyMovedThrough.m_techPoints = m_allyMoveThroughEnergyMod.GetModifiedValue(input.m_onAllyMovedThrough.m_techPoints);
		shallowCopy.m_onAllyMovedThrough.m_effect = m_allyMoveThroughEffectMod.GetModifiedValue(input.m_onAllyMovedThrough.m_effect);
		shallowCopy.m_removeAtTurnEndIfEnemyMovedThrough = m_removeOnTurnEndIfEnemyCrossed.GetModifiedValue(input.m_removeAtTurnEndIfEnemyMovedThrough);
		shallowCopy.m_removeAtTurnEndIfAllyMovedThrough = m_removeOnTurnEndIfAllyCrossed.GetModifiedValue(input.m_removeAtTurnEndIfAllyMovedThrough);
		shallowCopy.m_removeAtPhaseEndIfEnemyMovedThrough = m_removeOnPhaseEndIfEnemyCrossed.GetModifiedValue(input.m_removeAtPhaseEndIfEnemyMovedThrough);
		shallowCopy.m_removeAtPhaseEndIfAllyMovedThrough = m_removeOnPhaseEndIfAllyCrossed.GetModifiedValue(input.m_removeAtPhaseEndIfAllyMovedThrough);
		shallowCopy.m_endOnCasterDeath = m_removeOnCasterDeath.GetModifiedValue(input.m_endOnCasterDeath);
		shallowCopy.m_onEnemyMovedThrough.m_sequenceToPlay = m_onEnemyCrossHitSequenceOverride.GetModifiedValue(input.m_onEnemyMovedThrough.m_sequenceToPlay);
		shallowCopy.m_onAllyMovedThrough.m_sequenceToPlay = m_onAllyCrossHitSequenceOverride.GetModifiedValue(input.m_onAllyMovedThrough.m_sequenceToPlay);
		shallowCopy.m_responseOnShotBlock.m_onShotSequencePrefab = m_responseOnShotSequenceOverride.GetModifiedValue(input.m_responseOnShotBlock.m_onShotSequencePrefab);
		shallowCopy.m_responseOnShotBlock.m_useShooterPosAsReactionSequenceTargetPos = m_useShooterPosAsReactionSequenceTargetPosMod.GetModifiedValue(input.m_responseOnShotBlock.m_useShooterPosAsReactionSequenceTargetPos);
		shallowCopy.m_responseOnShotBlock.m_healOnOwnerFromEnemyShot = m_healOnOwnerForShotBlock.GetModifiedValue(input.m_responseOnShotBlock.m_healOnOwnerFromEnemyShot);
		shallowCopy.m_responseOnShotBlock.m_energyGainOnOwnerFromEnemyShot = m_energyGainOnOwnerForShotBlock.GetModifiedValue(input.m_responseOnShotBlock.m_energyGainOnOwnerFromEnemyShot);
		shallowCopy.m_responseOnShotBlock.m_effectOnOwnerFromEnemyShot = m_effectOnOwnerForShotBlock.GetModifiedValue(input.m_responseOnShotBlock.m_effectOnOwnerFromEnemyShot);
		shallowCopy.m_responseOnShotBlock.m_damageOnEnemyOnShot = m_damageOnEnemyForShotBlock.GetModifiedValue(input.m_responseOnShotBlock.m_damageOnEnemyOnShot);
		shallowCopy.m_responseOnShotBlock.m_energyLossOnEnemyOnShot = m_energyLossOnEnemyForShotBlock.GetModifiedValue(input.m_responseOnShotBlock.m_energyLossOnEnemyOnShot);
		shallowCopy.m_responseOnShotBlock.m_effectOnEnemyOnShot = m_effectOnEnemyForShotBlock.GetModifiedValue(input.m_responseOnShotBlock.m_effectOnEnemyOnShot);
		if (m_useBarrierSequenceOverride)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			shallowCopy.m_barrierSequencePrefabs = new List<GameObject>(m_barrierSequenceOverrides);
		}
		return shallowCopy;
	}
}
