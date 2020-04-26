using System;

[Serializable]
public class AbilityModPropertyEffectData
{
	public enum ModOp
	{
		Ignore,
		Override
	}

	public ModOp operation;

	public bool useSequencesFromSource = true;

	public StandardActorEffectData effectData;

	public StandardActorEffectData GetModifiedValue(StandardActorEffectData input)
	{
		if (operation == ModOp.Override)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					StandardActorEffectData result = effectData;
					if (useSequencesFromSource)
					{
						StandardActorEffectData shallowCopy = effectData.GetShallowCopy();
						shallowCopy.m_sequencePrefabs = input.m_sequencePrefabs;
						shallowCopy.m_tickSequencePrefab = input.m_tickSequencePrefab;
						result = shallowCopy;
					}
					return result;
				}
				}
			}
		}
		return input;
	}
}
