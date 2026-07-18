using System;

[Serializable]
public class CharacterAbilityVfxSwap
{
	public string m_swapName;
	public bool m_isHidden;
	public int m_uniqueID;
	public PrefabReplacement[] m_replacementSequences;
	public string m_swapVideoPath;
	public GameBalanceVars.AbilityVfxUnlockData m_vfxSwapUnlockData;
}
