using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameWideData : MonoBehaviour
{
	private static GameWideData s_instance;

	[Header("Buff [Haste]")]
	public int m_hasteHalfMovementAdjustAmount;

	public int m_hasteFullMovementAdjustAmount;

	public float m_hasteMovementMultiplier = 1f;

	[Header("Debuff [SlowMovement]")]
	public int m_slowHalfMovementAdjustAmount;

	public int m_slowFullMovementAdjustAmount;

	public float m_slowMovementMultiplier = 1f;

	[Header("Buff [Empowered]")]
	public AbilityModPropertyInt m_empoweredOutgoingDamageMod;

	public AbilityModPropertyInt m_empoweredOutgoingHealingMod;

	public AbilityModPropertyInt m_empoweredOutgoingAbsorbMod;

	[Header("Debuff [Weakened]")]
	public AbilityModPropertyInt m_weakenedOutgoingDamageMod;

	public AbilityModPropertyInt m_weakenedOutgoingHealingMod;

	public AbilityModPropertyInt m_weakenedOutgoingAbsorbMod;

	[Header("Buff [Armored]")]
	public AbilityModPropertyInt m_armoredIncomingDamageMod;

	[Header("Debuff [Vulnerable] (values used if positive)")]
	public float m_vulnerableDamageMultiplier = -1f;

	public int m_vulnerableDamageFlatAdd;

	[Separator("Gameplay Misc", true)]
	public List<StatusType> m_statusesToDelayFromCombatToNextTurn;

	public int m_killAssistMemory = 2;

	public int AdvancedSkinUnlockLevel = 6;

	public int ExpertSkinUnlockLevel = 8;

	public int MasterySkinUnlockLevel = 10;

	public Ability m_gameModeAbility;

	public int NumOverconsPerTurn = 3;

	public int NumOverconsPerMatch = 10;

	public int FreeAutomaticOverconOnCatalyst_OverconId = -1;

	public int FreeAutomaticOverconOnDeath_OverconID = -1;

	[Space(10f)]
	public bool m_useEnergyStatusForPassiveRegen;

	[Header("Buff [Energized]")]
	public AbilityModPropertyInt m_energizedEnergyGainMod;

	[Header("Debuff [SlowEnergyGain]")]
	public AbilityModPropertyInt m_slowEnergyGainEnergyGainMod;

	[Separator("Character Resource Links", true)]
	public CharacterResourceLink[] m_characterResourceLinks;

	public GameObject SpectatorPrefab;

	[Separator("Map Data", true)]
	public MapData[] m_mapData;

	private Dictionary<string, MapData> m_mapDataDictionary;

	[Separator("Targeting", true)]
	public float m_actorTargetingRadiusInSquares = 0.4f;

	public float m_laserInitialOffsetInSquares = 0.41f;

	public bool m_useActorRadiusForLaser;

	public bool m_useActorRadiusForCones;

	[Header("-- Max angle for bouncing off actors")]
	public float m_maxAngleForBounceOnActor = 90f;

	[Separator("Visibility On Ability Cast", true)]
	public bool m_abilityCasterVisibleOnCast;

	[Header("-- Game Balance Vars --")]
	public GameBalanceVars m_gameBalanceVars = new GameBalanceVars();

	[Header("-- Banned Words --")]
	public BannedWords m_bannedWords = new BannedWords();

	[Header("-- Loot Matrix Packs --")]
	public LootMatrixPackData m_lootMatrixPackData = new LootMatrixPackData();

	[Header("-- Game Packs --")]
	public GamePackData m_gamePackData = new GamePackData();

	[Header("-- GG Boost Packs --")]
	public GGPackData m_ggPackData = new GGPackData();

	[Header("-- Loading Tips --")]
	public string[] m_loadingTips;

	[Separator("Timebank", true)]
	public float m_tbInitial;

	public float m_tbRecharge;

	public float m_tbRechargeCap;

	public int m_tbConsumables;

	public float m_tbConsumableDuration = 5f;

	public float m_tbGracePeriodBeforeConsuming = 0.2f;

	[Separator("Key Command Data", true)]
	public KeyBindingCommand[] m_keyBindingData;

	private Dictionary<string, KeyBindingCommand> m_keyBindingDataDictionary;

	private void Awake()
	{
		s_instance = this;
		if (m_characterResourceLinks.Length == 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					throw new Exception("GameWideData failed to load (no character resource links)");
				}
			}
		}
		List<GameBalanceVars.CharacterUnlockData> list = new List<GameBalanceVars.CharacterUnlockData>();
		for (int i = 0; i < m_characterResourceLinks.Length; i++)
		{
			CharacterResourceLink characterResourceLink = m_characterResourceLinks[i];
			if (characterResourceLink.m_characterType == CharacterType.None)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						throw new Exception(new StringBuilder().Append("GameWideData failed to load (invalid data for character index ").Append(i).Append(")").ToString());
					}
				}
			}
			list.Add(m_characterResourceLinks[i].CreateUnlockData());
		}
		m_gameBalanceVars.characterUnlockData = list.ToArray();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public static GameWideData Get()
	{
		return s_instance;
	}

	public bool UseActorRadiusForLaser()
	{
		return m_useActorRadiusForLaser;
	}

	public bool UseActorRadiusForCone()
	{
		return m_useActorRadiusForCones;
	}

	public bool ShouldMakeCasterVisibleOnCast()
	{
		return m_abilityCasterVisibleOnCast;
	}

	public CharacterResourceLink GetCharacterResourceLink(CharacterType characterType)
	{
		CharacterResourceLink[] characterResourceLinks = m_characterResourceLinks;
		foreach (CharacterResourceLink characterResourceLink in characterResourceLinks)
		{
			if (characterResourceLink.m_characterType != characterType)
			{
				continue;
			}
			while (true)
			{
				return characterResourceLink;
			}
		}
		while (true)
		{
			throw new Exception(new StringBuilder().Append("Character resource link not found for: ").Append(characterType.ToString()).Append(" in GameWideData.").ToString());
		}
	}

	public string GetCharacterDisplayName(CharacterType characterType)
	{
		return StringUtil.TR_CharacterName(characterType.ToString());
	}

	public string GetLoadingScreenTip(int tipIndex)
	{
		return StringUtil.TR_LoadingScreenTip(tipIndex + 1);
	}

	public MapData GetMapDataByDisplayName(string mapDisplayName)
	{
		if (mapDisplayName == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		Debug.Log(new StringBuilder().Append("attempting to find: ").Append(mapDisplayName).ToString());
		MapData[] mapData = m_mapData;
		foreach (MapData mapData2 in mapData)
		{
			Debug.Log(mapData2.DisplayName);
			if (!(mapData2.DisplayName.ToLower() == mapDisplayName.ToLower()))
			{
				continue;
			}
			while (true)
			{
				return mapData2;
			}
		}
		return null;
	}

	public MapData GetMapData(string mapName)
	{
		if (mapName == null)
		{
			return null;
		}
		if (m_mapDataDictionary == null)
		{
			m_mapDataDictionary = new Dictionary<string, MapData>(StringComparer.OrdinalIgnoreCase);
			MapData[] mapData = m_mapData;
			foreach (MapData mapData2 in mapData)
			{
				m_mapDataDictionary.Add(mapData2.Name, mapData2);
			}
		}

		MapData value;
		if (m_mapDataDictionary.TryGetValue(mapName, out value))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return value;
				}
			}
		}
		return null;
	}

	public string GetMapDisplayName(string mapName)
	{
		MapData mapData = GetMapData(mapName);
		if (mapData == null)
		{
			string text = StringUtil.TR_MapName(mapName);
			if (text.IsNullOrEmpty())
			{
				return mapName;
			}
			return text;
		}
		return mapData.GetDisplayName();
	}

	public KeyBindingCommand GetKeyBindingCommand(string keyBindName)
	{
		if (keyBindName == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		if (m_keyBindingDataDictionary == null)
		{
			m_keyBindingDataDictionary = new Dictionary<string, KeyBindingCommand>();
			KeyBindingCommand[] keyBindingData = m_keyBindingData;
			foreach (KeyBindingCommand keyBindingCommand in keyBindingData)
			{
				m_keyBindingDataDictionary.Add(keyBindingCommand.Name, keyBindingCommand);
			}
		}

		KeyBindingCommand value;
		if (m_keyBindingDataDictionary.TryGetValue(keyBindName, out value))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return value;
				}
			}
		}
		return null;
	}

	public string GetKeyBindingDisplayName(string keyBindName)
	{
		KeyBindingCommand keyBindingCommand = GetKeyBindingCommand(keyBindName);
		if (keyBindingCommand == null)
		{
			return keyBindName;
		}
		return keyBindingCommand.GetDisplayName();
	}

	public string GetUnlockString(GameBalanceVars.UnlockData unlock)
	{
		if (unlock == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		string text = string.Empty;
		GameBalanceVars.UnlockCondition[] unlockConditions = unlock.UnlockConditions;
		foreach (GameBalanceVars.UnlockCondition unlockCondition in unlockConditions)
		{
			if (text != string.Empty)
			{
				text += Environment.NewLine;
			}
			if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.CharacterLevel)
			{
				CharacterType typeSpecificData = (CharacterType)unlockCondition.typeSpecificData;
				int typeSpecificData2 = unlockCondition.typeSpecificData2;
				if (typeSpecificData != 0)
				{
					text += new StringBuilder().Append(GetCharacterResourceLink(typeSpecificData).m_displayName).Append(" Level ").Append(typeSpecificData2).ToString();
				}
			}
			else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.PlayerLevel)
			{
				int typeSpecificData3 = unlockCondition.typeSpecificData;
				text += new StringBuilder().Append("Account Level ").Append(typeSpecificData3).ToString();
			}
			else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.ELO)
			{
				int typeSpecificData4 = unlockCondition.typeSpecificData;
				text += new StringBuilder().Append("ELO of ").Append(typeSpecificData4).ToString();
			}
		}
		while (true)
		{
			return text;
		}
	}

	private void OnValidate()
	{
		m_gameBalanceVars.OnValidate();
	}
}
