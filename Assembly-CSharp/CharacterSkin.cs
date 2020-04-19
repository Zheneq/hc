using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CharacterSkin
{
	public string m_name;

	public bool m_isHidden;

	public GameBalanceVars.CharResourceLinkSkinUnlockData m_skinUnlockData;

	[AssetFileSelector("Assets/UI/Textures/Resources/QuestRewards/", "QuestRewards/", ".png")]
	public string m_skinSelectionIconPath;

	public string m_description;

	public string m_flavorText;

	[Header("-- Replacements --")]
	public PrefabResourceLink[] m_satellitePrefabs;

	public PrefabReplacement[] m_replacementSequences;

	public AudioReplacement[] m_replacementAudio;

	public string[] m_allowedAudioTags;

	[FormerlySerializedAs("m_textures")]
	public List<CharacterPattern> m_patterns = new List<CharacterPattern>();

	[Tooltip("Audio assets prefabs that will override the default prefabs from the resource link. (For front end)")]
	public PrefabResourceLink[] m_audioAssetsFrontEndPrefabs;

	[Tooltip("Audio assets prefabs that will override the default prefabs from the resource link. (For in game)")]
	public PrefabResourceLink[] m_audioAssetsInGamePrefabs;

	[LeafDirectoryPopup("Directory containing all .pkfx files for this skin", "PackFx/Character/Hero")]
	public string m_pkfxDirectory;
}
