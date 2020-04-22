using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CharacterPattern
{
	public string m_name;

	public bool m_isHidden;

	public GameBalanceVars.CharResourceLinkPatternUnlockData m_patternUnlockData;

	[FormerlySerializedAs("m_textureSelectionIcon")]
	public Sprite m_patternSelectionIcon;

	public Color m_UIDisplayColor;

	public List<CharacterColor> m_colors = new List<CharacterColor>();

	[Header("-- Prefab Replacements --")]
	public PrefabResourceLink[] m_satellitePrefabs;

	public PrefabReplacement[] m_replacementSequences;
}
