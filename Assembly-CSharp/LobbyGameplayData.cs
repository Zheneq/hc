using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[Serializable]
public class LobbyGameplayData
{
	private static LobbyGameplayData s_instance;

	public Dictionary<CharacterType, LobbyCharacterGameplayData> CharacterData;

	public Dictionary<CardType, LobbyCardGameplayData> CardData;

	public GameBalanceVars GameBalanceVars;

	public BannedWords BannedWords;

	public LobbyLootMatrixPackData LootMatrixPackData;

	public LobbyGamePackData GamePackData;

	public LobbyGGPackData GGPackData;

	public LobbyInventoryData InventoryData;

	public LobbyStoreData StoreData;

	public LobbyQuestData QuestData;

	public LobbySeasonData SeasonData;

	public LobbyFactionData FactionData;

	[JsonIgnore]
	public IFreelancerSetQueryInterface FreelancerSetQueryInterface => new LobbyGameplayFreelancerSetQueryInterface(this);

	public LobbyGameplayData()
	{
		CharacterData = new Dictionary<CharacterType, LobbyCharacterGameplayData>();
		CardData = new Dictionary<CardType, LobbyCardGameplayData>();
		GameBalanceVars = new GameBalanceVars();
		BannedWords = new BannedWords();
		LootMatrixPackData = new LobbyLootMatrixPackData();
		GamePackData = new LobbyGamePackData();
		GGPackData = new LobbyGGPackData();
		InventoryData = new LobbyInventoryData();
		StoreData = new LobbyStoreData();
		QuestData = new LobbyQuestData();
		SeasonData = new LobbySeasonData();
		FactionData = new LobbyFactionData();
	}

	public static LobbyGameplayData Get()
	{
		return s_instance;
	}

	public static void Set(LobbyGameplayData instance)
	{
		s_instance = instance;
	}

