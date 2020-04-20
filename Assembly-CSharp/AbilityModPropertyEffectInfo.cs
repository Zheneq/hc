using System;

[Serializable]
public class AbilityModPropertyEffectInfo
{
	public AbilityModPropertyEffectInfo.ModOp operation;

	public bool useSequencesFromSource = true;

	public StandardEffectInfo effectInfo;

	public StandardEffectInfo GetModifiedValue(StandardEffectInfo input)
	{
		if (this.operation == AbilityModPropertyEffectInfo.ModOp.Override)
		{
			StandardEffectInfo result = this.effectInfo;
			if (this.useSequencesFromSource && input != null)
			{
				StandardEffectInfo shallowCopy = this.effectInfo.GetShallowCopy();
				shallowCopy.m_effectData.m_sequencePrefabs = input.m_effectData.m_sequencePrefabs;
				shallowCopy.m_effectData.m_tickSequencePrefab = input.m_effectData.m_tickSequencePrefab;
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
