﻿using System;

[Serializable]
public class AbilityModPropertyEffectData
{
	public AbilityModPropertyEffectData.ModOp operation;

	public bool useSequencesFromSource = true;

	public StandardActorEffectData effectData;

	public StandardActorEffectData GetModifiedValue(StandardActorEffectData input)
	{
		if (this.operation == AbilityModPropertyEffectData.ModOp.Override)
		{
			StandardActorEffectData result = this.effectData;
			if (this.useSequencesFromSource)
			{
				StandardActorEffectData shallowCopy = this.effectData.GetShallowCopy();
				shallowCopy.m_sequencePrefabs = input.m_sequencePrefabs;
				shallowCopy.m_tickSequencePrefab = input.m_tickSequencePrefab;
				result = shallowCopy;
			}
			return result;
		}
		return input;
	}

	public enum ModOp
	{
		Ignore,
		Override
	}
}