	public CharacterModInfo GetDefaultAbilityMods(CharacterType characterType)
	{
		LobbyCharacterGameplayData characterData = GetCharacterData(characterType);
		if (characterData != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return characterData.GetDefaultModInfo();
				}
			}
		}
		return default(CharacterModInfo);
	}

	public LobbyCharacterGameplayData GetCharacterData(CharacterType characterType)
	{
		LobbyCharacterGameplayData value = null;
		CharacterData.TryGetValue(characterType, out value);
		return value;
	}

	public string GetCharacterDisplayName(CharacterType characterType)
	{
		LobbyCharacterGameplayData characterData = GetCharacterData(characterType);
		string result;
		if (characterData != null)
		{
			result = characterData.DisplayName;
		}
		else
		{
			result = string.Empty;
		}
		return result;
	}

	public CharacterConfig GetDefaultCharacterConfig(CharacterType characterType)
	{
		LobbyCharacterGameplayData characterData = GetCharacterData(characterType);
		CharacterConfig characterConfig;
		if (characterData != null)
		{
			characterConfig = characterData.CharacterConfig;
		}
		else
		{
			characterConfig = new CharacterConfig();
			characterConfig.CharacterType = characterType;
			characterConfig.IsHidden = true;
			characterConfig.IsHiddenFromFreeRotationUntil = DateTime.MaxValue;
		}
		return characterConfig;
	}

	public LobbyCardGameplayData GetCardData(CardType cardType)
	{
		LobbyCardGameplayData value = null;
		CardData.TryGetValue(cardType, out value);
		return value;
	}

	public CharacterCardInfo GetDefaultCardInfo()
	{
		CharacterCardInfo result = default(CharacterCardInfo);
		Dictionary<CardType, LobbyCardGameplayData>.ValueCollection values = CardData.Values;
		if (_003C_003Ef__am_0024cache0 == null)
		{
			_003C_003Ef__am_0024cache0 = delegate(LobbyCardGameplayData c)
			{
				int result4;
				if (c.RunPhase == AbilityRunPhase.Prep)
				{
					result4 = (c.IsDefault ? 1 : 0);
				}
				else
				{
					result4 = 0;
				}
				return (byte)result4 != 0;
			};
		}
		result.PrepCard = values.SingleOrDefault(_003C_003Ef__am_0024cache0).CardType;
		Dictionary<CardType, LobbyCardGameplayData>.ValueCollection values2 = CardData.Values;
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = delegate(LobbyCardGameplayData c)
			{
				int result3;
				if (c.RunPhase == AbilityRunPhase.Combat)
				{
					result3 = (c.IsDefault ? 1 : 0);
				}
				else
				{
					result3 = 0;
				}
				return (byte)result3 != 0;
			};
		}
		result.CombatCard = values2.SingleOrDefault(_003C_003Ef__am_0024cache1).CardType;
		result.DashCard = CardData.Values.SingleOrDefault(delegate(LobbyCardGameplayData c)
		{
			int result2;
			if (c.RunPhase == AbilityRunPhase.Dash)
			{
				result2 = (c.IsDefault ? 1 : 0);
			}
			else
			{
				result2 = 0;
			}
			return (byte)result2 != 0;
		}).CardType;
		return result;
	}

	public CardType GetDefaultCardType(AbilityRunPhase runPhase)
	{
		return CardData.Values.SingleOrDefault(delegate(LobbyCardGameplayData c)
		{
			int result;
			if (c.RunPhase == runPhase)
			{
				result = (c.IsDefault ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}).CardType;
	}

	public LobbyInventoryItemTemplate GetItemTemplate(int itemTemplateId)
	{
		LobbyInventoryItemTemplate value = null;
		InventoryData.ItemTemplates.TryGetValue(itemTemplateId, out value);
		if (value == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					throw new ArgumentException($"Invalid itemTemplateId {itemTemplateId}");
				}
			}
		}
		return value;
	}

	public KarmaTemplate GetKarmaTemplate(int karmaTemplateId)
	{
		KarmaTemplate karmaTemplate = InventoryData.KarmaTemplates.Where((KarmaTemplate x) => x.Index == karmaTemplateId).Single();
		if (karmaTemplate != null)
		{
			if (karmaTemplate.IsValid())
			{
				return karmaTemplate;
			}
		}
		throw new InvalidOperationException($"Invalid karmaTemplateId={karmaTemplateId}");
	}

	public LootTable GetLootTable(int lootTableId)
	{
		LootTable lootTable = InventoryData.LootTables.Where((LootTable x) => x.Index == lootTableId).Single();
		if (lootTable != null)
		{
			if (lootTable.IsValid())
			{
				return lootTable;
			}
		}
		throw new InvalidOperationException($"Invalid lootTableId={lootTableId}");
	}

	public int GetDefaultItemValue()
	{
		return InventoryData.DefaultItemValue;
	}

	public int GetCharacterItemDropBalance()
	{
		return InventoryData.CharacterItemDropBalance;
	}

	public FactionCompetition GetFactionCompetition(int competitionId)
	{
		if (0 < competitionId)
		{
			if (competitionId <= FactionData.m_factionCompetitions.Count)
			{
				return FactionData.m_factionCompetitions[competitionId - 1];
			}
		}
		throw new ArgumentException($"Invalid Faction competitionId={competitionId}");
	}

	public FactionGroup GetFactionGroup(int factionID)
	{
		for (int i = 0; i < FactionData.m_factionGroups.Count; i++)
		{
			if (FactionData.m_factionGroups[i].FactionGroupID != factionID)
			{
				continue;
			}
			while (true)
			{
				return FactionData.m_factionGroups[i];
			}
		}
		while (true)
		{
			return new FactionGroup();
		}
	}

	public Faction GetFaction(int competitionId, int factionId)
	{
		if (0 < competitionId)
		{
			if (competitionId <= FactionData.m_factionCompetitions.Count && 0 <= factionId)
			{
				if (factionId < FactionData.m_factionCompetitions[competitionId - 1].Factions.Count)
				{
					return FactionData.m_factionCompetitions[competitionId - 1].Factions[factionId];
				}
			}
		}
		throw new ArgumentException($"Invalid Faction competitionId={competitionId} factionId={factionId}");
	}

	public virtual void LoadFromFile(string dirName)
	{
		string fileName = Path.Combine(dirName, "LobbyCharacterData.json");
		LoadFromFile(fileName, CharacterData);
		string fileName2 = Path.Combine(dirName, "LobbyCardData.json");
		LoadFromFile(fileName2, CardData);
		string fileName3 = Path.Combine(dirName, "GameBalanceVars.json");
		LoadFromFile(fileName3, GameBalanceVars);
		string fileName4 = Path.Combine(dirName, "BannedWords.json");
		LoadFromFile(fileName4, BannedWords);
		string fileName5 = Path.Combine(dirName, "LootMatrixPackData.json");
		LoadFromFile(fileName5, LootMatrixPackData);
		string fileName6 = Path.Combine(dirName, "GamePackData.json");
		LoadFromFile(fileName6, GamePackData);
		string fileName7 = Path.Combine(dirName, "GGPackData.json");
		LoadFromFile(fileName7, GGPackData);
		string fileName8 = Path.Combine(dirName, "LobbyInventoryData.json");
		LoadFromFile(fileName8, InventoryData);
		string fileName9 = Path.Combine(dirName, "LobbyStoreData.json");
		LoadFromFile(fileName9, StoreData);
		string fileName10 = Path.Combine(dirName, "LobbyQuestData.json");
		LoadFromFile(fileName10, QuestData);
		string fileName11 = Path.Combine(dirName, "LobbySeasonData.json");
		LoadFromFile(fileName11, SeasonData);
		string fileName12 = Path.Combine(dirName, "LobbyFactionData.json");
		LoadFromFile(fileName12, FactionData);
	}

	public virtual void SaveToFile(string dirName)
	{
		string fileName = Path.Combine(dirName, "LobbyCharacterData.json");
		SaveToFile(fileName, CharacterData);
		string fileName2 = Path.Combine(dirName, "LobbyCardData.json");
		SaveToFile(fileName2, CardData);
		string fileName3 = Path.Combine(dirName, "GameBalanceVars.json");
		SaveToFile(fileName3, GameBalanceVars);
		string fileName4 = Path.Combine(dirName, "BannedWords.json");
		SaveToFile(fileName4, BannedWords);
		string fileName5 = Path.Combine(dirName, "LootMatrixPackData.json");
		SaveToFile(fileName5, LootMatrixPackData);
		string fileName6 = Path.Combine(dirName, "GamePackData.json");
		SaveToFile(fileName6, GamePackData);
		string fileName7 = Path.Combine(dirName, "GGPackData.json");
		SaveToFile(fileName7, GGPackData);
		string fileName8 = Path.Combine(dirName, "LobbyInventoryData.json");
		SaveToFile(fileName8, InventoryData);
		string fileName9 = Path.Combine(dirName, "LobbyStoreData.json");
		SaveToFile(fileName9, StoreData);
		string fileName10 = Path.Combine(dirName, "LobbyQuestData.json");
		SaveToFile(fileName10, QuestData);
		string fileName11 = Path.Combine(dirName, "LobbySeasonData.json");
		SaveToFile(fileName11, SeasonData);
		string fileName12 = Path.Combine(dirName, "LobbyFactionData.json");
		SaveToFile(fileName12, FactionData);
	}

	public virtual void LoadFromFile(string fileName, object obj)
	{
		string text = "{}";
		FileInfo fileInfo = new FileInfo(fileName);
		if (!fileInfo.Exists)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		text = File.ReadAllText(fileName);
		JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
		jsonSerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
		JsonSerializerSettings settings = jsonSerializerSettings;
		JsonConvert.PopulateObject(text, obj, settings);
	}

	public virtual void SaveToFile(string fileName, object obj)
	{
		string contents = JsonConvert.SerializeObject(obj, Formatting.Indented);
		File.WriteAllText(fileName, contents);
	}
}
