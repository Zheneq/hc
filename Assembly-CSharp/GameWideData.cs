using System;
using System.Collections.Generic;
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

	public int MasterySkinUnlockLevel = 0xA;

	public Ability m_gameModeAbility;

	public int NumOverconsPerTurn = 3;

	public int NumOverconsPerMatch = 0xA;

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
		GameWideData.s_instance = this;
		if (this.m_characterResourceLinks.Length == 0)
		{
			throw new Exception("GameWideData failed to load (no character resource links)");
		}
		List<GameBalanceVars.CharacterUnlockData> list = new List<GameBalanceVars.CharacterUnlockData>();
		for (int i = 0; i < this.m_characterResourceLinks.Length; i++)
		{
			CharacterResourceLink characterResourceLink = this.m_characterResourceLinks[i];
			if (characterResourceLink.m_characterType == CharacterType.None)
			{
				throw new Exception(string.Format("GameWideData failed to load (invalid data for character index {0})", i));
			}
			list.Add(this.m_characterResourceLinks[i].CreateUnlockData());
		}
		this.m_gameBalanceVars.characterUnlockData = list.ToArray();
	}

	private void OnDestroy()
	{
		GameWideData.s_instance = null;
	}

	public static GameWideData Get()
	{
		return GameWideData.s_instance;
	}

	public bool UseActorRadiusForLaser()
	{
		return this.m_useActorRadiusForLaser;
	}

	public bool UseActorRadiusForCone()
	{
		return this.m_useActorRadiusForCones;
	}

	public bool ShouldMakeCasterVisibleOnCast()
	{
		return this.m_abilityCasterVisibleOnCast;
	}

	public CharacterResourceLink GetCharacterResourceLink(CharacterType characterType)
	{
		foreach (CharacterResourceLink characterResourceLink in this.m_characterResourceLinks)
		{
			if (characterResourceLink.m_characterType == characterType)
			{
				return characterResourceLink;
			}
		}
		throw new Exception("Character resource link not found for: " + characterType.ToString() + " in GameWideData.");
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
			return null;
		}
		Debug.Log("attempting to find: " + mapDisplayName);
		foreach (MapData mapData2 in this.m_mapData)
		{
			Debug.Log(mapData2.DisplayName);
			if (mapData2.DisplayName.ToLower() == mapDisplayName.ToLower())
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
		if (this.m_mapDataDictionary == null)
		{
			this.m_mapDataDictionary = new Dictionary<string, MapData>(StringComparer.OrdinalIgnoreCase);
			foreach (MapData mapData2 in this.m_mapData)
			{
				this.m_mapDataDictionary.Add(mapData2.Name, mapData2);
			}
		}
		MapData result;
		if (this.m_mapDataDictionary.TryGetValue(mapName, out result))
		{
			return result;
		}
		return null;
	}

	public string GetMapDisplayName(string mapName)
	{
		MapData mapData = this.GetMapData(mapName);
		if (mapData != null)
		{
			return mapData.GetDisplayName();
		}
		string text = StringUtil.TR_MapName(mapName);
		if (text.IsNullOrEmpty())
		{
			return mapName;
		}
		return text;
	}

	public KeyBindingCommand GetKeyBindingCommand(string keyBindName)
	{
		if (keyBindName == null)
		{
			return null;
		}
		if (this.m_keyBindingDataDictionary == null)
		{
			this.m_keyBindingDataDictionary = new Dictionary<string, KeyBindingCommand>();
			foreach (KeyBindingCommand keyBindingCommand in this.m_keyBindingData)
			{
				this.m_keyBindingDataDictionary.Add(keyBindingCommand.Name, keyBindingCommand);
			}
		}
		KeyBindingCommand result;
		if (this.m_keyBindingDataDictionary.TryGetValue(keyBindName, out result))
		{
			return result;
		}
		return null;
	}

	public string GetKeyBindingDisplayName(string keyBindName)
	{
		KeyBindingCommand keyBindingCommand = this.GetKeyBindingCommand(keyBindName);
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
			return string.Empty;
		}
		string text = string.Empty;
		foreach (GameBalanceVars.UnlockCondition unlockCondition in unlock.UnlockConditions)
		{
			if (text != string.Empty)
			{
				text += Environment.NewLine;
			}
			if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.CharacterLevel)
			{
				CharacterType typeSpecificData = (CharacterType)unlockCondition.typeSpecificData;
				int typeSpecificData2 = unlockCondition.typeSpecificData2;
				if (typeSpecificData != CharacterType.None)
				{
					text += string.Format("{0} Level {1}", this.GetCharacterResourceLink(typeSpecificData).m_displayName, typeSpecificData2);
				}
			}
			else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.PlayerLevel)
			{
				int typeSpecificData3 = unlockCondition.typeSpecificData;
				text += string.Format("Account Level {0}", typeSpecificData3);
			}
			else if (unlockCondition.ConditionType == GameBalanceVars.UnlockData.UnlockType.ELO)
			{
				int typeSpecificData4 = unlockCondition.typeSpecificData;
				text += string.Format("ELO of {0}", typeSpecificData4);
			}
		}
		return text;
	}

	private void OnValidate()
	{
		this.m_gameBalanceVars.OnValidate();
	}
}
