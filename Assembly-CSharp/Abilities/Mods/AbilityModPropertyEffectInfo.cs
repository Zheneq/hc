using System;

[Serializable]
public class AbilityModPropertyEffectInfo
{
	public enum ModOp
	{
		Ignore,
		Override
	}

	public ModOp operation;

	public bool useSequencesFromSource = true;

	public StandardEffectInfo effectInfo;

	public StandardEffectInfo GetModifiedValue(StandardEffectInfo input)
	{
		if (operation == ModOp.Override)
		{
			StandardEffectInfo result = effectInfo;
			if (useSequencesFromSource && input != null)
			{
				StandardEffectInfo shallowCopy = effectInfo.GetShallowCopy();
				shallowCopy.m_effectData.m_sequencePrefabs = input.m_effectData.m_sequencePrefabs;
				shallowCopy.m_effectData.m_tickSequencePrefab = input.m_effectData.m_tickSequencePrefab;
				result = shallowCopy;
			}
			return result;
		}
		return input;
	}
}
